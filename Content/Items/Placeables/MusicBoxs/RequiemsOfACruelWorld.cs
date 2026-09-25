using CalamityInheritance.Content.Tiles.MusicBoxs;
using CalamityInheritance.Core.Path;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Placeables.MusicBoxs
{
    public class RequiemsOfACruelWorld : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => LocalizationPath.MusicBoxs;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false; // music boxes can't get prefixes in vanilla
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox; // recorded music boxes transform into the basic form in shimmer
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/RequiemsOfACruelWorld"), ItemType<RequiemsOfACruelWorld>(), TileType<RequiemsOfACruelWorldTile>());
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(TileType<RequiemsOfACruelWorldTile>(), 0);
        }
    }
}
