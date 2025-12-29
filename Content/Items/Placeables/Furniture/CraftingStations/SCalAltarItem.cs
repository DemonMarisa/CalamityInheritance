using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Items.Placeables.Crags;

namespace CalamityInheritance.Content.Items.Placeables.Furniture.CraftingStations
{
    public class SCalAltarItem : CIPlaceable, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Local}.Furniture.CraftingStations";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 32;
            Item.createTile = TileType<SCalAltar>();
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = RarityType<BurnishedAuric>();
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
