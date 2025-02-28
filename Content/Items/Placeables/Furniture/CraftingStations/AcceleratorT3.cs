using CalamityInheritance.Rarity;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Placeables.Furniture.CraftingStations
{
    public class AcceleratorT3: ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Placeables.Furniture.CraftingStations";

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
            Item.value = CIShopValue.RarityPricePureRed;
            Item.createTile = ModContent.TileType<AcceleratorT3Tile>();
            Item.rare = ModContent.RarityType<PureRed>();    
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AcceleratorT2>().
                AddIngredient<ProfanedCrucible>().
                AddIngredient<BotanicPlanter>().
                AddIngredient<EffulgentManipulator>().
                AddIngredient<AltarOfTheAccursedItem>().
                AddIngredient<DraedonsForge>().
                Register();
        }
    }
}