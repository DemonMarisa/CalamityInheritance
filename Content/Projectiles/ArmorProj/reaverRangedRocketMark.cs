using CalamityMod;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ArmorProj
{
    public class reaverRangedRocketMark : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public override string Texture => "CalamityInheritance/Content/Projectiles/ArmorProj/MiniRocket";
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 20;
        }
        public override void AI()
        {
            Vector2 armPosition = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            var source = Projectile.GetSource_FromThis();
            Vector2 verticalOffset = Vector2.UnitY.RotatedBy(Projectile.rotation);
            Vector2 position = armPosition + Projectile.velocity * 55f - verticalOffset * 10f;
            Vector2 velocity = Projectile.velocity * 500f;

            if (Projectile.timeLeft == 20)
            {
                int RocketDamage = (int)(0.70f * Projectile.damage);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MiniRocket>(), RocketDamage , 2f, Projectile.owner, 0f, 0f);
            }
            if (Projectile.timeLeft == 10)
            {
                int RocketDamage = (int)(0.70f * Projectile.damage);
                Vector2 adjustedVelocity = velocity.RotatedBy(MathHelper.ToRadians(-140));
                Projectile.NewProjectile(source, position, adjustedVelocity, ModContent.ProjectileType<MiniRocket>(), RocketDamage, 2f, Projectile.owner, 0f, 0f);
            }
            if (Projectile.timeLeft == 1)
            {
                int RocketDamage = (int)(0.70f * Projectile.damage);
                Vector2 adjustedVelocity = velocity.RotatedBy(MathHelper.ToRadians(140));
                Projectile.NewProjectile(source, position, adjustedVelocity, ModContent.ProjectileType<MiniRocket>(), RocketDamage, 2f, Projectile.owner, 0f, 0f);
            }
        }
    }
}
