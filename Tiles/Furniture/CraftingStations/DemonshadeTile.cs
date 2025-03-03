using System.Drawing;
using CalamityInheritance.Utilities;
using CalamityMod.Projectiles.Summon;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;
using Terraria.ID;
using CalamityInheritance.Content.Items.Placeables.Furniture.CraftingStations;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace CalamityInheritance.Tiles.Furniture.CraftingStations
{
    public class DemonshadeTile: ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
            TileObjectData.newTile.DrawFlipHorizontal = false;
            TileObjectData.newTile.DrawFlipVertical = false;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16, 18
            };
            TileObjectData.addTile(Type);
            CreateMapEntryName();
            AddMapEntry(Color.Purple, CIFunction.GetText("Tiles.DemonshadeTile"));
            TileID.Sets.DisableSmartCursor[Type] = true;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = Main.DiscoR / 255f;
            g = Main.DiscoG / 255f;
            b = Main.DiscoB / 255f;
        }
        // public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        // {
		// 	Tile tile = Main.tile[i, j];
		// 	if ((tile.TileFrameX == 0 || tile.TileFrameX == 72) && tile.TileFrameY == 0) 
        //     {
        //         Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Tiles/Furniture/CraftingStations/DemonshadeTileFire");
        //         Rectangle dRect2 = new Rectangle((int)(i * 16 + 192 - Main.screenPosition.X), (int)(j * 16 + 176 - Main.screenPosition.Y), texture.Width, texture.Height);
        //         spriteBatch.Draw(texture, dRect2, Color.White);
        //     }
        //     return true;
        // }
    }
}