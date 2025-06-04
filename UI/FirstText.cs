using CalamityInheritance.Content.Items.Placeables.Vanity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityInheritance.World;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CalamityInheritance.UI
{
    public class FirstText
    {
        public static void Draw(SpriteBatch spriteBatch)
        {
            if (CIConfig.Instance.TurnOffFirstText == false)
                return;

            if(Main.netMode != NetmodeID.SinglePlayer)
                return;

            Texture2D InvisibleUI = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/InvisibleUI").Value;
            string key = Language.GetTextValue("Mods.CalamityInheritance.ConfigsMessage.FirstText");
            DrawText(spriteBatch, key, 1.5f, 1.5f, 0, -120, 1f, Color.Gold, Color.DarkGoldenrod, InvisibleUI, 15, 1600f, 1.2f);
        }
        public static void DrawText(
            SpriteBatch spriteBatch,
            string textContent, // 改为接收原始文本内容
            float xResolutionScale,
            float yResolutionScale,
            float xOffset,
            float yOffset,
            float scale,
            Color TextColor,
            Color TextOutLineColor,
            Texture2D texture,
            int lineOffset,
            float maxWidth = 0f, // 最大宽度
            float lineSpacing = 1.2f // 行间距系数
            )
        {
            // 获取字体引用
            DynamicSpriteFont font = FontAssets.MouseText.Value;

            // 自动换行处理
            List<string> wrappedLines = new List<string>();

            if (maxWidth > 0)
            {
                // 计算实际可用宽度（考虑缩放）
                float actualMaxWidth = maxWidth / xResolutionScale;
                wrappedLines = CIFunction.BetterWordwrapString(textContent, font, (int)actualMaxWidth, 999, out _)
                    .Where(line => !string.IsNullOrEmpty(line))
                    .ToList();
            }

            // 计算基准行高
            float baseLineHeight = font.LineSpacing * scale * lineSpacing;

            // 计算起始位置（屏幕中心 + 偏移量）
            Vector2 startPosition = new Vector2(
                Main.screenWidth / 2f + xOffset * xResolutionScale,
                Main.screenHeight / 2f + yOffset * yResolutionScale
            );

            for (int i = 0; i < wrappedLines.Count; i++)
            {
                string line = wrappedLines[i];
                if (string.IsNullOrEmpty(line)) continue;

                // 计算当前行位置
                Vector2 linePosition = new Vector2(
                    startPosition.X,
                    startPosition.Y + (baseLineHeight * i)
                );

                // 计算文本尺寸
                Vector2 textSize = ChatManager.GetStringSize(font, line, new Vector2(scale));

                spriteBatch.Draw(texture, new Vector2(startPosition.X, startPosition.Y + (baseLineHeight * i) + lineOffset), null, Color.White, 0f, texture.Size() / 2, new Vector2(1.15f, 1f) * 0.95f, SpriteEffects.None, 0f);

                for (int j = 0; j < 4; j++)
                {
                    ChatManager.DrawColorCodedString(spriteBatch, font, line, linePosition, TextOutLineColor, 0f, new Vector2(textSize.X / 2f, textSize.Y / 2f),
                        new Vector2(xResolutionScale, yResolutionScale) * scale, maxWidth, false);
                }

                ChatManager.DrawColorCodedString(spriteBatch, font, line, linePosition, TextColor, 0f, new Vector2(textSize.X / 2f, textSize.Y / 2f),
                    new Vector2(xResolutionScale, yResolutionScale) * scale, maxWidth, false);
            }
        }
    }
}
