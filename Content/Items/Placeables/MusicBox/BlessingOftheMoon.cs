using CalamityInheritance.Tiles.MusicBox;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Placeables.MusicBox
{
    public class BlessingOftheMoon : CIPlaceable, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Local}.MusicBox";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/BlessingoftheMoon"), ItemType<BlessingOftheMoon>(), TileType<BlessingOftheMoonTile>());
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(TileType<BlessingOftheMoonTile>(), 0);
        }
    }
}
