using CalamityMod.Skies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor
{
    public abstract class CIArmor: ModItem
    {
        /// <summary>
        /// ��д���ˣ�0Ϊͷ����1Ϊ�ؼף�2Ϊ����
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