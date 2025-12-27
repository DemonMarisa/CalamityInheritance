using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Placeables.FurnitureAncient;
using CalamityMod.Items.Placeables.FurnitureMonolith;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Placeables.Furniture.CraftingStations
{
    public class AcceleratorT2: CIPlaceable, ILocalizedModType
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
            Item.value = CIShopValue.RarityPricePink;
            Item.createTile = ModContent.TileType<AcceleratorT2Tile>();
            Item.rare = ItemRarityID.Pink;    
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AcceleratorT1>().
                AddIngredient<AncientAltar>().
                AddIngredient<AshenAltar>().
                AddIngredient<MonolithAmalgam>().
                AddIngredient<PlagueInfuser>().
                AddIngredient<VoidCondenser>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}