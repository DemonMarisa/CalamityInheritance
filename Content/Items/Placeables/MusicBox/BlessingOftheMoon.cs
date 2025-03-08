using CalamityInheritance.Tiles.MusicBox;
using CalamityMod.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Placeables.MusicBox
{
    public class BlessingOftheMoon : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Placeables.MusicBox";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/BlessingoftheMoon"), ModContent.ItemType<BlessingOftheMoon>(), ModContent.TileType<BlessingOftheMoonTile>());
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(ModContent.TileType<BlessingOftheMoonTile>(), 0);
        }
    }
}
