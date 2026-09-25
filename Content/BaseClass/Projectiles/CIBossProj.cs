using CalamityInheritance.Core.Path;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Projectiles
{
    public abstract class CIBossProj : ModProjectile, ILocalizedModType
    {
        public override string LocalizationCategory => LocalizationPath.BossProj;
    }
}
