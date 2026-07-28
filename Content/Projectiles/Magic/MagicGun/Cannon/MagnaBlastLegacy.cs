using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.MagicGun.Cannon
{
    public class MagnaBlastLegacy : CIMagicProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            if (Projectile.scale <= 1.6f)
            {
                Projectile.scale *= 1.01f;
                Projectile.width = (int)(16f * Projectile.scale);
                Projectile.height = (int)(32f * Projectile.scale);
            }
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.1f / 255f, (255 - Projectile.alpha) * 0.1f / 255f, (255 - Projectile.alpha) * 1f / 255f);
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int i = 0; i < 3; i++)
                {
                    int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.BlueTorch, 0f, 0f, 100, default, Projectile.scale);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0f;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }
    }
}
