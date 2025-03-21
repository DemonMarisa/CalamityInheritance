using System;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class VictideBookSpike: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.aiStyle = 14;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 360;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.velocity.X *= 0.9f;
            Projectile.velocity.Y *= 0.99f;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
        }
    }
}
