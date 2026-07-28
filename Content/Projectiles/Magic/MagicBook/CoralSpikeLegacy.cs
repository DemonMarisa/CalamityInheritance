using CalamityInheritance.Content.BaseClass.Projectiles;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.MagicBook
{
    public class CoralSpikeLegacy : CIMagicProj
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.aiStyle = ProjAIStyleID.GroundProjectile;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 360;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.velocity.Y *= 0.99f;
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
        }
    }
}
