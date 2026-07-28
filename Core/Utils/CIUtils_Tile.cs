using Microsoft.Xna.Framework;
using Terraria;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        public static Tile ParanoidTileRetrieval(int x, int y)
        {
            if (!WorldGen.InWorld(x, y))
                return new Tile();

            return Main.tile[x, y];
        }
        public static bool IsTileSolid(this Tile tile) => tile != null && tile.HasUnactuatedTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType];
        // 在坐标 (x, y) 处引发半径 5 的爆炸，仅摧毁矩形 [x-10, x+10] × [y-10, y+10] 内的物块，且不破坏墙体
        // ExplodeTiles(new Vector2(x, y), 5, x / 16 - 10, x / 16 + 10, y / 16 - 10, y / 16 + 10, false);
        public static void CIExplodeTiles(this Projectile proj, Vector2 pos, int radius, bool wall = true)
        {
            proj.ExplodeTiles(pos, radius,
                (int)(proj.Center.X / 16 - radius),
                (int)(proj.Center.X / 16 + radius),
                (int)(proj.Center.Y / 16 + radius),
                (int)(proj.Center.Y / 16 + radius),
                wall);
        }
    }
}
