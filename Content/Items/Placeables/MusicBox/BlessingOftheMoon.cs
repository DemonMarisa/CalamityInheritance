using CalamityInheritance.Tiles.MusicBox;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Placeables.MusicBox
{
    public class BlessingOftheMoon : CIPlaceable, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Placeables.MusicBox";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/BlessingoftheMoon"), ModContent.ItemType<BlessingOftheMoon>(), ModContent.TileType<BlessingOftheMoonTile>());
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(ModContent.TileType<BlessingOftheMoonTile>(), 0);
        }
    }
}
