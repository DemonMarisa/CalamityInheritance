using System;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class DestroyerLegendaryBoom: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        public override void SetDefaults()
        {
            Projectile.width = 500;
            Projectile.height = 500;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Main.player[Projectile.owner].CIMod().DestroyerTier1)
            {
                Projectile.width = Projectile.height = 800;
            }

            float lights = Main.rand.Next(90, 111) * 0.01f;
            lights *= Main.essScale;
            Lighting.AddLight(Projectile.Center, 5f * lights, 1f * lights, 4f * lights);
            float pTimer = 25f;
            if (Projectile.ai[0] > 180f)
            {
                pTimer -= (Projectile.ai[0] - 180f) / 2f;
            }
            if (pTimer <= 0f)
            {
                pTimer = 0f;
                Projectile.Kill();
            }
            pTimer *= 0.7f;
            Projectile.ai[0] += 4f;
            int tCounter = 0;
            while (tCounter < pTimer)
            {
                float r = Main.rand.Next(-40, 41);
                float r2 = Main.rand.Next(-40, 41);
                float r3 = Main.rand.Next(12, 36);
                float rAdj = (float)Math.Sqrt((double)(r * r + r2 * r2));
                rAdj = r3 / rAdj;
                r *= rAdj;
                r2 *= rAdj;
                int rDust = Main.rand.Next(3);
                rDust = rDust switch
                {
                    0 => 246,
                    1 => 73,
                    _ => 187,
                };

                int boom = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, rDust, 0f, 0f, 100, default, 2f);
                Main.dust[boom].noGravity = true;
                Main.dust[boom].position.X = Projectile.Center.X;
                Main.dust[boom].position.Y = Projectile.Center.Y;
                Dust extraBoom = Main.dust[boom];
                extraBoom.position.X += Main.rand.Next(-10, 11);
                Dust extraBoom2 = Main.dust[boom];
                extraBoom2.position.Y += Main.rand.Next(-10, 11);
                Main.dust[boom].velocity.X = r;
                Main.dust[boom].velocity.Y = r2;
                tCounter++;
            }
        }
    }
}
