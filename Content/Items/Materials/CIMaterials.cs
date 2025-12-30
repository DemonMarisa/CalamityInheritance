using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Materials
{
    public abstract class CIMaterials: ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Materials";
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Material;
        }
    }
}