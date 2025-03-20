using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class VictideWater: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0f, 0.5f);
            for (int i = 0; i < 10; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default, 0.9f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.5f;
                Main.dust[d].velocity += Projectile.velocity * 0.1f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
            for (int dust = 0; dust <= 40; dust++)
            {
                float dPosX = Main.rand.Next(-10, 11);
                float dPosY = Main.rand.Next(-10, 11);
                float dSpeed = Main.rand.Next(3, 9);
                float dDist = (float)Math.Sqrt((double)(dPosX * dPosX + dPosY * dPosY));
                dDist = dSpeed / dDist;
                dPosX *= dDist;
                dPosY *= dDist;
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 33, 0f, 0f, 100, default, 0.6f);
                Dust d2 = Main.dust[d];
                d2.noGravity = true;
                d2.position.X = Projectile.Center.X;
                d2.position.Y = Projectile.Center.Y;
                d2.position.X += Main.rand.Next(-10, 11);
                d2.position.Y += Main.rand.Next(-10, 11);
                d2.velocity.X = dPosX;
                d2.velocity.Y = dPosY;
            }
        }
    }
}
