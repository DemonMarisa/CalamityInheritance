using CalamityInheritance.World;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.UI;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework.Input;

namespace CalamityInheritance.Common.CIHook
{
    public class FlightBarDrawHook
    {
        // 这一段是为了修复神殇模式导致的飞行条绘制bug
        // 原灾判断了坐骑，但是刚进入世界没有坐骑记录，导致抛出异常
        // 用另一种材质获取方法替代了原方法
        public static void Load()
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            MethodInfo original = typeof(FlightBar).GetMethod("DrawFlightBar", bindingFlags);
            var hook = new Hook(original, DrawFlightBar_Hook);
            hook.Apply();

            MethodInfo originalMethod = typeof(FlightBar).GetMethod("Draw");
            MonoModHooks.Add(originalMethod, Draw_Hook);
        }
        public static void Draw_Hook(SpriteBatch spriteBatch, Player player)
        {
            // Sanity check the planned position before drawing
            Vector2 screenRatioPosition = new Vector2(CalamityConfig.Instance.FlightBarPosX, CalamityConfig.Instance.FlightBarPosY);
            if (screenRatioPosition.X < 0f || screenRatioPosition.X > 100f)
                screenRatioPosition.X = FlightBar.DefaultFlightPosX;
            if (screenRatioPosition.Y < 0f || screenRatioPosition.Y > 100f)
                screenRatioPosition.Y = FlightBar.DefaultFlightPosY;

            // Convert the screen ratio position to an absolute position in pixels
            // Cast to integer to prevent blurriness which results from decimal pixel positions
            float uiScale = Main.UIScale;
            Vector2 screenPos = screenRatioPosition;
            screenPos.X = (int)(screenPos.X * 0.01f * Main.screenWidth);
            screenPos.Y = (int)(screenPos.Y * 0.01f * Main.screenHeight);

            CalamityPlayer modPlayer = player.Calamity();

            // If not drawing the flight bar, save its latest position to config and leave.
            if (CalamityConfig.Instance.FlightBar && (player.wingsLogic > 0 || (player.mount.Active && player.mount._data.flightTimeMax > 0) || player.carpet && !player.canCarpet))
            {
                DrawFlightBar_Hook(spriteBatch, modPlayer, screenPos);
            }
            else
            {
                bool changed = false;
                if (CalamityConfig.Instance.FlightBarPosX != screenRatioPosition.X)
                {
                    CalamityConfig.Instance.FlightBarPosX = screenRatioPosition.X;
                    changed = true;
                }
                if (CalamityConfig.Instance.FlightBarPosY != screenRatioPosition.Y)
                {
                    CalamityConfig.Instance.FlightBarPosY = screenRatioPosition.Y;
                    changed = true;
                }

                if (changed)
                    CalamityInheritance.SaveConfig(CalamityConfig.Instance);
                return;
            }

            Rectangle mouseHitbox = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 8, 8);
            Rectangle flightBar = Utils.CenteredRectangle(screenPos, FlightBar.borderTexture.Size() * uiScale);

            MouseState ms = Mouse.GetState();
            Vector2 mousePos = Main.MouseScreen;

