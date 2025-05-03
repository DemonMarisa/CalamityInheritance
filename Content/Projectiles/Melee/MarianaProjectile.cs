using CalamityMod.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class MarianaProjectile : ModProjectile, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.timeLeft > 150)
                return false;
            else
                return true;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.2f, 0.8f);
            if (Projectile.timeLeft > 150)
                Projectile.velocity *= 0.97f;
            if (Projectile.timeLeft < 150)
                CIFunction.HomeInOnNPC(Projectile, false, 1500f, 9f, 20f);

            Projectile.ai[0]++;
            if (Projectile.ai[0] % 10 == 0)
            {
                for (int l = 0; l < 12; l++)
                {
                    Vector2 vector3 = Vector2.UnitX * (float)-(float)Projectile.width / 2f;
                    vector3 += -Vector2.UnitY.RotatedBy((double)((float)l * 3.14159274f / 6f), default) * new Vector2(8f, 16f);
                    vector3 = vector3.RotatedBy((double)(Projectile.rotation - 1.57079637f), default);
                    int num9 = Dust.NewDust(Projectile.Center, 0, 0, 221, 0f, 0f, 160, default, 1f);
                    Main.dust[num9].scale = 1.1f;
                    Main.dust[num9].noGravity = true;
                    Main.dust[num9].position = Projectile.Center + vector3;
                    Main.dust[num9].velocity = Projectile.velocity * 0.1f;
                    Main.dust[num9].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num9].position) * 1.25f;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * ((float)b2 / 255f));
                return new Color((int)b2, (int)b2, (int)b2, (int)a2);
            }
            return new Color(255, 255, 255, 100);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int l = 0; l < 12; l++)
            {
                Vector2 vector3 = Vector2.UnitX * (float)-(float)Projectile.width / 2f;
                vector3 += -Vector2.UnitY.RotatedBy((double)((float)l * 3.14159274f / 6f), default) * new Vector2(8f, 16f);
                vector3 = vector3.RotatedBy((double)(Projectile.rotation - 1.57079637f), default);
                int num9 = Dust.NewDust(Projectile.Center, 0, 0, DustID.FireworkFountain_Blue, 0f, 0f, 160, default, 1f);
                Main.dust[num9].scale = 1.1f;
                Main.dust[num9].noGravity = true;
                Main.dust[num9].position = Projectile.Center + vector3;
                Main.dust[num9].velocity = Projectile.velocity * 0.1f;
                Main.dust[num9].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num9].position) * 1.25f;
            }
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }
    }
}
