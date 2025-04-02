using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.System
{
    public class PlanetoidsCounts : ModSystem
    {
        public static bool Planetoids;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            Planetoids = (tileCounts[TileID.Dirt] > 20 || tileCounts[TileID.Mud] > 20 || tileCounts[TileID.Stone] > 20) && tileCounts[TileID.Cloud] < 2;
        }
    }
}
