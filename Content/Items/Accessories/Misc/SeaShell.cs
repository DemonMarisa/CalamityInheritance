using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Misc
{
    public class SeaShell : CIAccessories, ILocalizedModType
    {
        public override int AccessoriesStyle => Misc;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 44;
            Item.height = 50;
            Item.rare = ItemRarityID.Green;
            Item.value = CIShopValue.RarityPriceGreen;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ignoreWater = true;
            if (player.IsUnderwater())
            {
                player.statDefense += 3;
                player.endurance += 0.05f;
                player.moveSpeed += 0.1f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Seashell, 5).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
