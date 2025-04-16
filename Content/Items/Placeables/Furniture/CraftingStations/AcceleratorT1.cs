using CalamityInheritance.Rarity;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Placeables.Furniture.CraftingStations
{
    public class AcceleratorT1: CIPlaceable, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Local}.Furniture.CraftingStations";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 34;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.createTile = ModContent.TileType<AcceleratorT1Tile>();
            Item.rare = ItemRarityID.Green;    
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<WulfrumLabstationItem>().
                AddIngredient<EutrophicShelf>().
                AddIngredient<StaticRefiner>().
                AddTile(TileID.Anvils).
                Register();
        }
    }
}