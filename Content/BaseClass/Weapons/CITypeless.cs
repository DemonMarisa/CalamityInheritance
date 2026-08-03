using CalamityInheritance.Core.Path;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Weapons
{
    public abstract class CITypeless : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => $"{LocalizationPath.TypelessProj}";
    }
}
