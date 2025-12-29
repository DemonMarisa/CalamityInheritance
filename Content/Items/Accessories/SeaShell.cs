using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class SeaShell : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:44,
            itemHeight:50,
            itemRare:ItemRarityID.Green,
            itemValue:CIShopValue.RarityPriceGreen
        );
        public override void ExSSD() => CIFunction.ShimmerTo<SeaShell>(ItemID.Seashell);
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
