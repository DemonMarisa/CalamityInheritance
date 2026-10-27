using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.TarragonLegacy
{
    [AutoloadEquip(EquipType.Legs)]
    public class TarragonLeglegacy : CIArmor
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.defense = 32;
            Item.rare = ModContent.RarityType<BlueGreen>();
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<GenericDamageClass>() += 0.08f;
            player.GetCritChance<GenericDamageClass>() += 8;
            player.moveSpeed += 0.1f;
        }
    }
}
