using CalamityInheritance.Core.Path;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Tiles
{
    public abstract class CIPlaceable : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.CICraftingStation;
    }
}
