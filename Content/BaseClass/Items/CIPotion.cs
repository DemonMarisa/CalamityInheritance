using CalamityInheritance.Core.Path;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Items
{
    public abstract class CIPotion : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => LocalizationPath.Potion;
    }
}
