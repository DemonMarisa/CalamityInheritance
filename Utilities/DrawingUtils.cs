using CalamityInheritance.CIPlayer;
using CalamityInheritance.UI.QolPanelTotal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.UI.Chat;
using static System.Net.Mime.MediaTypeNames;

namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
    {
        public static Texture2D AuroraTexture
        {
            get
            {
                Main.instance.LoadProjectile(ProjectileID.HallowBossDeathAurora);
                return TextureAssets.Projectile[ProjectileID.HallowBossDeathAurora].Value;
            }
        }

        public static readonly Color[] ExoPalette = new Color[]
        {
            new Color(250, 255, 112),
            new Color(211, 235, 108),
            new Color(166, 240, 105),
            new Color(105, 240, 220),
            new Color(64, 130, 145),
            new Color(145, 96, 145),
            new Color(242, 112, 73),
            new Color(199, 62, 62),
        };
        #region 随机颜色
        public static void IterateDisco(ref Color c, ref float aiParam, in byte discoIter = 7)
        {
            switch (aiParam)
            {
                case 0f:
                    c.G += discoIter;
                    if (c.G >= 255)
                    {
                        c.G = 255;
                        aiParam = 1f;
                    }
                    break;
                case 1f:
                    c.R -= discoIter;
                    if (c.R <= 0)
                    {
                        c.R = 0;
                        aiParam = 2f;
                    }
                    break;
                case 2f:
                    c.B += discoIter;
                    if (c.B >= 255)
                    {
                        c.B = 255;
                        aiParam = 3f;
                    }
                    break;
                case 3f:
                    c.G -= discoIter;
                    if (c.G <= 0)
                    {
                        c.G = 0;
                        aiParam = 4f;
                    }
                    break;
                case 4f:
                    c.R += discoIter;
                    if (c.R >= 255)
                    {
                        c.R = 255;
                        aiParam = 5f;
                    }
                    break;
                case 5f:
                    c.B -= discoIter;
                    if (c.B <= 0)
                    {
                        c.B = 0;
                        aiParam = 0f;
                    }
                    break;
                default:
                    aiParam = 0f;
                    c = Color.Red;
                    break;
            }

        }
        #endregion
        #region 绘制按钮
        /// <summary>
        /// 在屏幕上绘制可交互图片的方法
        /// 一定要手动赋予buttonTexChange的初始值，方法只内置了悬停后必定切换为2或者4，点击前的1与3的状态标记必须手动赋予
        /// </summary>
        /// <param name="available">UI交互可用与否</param>
        /// <param name="buttonCount">标记按钮贴图状态，1：默认贴图，2：鼠标悬浮，3：按下按钮</param>
        /// <param name="UIID">UI的ID, 作本地化准备</param>
        /// <param name="Scale">尺寸</param>
        /// <param name="xPageBottom">以屏幕中心锚点X偏移</param>
        /// <param name="yPageBottom">以屏幕中心锚点Y偏移</param>
        /// <param name="xResolutionScale">X缩放尺寸</param>
        /// <param name="yResolutionScale">Y缩放尺寸</param>
        /// <param name="flipHorizontally">UI水平镜像</param>
        /// <param name="mouseRectangle">鼠标判定</param>
        /// <param name="spriteBatch">绘制</param>
        /// <param name="falseTexture">关闭时的按钮贴图</param>
        /// <param name="falseHoveredTexture">关闭时的悬停贴图</param>
        /// <param name="trueTexture">可用贴图</param>
        /// <param name="trueHoveredTexture">可用时的悬停贴图</param>
        /// <param name="unavailableTexture"不可用贴图></param>
        public static void DrawButtom(ref bool available, ref int buttonCount, ref int UIID, float Scale,
                                      float xPageBottom, float yPageBottom, float xResolutionScale,
                                      float yResolutionScale, bool flipHorizontally, Rectangle mouseRectangle,
                                      SpriteBatch spriteBatch, Texture2D falseTexture, Texture2D falseHoveredTexture,
                                      Texture2D trueTexture, Texture2D trueHoveredTexture, Texture2D unavailableTexture)
        // 这一段接受数量也太多了，我只能这么写了（
        // Scarlet: 这么写是正常的，只是有些地方需要注意：
        // 如果你默认你输入的一些东西大部分情况保持不变，你应当把这些置函数最后方
        {
            float scale = Scale;
            Texture2D targetTexture = trueTexture;

            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer cIPlayer = player.CIMod();

            // 绘制坐标
            Vector2 drawPosition = new Vector2(Main.screenWidth / 2 + xPageBottom, Main.screenHeight / 2 + yPageBottom);
            Rectangle arrowRect = new Rectangle(
                (int)(drawPosition.X - trueTexture.Width * xResolutionScale * scale / 2),
                (int)(drawPosition.Y - trueTexture.Height * yResolutionScale * scale / 2),
                (int)(trueTexture.Width * xResolutionScale * scale),
                (int)(trueTexture.Height * yResolutionScale * scale)
            );

            // 检测悬停
            bool isHovering = arrowRect.Intersects(mouseRectangle);

            // 获取当前鼠标状态
            bool isMouseDown = Main.mouseLeft;

            // 动态切换纹理
            // 悬停时切换纹理为 2
            if (isHovering && available)
            {
                if (buttonCount == 1)
                    buttonCount = 2;
                if (buttonCount == 3)
                    buttonCount = 4;

                // 未悬停时恢复为1（默认贴图
                // 按下时缩小
                if (isMouseDown)
                {
                    scale *= 0.9f;
                    Main.blockMouse = true;
                    cIPlayer.wasMouseDown = true;
                }
                // 这一段解释一下就是，2是false的悬停贴图，4是true的悬停贴图，如果按下，且当前贴图是2，就切换到3，反之同理
                if (cIPlayer.wasMouseDown && isHovering && !isMouseDown)
                {
                    // 释放瞬间：切换永久状态
                    if (buttonCount == 2)
                    {
                        buttonCount = 3;
                        cIPlayer.wasMouseDown = false;
                    }
                    if (buttonCount == 4)
                    {
                        // 在1和3之间切换
                        buttonCount = 1;
                        cIPlayer.wasMouseDown = false;
                    }
                }
            }

            if (!Main.mouseLeft)
                cIPlayer.wasMouseDown = false;

            // 重置为对应的默认贴图
            if (buttonCount == 2 && !isHovering && cIPlayer.wasMouseDown == false)
                buttonCount = 1;
            if (buttonCount == 4 && !isHovering && cIPlayer.wasMouseDown == false)
                buttonCount = 3;
            
            // 最终材质选择
            if (buttonCount == 1)
                targetTexture = falseTexture;
            if (buttonCount == 2)
                targetTexture = falseHoveredTexture;
            if (buttonCount == 3)
                targetTexture = trueTexture;
            if (buttonCount == 4)
                targetTexture = trueHoveredTexture;
            if (!available)
                targetTexture = unavailableTexture;

            // 改为中心锚点
            spriteBatch.Draw(targetTexture, drawPosition, null, Color.White, 0f,targetTexture.Size() / 2,new Vector2(xResolutionScale, yResolutionScale) * scale, flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None , 0f);
        }
        #endregion
        #region 绘制按钮2
        /// <summary>
        /// 专门用于绘制Lore下方按钮的方法
        /// </summary>
        /// <param name="thisUI">按钮贴图数据</param>
        /// <param name="xPageBottom">按钮水平坐标，相对于屏幕中心</param>
        /// <param name="yPageBottom">按钮垂直坐标，相对于屏幕中心</param>
        /// <param name="available">是否可用</param>
        /// <param name="buttonCount">按钮状态，建议手动输入</param>
        /// <param name="UIID">UIID，准备用于本地化</param>
        public static void DrawBton(DrawUIData thisUI,float xPageBottom, float yPageBottom,  ref bool available, ref int buttonCount, ref int UIID)
        // 这一段接受数量也太多了，我只能这么写了（
        // Scarlet: 这么写是正常的，只是有些地方需要注意：
        // 如果你默认你输入的一些东西大部分情况保持不变，你应当把这些置函数最后方
        {
            if (buttonCount < 1)
                buttonCount = 1;
            if (buttonCount > 4)
                buttonCount = 3;
            float scale = thisUI.scale;
            Texture2D targetTexture = thisUI.buttonTextureTrue;

            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer cIPlayer = player.CIMod();

            // 绘制坐标
            Vector2 drawPosition = new Vector2(Main.screenWidth / 2 + xPageBottom, Main.screenHeight / 2 + yPageBottom);
            Rectangle arrowRect = new Rectangle(
                (int)(drawPosition.X - thisUI.buttonTextureTrue.Width * thisUI.xResolutionScale* scale / 2),
                (int)(drawPosition.Y - thisUI.buttonTextureTrue.Height * thisUI.yResolutionScale* scale / 2),
                (int)(thisUI.buttonTextureTrue.Width * thisUI.xResolutionScale* scale),
                (int)(thisUI.buttonTextureTrue.Height * thisUI.xResolutionScale* scale)
            );

            // 检测悬停
            bool isHovering = arrowRect.Intersects(thisUI.mouseRectangle);

            // 获取当前鼠标状态
            bool isMouseDown = Main.mouseLeft;

            // 动态切换纹理
            // 悬停时切换纹理为 2
            if (isHovering && available)
            {

                if (buttonCount == 1)
                    buttonCount = 2;
                if (buttonCount == 3)
                    buttonCount = 4;

                // 未悬停时恢复为1（默认贴图
                // 按下时缩小
                if (isMouseDown)
                {
                    scale *= 0.9f;
                    Main.blockMouse = true;
                    cIPlayer.wasMouseDown = true;
                }
                // 这一段解释一下就是，2是false的悬停贴图，4是true的悬停贴图，如果按下，且当前贴图是2，就切换到3，反之同理
                if (cIPlayer.wasMouseDown && isHovering && !isMouseDown)
                {
                    // 释放瞬间：切换永久状态
                    if (buttonCount == 2)
                    {
                        buttonCount = 3;
                        cIPlayer.wasMouseDown = false;
                    }
                    if (buttonCount == 4)
                    {
                        // 在1和3之间切换
                        buttonCount = 1;
                        cIPlayer.wasMouseDown = false;
                    }
                }
            }

            // 重置为对应的默认贴图
            if (buttonCount == 2 && !isHovering)
                buttonCount = 1;
            if (buttonCount == 4 && !isHovering)
                buttonCount = 3;
            
            // 最终材质选择
            if (buttonCount == 1)
                targetTexture = thisUI.buttonTextureFalse;
            if (buttonCount == 2)
                targetTexture = thisUI.buttonTextureFalseHover;
            if (buttonCount == 3)
                targetTexture = thisUI.buttonTextureTrue;
            if (buttonCount == 4)
                targetTexture = thisUI.buttonTextureTrueHover;

            if (!available)
            {
                buttonCount = 1;
                targetTexture = thisUI.buttonTextureUnAvailable;
            }

            // 改为中心锚点
            thisUI.spriteBatch.Draw(targetTexture, drawPosition, null, Color.White, 0f,targetTexture.Size() / 2,new Vector2(thisUI.xResolutionScale, thisUI.yResolutionScale) * scale, thisUI.flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None , 0f);
        }
        #endregion
        #region 绘制图片
        /// <summary>
        /// 在屏幕上绘制图片的方法
        /// 你可以在指定地点绘制一张图片
        /// </summary>
        /// <param name="spriteBatch">绘制</param>
        /// <param name="texture">材质</param>
        /// <param name="Scale">缩放</param>
        /// <param name="xResolutionScale">X 缩放</param>
        /// <param name="yResolutionScale">Y 缩放</param>
        /// <param name="xPageBottom">以屏幕中心为锚点的 X 偏移</param>
        /// <param name="yPageBottom">以屏幕中心为锚点的 Y 偏移</param>
        /// <param name="available">按钮是否可用的提示</param>
        /// <param name="flipHorizontally">图片是否镜像</param>
        /// <summary>
        public static void DrawImage(
            SpriteBatch spriteBatch,
            Texture2D texture,
            Texture2D unavailableTexture,
            float Scale,
            float xResolutionScale,
            float yResolutionScale,
            float xPageBottom,
            float yPageBottom,
            bool flipHorizontally, // 是否镜像
            ref bool available
            )
        {

            float scale = Scale;
            Texture2D targetTexture = texture;

            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer cIPlayer = player.CIMod();

            // 绘制坐标
            Vector2 drawPosition = new Vector2(Main.screenWidth / 2 + xPageBottom, Main.screenHeight / 2 + yPageBottom);

            // 改为中心锚点
            spriteBatch.Draw(targetTexture, drawPosition, null, Color.White, 0f, targetTexture.Size() / 2, new Vector2(xResolutionScale, yResolutionScale) * scale, flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            // 无法使用时，便会盖住
            if (!available)
                spriteBatch.Draw(unavailableTexture, drawPosition, null, Color.White, 0f, targetTexture.Size() / 2, new Vector2(xResolutionScale, yResolutionScale) * scale, flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
        #endregion
        #region 绘制lore
        /// <summary>
        /// 专门用于绘制lore的方法
        /// </summary>
        /// <param name="loreData">存放Lore数据的大结构体</param>
        /// <param name="xPageBottom">Lore位置的x坐标，相对于屏幕中心</param>
        /// <param name="yPageBottom">Lore位置的y坐标，相对于屏幕中心</param>
        /// <param name="texture">lore贴图</param>
        /// <param name="displayTextCount">关联的右侧显示文本</param>
        /// <param name="TextID">显示文本对应的ID，一次只能显示一个文本</param>
        /// <param name="available">Lore是否可用</param>
        /// <param name="MouseDownScale">点击时的缩放，不写默认 * 0.9f</param>
        public static void DrawLore(
            DrawLoreData loreData,
            float xPageBottom,
            float yPageBottom,
            Texture2D texture,
            ref int displayTextCount,
            ref int TextID,
            ref bool available,
            ref int DraedonsLoreChoice,
            ref int DraedonsLoreID,
            /* Texture2D textureBG,
             string textContent, // 接收原始文本内容*/
            float? MouseDownScale = null
            )
        {

            float scale = loreData.scale;
            Texture2D targetTexture = texture;

            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer cIPlayer = player.CIMod();

            // 绘制坐标
            Vector2 drawPosition = new Vector2(Main.screenWidth / 2 + xPageBottom, Main.screenHeight / 2 + yPageBottom);

            Rectangle loreRect = new Rectangle(
                (int)(drawPosition.X - texture.Width * scale / 2),
                (int)(drawPosition.Y - texture.Height * scale / 2),
                (int)(texture.Width * scale),
                (int)(texture.Height * scale)
                );

            // 检测悬停
            bool isHovering = loreRect.Intersects(loreData.mouseRectangle);

            // 获取当前鼠标状态
            bool isMouseDown = Main.mouseLeft;

            // 动态切换纹理
            // 悬停时切换纹理为 2
            if (isHovering)
            {

                if (available)
                {
                    if (loreData.buttonCount == 1)
                        loreData.buttonCount = 2;
                    if (loreData.buttonCount == 3)
                        loreData.buttonCount = 4;

                    // 未悬停时恢复为1（默认贴图
                    // 按下时缩小
                    if (isMouseDown)
                    {

                        if (!MouseDownScale.HasValue)
                            scale *= 0.9f;
                        if (MouseDownScale.HasValue)
                            scale *= MouseDownScale.Value;

                        Main.blockMouse = true;
                        cIPlayer.wasMouseDown = true;
                    }
                    // 这一段解释一下就是，2是false的悬停贴图，4是true的悬停贴图，如果按下，且当前贴图是2，就切换到3，反之同理
                    if (cIPlayer.wasMouseDown && !isMouseDown && isHovering)
                    {
                        displayTextCount = TextID;
                        DraedonsLoreChoice = DraedonsLoreID;
                        // 释放瞬间：切换永久状态
                        if (loreData.buttonCount == 2)
                        {
                            loreData.buttonCount = 3;
                            cIPlayer.wasMouseDown = false;
                        }
                        if (loreData.buttonCount == 4)
                        {
                            // 在1和3之间切换
                            loreData.buttonCount = 1;
                            cIPlayer.wasMouseDown = false;
                        }
                    }
                    loreData.spriteBatch.Draw(loreData.loreTextureOutLine, drawPosition, null, Color.White, 0f, loreData.loreTextureOutLine.Size() / 2, new Vector2(loreData.xResolutionScale, loreData.yResolutionScale) * scale, loreData.flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                    //DrawHoverText(loreData.spriteBatch, textContent, loreData.xResolutionScale, loreData.yResolutionScale, loreData.scale);
                }
                else
                {
                    loreData.spriteBatch.Draw(loreData.loreTextureOutLineUnAvailable, drawPosition, null, Color.White, 0f, loreData.loreTextureOutLine.Size() / 2, new Vector2(loreData.xResolutionScale, loreData.yResolutionScale) * scale, loreData.flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                    //DrawHoverText(loreData.spriteBatch, textContent, loreData.xResolutionScale, loreData.yResolutionScale, loreData.scale);
                }
            }

            // 改为中心锚点
            loreData.spriteBatch.Draw(targetTexture, drawPosition, null, Color.White, 0f, targetTexture.Size() / 2, new Vector2(loreData.xResolutionScale, loreData.yResolutionScale) * scale, loreData.flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            // 无法使用时，便会盖住
            if (!available)
                loreData.spriteBatch.Draw(loreData.loreTextureUnAvailable, drawPosition, null, Color.White, 0f, targetTexture.Size() / 2, new Vector2(loreData.xResolutionScale, loreData.yResolutionScale) * scale, loreData.flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

        }
        #endregion
        #region 绘制文字
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
                wrappedLines = BetterWordwrapString(textContent, font, (int)actualMaxWidth, 999, out _)
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
                    ChatManager.DrawColorCodedString(spriteBatch, font, line, linePosition, TextOutLineColor, 0f, new Vector2(textSize.X / 2f, 0f),
                        new Vector2(xResolutionScale, yResolutionScale) * scale, maxWidth, false);
                }

                ChatManager.DrawColorCodedString(spriteBatch, font, line, linePosition, TextColor, 0f, new Vector2(textSize.X / 2f, 0f),
                    new Vector2(xResolutionScale, yResolutionScale) * scale, maxWidth, false);
            }
        }

        public static void DrawTextNoLine(
            SpriteBatch spriteBatch,
            string textContent, // 改为接收原始文本内容
            float xResolutionScale,
            float yResolutionScale,
            float xOffset,
            float yOffset,
            float scale,
            Color TextColor,
            Color TextOutLineColor,
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
                wrappedLines = BetterWordwrapString(textContent, font, (int)actualMaxWidth, 999, out _)
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

                for (int j = 0; j < 4 ; j++)
                {
                    ChatManager.DrawColorCodedString(spriteBatch, font, line, linePosition, TextOutLineColor, 0f, new Vector2(textSize.X / 2f, 0f),
                        new Vector2(xResolutionScale, yResolutionScale) * scale, maxWidth, false);
                }

                ChatManager.DrawColorCodedString(spriteBatch, font, line, linePosition, TextColor, 0f, new Vector2(textSize.X / 2f, 0f),
                    new Vector2(xResolutionScale, yResolutionScale) * scale, maxWidth, false);
            }
        }
        #endregion
        #region 绘制悬停时的文字（未完成）
        public static void DrawHoverText(
            SpriteBatch spriteBatch,
            string textContent, // 接收原始文本内容
            float xResolutionScale,
            float yResolutionScale,
            float scale,
            Color TextColor,
            Color TextOutLineColor,
            Texture2D textureBG,
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
                wrappedLines = Utils.WordwrapString(textContent, font, (int)actualMaxWidth, 999, out _)
                    .Where(line => !string.IsNullOrEmpty(line))
                    .ToList();
            }
            // 计算基准行高
            float baseLineHeight = font.LineSpacing * scale * lineSpacing;

            // 计算起始位置（屏幕中心 + 偏移量）
            // 在这个方法中，改为鼠标中心
            Vector2 mousePosition = new Vector2(
                Main.mouseX / 2f,
                Main.mouseY / 2f
            );

            for (int i = 0; i < wrappedLines.Count; i++)
            {
                string line = wrappedLines[i];
                if (string.IsNullOrEmpty(line)) continue;

                // 计算当前行位置
                Vector2 linePosition = new Vector2(
                    mousePosition.X,
                    mousePosition.Y + (baseLineHeight * i)
                );

                // 计算文本尺寸
                Vector2 textSize = ChatManager.GetStringSize(font, line, new Vector2(scale));

                // 绘制带描边文本
                Utils.DrawBorderStringFourWay(
                    spriteBatch,
                    font,
                    line,
                    linePosition.X,
                    linePosition.Y,
                    TextColor,
                    TextOutLineColor,
                    new Vector2(textSize.X / 2f, 0f),
                    scale
                );
            }

            spriteBatch.Draw(textureBG, new Vector2(mousePosition.X, mousePosition.Y), null, Color.White, 0f, textureBG.Size() / 2, new Vector2(xResolutionScale * maxWidth + 15, yResolutionScale * baseLineHeight + 15) * scale, SpriteEffects.None, 0f);

            for (int i = 0; i < 5; i++)
            {
                spriteBatch.Draw(textureBG, new Vector2(mousePosition.X, mousePosition.Y), null, Color.White, 0f, textureBG.Size() / 2, new Vector2(xResolutionScale * maxWidth + 15, yResolutionScale * baseLineHeight + 15) * scale, SpriteEffects.None, 0f);
            }
        }
        #endregion
        #region 更好的文字换行
        /// <summary>
        /// 用于自动换行的方法
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="font">字体</param>
        /// <param name="maxWidth">一行最多多少像素</param>
        /// <param name="maxLines">最多多少航</param>
        /// <param name="lineAmount">关联的右侧显示文本</param>
        public static string[] BetterWordwrapString(string text, DynamicSpriteFont font, int maxWidth, int maxLines, out int lineAmount)
        {
            // 创建数组array
            string[] array = new string[maxLines];

            int Lines = 0;

            // 按换行拆分后存入list
            // 把list1拆分后再按空格拆分存入list2
            List<string> list = new List<string>(text.Split('\n'));
            List<string> list2 = new List<string>(list[0].Split(' '));

            // 最多处理maxLines - 1个段落
            for (int i = 1; i < list.Count && i < maxLines; i++)
            {
                list2.Add("\n");
                list2.AddRange(list[i].Split(' '));
            }

            // 遍历list2，分四种情况处理

            // flag为标记新行开始用
            bool flag = true;
            while (list2.Count > 0)
            {

                string text2 = list2[0];
                string text3 = " ";
                if (list2.Count == 1)
                    text3 = "";

                // 如果遇到换行符
                if (text2 == "\n")
                {
                    // 强制换行并增加行号
                    // 若超过最大行数则终止处理

                    array[Lines++] += text2;
                    flag = true;

                    if (Lines >= maxLines)
                        break;

                    list2.RemoveAt(0);
                }
                else if (flag)
                {
                    // 逐字符拼接直到超出宽度
                    // 将剩余部分重新插入列表开头

                    if (font.MeasureString(text2).X > (float)maxWidth)
                    {
                        // 拆分单词逻辑

                        string text4 = text2[0].ToString() ?? "";
                        int num2 = 1;
                        while (font.MeasureString(text4 + text2[num2]).X <= (float)maxWidth)
                        {
                            text4 += text2[num2++];
                        }

                        array[Lines++] = text4;
                        if (Lines >= maxLines)
                            break;

                        list2.RemoveAt(0);
                        list2.Insert(0, text2.Substring(num2));
                    }
                    else
                    {
                        ref string reference = ref array[Lines];
                        reference = reference + text2 + text3;
                        flag = false;
                        list2.RemoveAt(0);
                    }
                }
                else if (font.MeasureString(array[Lines] + text2).X > (float)maxWidth)
                {
                    Lines++;
                    if (Lines >= maxLines)
                        break;

                    flag = true;
                }
                else
                {
                    ref string reference2 = ref array[Lines];
                    reference2 = reference2 + text2 + text3;
                    flag = false;
                    list2.RemoveAt(0);
                }

            }

            lineAmount = Lines;
            if (lineAmount == maxLines)
                lineAmount--;

            return array;
        }
        #endregion
        #region 基础PreDrew绘制
        public static void BaseProjPreDraw(this Projectile proj, Texture2D texture, Color lightColor, float rotOffset = 0f, float scale = 1f, Vector2? drawOffset = null)
        {
            Vector2 drawPosition = proj.Center - Main.screenPosition;
            float drawRotation = proj.rotation + (proj.spriteDirection == -1 ? MathHelper.Pi : 0f);
            Vector2 rotationPoint = texture.Size() / 2f;
            SpriteEffects flipSprite = (proj.spriteDirection * Main.player[proj.owner].gravDir == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.spriteBatch.Draw(texture, drawPosition + (Vector2)drawOffset, null, lightColor, drawRotation + rotOffset, rotationPoint, proj.scale * Main.player[proj.owner].gravDir * scale, flipSprite, 0f);
        }
        #endregion
    }
}
