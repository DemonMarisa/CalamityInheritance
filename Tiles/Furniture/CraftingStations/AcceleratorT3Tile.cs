using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Utilities;
using BotanicPlanter = CalamityMod.Tiles.Furniture.CraftingStations.BotanicPlanter;
using ProfanedCrucible = CalamityMod.Tiles.Furniture.CraftingStations.ProfanedCrucible;
using DraedonsForge = CalamityMod.Tiles.Furniture.CraftingStations.DraedonsForge;

namespace CalamityInheritance.Tiles.Furniture.CraftingStations
{
    public class AcceleratorT3Tile: ModTile
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
            AddMapEntry(Color.Gold, CIFunction.GetText("Tiles.AcceleratorT3"));
            TileID.Sets.DisableSmartCursor[Type] = true;
            AdjTiles =
            [
                ModContent.TileType<AcceleratorT2Tile>(),
                ModContent.TileType<ProfanedCrucible>(),
                ModContent.TileType<BotanicPlanter>(),
                ModContent.TileType<SilvaBasin>(),
                ModContent.TileType<DraedonsForge>(),
                ModContent.TileType<CosmicAnvil>(),
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
