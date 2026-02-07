using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class AftershockRock : GeneralDamageProj
    {
        public override ProjDamageType UseDamageClass => ProjDamageType.Melee;

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.aiStyle = ProjAIStyleID.GroundProjectile;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.MaxUpdates = 3;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300 * Projectile.MaxUpdates;
            Projectile.ignoreWater = true;
            AIType = ProjectileID.BoulderStaffOfEarth;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10 * Projectile.MaxUpdates;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Projectile.Center.Y > Projectile.ai[2])
                Projectile.tileCollide = true;
            else
                Projectile.tileCollide = false;
        }
    }
}
