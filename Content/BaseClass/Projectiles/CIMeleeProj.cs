using CalamityInheritance.Core.Path;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Projectiles
{
    public abstract class CIMeleeProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => $"{LocalizationPath.MeleeProj}";
    }
}
