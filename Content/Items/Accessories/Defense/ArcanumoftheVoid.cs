using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;

namespace CalamityInheritance.Content.Items.Accessories.Defense
{
    public class ArcanumoftheVoid : CIAccessories
    {
        public override int AccessoriesStyle => Defense;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 26;
            Item.height = 26;
            Item.rare = RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.defense = 12;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().projRef = true;
        }
    }
}
