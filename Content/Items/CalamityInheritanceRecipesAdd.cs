using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityMod.Projectiles.Pets;
using CalamityMod.Items.DraedonMisc;

namespace CalamityInheritance.Content.Items
{
    //Recipe recipe = CreateRecipe();

    //recipe.AddIngredient(ModContent.ItemType<TrueNightsStabber>());
    //recipe.AddIngredient(ItemID.BrokenHeroSword);

    //recipe.AddTile(TileID.MythrilAnvil);
    //recipe.AddTile(TileID.DemonAltar);
    //recipe.AddTile(ModContent.TileType<DraedonsForge>());
    //recipe.AddTile(ModContent.TileType<CosmicAnvil>());

    //recipe.Register();
    public class CalamityInheritanceRecipesAdd : ModSystem
    {
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<ArkoftheCosmos>()).
                AddIngredient(ModContent.ItemType<ArkoftheCosmosold>()).
                Register();
            Recipe.Create(ModContent.ItemType<FourSeasonsGalaxia>()).
                AddIngredient(ModContent.ItemType<FourSeasonsGalaxiaold>()).
                Register();
            Recipe.Create(ModContent.ItemType<Exoblade>()).
                AddIngredient(ModContent.ItemType<Exobladeold>()).
                Register();
            Recipe.Create(ModContent.ItemType<Murasama>()).
                AddIngredient(ModContent.ItemType<MurasamaNeweffect>()).
                Register();

            Recipe.Create(ModContent.ItemType<ElementalShiv>()).
                AddIngredient(ModContent.ItemType<TerraShiv>()).
                AddIngredient(ModContent.ItemType<GalacticaSingularity>()).
                AddIngredient(ItemID.LunarBar).
                AddTile(TileID.AncientMythrilBrick).
                Register();

            Recipe.Create(ModContent.ItemType<Terratomere>()).
                AddIngredient(ModContent.ItemType<TerraEdge>()).
                AddIngredient(ModContent.ItemType<Hellkite>()).
                AddIngredient(ModContent.ItemType<UelibloomBar>()).
                AddIngredient(ModContent.ItemType<Floodtide>()).
                AddIngredient(ItemID.LunarBar).
                AddTile(TileID.AncientMythrilBrick).
                Register();

            Recipe.Create(ModContent.ItemType<Exoblade>()).
                AddIngredient(ModContent.ItemType<Exobladeold>()).         
                Register();

            Recipe.Create(ItemID.TerraBlade).
                AddIngredient(ModContent.ItemType<TrueBloodyEdge>()).
                AddIngredient(ItemID.TrueExcalibur).
                AddIngredient(ModContent.ItemType<LivingShard>(),7).
                AddTile(TileID.MythrilAnvil).
                Register();

            Recipe.Create(ModContent.ItemType<DraedonPowerCell>(),333).
                AddIngredient(ModContent.ItemType<DubiousPlating>(),2).
                AddIngredient(ModContent.ItemType<MysteriousCircuitry>()).
                AddIngredient(ItemID.CopperBar).
                Register();
        }
    }
}
