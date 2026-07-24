using CalamityInheritance.Core.Utils;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CalamityModCross
{
    public class CalamityTile : ModSystem
    {
        public static int DraedonsForgeTile;// 嘉登熔炉
        public override void OnModLoad()
        {
            if (CIUtils.HasCalamity())
            {
                GetCalamityTileID();
            }
        }

        [JITWhenModsEnabled("CalamityMod")]
        public static void GetCalamityTileID()
        {
            DraedonsForgeTile = TileType<DraedonsForge>();
        }
    }
}
