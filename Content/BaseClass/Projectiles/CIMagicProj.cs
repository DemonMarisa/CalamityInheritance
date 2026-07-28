using CalamityInheritance.Core.Path;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Projectiles
{
    public abstract class CIMagicProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => $"{LocalizationPath.MagicProj}";
    }
}
