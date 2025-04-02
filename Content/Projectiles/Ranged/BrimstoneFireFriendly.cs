using CalamityInheritance.Utilities;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class BrimstoneFireFriendlyLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 90;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.25f / 255f, (255 - Projectile.alpha) * 0.05f / 255f, (255 - Projectile.alpha) * 0.05f / 255f);
            if (Projectile.timeLeft > 90)
            {
                Projectile.timeLeft = 90;
            }
            if (Projectile.ai[0] > 7f)
            {
                if (Main.zenithWorld)
                    CIFunction.HomeInOnNPC(Projectile, true, 1800f, 24f, 20f);
                float pScale = 1f;
                if (Projectile.ai[0] == 8f)
                {
                    pScale = 0.25f;
                }
                else if (Projectile.ai[0] == 9f)
                {
                    pScale = 0.5f;
                }
                else if (Projectile.ai[0] == 10f)
                {
                    pScale = 0.75f;
                }
                Projectile.ai[0] += 1f;
                int num297 = 235;
                if (Main.rand.NextBool(2))
                {
                    for (int i = 0; i < 1; i++)
                    {
                        int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, num297, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1f);
                        if (Main.rand.NextBool(3))
                        {
                            Main.dust[d].noGravity = true;
                            Main.dust[d].scale *= 3f;
                            Dust expr_DBEF_cp_0 = Main.dust[d];
                            expr_DBEF_cp_0.velocity.X *= 2f;
                            Dust expr_DC0F_cp_0 = Main.dust[d];
                            expr_DC0F_cp_0.velocity.Y *= 2f;
                        }
                        else
                        {
                            Main.dust[d].scale *= 1.5f;
                        }
                        Dust expr_DC74_cp_0 = Main.dust[d];
                        expr_DC74_cp_0.velocity.X *= 1.2f;
                        Dust expr_DC94_cp_0 = Main.dust[d];
                        expr_DC94_cp_0.velocity.Y *= 1.2f;
                        Main.dust[d].scale *= pScale;
                    }
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            Projectile.rotation += 0.3f * Projectile.direction;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 7;
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 240);
        }
    }
}
