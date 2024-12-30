using CalamityMod.Projectiles.Melee;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class Exoboompersistentold : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.arrow = false;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.extraUpdates = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 5;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<Exoboomold>(), Projectile.damage / 3, 0, Projectile.owner, 0f, 0f);
                }
            }
            target.immune[Projectile.owner] = 0;
            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Exoboomold>(), Projectile.damage / 3, 0, Projectile.owner, 0f, 0f);
            }
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<Exoboomold>(), Projectile.damage / 3, 0, Projectile.owner, 0f, 0f);
                }
            }
        }
    }
}
