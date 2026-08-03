using CalamityInheritance.Core.Path;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Items
{
    public abstract class CIAmmo : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => LocalizationPath.AmmoItem;
    }
}
