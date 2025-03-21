using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Potions.CIPotions
{
    public abstract class CIPotion: ModItem
    {
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BuffPotion;
        }
    }
}