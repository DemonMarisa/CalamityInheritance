using CalamityInheritance.Content.BaseClass.Items;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeLeviathanAnahita : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Lime;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                AddEffect(player);
            }
        }
        public static void AddEffect(Player player)
        {
            player.CI().AddEffect("KnowledgeLeviathanAnahita", () =>
            {
                if (!player.IsUnderwater())
                {
                    player.statDefense -= 8;
                    player.endurance -= 0.05f;
                }
                else if (player.IsUnderwater())
                {
                    player.statLifeMax2 += player.statLifeMax2 / 20;
                    if (player.miscCounter % 10 == 0)
                    {
                        int offset = 30;
                        int playerSpriteX = (int)player.Center.X / 16;
                        int playerSpriteY = (int)player.Center.Y / 16;

                        for (int i = playerSpriteX - offset; i <= playerSpriteX + offset; i++)
                        {
                            for (int j = playerSpriteY - offset; j <= playerSpriteY + offset; j++)
                            {
                                if (Main.rand.NextBool(4))
                                {
                                    Vector2 vector = new(playerSpriteX - i, playerSpriteY - j);
                                    if (vector.Length() < offset && i > 0 && i < Main.maxTilesX - 1 && j > 0 && j < Main.maxTilesY - 1 && Main.tile[i, j] != null && Main.tile[i, j].HasTile)
                                    {
                                        bool ifSubm = false;
                                        if (Main.tile[i, j].TileType == 185 && Main.tile[i, j].TileFrameY == 18)
                                        {
                                            if (Main.tile[i, j].TileFrameX >= 576 && Main.tile[i, j].TileFrameX <= 882)
                                                ifSubm = true;
                                        }
                                        else if (Main.tile[i, j].TileType == 186 && Main.tile[i, j].TileFrameX >= 864 && Main.tile[i, j].TileFrameX <= 1170)
                                            ifSubm = true;

                                        if (ifSubm || Main.tileSpelunker[Main.tile[i, j].TileType] || (Main.tileAlch[Main.tile[i, j].TileType] && Main.tile[i, j].TileType != 82))
                                        {
                                            int dType = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.TreasureSparkle, 0f, 0f, 150, default, 0.3f);
                                            Main.dust[dType].fadeIn = 0.75f;
                                            Main.dust[dType].velocity *= 0.1f;
                                            Main.dust[dType].noLight = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }
    }
}
