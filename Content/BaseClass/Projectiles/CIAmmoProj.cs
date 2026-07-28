using CalamityInheritance.Core.Path;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Projectiles
{
    public abstract class CIAmmoProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => $"{LocalizationPath.AmmoProj}";
    }
}
