using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityInheritance.Tiles.MusicBox;

namespace CalamityInheritance.Content.Items.Placeables.MusicBox
{
	public class DoGNonStop: ModItem, ILocalizedModType
	{
        public new string LocalizationCategory => "Content.Items.Placeables.MusicBox";
        public override void SetStaticDefaults() {
			ItemID.Sets.CanGetPrefixes[Type] = false;
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/DoGNonstopRemix"), ModContent.ItemType<DoGNonStop>(), ModContent.TileType<DoGNonStopBox>());
		}

		public override void SetDefaults() {
			Item.DefaultToMusicBox(ModContent.TileType<DoGNonStopBox>(), 0);
		}

        public override void AddRecipes()
        {
			CreateRecipe().
			 	AddIngredient<GalacticaSingularity>(5).
				AddIngredient<DivineGeode>(5).
				AddIngredient<RuinousSoul>(5).
				AddTile(TileID.LunarCraftingStation).
				Register();
        }
    }
}
