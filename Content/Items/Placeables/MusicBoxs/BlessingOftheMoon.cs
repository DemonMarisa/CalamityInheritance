using CalamityInheritance.Content.Tiles.MusicBoxs;
using CalamityInheritance.Core.Path;
using CalamityInheritance.Music;
using LAP.Music;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Placeables.MusicBoxs
{
    public class BlessingOftheMoon : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => LocalizationPath.MusicBoxs;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, CIMusicRegister.BlessingoftheMoon), ItemType<BlessingOftheMoon>(), TileType<BlessingOftheMoonTile>());
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(TileType<BlessingOftheMoonTile>(), 0);
        }
    }
}
