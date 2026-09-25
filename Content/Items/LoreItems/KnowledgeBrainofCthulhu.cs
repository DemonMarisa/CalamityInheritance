using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Core.Keys;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeBrainofCthulhu : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
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
            player.CI().AddEffect("KnowledgeBrainofCthulhu", () =>
            {
                if (CIKeybinds.BoCLoreTeleportation.JustPressed && Main.myPlayer == player.whoAmI)
                {
                    if (!player.chaosState)
                    {
                        Vector2 vector31;
                        vector31.X = Main.mouseX + Main.screenPosition.X;
                        if (player.gravDir == 1f)
                        {
                            vector31.Y = Main.mouseY + Main.screenPosition.Y - player.height;
                        }
                        else
                        {
                            vector31.Y = Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
                        }
                        vector31.X -= player.width / 2;
                        if (vector31.X > 50f && vector31.X < Main.maxTilesX * 16 - 50 && vector31.Y > 50f && vector31.Y < Main.maxTilesY * 16 - 50)
                        {
                            int tileX = (int)(vector31.X / 16f);
                            int tileY = (int)(vector31.Y / 16f);
                            if ((Main.tile[tileX, tileY].WallType != WallID.LihzahrdBrickUnsafe || tileY <= Main.worldSurface || NPC.downedPlantBoss) && !Collision.SolidCollision(vector31, player.width, player.height))
                            {
                                player.Teleport(vector31, 1, 0);
                                NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, vector31.X, vector31.Y, 1, 0, 0);
                                player.AddBuff(BuffID.ChaosState, 300, true);
                                player.AddBuff(BuffID.Confused, 150, true);
                            }
                        }
                    }
                }
            });
        }
    }
}
