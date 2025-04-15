using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Placeables.Furniture.CraftingStations
{
    public class DraedonsForgeold : CIPlaceable, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Local}.Furniture.CraftingStations";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.createTile = ModContent.TileType<Tiles.Furniture.CraftingStations.DraedonsForgeold>();
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("HardmodeForge");
            recipe.AddRecipeGroup("HardmodeAnvil");
            recipe.AddIngredient(ItemID.LunarCraftingStation);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<NightmareFuel>(), 20);
            recipe.AddIngredient(ModContent.ItemType<EndothermicEnergy>(), 20);
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 20);
            recipe.Register();
        }
    }
}
