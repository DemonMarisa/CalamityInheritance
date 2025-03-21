using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.MiscItem
{
    public abstract class CIMisc:ModItem
    {
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.EverythingElse;
        }
    }
}