using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using AshenAltar = CalamityMod.Tiles.Furniture.CraftingStations.AshenAltar;
using AncientAltar = CalamityMod.Tiles.Furniture.CraftingStations.AncientAltar;
using MonolithAmalgam = CalamityMod.Tiles.Furniture.CraftingStations.MonolithAmalgam;
using PlagueInfuser = CalamityMod.Tiles.Furniture.CraftingStations.PlagueInfuser;
using VoidCondenser = CalamityMod.Tiles.Furniture.CraftingStations.VoidCondenser;

namespace CalamityInheritance.Tiles.Furniture.CraftingStations
{
    public class AcceleratorT2Tile: ModTile
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
            TileObjectData.newTile.CoordinateHeights = [16, 18];
            TileObjectData.addTile(Type);
            CreateMapEntryName();
            AddMapEntry(Color.Gold, CIFunction.GetText("Tiles.AcceleratorT2"));
            TileID.Sets.DisableSmartCursor[Type] = true;
            AdjTiles =
            [
                ModContent.TileType<AshenAltar>(),
                ModContent.TileType<AncientAltar>(),
                ModContent.TileType<MonolithAmalgam>(),
                ModContent.TileType<PlagueInfuser>(),
                ModContent.TileType<VoidCondenser>(),
                ModContent.TileType<AcceleratorT1Tile>()
            ];
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
