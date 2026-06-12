using CalamityInheritance.Rarity.Special;
using CalamityInheritance.Tiles.MusicBox;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Placeables.MusicBox
{
    public class Arcueid : CIPlaceable, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Local}.MusicBox";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/MoonPrincess"), ItemType<Arcueid>(), TileType<ArcueidTile>());
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(TileType<ArcueidTile>(), 0);
            Item.rare = RarityType<ArcueidColor>();
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
