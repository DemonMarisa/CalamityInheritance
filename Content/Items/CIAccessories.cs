using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items
{
    public abstract class CIAccessories: ModItem
    {
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Accessories;
        }
    }
}