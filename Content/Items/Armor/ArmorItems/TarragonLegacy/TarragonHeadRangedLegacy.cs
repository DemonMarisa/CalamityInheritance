using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.TarragonLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class TarragonHeadRangedLegacy : CIArmor
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 21; //110
            Item.rare = RarityType<BlueGreen>();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<TarragonBreastplateLegacy>() && legs.type == ItemType<TarragonLeglegacy>();
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }
        public override void UpdateArmorSet(Player player)
        {
            player.CI().TarragonSet = true;
            player.CI().TarragonRanged = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<RangedDamageClass>() += 0.1f;
            player.GetCritChance<RangedDamageClass>() += 10;
            player.AddDR(0.05f);
        }
    }
}
