using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace CalamityInheritance.Tiles.Furniture.CraftingStations
{
    public class DraedonsForgeold : ModTile
    {
        public new string LocalizationCategory => "Tiles.Furniture.CraftingStations";
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
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(0, 255, 0), name);
            TileID.Sets.DisableSmartCursor[Type] = true;
            AdjTiles = new int[] { TileID.Anvils, TileID.Furnaces, TileID.WorkBenches, TileID.LunarCraftingStation,
                TileID.MythrilAnvil, TileID.AdamantiteForge, TileID.Hellforge, TileID.DemonAltar, ModContent.TileType<DraedonsForge>(), ModContent.TileType<CosmicAnvil>() };
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = (float)Main.DiscoR / 255f;
            g = (float)Main.DiscoG / 255f;
            b = (float)Main.DiscoB / 255f;
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
