using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class MeleeTypeVictideBoomerangProj: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 45;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            float rSpeed = 16f;
            float accle = 1.5f;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 30f)
            {
                CIFunction.BoomerangReturningAI(owner, Projectile, rSpeed, accle);
                if (Projectile.Hitbox.Intersects(owner.Hitbox))
                    Projectile.Kill();
            }
            Projectile.rotation -= 0.22f;
        }
    }
}
