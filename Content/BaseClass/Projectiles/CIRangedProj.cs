using CalamityInheritance.Core.Path;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Projectiles
{
    public abstract class CIRangedProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.RangedProj;
        public Player Owner => Main.player[Projectile.owner];
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            ExSD();
        }
        public virtual void ExSD()
        {

        }
    }
}
