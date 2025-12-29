using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityInheritance.Tiles.MusicBox;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.Placeables.MusicBox
{
	public class NowStopAskingWhere: CIPlaceable, ILocalizedModType
	{
        public new string LocalizationCategory => $"{Local}.MusicBox";
        public override void SetStaticDefaults() {
			ItemID.Sets.CanGetPrefixes[Type] = false;
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/NowStopAskingWhere"), ItemType<NowStopAskingWhere>(), TileType<NowStopAskingWhereBox>());
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToMusicBox(TileType<NowStopAskingWhereBox>(), 0);
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
