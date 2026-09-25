using CalamityInheritance.Assets;
using CalamityMod.World;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;

namespace CalamityInheritance.Core.UI
{
    public static class AstralArcanumUI
    {
        enum CircleStyle
        {
            Normal,
            Selected
        }
        private const int WindowBorder = 50;
        private const int CircleTextureSize = 40; //don't change unless textures change size
        private const int CircleOffset = 36;
        private static Vector2 CenterPoint;
        public static bool Open;
        private static int LastHovered;
        public static void Toggle()
        {
            Open = !Open;

            if (Open)
            {
                CenterPoint = new Vector2(Main.mouseX, Main.mouseY);
                //Clamp center so UI doesn't go off screen
                CenterPoint.X = MathHelper.Clamp(CenterPoint.X, WindowBorder, Main.screenWidth - WindowBorder);
                CenterPoint.Y = MathHelper.Clamp(CenterPoint.Y, WindowBorder, Main.screenHeight - WindowBorder);
            }
            else
            {
                //Closed, do stuff in here if need be
            }
        }

        public static void UpdateAndDraw(SpriteBatch sb)
        {
            // Don't do anything if not open.
            if (!Open)
                return;
            // Draw center circle
            DrawCircle(sb, CenterPoint, 0, CircleStyle.Normal);
            Vector2 centerToMouse = Main.MouseScreen - CenterPoint;
            float rotation = centerToMouse.ToRotation();
            // Draw the arrow that points towards the mouse.
            sb.Draw(CITextureRegister_UI.AstralArcanumCircles.Value, CenterPoint, new Rectangle(0, CircleTextureSize, 24, 10), Color.White, rotation, new Vector2(12, 5f), 1f, SpriteEffects.None, 0f);
            int current = 0;
            int selectedCircle = -1;
            int offset = CircleOffset * 2;
            int radius = CircleTextureSize / 2;
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    Vector2 center = new Vector2(
                        CenterPoint.X - CircleOffset + x * offset,
                        CenterPoint.Y - CircleOffset + y * offset);
                    Circle c = new Circle(center, radius);
                    CircleStyle style = CircleStyle.Normal;
                    // If the mouse is in the circle.
                    if (c.Contains(Main.MouseScreen))
                    {
                        style = CircleStyle.Selected;
                        Main.LocalPlayer.mouseInterface = true;
                        selectedCircle = current;
                        // If left clicked.
                        if ((Main.mouseLeft && Main.mouseLeftRelease))
                        {
                            Main.mouseLeftRelease = false;
                            Main.mouseLeft = false;
                            DoTeleportation(current);
                        }
                    }
                    DrawCircle(sb, center, 1 + current, style);
                    current++;
                }
            }
            // Literally so the sound plays.
            if (LastHovered != selectedCircle)
            {
                LastHovered = selectedCircle;
                if (LastHovered != -1)
                    SoundEngine.PlaySound(SoundID.MenuTick);
            }
            // Default to "Select"
            string text = Language.GetTextValue("LegacyMisc.53");
            switch (selectedCircle)
            {
                case 0:
                    text = Language.GetTextValue("Bestiary_Biomes.TheUnderworld");
                    break;
                case 1:
                    text = Language.GetTextValue("Bestiary_Biomes.TheDungeon");
                    break;
                case 2:
                    text = Language.GetTextValue("Bestiary_Biomes.Jungle");
                    break;
                case 3: // Random
                    text = Language.GetTextValue("LegacyMenu.27");
                    break;
                default:
                    break;
            }
            Vector2 size = FontAssets.MouseText.Value.MeasureString(text);
            Terraria.Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, text, CenterPoint.X - size.X / 2f, CenterPoint.Y + CircleOffset + CircleTextureSize / 2 + 4, Color.White, Color.Black, default);
        }

        public static void DoTeleportation(int circle)
        {
            Open = false;
            Player p = Main.LocalPlayer;
            switch (circle)
            {
                case 0:
                    Vector2? underworld = GetUnderworldPosition(p);
                    if (!underworld.HasValue)
                        return;
                    ModTeleport(p, underworld.Value, true, TeleportationStyleID.DemonConch);
                    break;
                case 1:
                    ModTeleport(p, new Vector2(Main.dungeonX * 16, Main.dungeonY * 16 - 8));
                    break;
                case 2:
                    Vector2? jungle = GetJunglePosition(p);
                    if (!jungle.HasValue)
                        return;
                    ModTeleport(p, jungle.Value);
                    break;
                case 3:
                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        p.TeleportationPotion();
                        SoundEngine.PlaySound(SoundID.Item6, p.position);
                    }
                    else if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendData(MessageID.RequestTeleportationByServer, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
                    }
                    break;
                default:
                    break;
            }
        }
        private static void DrawCircle(SpriteBatch sb, Vector2 center, int circle, CircleStyle style)
        {
            sb.Draw(CITextureRegister_UI.AstralArcanumCircles.Value, center, new Rectangle(circle * CircleTextureSize, (int)style * CircleTextureSize,
                CircleTextureSize, CircleTextureSize), Color.White, 0f, new Vector2(CircleTextureSize / 2), 1f, SpriteEffects.None, 0f);
        }
        public static Vector2? GetUnderworldPosition(Player player)
        {
            bool canSpawn = false;
            int num = Main.maxTilesX / 2;
            int num2 = 100;
            int num3 = 50;
            int teleportStartY = Main.UnderworldLayer + 20;
            int teleportRangeY = 80;
            Player.RandomTeleportationAttemptSettings settings = new Player.RandomTeleportationAttemptSettings
            {
                mostlySolidFloor = true,
                avoidAnyLiquid = true,
                avoidLava = true,
                avoidHurtTiles = true,
                avoidWalls = true,
                attemptsBeforeGivingUp = 1000,
                maximumFallDistanceFromOrignalPoint = 30
            };
            Vector2 value = player.CheckForGoodTeleportationSpot(ref canSpawn, num - num3, num2, teleportStartY, teleportRangeY, settings);
            if (!canSpawn)
            {
                value = player.CheckForGoodTeleportationSpot(ref canSpawn, num - num2, num3, teleportStartY, teleportRangeY, settings);
            }

            if (!canSpawn)
            {
                value = player.CheckForGoodTeleportationSpot(ref canSpawn, num + num3, num3, teleportStartY, teleportRangeY, settings);
            }

            if (canSpawn)
            {
                return value;
            }

            return null;
        }
        public static void ModTeleport(Player player, Vector2 pos, bool playSound = true, int style = 3)
        {
            bool immune = player.immune;
            int immuneTime = player.immuneTime;
            player.StopVanityActions(multiplayerBroadcast: false);
            player.RemoveAllGrapplingHooks();
            player.Teleport(pos, style);
            if (Main.dedServ)
            {
                RemoteClient.CheckSection(player.whoAmI, player.Center);
            }

            NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, pos.X, pos.Y, style);
            player.velocity = Vector2.Zero;
            player.immune = immune;
            player.immuneTime = immuneTime;
            for (int i = 0; i < 100; i++)
            {
                Dust.NewDust(player.position, player.width, player.height, DustID.TeleportationPotion, player.velocity.X * 0.1f, player.velocity.Y * 0.1f, 150, Color.Cyan, 1.2f);
            }

            Rectangle rect = player.getRect();
            int num = rect.Width * rect.Height / 5;
            for (int j = 0; j < num; j++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, DustID.TeleportationPotion);
                dust.scale = Main.rand.NextFloat(0.2f, 0.7f);
                if (j < 10)
                {
                    dust.scale += 0.25f;
                }

                if (j < 5)
                {
                    dust.scale += 0.25f;
                }
            }

            for (int k = 0; k < 50; k++)
            {
                Dust dust2 = Dust.NewDustDirect(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, DustID.DungeonSpirit);
                dust2.noGravity = true;
                for (int l = 0; l < 5; l++)
                {
                    if (Main.rand.NextBool(3))
                    {
                        dust2.velocity *= 0.75f;
                    }
                }

                if (Main.rand.NextBool(3))
                {
                    dust2.velocity *= 2f;
                    dust2.scale *= 1.2f;
                }

                if (Main.rand.NextBool(3))
                {
                    dust2.velocity *= 2f;
                    dust2.scale *= 1.2f;
                }

                if (Main.rand.NextBool())
                {
                    dust2.fadeIn = Main.rand.NextFloat(0.75f, 1f);
                    dust2.scale = Main.rand.NextFloat(0.25f, 0.75f);
                }

                dust2.scale *= 0.8f;
            }

            if (playSound)
            {
                SoundEngine.PlaySound(in SoundID.Item6, player.Center);
            }
        }
        public static Vector2? GetJunglePosition(Player player)
        {
            bool canSpawn = false;
            int teleportStartX = (int)(Main.maxTilesX * 0.2);
            int teleportRangeX = (int)(Main.maxTilesX * 0.15);
            int teleportStartY = (int)Main.worldSurface - 75;
            int teleportRangeY = 50;

            Player.RandomTeleportationAttemptSettings settings = new Player.RandomTeleportationAttemptSettings
            {
                mostlySolidFloor = true,
                avoidAnyLiquid = true,
                avoidLava = true,
                avoidHurtTiles = true,
                avoidWalls = true,
                attemptsBeforeGivingUp = 1000,
                maximumFallDistanceFromOrignalPoint = 30
            };

            Vector2 vector = player.CheckForGoodTeleportationSpot(ref canSpawn, teleportStartX, teleportRangeX, teleportStartY, teleportRangeY, settings);

            if (canSpawn)
            {
                return (Vector2?)vector;
            }
            return null;
        }
        private class Circle
        {

            public Vector2 Center;
            public float Radius;
            public Circle(Vector2 C, float R)
            {
                this.Center = C;
                this.Radius = R;
            }
            public bool Contains(Vector2 point)
            {
                return Vector2.Distance(Center, point) < Radius;
            }
            public bool Contains(Point point)
            {
                return Contains(point.ToVector2());
            }
        }
    }
}
