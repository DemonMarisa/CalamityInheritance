using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityInheritance.Tiles.MusicBox;

namespace CalamityInheritance.Content.Items.Placeables.MusicBox
{
	public class ProvidenceLegacy: ModItem, ILocalizedModType
	{
        public new string LocalizationCategory => "Mods.CalamityInheritance.Content.Items.Placeables";
		public override void SetStaticDefaults() {
			ItemID.Sets.CanGetPrefixes[Type] = false;
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/ProvidenceLegacy"), ModContent.ItemType<ProvidenceLegacy>(), ModContent.TileType<ProvidenceLegacyBox>());
		}

		public override void SetDefaults() {
			Item.DefaultToMusicBox(ModContent.TileType<ProvidenceLegacyBox>(), 0);
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
