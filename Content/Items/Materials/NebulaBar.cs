using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.Bars;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Materials
{
    public class NebulaBar : CIMaterials
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(TileType<NebulaBarTile>());
            Item.value = CIShopValue.RarityPriceCyan;
            Item.rare = ItemRarityID.Cyan;
        }

        public override void AddRecipes()
        {
            CreateRecipe(3).
                AddIngredient(ItemID.HallowedBar, 1).
                AddIngredient(ItemID.Ectoplasm, 1).
                AddIngredient<GalacticaSingularity>().
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}