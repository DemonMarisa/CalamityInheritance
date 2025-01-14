using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityInheritance.Tiles.MusicBox;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.MusicBox
{
	public class NowStopAskingWhere: ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.CanGetPrefixes[Type] = false;
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/NowStopAskingWhere"), ModContent.ItemType<NowStopAskingWhere>(), ModContent.TileType<NowStopAskingWhereBox>());
		}

		public override void SetDefaults() {
			Item.DefaultToMusicBox(ModContent.TileType<NowStopAskingWhereBox>(), 0);
		}

        public override void AddRecipes()
        {
			CreateRecipe().
				AddIngredient<EssenceofHavoc>(10).
				AddIngredient<AshesofCalamity>(10).
				AddIngredient<AuricBar>(5).
				AddTile<CosmicAnvil>().
				Register();
			CreateRecipe().
				AddIngredient<EssenceofHavoc>(10).
				AddIngredient<AshesofCalamity>(10).
				AddIngredient<AuricBarold>(1).
				AddTile<CosmicAnvil>().
				Register();
        }
    }
}
