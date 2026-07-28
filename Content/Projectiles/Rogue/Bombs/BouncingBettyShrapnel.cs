using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Rogue.Bombs
{
    public class BouncingBettyShrapnel : CIRogueProj
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.DamageType = RogueDamage.Instance;
        }
        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override void AI()
        {
            if (Math.Abs(Projectile.position.Y - Projectile.oldPosition.Y) > 4f)
            {
                Projectile.velocity.X = 0f;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            else
            {
                Projectile.rotation = MathHelper.Pi;
            }
            Projectile.velocity.Y += 0.2f;
        }
    }
}
