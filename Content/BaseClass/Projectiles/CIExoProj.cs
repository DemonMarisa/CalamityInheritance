using CalamityInheritance.Core.Path;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Projectiles
{
    public abstract class CIExoProj : ModProjectile, ILocalizedModType
    {
        public override string LocalizationCategory => LocalizationPath.ExoProj;
    }
}
