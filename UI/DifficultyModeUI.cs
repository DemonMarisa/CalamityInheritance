using CalamityInheritance.Content.Items.Placeables.Vanity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.World;
using CalamityMod.CalPlayer;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static CalamityInheritance.Utilities.CIFunction;

namespace CalamityInheritance.UI
{
    public class DifficultyModeUI
    {
        public static bool InfernumMode;
        private static Texture2D BG, Arma, Death, Malice, Rev, Rune;
        public static int count = 0;
        public static int FrameCount = 0;
        public static void Load()
        {
            BG = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/ModeIndicatorArea", AssetRequestMode.ImmediateLoad).Value;
            Arma = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/ModeIndicatorArma", AssetRequestMode.ImmediateLoad).Value;
            Death = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/ModeIndicatorDeath", AssetRequestMode.ImmediateLoad).Value;
            Malice = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/ModeIndicatorMalice", AssetRequestMode.ImmediateLoad).Value;
            Rev = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/ModeIndicatorRev", AssetRequestMode.ImmediateLoad).Value;
            Rune = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/ModeIndicatorRune", AssetRequestMode.ImmediateLoad).Value;
        }

        public static void Unload()
        {
            BG = Arma = Death = Malice = Rev = Rune = null;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (CIConfig.Instance.DrawDifficultyUI == false)
                return;

            if (Main.playerInventory == false)
                return;

            Mod infernumMode = CalamityInheritance.Instance.infernumMode;

            if (infernumMode is null)
            {
                DrawCalamityUI(spriteBatch);
            }
            else
            {
                if (infernumMode is not null)
                    InfernumMode = (bool)infernumMode.Call("GetInfernumActive");

                if (InfernumMode)
                    DrawInfernumModeUI(spriteBatch);
                else
                    DrawCalamityUI(spriteBatch);
            }
        }

        public static void DrawCalamityUI(SpriteBatch spriteBatch)
        {
            CIWorld world = ModContent.GetInstance<CIWorld>();
            // 右上角为锚点绘制
            float yCenter = 80;
            float xCenter = Main.screenWidth - 450;
            Vector2 drawBGPosition = new(xCenter, yCenter);
            Vector2 org = new(BG.Width, 0);
            // 绘制底子
            spriteBatch.Draw(BG, drawBGPosition, null, Color.White, 0f, org, 1f, SpriteEffects.None, 0f);

            if (world.Malice)
                spriteBatch.Draw(Malice, drawBGPosition, null, Color.White, 0f, org, 1f, SpriteEffects.None, 0f);
            else if (CalamityWorld.death)
                spriteBatch.Draw(Death, drawBGPosition, null, Color.White, 0f, org, 1f, SpriteEffects.None, 0f);
            else if (CalamityWorld.revenge)
                spriteBatch.Draw(Rev, drawBGPosition, null, Color.White, 0f, org, 1f, SpriteEffects.None, 0f);

            if (world.Armageddon)
                spriteBatch.Draw(Arma, drawBGPosition, null, Color.White, 0f, org, 1f, SpriteEffects.None, 0f);

            if (world.Defiled)
                spriteBatch.Draw(Rune, drawBGPosition, null, Color.White, 0f, org, 1f, SpriteEffects.None, 0f);
        }

        public static void DrawInfernumModeUI(SpriteBatch spriteBatch)
        {
            CIWorld world = ModContent.GetInstance<CIWorld>();
            Texture2D outerAreaTexture = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/InfernumBG").Value;

            count++;
            if (count > int.MaxValue)
                count = 0;

            if (count % 6 == 0)
                FrameCount++;

            if (world.Armageddon && world.Defiled)
                outerAreaTexture = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/InfernumDefiledArmaBG").Value;
            else if (world.Defiled)
                outerAreaTexture = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/InfernumDefiledBG").Value;
            else if (world.Armageddon)
                outerAreaTexture = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/InfernumArmaBG").Value;

            float pulseRate = 11f;

            if (CalamityPlayer.areThereAnyDamnBosses)
                pulseRate = 25f;

            Rectangle areaFrame = outerAreaTexture.Frame(1, 13, 0, (int)(FrameCount * pulseRate) % 13);

            float yCenter = 80;
            float xCenter = Main.screenWidth - 450;
            Vector2 drawBGPosition = new(xCenter, yCenter);
            Vector2 org = new(areaFrame.Width, 0);

            if (CalamityPlayer.areThereAnyDamnBosses)
            {
                Color drawColor = Color.Red * 0.4f;
                drawColor.A = 0;
                for (int i = 0; i < 12; i++)
                {
                    Vector2 drawOffset = (MathHelper.TwoPi * i / 12f + FrameCount * 4f).ToRotationVector2() * 5f;
                    Main.spriteBatch.Draw(outerAreaTexture, drawBGPosition + drawOffset, areaFrame, drawColor, 0f, org, 1f, SpriteEffects.None, 0f);
                }
            }
            Main.spriteBatch.Draw(outerAreaTexture, drawBGPosition, areaFrame, Color.White, 0f, org, 1f, SpriteEffects.None, 0f);
        }
    }
}
