using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityInheritance.Tiles.MusicBox;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.Content.Items.Potions;

namespace CalamityInheritance.Content.Items.Placeables.MusicBox
{
	public class Kunoji: CIPlaceable, ILocalizedModType
	{
        public new string LocalizationCategory => $"{Local}.MusicBox";
        public override void SetStaticDefaults() {
			ItemID.Sets.CanGetPrefixes[Type] = false;
			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/Kunojihousing"), ModContent.ItemType<Kunoji>(), ModContent.TileType<KunojiTile>());
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToMusicBox(ModContent.TileType<KunojiTile>(), 0);
			Item.rare = ModContent.RarityType<IchikaBlack>();
		}

        public override void AddRecipes()
        {
			CreateRecipe().
				AddIngredient(ItemID.CoffeeCup, 3).
				AddIngredient<Bread>(3).
				AddIngredient(ItemID.FoodPlatter, 3).
				AddRecipeGroup("AnyGoldBar", 9).
				AddTile(TileID.Tables).
				Register();
        }
    }
}
