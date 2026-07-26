using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        /// <summary>
        /// Determines the distance required before a ray in a given direction from a given starting position hits solid tiles, taking slopes into account.
        /// </summary>
        /// <param name="start">The point to check from.</param>
        /// <param name="rotation">The direction in which tiles are checked.</param>
        /// <param name="length">How far in the direction that will be checked.</param>
        /// <param name="step">How many units moved forward each loop. Greater = less precise.</param>
        /// <returns>The length until the first collision detected. Returns input length if no collision occurs.</returns>
        public static float PreciseDistanceToTileCollisionHit(Vector2 start, float rotation, float length, float step = 1)
        {
            Vector2 unitVect = rotation.ToRotationVector2();
            Vector2 end = unitVect * length;

            if (length < 1f)
            {
                Point endWorldPos = end.ToTileCoordinates();
                return ParanoidTileRetrieval(endWorldPos.X, endWorldPos.Y).IsTileSolid() ? 0 : length;
            }

            Vector2 currentPos = start;
            Point lastAirPos = new Point(-1, -1);
            for (float i = 0; i < length; i += step)
            {
                currentPos += unitVect * step;

                Point tilePos = currentPos.ToTileCoordinates();

                if (tilePos == lastAirPos)
                    continue;

                if (!WorldGen.InWorld(tilePos.X, tilePos.Y))
                    continue;

                Tile tile = Main.tile[tilePos.X, tilePos.Y];
                if (!tile.IsTileSolid())
                {
                    lastAirPos = tilePos;
                    continue;
                }

                if (tile.Slope == SlopeType.Solid && !tile.IsHalfBlock)
                    return (currentPos - start).Length();

                Vector2 tileWorldPos = new Vector2(tilePos.X * 16, tilePos.Y * 16);
                Vector2 currentPosInTile = currentPos - tileWorldPos;
                if (tile.IsHalfBlock)
                {
                    if (currentPosInTile.Y >= 8f)
                        return (currentPos - start).Length();
                }
                else if (tile.Slope == SlopeType.SlopeDownLeft)
                {
                    if (currentPosInTile.X <= currentPosInTile.Y)
                        return (currentPos - start).Length();
                }
                else if (tile.Slope == SlopeType.SlopeDownRight)
                {
                    if ((16 - currentPosInTile.X) <= currentPosInTile.Y)
                        return (currentPos - start).Length();
                }
                else if (tile.Slope == SlopeType.SlopeUpLeft)
                {
                    if (currentPosInTile.X <= (16 - currentPosInTile.Y))
                        return (currentPos - start).Length();
                }
                else if (tile.Slope == SlopeType.SlopeUpRight)
                {
                    if (currentPosInTile.X >= currentPosInTile.Y)
                        return (currentPos - start).Length();
                }
            }
            return length;
        }
    }
}
