using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityInheritance.Tiles.MusicBox;

namespace CalamityInheritance.Content.Items.Placeables.MusicBox
{
	public class ProvidenceLegacy: CIPlaceable, ILocalizedModType
	{
        public new string LocalizationCategory => $"{Local}.MusicBox";
        public override void SetStaticDefaults() {
			ItemID.Sets.CanGetPrefixes[Type] = false;
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/ProvidenceLegacy"), ItemType<ProvidenceLegacy>(), TileType<ProvidenceLegacyBox>());
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToMusicBox(TileType<ProvidenceLegacyBox>(), 0);
		}

        public override void AddRecipes()
        {
			CreateRecipe().
				AddIngredient<GalacticaSingularity>(10).	
				AddIngredient(ItemID.LunarBar, 10).
				AddTile(TileID.LunarCraftingStation).
				Register();
        }
    }
}
