using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Restorative
{
    public class RegenatorLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Restorative;
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 32;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 10;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().RegenatorLegacy = true;
        }
    }
}
