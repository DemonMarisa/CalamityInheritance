using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Materials
{
    public abstract class CIMaterials: ModItem
    {
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Material;
        }
    }
}