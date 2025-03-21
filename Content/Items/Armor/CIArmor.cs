using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor
{
    public abstract class CIArmor: ModItem
    {
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Headgear;
        }
    }
}