using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class StratusSphereProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        int roundsGone = 0;
        int dust_nut = 0;
        public override void SetStaticDefaults()
        {
            Main.projFrames[base.Projectile.type] = 6;
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.width = 314;
            Projectile.height = 198;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.knockBack = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 500;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(230, 230, 255, Projectile.alpha);
        }
        public override void AI()
        {
            Projectile.ai[0]++;

            if (Projectile.ai[0] < 200)
            {
                if (Projectile.ai[0] > 100)
                {
                    Projectile.velocity.X = Projectile.velocity.X * 40 / 41;
                    Projectile.velocity.Y = Projectile.velocity.Y * 50 / 51 - 0.005f;
                }
            }
            else
            {
                Projectile.velocity.X = Projectile.velocity.X * 10 / 11;
                Projectile.velocity.Y = Projectile.velocity.Y * 10 / 11;
                if (roundsGone <= 4)
                {
                    Projectile.ai[1] += 1f;
                }
                int rand = Main.rand.Next(-50, 51);
                int rand2 = Main.rand.Next(-50, 51);
                Vector2 targetDir = Projectile.Center + new Vector2(rand, rand2);
                if (Projectile.ai[1] > 40)
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(),Projectile.Center, CalamityUtils.SafeDirectionTo(Projectile, targetDir, null) * 12f, ModContent.ProjectileType<Crescent>(), Projectile.damage / 2, 0.4f, Projectile.owner, Projectile.whoAmI);

                if (Projectile.ai[1] > 46)
                {
                    Projectile.ai[1] = 0;
                    roundsGone++;
                }
                if (roundsGone > 4 && Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<Crescent>()] == 0)
                    Projectile.Kill();
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= 6)
                {
                    Projectile.frame = 0;
                }
            }

            if (Main.rand.NextFloat() < 1f)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                Vector2 position = Projectile.Center;
                dust = Main.dust[Terraria.Dust.NewDust(position, 0, 0, DustID.Electric, 0f, 0f, 0, new Color(255, 255, 255), 0.7236842f)];
            }

            dust_nut++;

            if (dust_nut > 22)
            {
                int num20 = 36;
                for (int i = 0; i < num20; i++)
                {
                    Vector2 spinningpoint = Vector2.Normalize(Projectile.velocity) * new Vector2((float)Projectile.width / 2f, (float)Projectile.height) * 0.75f * 0.5f;
                    spinningpoint = spinningpoint.RotatedBy((double)((float)(i - (num20 / 2 - 1)) * 6.28318548f / (float)num20), default(Vector2)) + Projectile.Center;
                    Vector2 vector = spinningpoint - Projectile.Center;
                    int num21 = Dust.NewDust(spinningpoint + vector, 0, 0, DustID.Electric, vector.X * 2f, vector.Y * 2f, 0, new Color(255, 255, 255), 0.7236842f);
                    Main.dust[num21].noGravity = true;
                    Main.dust[num21].noLight = true;
                    Main.dust[num21].velocity = Vector2.Normalize(vector) * 3f;
                }
                dust_nut = 0;
            }

                CalamityUtils.HomeInOnNPC(Projectile, true, 1500, 7, 7);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
