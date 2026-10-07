using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Core.Misc;
using CalamityInheritance.Core.Utils;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons
{
    public class ExoGladProj : CIExoProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
        }

        public float counter = 0f;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Vector2 value7 = new Vector2(5f, 10f);
            counter += 1f;
            if (counter == 48f)
            {
                counter = 0f;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    int dustType = i == 0 ? 107 : 234;
                    if (Main.rand.NextBool(4))
                    {
                        dustType = CIDustID.DustSandnado;
                    }
                    Vector2 offset = Vector2.UnitX * -12f;
                    offset = -Vector2.UnitY.RotatedBy((double)(counter * 0.1308997f + i * MathHelper.Pi), default) * value7;
                    int exo = Dust.NewDust(Projectile.Center, 0, 0, dustType, 0f, 0f, 160, default, 1.5f);
                    Main.dust[exo].noGravity = true;
                    Main.dust[exo].position = Projectile.Center + offset;
                    Main.dust[exo].velocity = Projectile.velocity;
                    int dusters = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 0.8f);
                    Main.dust[dusters].noGravity = true;
                    Main.dust[dusters].velocity *= 0f;
                }
            }
            if (Projectile.ai[1] != 0f)
            {
                Projectile.extraUpdates = 1;
                NPC target = LAPUtilities.FindClosestTarget(Projectile.Center, 1500f);
                if (target is not null)
                    Projectile.PredictHomeIn(22f, 35f, target);
                else
                    Projectile.extraUpdates = 1;
            }
            else
            {
                Projectile.extraUpdates = 1;
                Projectile.HomeInNPC(1000f, 12f, 35f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            int dustType = Utils.SelectRandom(Main.rand, new int[]
            {
                107,
                234,
                CIDustID.DustSandnado
            });
            for (int k = 0; k < 4; k++)
            {
                int exo = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, dustType, Projectile.direction * 2, 0f, 150, default, 1f);
                Main.dust[exo].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIMiracleBlight>(), 300);
            OnHitEffects(target.Center);
        }
        private void OnHitEffects(Vector2 targetPos)
        {
            Player player = Main.player[Projectile.owner];
            var source = Projectile.GetSource_FromThis();
            float swordKB = Projectile.knockBack;
            int swordDmg = (int)(Projectile.damage * 0.25);
            int numSwords = Main.rand.Next(1, 4);
            int spearAmt = Main.rand.Next(1, 4);
            int comet = Main.rand.Next(1, 2);
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < numSwords; ++i)
                {
                    CIUtils.ProjectileBarrage(source, Projectile.Center, targetPos, Main.rand.NextBool(), 1000f, 1400f, 80f, 900f, Main.rand.NextFloat(24f, 30f), ProjectileType<ExoGladiusBeam>(), swordDmg, swordKB, Projectile.owner, false, 5, 0, 0, Projectile.ai[1]);
                }

                for (int n = 0; n < spearAmt; n++)
                {
                    CIUtils.ProjectileRain(source, targetPos, 400f, 100f, -1000f, -800f, 29f, ProjectileType<ExoGladSpears>(), swordDmg, swordKB, Projectile.owner, 0, Projectile.ai[1]);
                }
                if (Projectile.ai[1] != 0f)
                {
                    for (int j = 0; j < comet; ++j)
                    {
                        CIUtils.ProjectileRain(source, targetPos, 400f, 100f, 500f, 800f, 25f, ProjectileType<ExoGladComet>(), swordDmg, swordKB, Projectile.owner, 0, Projectile.ai[1]);
                    }
                }
            }
        }
    }
}
