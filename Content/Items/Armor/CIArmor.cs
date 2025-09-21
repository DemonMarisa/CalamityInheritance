using CalamityMod.Skies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor
{
    public abstract class CIArmor: ModItem
    {
        /// <summary>
        /// 别写反了，0为头盔，1为胸甲，2为护腿
        /// </summary>
        public virtual ModItem[] ArmorBags { get; }
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ExSSD();
        }
        public virtual void ExSSD() { }
        public bool ShouldFullSet(Player player) => player.armor[0].type == ArmorBags[0].Type && player.armor[1].type == ArmorBags[1].Type && player.armor[2].type == ArmorBags[2].Type;
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Headgear;
        }
    }
}