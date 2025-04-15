using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityInheritance.Tiles.MusicBox;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity.Special;

namespace CalamityInheritance.Content.Items.Placeables.MusicBox
{
	public class Arcueid: CIPlaceable, ILocalizedModType
	{
        public new string LocalizationCategory => $"{Local}.MusicBox";
        public override void SetStaticDefaults() {
			ItemID.Sets.CanGetPrefixes[Type] = false;
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/MoonPrincess"), ModContent.ItemType<Arcueid>(), ModContent.TileType<ArcueidTile>());
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToMusicBox(ModContent.TileType<ArcueidTile>(), 0);
			Item.rare = ModContent.RarityType<ArcueidColor>();
		}

        public override void AddRecipes()
        {
			CreateRecipe().
			 	AddIngredient(ItemID.FallenStar, 5).
			 	AddIngredient(ItemID.SunplateBlock, 5).
				AddTile(TileID.SkyMill).
				Register();
        }
    }
}
