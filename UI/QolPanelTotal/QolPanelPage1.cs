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
            Texture2D InvisibleUI = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/InvisibleUI").Value;
            // 文字
            string HeadText = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.HeadText1");
            #endregion

            // 调到lore界面
            // 绘制跳转到Lore界面的按钮
            // -321为第一行的对应偏移
            NorDrawImage(spriteBatch, textOutLine, PageRightCenter, -321);
            DrawSkipBtn(genericHeadPageData, PageRightCenter + 215, -321, ref Page, 1); // 215为额外的偏移，就是贴图宽度的一半左右
            DrawImage(spriteBatch, TextLine, PageRightCenter, -321, 388 , TextLine.Height, -110);// 默认截取387比较合理 110为左侧的文字像素占用
            // 140为文字的X位置，-337是Y位置，我也不知道为啥337比较合适
            CIFunction.DrawText(spriteBatch, HeadText, 1f, 1f, 140, -337, 1.6f, TextColor, Color.DarkSlateGray, InvisibleUI, 0, 400f, 0f);

            //DrawSkipBtn(genericHeadPageData, CIConfig.Instance.UIX, CIConfig.Instance.UIY, ref Page, 3);
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
        #region 绘制封装
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
        #endregion
    }
}
