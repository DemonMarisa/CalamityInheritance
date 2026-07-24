using CalamityInheritance.Core.Utils;
using CalamityMod.Tiles.Furniture.CraftingStations;
using LAP.Common.CalamityModCross;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalamityInheritance.Content.Tiles.CraftingStations
{
    public class DraedonsForgeoldTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            AnimationFrameHeight = 36;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.addTile(Type);
            AddMapEntry(Color.Gold);
            TileID.Sets.DisableSmartCursor[Type] = true;
            if (CIUtils.HasCalamity())
            {
                AdjTiles = new int[] { TileID.Anvils, TileID.Furnaces, TileID.WorkBenches, TileID.LunarCraftingStation,
                TileID.MythrilAnvil, TileID.AdamantiteForge, TileID.Hellforge, TileID.DemonAltar, CalTileID.CosmicAnvilID, CalTileID.DraedonsForgeID };
            }
            else
            {
                AdjTiles = new int[] { TileID.Anvils, TileID.Furnaces, TileID.WorkBenches, TileID.LunarCraftingStation,
                TileID.MythrilAnvil, TileID.AdamantiteForge, TileID.Hellforge, TileID.DemonAltar };
            }
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = Main.DiscoR / 255f;
            g = Main.DiscoG / 255f;
            b = Main.DiscoB / 255f;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter > 12)
            {
                frameCounter = 0;
                frame++;
                if (frame > 3)
                {
                    frame = 0;
                }
            }
        }
    }
}
