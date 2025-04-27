using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Core;
using CalamityInheritance.System.Configs;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Terraria.Localization;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace CalamityInheritance.UI.QolPanelTotal
{
    #region 存储右侧跳过按钮数据的结构体
    public struct HeadPageData
    {
        public SpriteBatch spriteBatch;
        public Texture2D headPageArrow;
        public Texture2D headPageArrowHover;
        public float scale;
        public bool flipHorizontally;
    }
    #endregion
    public partial class QolPanel
    {
        public int PageRightCenter = 300;
        public void Page1Draw(SpriteBatch spriteBatch)
        {

            #region 左侧头图绘制
            #region 绘制数据
            // 头图贴图
            Texture2D HeadPage = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/HeadPage/HeadPageStyle1").Value;
            // 绘制位置
            float drawPositionX = Main.screenWidth * 0.5f;
            Vector2 drawPosition = new Vector2(drawPositionX, Main.screenHeight * 0.5f);
            // 绘制源
            Vector2 pageOriginLeft = new(HeadPage.Width, HeadPage.Height / 2);
            // UI缩放
            Vector2 scale = new(0.5f, 0.5f);
            scale.X *= scaleX;
            scale.Y *= scaleY;
            #endregion
            spriteBatch.Draw(HeadPage, drawPosition, null, Color.White, 0f, pageOriginLeft, scale, SpriteEffects.None, 0f);
            #endregion

            #region 右侧跳过绘制
            #region 绘制数据
            // 箭头材质与数据应用
            Texture2D HeadPageArrow = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/HeadPage/TextArrow").Value;
            Texture2D HeadPageArrowHover = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/HeadPage/TextArrowHover").Value;
            HeadPageData genericHeadPageData = GetSkipBtnData(
                spriteBatch, HeadPageArrow, HeadPageArrowHover,
                1f, false);
            #endregion
            #region 边框的绘制数据
            Texture2D textOutLine = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/HeadPage/TextOutLine").Value;
            Texture2D TextLine = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/HeadPage/TextLine").Value;
            #region 本地化文字
            // 文字
            string fastSkip = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.HeadText");
            string HeadTextLore = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.HeadText1");
            string HeadTextLevel = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.HeadText2");
            #endregion
            #endregion
            int TextOffsetX = 46;
            int PageRightFirstLine = -270;
            #region 绘制最上面的文字
            Texture2D LevelHead = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/LevelSystem/LevelHead").Value;
            CIFunction.DrawTextNoLine(spriteBatch, fastSkip, 1f, 1f, PageRightCenter + 16, -368, 1.4f, TextColor, Color.DarkSlateGray, 400f, 0f);
            NorDrawImage(spriteBatch, LevelHead, PageRightCenter, -344);
            #endregion
            #region 跳到lore界面
            // 绘制跳转到Lore界面的按钮
            // -321为第一行的对应偏移
            NorDrawImage(spriteBatch, textOutLine, PageRightCenter, PageRightFirstLine);
            DrawSkipBtn(genericHeadPageData, PageRightCenter + 215, PageRightFirstLine, ref Page, 1); // 215为额外的偏移，就是贴图宽度的一半左右
            DrawImage(spriteBatch, TextLine, PageRightCenter, PageRightFirstLine, 388 , TextLine.Height, -110);// 默认截取387比较合理 110为左侧的文字像素占用
            // 140为文字的X位置，-337是Y位置，我也不知道为啥337比较合适
            // 改为了常规数据后应用偏移，-321 - 16 = -337
            NewDrawTextLeftOrigin(spriteBatch, HeadTextLore, 1f, 1f, TextOffsetX, PageRightFirstLine + 5, 1.5f, TextColor, Color.DarkSlateGray, 400f, 0f);
            #endregion

            #region 跳到等级界面
            // 绘制跳转到Lore界面的按钮
            // -321为第一行的对应偏移
            NorDrawImage(spriteBatch, textOutLine, PageRightCenter, GetPos(2, 135));
            DrawSkipBtn(genericHeadPageData, PageRightCenter + 215, GetPos(2, 135), ref Page, 3); // 215为额外的偏移，就是贴图宽度的一半左右
            DrawImage(spriteBatch, TextLine, PageRightCenter, GetPos(2, 135), 388, TextLine.Height, -110);// 默认截取388比较合理 60为左侧的文字像素占用
            // 93为文字的偏移
            // 135为一行的偏移
            // 我也不知道为啥跑统一偏移就会有很大的误差
            NewDrawTextLeftOrigin(spriteBatch, HeadTextLevel, 1f, 1f, TextOffsetX, GetPos(2, 135) + 5, 1.5f, TextColor, Color.DarkSlateGray, 400f, 0f);
            #endregion

            #endregion
        }
        #region 结构体方法
        /// <summary>
        /// 存储跳过的按钮的数据
        /// </summary>
        public HeadPageData GetSkipBtnData(SpriteBatch spriteBatch, Texture2D headPageArrow, Texture2D headPageArrowHover, float scale, bool canFlip)
        {
            HeadPageData newData;
            newData.spriteBatch = spriteBatch;
            newData.headPageArrow = headPageArrow;
            newData.headPageArrowHover = headPageArrowHover;
            newData.scale = scale;
            newData.flipHorizontally = canFlip;
            return newData;
        }
        #endregion
        #region 封装
        #region 位置获取
        // 只适用于第一页的快速位置获取
        // 第一行开始
        public static int GetPos(int Line, int LineSpace)
        {
            return (-270 + Line + 1 * LineSpace);
        }
        #endregion
        #region 绘制箭头
        /// <summary>
        /// 用于绘制跳过按钮的函数
        /// </summary>
        public static void DrawSkipBtn(HeadPageData thisUI, float xPageBottom, float yPageBottom, ref int skipWhoPage, int skipPage)
        {
            #region 数据
            // 缩放
            float scale = thisUI.scale;
            // 材质
            Texture2D targetTexture = thisUI.headPageArrow;
            // 玩家获取
            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer cIPlayer = player.CIMod();
            // 绘制坐标，以屏幕中心为原点
            Vector2 drawPosition = new Vector2(Main.screenWidth / 2 + xPageBottom, Main.screenHeight / 2 + yPageBottom);
            // 绘制源为目标中心 绘制区域为全部区域
            Rectangle arrowRect = new Rectangle(
                (int)(drawPosition.X - thisUI.headPageArrow.Width * scale + 50),// 添加50像素的偏移让判定更准确
                (int)(drawPosition.Y - thisUI.headPageArrow.Height * scale / 2),
                (int)(thisUI.headPageArrow.Width * scale),
                (int)(thisUI.headPageArrow.Height * scale)
            );
            // 鼠标的碰撞
            Rectangle mouseRectangle = new((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 2, 2);
            #endregion
            // 检测悬停
            bool isHovering = arrowRect.Intersects(mouseRectangle);

            #region 检测到碰撞时执行
            // 动态切换纹理
            // 悬停时切换纹理为 2
            if (isHovering)
            {
                targetTexture = thisUI.headPageArrowHover;
                // 未悬停时恢复为1（默认贴图
                // 按下时缩小
                if (Main.mouseLeft)
                {
                    scale *= 0.9f;
                    Main.blockMouse = true;
                    cIPlayer.wasMouseDown = true;
                }
                if (cIPlayer.wasMouseDown && !Main.mouseLeft && isHovering)
                {
                    skipWhoPage = skipPage;
                    cIPlayer.wasMouseDown = false;
                }
            }
            #endregion

            // 重置材质
            if (!isHovering)
                targetTexture = thisUI.headPageArrow;

            // 改为中心锚点
            // 偏移95个像素，以真正的贴图中央为锚点
            // 180为额外的偏移，就是箭头贴图的中心左右
            Vector2 newOrigin = new(targetTexture.Width / 2 + 180, targetTexture.Height / 2);
            thisUI.spriteBatch.Draw(targetTexture, drawPosition, null, Color.White, 0f, newOrigin, new Vector2(1f, 1f) * scale, thisUI.flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
        #endregion
        #region 可变绘制区域的图片绘制
        public static void DrawImage(SpriteBatch spriteBatch,Texture2D texture,float xPageBottom, float yPageBottom, int DrawWidth, int DrawHeight, int startPosX)
        {
            Texture2D targetTexture = texture;
            // 绘制坐标
            Vector2 drawPosition = new Vector2(Main.screenWidth / 2 + xPageBottom, Main.screenHeight / 2 + yPageBottom);
            Rectangle drawArea = new Rectangle(startPosX, 0, DrawWidth, DrawHeight);
            // 改为左上角为锚点
            spriteBatch.Draw(targetTexture, drawPosition, drawArea, Color.White, 0f, targetTexture.Size() / 2, new Vector2(1.2f, 1.2f), SpriteEffects.None, 0f);
        }
        public static void NorDrawImage(SpriteBatch spriteBatch, Texture2D texture, float xPageBottom, float yPageBottom)
        {
            Texture2D targetTexture = texture;
            // 绘制坐标
            Vector2 drawPosition = new Vector2(Main.screenWidth / 2 + xPageBottom, Main.screenHeight / 2 + yPageBottom);
            // 改为中心锚点
            spriteBatch.Draw(targetTexture, drawPosition, null, Color.White, 0f, targetTexture.Size() / 2, new Vector2(1.2f, 1.2f), SpriteEffects.None, 0f);
        }
        #endregion
        #region 绘制文字
        public static void NewDrawTextLeftOrigin(
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

                for (int j = 0; j < 4; j++)
                {
                    ChatManager.DrawColorCodedString(spriteBatch, font, line, linePosition, TextOutLineColor, 0f, new Vector2(0f, textSize.Y / 2f),
                        new Vector2(xResolutionScale, yResolutionScale) * scale, maxWidth, false);
                }

                ChatManager.DrawColorCodedString(spriteBatch, font, line, linePosition, TextColor, 0f, new Vector2(0f, textSize.Y / 2f),
                    new Vector2(xResolutionScale, yResolutionScale) * scale, maxWidth, false);
            }
        }
        #endregion
        #endregion
    }
}
