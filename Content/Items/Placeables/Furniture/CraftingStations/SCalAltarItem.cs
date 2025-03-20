using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Placeables;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.Placeables.Furniture.CraftingStations
{
    public class SCalAltarItem : CIPlaceable, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Placeables.Furniture.CraftingStations";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 32;
            Item.createTile = ModContent.TileType<SCalAltar>();
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ModContent.RarityType<Violet>();
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.CraftingObjects;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BrimstoneSlag>(30).
                AddIngredient<AuricBarold>(1).
                AddIngredient<CoreofCalamity>().
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient<BrimstoneSlag>(30).
                AddIngredient<AuricBar>(5).
                AddIngredient<CoreofCalamity>().
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
