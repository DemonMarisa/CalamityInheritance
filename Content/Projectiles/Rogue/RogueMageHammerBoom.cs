using CalamityMod;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueMageHammerBoom : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 500;
            Projectile.height = 500;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }
    }
}
