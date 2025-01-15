using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Tiles.MusicBox;

namespace CalamityInheritance.Content.Items.Placeables.MusicBox
{
    public class TyrantPart1 : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Placeables.MusicBox";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false; // music boxes can't get prefixes in vanilla
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox; // recorded music boxes transform into the basic form in shimmer
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/Tyrant"), ModContent.ItemType<TyrantPart1>(), ModContent.TileType<TyrantPart1Tile>());
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(ModContent.TileType<TyrantPart1Tile>(), 0);
        }
    }
}
