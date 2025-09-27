using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityInheritance.Core.Enums;

namespace CalamityInheritance.Content.Items.Armor
{
    public abstract class CIArmor: ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ExSSD();
        }
        public virtual void ExSSD() { }
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Headgear;
        }
    }
}