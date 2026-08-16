using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Movement
{
    public class AeroStoneLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Movement;
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 8));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 48;
            Item.height = 50;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().AeroStonePower = true;
            player.jumpSpeedBoost += 0.2f;
            player.moveSpeed += 0.1f;
            player.wingTime += 0.1f;
        }
    }
}
