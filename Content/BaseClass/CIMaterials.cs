using CalamityInheritance.Core.Path;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass
{
    public abstract class CIMaterials : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.CIMaterials;
    }
}
