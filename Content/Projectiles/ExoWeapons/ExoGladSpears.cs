using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.ExoWeapons;
using CalamityInheritance.Core.Misc;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons
{
    public class ExoGladSpears : CIExoProj
    {
        public override string Texture => GetInstance<ExoGladius>().Texture;
        private Color currentColor = Color.Black;
        internal ref float FlySpeedMultiplier => ref Projectile.ai[1];
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gladius Beam");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.timeLeft = 1080;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (Main.player[Projectile.owner].active && !Main.player[Projectile.owner].dead)
            {
                NPC potentialTarget = LAPUtilities.FindClosestTarget(Projectile.Center, 1000f);

                if (potentialTarget != null)
                    Projectile.velocity = (Projectile.velocity * 20f + LAPUtilities.GetVector2(Projectile.Center, potentialTarget.Center) * 8f) / 21f;
                else if (Projectile.Distance(Main.player[Projectile.owner].Center) > 1000f)
                {
                    float inertia = 25f * FlySpeedMultiplier;
                    Vector2 directionToOwner = (Main.player[Projectile.owner].Center - Projectile.Center).SafeNormalize(Vector2.UnitY);
                    Projectile.velocity = (Projectile.velocity * (inertia - 1f) + directionToOwner * 5f * FlySpeedMultiplier) / inertia;
                }
            }
            else if (Projectile.timeLeft > 30)
                Projectile.timeLeft = 30;

            Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.5f / 255f, Main.DiscoG * 0.5f / 255f, Main.DiscoB * 0.5f / 255f);
            if (currentColor == Color.Black)
            {
                int startPoint = Main.rand.Next(6);
                Projectile.localAI[0] = startPoint;
                currentColor = GetStartingColor(startPoint);
            }
            Visuals(Projectile, ref currentColor);
        }
        internal static void Visuals(Projectile projectile, ref Color c)
        {
            CIUtils.IterateDisco(ref c, ref projectile.localAI[0], 15);
            Vector3 compositeColor = 0.1f * Color.White.ToVector3() + 0.05f * c.ToVector3();
            Lighting.AddLight(projectile.Center, compositeColor);
        }

        internal static Color GetStartingColor(int startPoint = 0)
        {
            switch (startPoint)
            {
                default: return new Color(1f, 0f, 0f, 0f); // Color.Red
                case 1: return new Color(1f, 1f, 0f, 0f); // Color.Yellow
                case 2: return new Color(0f, 1f, 0f, 0f); // Color.Green
                case 3: return new Color(0f, 1f, 1f, 0f); // Color.Turquoise
                case 4: return new Color(0f, 0f, 1f, 0f); // Color.Blue
                case 5: return new Color(1f, 0f, 1f, 0f); // Color.Violet
            }
        }

        // This projectile is always fullbright.
        public override Color? GetAlpha(Color lightColor) => currentColor;

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.Resize(64, 64);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.Item74, Projectile.position);
            int dustType = Utils.SelectRandom(Main.rand, new int[]
            {
                107,
                234,
                CIDustID.DustSandnado
            });
            for (int i = 0; i < 6; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0);
            }
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0);
                dust.noGravity = true;
                dust.velocity *= 3f;

                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0);
                dust.velocity *= 2f;
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIMiracleBlight>(), 300);
        }
    }
}
