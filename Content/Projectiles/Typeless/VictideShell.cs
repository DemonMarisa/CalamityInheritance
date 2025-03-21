using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Typeless
{
    public class VictideShell: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.aiStyle = 3;
            Projectile.timeLeft = 300;
            AIType = ProjectileID.WoodenBoomerang;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            if (Projectile.Calamity().stealthStrike)
            {
                Projectile.aiStyle = 0;
                Projectile.rotation += 0.10f;
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] > 10f)
                    CIFunction.HomeInOnNPC(Projectile, false, 400f , 14f, 20f);
            }
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0] += 0.1f;
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
