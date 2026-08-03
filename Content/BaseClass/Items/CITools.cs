using CalamityInheritance.Core.Path;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Items
{
    public abstract class CITools : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.Tool;
    }
}
