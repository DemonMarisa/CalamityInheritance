using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DraedonsForge = CalamityMod.Tiles.Furniture.CraftingStations.DraedonsForge;

namespace CalamityInheritance.Content.Items.Placeables.Furniture.CraftingStations
{
    public class DemonshadeWorkbench: CIPlaceable, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Local}.Furniture.CraftingStations";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 50;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = CIShopValue.RarityPricePureRed;
            Item.rare = CIConfig.Instance.SpecialRarityColor? ModContent.RarityType<PlantareGreen>() : ModContent.RarityType<PureRed>();
            Item.createTile = ModContent.TileType<DemonshadeTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WorkBench, 1).
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}