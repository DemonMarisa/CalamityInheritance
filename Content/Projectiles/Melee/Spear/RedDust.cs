using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Projectiles.Melee.Spear
{
    public class RedDust : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 3600;
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.velocity.X * 0.02f;
            if (Projectile.velocity.X < 0f)
            {
                Projectile.rotation -= Math.Abs(Projectile.velocity.Y) * 0.02f;
            }
            else
            {
                Projectile.rotation += Math.Abs(Projectile.velocity.Y) * 0.02f;
            }
            Projectile.velocity *= 0.98f;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 60f)
            {
                if (Projectile.timeLeft > 85)
                    Projectile.timeLeft = 85;
                if (Projectile.alpha < 255)
                {
                    Projectile.alpha += 2;
                    if (Projectile.alpha > 255)
                    {
                        Projectile.alpha = 255;
                    }
                }
                else if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.Kill();
                }
            }
            else if (Projectile.alpha > 80)
            {
                Projectile.alpha -= 30;
                if (Projectile.alpha < 80)
                {
                    Projectile.alpha = 80;
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
    }
}