            // Handle mouse dragging
            if (flightBar.Intersects(mouseHitbox))
            {
                if (!CalamityConfig.Instance.MeterPosLock)
                    Main.LocalPlayer.mouseInterface = true;

                if (modPlayer.Player.equippedWings != null && modPlayer.Player.wingTimeMax > 0 || (player.mount.Active && modPlayer.Player.mount._data.flightTimeMax > 0) || player.carpet && !player.canCarpet) //equipped wings or riding a flying mount and max wingtime/flighttime above 0 (so not disabled bar)
                {
                    string textToDisplay = CalamityUtils.GetText("UI.Flight").Format((FlightBar.GetFlightTime(modPlayer).ToString() + (modPlayer.infiniteFlight ? "" : "%"))); //the percent is here and not in localisation otherwise it looks like a dick when it's infinite flight
                    Main.instance.MouseText(textToDisplay, 0, 0, -1, -1, -1, -1);
                }

                Vector2 newScreenRatioPosition = screenRatioPosition;
                // As long as the mouse button is held down, drag the meter along with an offset.
                if (!CalamityConfig.Instance.MeterPosLock && ms.LeftButton == ButtonState.Pressed)
                {
                    // If the drag offset doesn't exist yet, create it.
                    if (!FlightBar.dragOffset.HasValue)
                        FlightBar.dragOffset = mousePos - screenPos;

                    // Given the mouse's absolute current position, compute where the corner of the flight bar should be based on the original drag offset.
                    Vector2 newCorner = mousePos - FlightBar.dragOffset.GetValueOrDefault(Vector2.Zero);

                    // Convert the new corner position into a screen ratio position.
                    newScreenRatioPosition.X = (100f * newCorner.X) / Main.screenWidth;
                    newScreenRatioPosition.Y = (100f * newCorner.Y) / Main.screenHeight;
                }

                // Compute the change in position. If it is large enough, actually move the meter
                Vector2 delta = newScreenRatioPosition - screenRatioPosition;
                if (Math.Abs(delta.X) >= FlightBar.MouseDragEpsilon || Math.Abs(delta.Y) >= FlightBar.MouseDragEpsilon)
                {
                    CalamityConfig.Instance.FlightBarPosX = newScreenRatioPosition.X;
                    CalamityConfig.Instance.FlightBarPosY = newScreenRatioPosition.Y;
                }

                // When the mouse is released, save the config and destroy the drag offset.
                if (ms.LeftButton == ButtonState.Released)
                {
                    FlightBar.dragOffset = null;
                    CalamityInheritance.SaveConfig(CalamityConfig.Instance);
                }
            }
        }
        public static void DrawFlightBar_Hook(SpriteBatch spriteBatch, CalamityPlayer modPlayer, Vector2 screenPos)
        {
            float uiScale = Main.UIScale;
            Player player = modPlayer.Player;
            float flightRatio = 1;
            if (!modPlayer.infiniteFlight && !FlightBar.RidingInfiniteFlightMount(player))
                flightRatio = player.carpet && !player.canCarpet ? Math.Min((float)player.carpetTime / 300f, 1f) : player.mount.Active && player.mount._data.flightTimeMax > 0 ? Math.Min((float)(player.mount._flyTime + (player.mount._data.fatigueMax - player.mount._fatigue)) / (player.mount._data.flightTimeMax + player.mount._data.fatigueMax), 1f) : Math.Min(player.wingTime / player.wingTimeMax, 1f); // why the FUCK can wingtime be higher than max wingtime?????????
            if (!FlightBar.completedAnimation && FlightBar.FlightAnimFrame == -1 && (modPlayer.infiniteFlight || FlightBar.RidingInfiniteFlightMount(modPlayer.Player)))
                FlightBar.FlightAnimFrame++;
            if (FlightBar.FlightAnimFrame > -1) //animation started, complete it.
            {
                FlightBar.FlightAnimTimer++;
                if (FlightBar.FlightAnimTimer >= FlightBar.FlightAnimFrameDelay)
                {
                    if (FlightBar.FlightAnimFrame >= FlightBar.FlightAnimFrames)
                    {
                        FlightBar.FlightAnimFrame = -1;
                        FlightBar.FlightAnimTimer = 0;
                        FlightBar.completedAnimation = modPlayer.infiniteFlight || FlightBar.RidingInfiniteFlightMount(modPlayer.Player); //completed animation sets to true if infinite flight still exists
                    }
                    else
                    {
                        FlightBar.FlightAnimTimer = 0;
                        FlightBar.FlightAnimFrame++;
                    }
                }
            }
            Texture2D correctBorder = ModContent.Request<Texture2D>("CalamityMod/UI/FlightBar/FlightBarBorder").Value;

            if (modPlayer.Player.equippedWings != null && modPlayer.Player.wingTimeMax == 0)
                correctBorder = ModContent.Request<Texture2D>("CalamityMod/UI/FlightBar/FlightBarBorderDisabled").Value;
            if (modPlayer.weakPetrification || modPlayer.vHex || modPlayer.icarusFolly || modPlayer.DoGExtremeGravity)
                correctBorder = ModContent.Request<Texture2D>("CalamityMod/UI/FlightBar/FlightBarBorderReduced").Value;
            if ((modPlayer.infiniteFlight || FlightBar.RidingInfiniteFlightMount(modPlayer.Player)) && FlightBar.completedAnimation)
                correctBorder = ModContent.Request<Texture2D>("CalamityMod/UI/FlightBar/FlightBarBorderInfinite").Value;
            CIWorld world = ModContent.GetInstance<CIWorld>();
            if (world.Defiled)
                correctBorder = ModContent.Request<Texture2D>("CalamityMod/UI/FlightBar/FlightBarBorderDisabled").Value;

            if (FlightBar.completedAnimation && !modPlayer.infiniteFlight && correctBorder != FlightBar.infiniteBarTexture)
                FlightBar.completedAnimation = false; //reset flight anim once infinite flight expires.

            float offset = (correctBorder.Width - FlightBar.barTexture.Width) * 0.5f;
            spriteBatch.Draw(correctBorder, screenPos, null, Color.White, 0f, correctBorder.Size() * 0.5f, uiScale, SpriteEffects.None, 0);
            if (correctBorder != FlightBar.disabledBarTexture && correctBorder != FlightBar.infiniteBarTexture) //neither requires an internal bar to be drawn
            {
                int correctHeight = (correctBorder == FlightBar.limitedBarTexture ? FlightBar.barTexture.Height / 2 : FlightBar.barTexture.Height);
                Rectangle barRectangle = FlightBar.barTexture.Bounds;
                barRectangle.Height = (int)(correctHeight * flightRatio);
                Vector2 origin = correctBorder.Size() * 0.5f;
                origin.Y += 0.1f;
                Vector2 drawPos = screenPos - new Vector2(offset * uiScale, 12 * uiScale);
                spriteBatch.Draw(FlightBar.barTexture, drawPos, barRectangle, Color.White, MathHelper.ToRadians(180), origin, uiScale, SpriteEffects.None, 0);
            }
            if (!FlightBar.completedAnimation && FlightBar.FlightAnimFrame >= 0)
            {
                Vector2 origin = new Vector2(correctBorder.Width * 0.5f, (correctBorder.Height / FlightBar.FlightAnimFrames) * 0.5f);
                float xOffset = (correctBorder.Width - FlightBar.flightBarAnimTexture.Width) / 2f;
                int frameHeight = (FlightBar.flightBarAnimTexture.Height / FlightBar.FlightAnimFrames) - 1;
                float yOffset = FlightBar.FlightAnimFrame == 0 ? 0 : ((correctBorder.Height / FlightBar.FlightAnimFrame) - frameHeight) / 2f;
                Vector2 sizeDiffOffset = new Vector2(xOffset, yOffset);
                Rectangle animCropRect = new Rectangle(0, (frameHeight + 1) * FlightBar.FlightAnimFrame, FlightBar.flightBarAnimTexture.Width, frameHeight);
                spriteBatch.Draw(FlightBar.flightBarAnimTexture, screenPos + sizeDiffOffset, animCropRect, Color.White, 0f, origin * Main.UIScale, uiScale, SpriteEffects.None, 0);
            }
        }
    }
}
