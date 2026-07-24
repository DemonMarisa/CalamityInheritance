using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Tiles;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Utils;
using LAP.Common.CalamityModCross;
using LAP.Content.RecipeGroupAdd;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.CraftingStations
{
    public class DraedonsForgeold : CIPlaceable
    {
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
            Item.createTile = TileType<DraedonsForgeoldTile>();
            Item.rare = RarityType<CatalystViolet>();
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                Recipe recipe = CreateRecipe();
                recipe.AddRecipeGroup(LAPRecipeGroup.AnyHardmodeAnvil);
                recipe.AddRecipeGroup(LAPRecipeGroup.AnyHardmodeForge);
                recipe.AddIngredient(ItemID.LunarCraftingStation);
                recipe.AddIngredient(ItemID.LunarBar, 5);
                recipe.AddIngredient(CalMaterialsID.CosmiliteBarID, 5);
                recipe.AddIngredient(CalamityMaterials.NightmareFuel, 20);
                recipe.AddIngredient(CalamityMaterials.EndothermicEnergy, 20);
                recipe.AddIngredient(CalamityMaterials.DarksunFragment, 20);
                recipe.Register();
            }
            else
            {
                Recipe recipe = CreateRecipe();
                recipe.AddRecipeGroup(LAPRecipeGroup.AnyHardmodeAnvil);
                recipe.AddRecipeGroup(LAPRecipeGroup.AnyHardmodeForge);
                recipe.AddIngredient(ItemID.LunarCraftingStation);
                recipe.AddIngredient(ItemID.LunarBar, 5);
                recipe.Register();
            }
        }
    }
}
