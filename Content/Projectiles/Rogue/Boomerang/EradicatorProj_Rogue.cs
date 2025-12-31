using CalamityInheritance.Content.Items.Weapons.Melee.Boomerang;
using CalamityInheritance.Content.Projectiles.Typeless.NorProj;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue.Boomerang
{
    internal class EradicatorProj_Rogue : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public static readonly float ChasingRange = 60000f;
        public static readonly float ChasingSpeed = 24f;
        private static float RotationIncrement = 0.15f;
        private static int Lifetime = 350;
        private static int ReboundTime = 60;
        public int Time;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 2;
            Projectile.timeLeft = Lifetime;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 3;
            Projectile.DamageType = GetInstance<RogueDamageClass>();
        }

        public override void AI()
        {
            Time++;
            if (Projectile.timeLeft == Lifetime - ReboundTime)
                Projectile.netUpdate = true;
            if (Projectile.timeLeft <= Lifetime - ReboundTime)
            {
                float returnSpeed = Eradicator_Melee.Speed * 1.3f;
                float acceleration = 0.25f;
                Player owner = Main.player[Projectile.owner];
                CIFunction.BoomerangReturningAI(owner, Projectile, returnSpeed, acceleration);
                if (Main.myPlayer == Projectile.owner)
                    if (Projectile.Hitbox.Intersects(owner.Hitbox))
                        Projectile.Kill();
            }
            Lighting.AddLight(Projectile.Center, 0.35f, 0f, 0.25f);
            float spin = Projectile.direction <= 0 ? -1f : 1f;
            Projectile.rotation += spin * RotationIncrement;
            if (Time % 10 == 0)
            {
                if (LAPUtilities.IsLocalPlayer(Projectile.owner))
                {
                    NPC npc = LAPUtilities.FindClosestTarget(Projectile.Center, 900);
                    if (npc is not null)
                    {
                        int damage = (int)(Projectile.damage * 0.8f);
                        Vector2 vel = LAPUtilities.GetVector2(Projectile.Center, npc.Center) * 9f;
                        Projectile proj = LAPUtilities.NewProjWithClass(Projectile.GetSource_FromThis(), Projectile.Center, vel, ProjectileType<NebulaShotLegacy>(), damage, Projectile.knockBack, Projectile.owner, GetInstance<RogueDamageClass>());
                    }
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<GodSlayerInferno>(), 180);

            // Spawn sparks; taken from Despair stone then adapted to a projectile
            Vector2 particleSpawnDisplacement;
            Vector2 splatterDirection;

            particleSpawnDisplacement = new Vector2(2f * -Projectile.ai[2], 2f * -Projectile.ai[2]);
            splatterDirection = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);

            Vector2 SparkSpawnPosition = target.Center + particleSpawnDisplacement;

            if (Projectile.ai[1] % 4 == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    int sparkLifetime = Main.rand.Next(14, 21);
                    float sparkScale = Main.rand.NextFloat(0.8f, 1f) + 1f * 0.05f;
                    Color sparkColor = Color.Lerp(Color.Fuchsia, Color.AliceBlue, Main.rand.NextFloat(0.5f));
                    sparkColor = Color.Lerp(sparkColor, Color.Cyan, Main.rand.NextFloat());

                    if (Main.rand.NextBool(5))
                        sparkScale *= 1.4f;

                    Vector2 sparkVelocity = splatterDirection.RotatedByRandom(MathHelper.TwoPi);
                    sparkVelocity.Y -= 6f;
                    SparkParticle spark = new SparkParticle(SparkSpawnPosition, sparkVelocity, true, sparkLifetime, sparkScale, sparkColor);
                    GeneralParticleHandler.SpawnParticle(spark);
                }
            }

        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffType<GodSlayerInferno>(), 180);

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor);
            return false;
        }
    }
}
