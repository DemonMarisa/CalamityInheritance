using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using static CalamityInheritance.Utilities.CIFunction;
using Terraria.Audio;
using Terraria.ID;
using CalamityInheritance.CIPlayer;

namespace CalamityInheritance.UI
{
    public abstract class DraedonsPanelUI : CalPopupGUI
    {
        public int Page = 0;
        public int ArrowClickCooldown;
        public bool HoveringOverBook = false;
        public int TotalLinesPerPage => 16;
        public abstract int TotalPages { get; }

        public const int TextStartOffsetX = 40;

        private bool _leftArrowHovered;
        private bool _rightArrowHovered;

        private bool _AllowPageChange = false;
        private bool _AllowPageChange2;
        // 重置翻页判定的时间
        // 当鼠标按下后两帧后，允许翻页将为false，避免按下后移动到翻页箭头后又触发一次
        private int _AllowPageChangeReTime;

        public abstract void PageDraw(SpriteBatch spriteBatch);
        public abstract void StateSaving();
        public override void Update()
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer cIPlayer = player.CIMod();

            StateSaving();

            if (_AllowPageChangeReTime > 0)
            {
                _AllowPageChangeReTime--;
                _AllowPageChange2 = true;
            }
            else
                _AllowPageChange2 = false;

            if (Active)
            {
                if (FadeTime < FadeTimeMax)
                    FadeTime++;
            }
            else if (FadeTime > 0)
            {
                FadeTime--;
            }

            if (Main.mouseLeft && !HoveringOverBook && FadeTime >= 30)
            {
                Page = 0;
                Active = false;
            }

            //必须先按下才能检测释放事件，否则会触发两次点击事件（鼠标按下和移动都会触发）
            if(Main.mouseLeft)
            {
                _AllowPageChange = true;
                _AllowPageChange2 = false;
                _AllowPageChangeReTime = 2;
            }

            // 检测鼠标释放事件
            if (Main.mouseLeftRelease && _AllowPageChange2)
            {
                if (_leftArrowHovered && Page > 0 && _AllowPageChange)
                {
                    Page--;
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    _AllowPageChange = false;
                }
                else if (_rightArrowHovered && Page < TotalPages - 1 && _AllowPageChange)
                {
                    Page++;
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    _AllowPageChange = false;
                }
            }

            if (ArrowClickCooldown > 0)
                ArrowClickCooldown--;
            HoveringOverBook = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D pageTexture = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/DraedonsLogPage").Value;
            float xScale = MathHelper.Lerp(0.004f, 1f, FadeTime / (float)FadeTimeMax);
            Vector2 scale = new Vector2(xScale, 1f) * new Vector2(Main.screenWidth, Main.screenHeight) / pageTexture.Size();
            // UIY轴缩放
            scale.Y *= 2;
            // UI缩放
            scale *= 0.5f;

            float xResolutionScale = Main.screenWidth / 2560f;
            float yResolutionScale = Main.screenHeight / 1440f;
            // ?你为啥再乘一遍
            float bookScale = 0.75f;
            scale *= bookScale;
            // 修改页面滑动动画，同样优化为曲线
            float yPageTop = MathHelper.Lerp(Main.screenHeight * 2,Main.screenHeight * 0.12f,EasingHelper.EaseInOutQuad(FadeTime / (float)FadeTimeMax));

            Rectangle mouseRectangle = new Rectangle((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 2, 2);
            float drawPositionX = Main.screenWidth * 0.5f;
            Vector2 drawPosition = new Vector2(drawPositionX, yPageTop);
            Rectangle pageRectangle = new Rectangle((int)drawPosition.X - (int)(pageTexture.Width * scale.X), (int)yPageTop, (int)(pageTexture.Width * scale.X) * 2, (int)(pageTexture.Height * scale.Y));
            for (int i = 0; i < 2; i++)
            {
                spriteBatch.Draw(pageTexture, drawPosition, null, Color.White, 0f, new Vector2(i == 0f ? pageTexture.Width : 0f, 0f), scale, i == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

                if (!HoveringOverBook)
                    HoveringOverBook = mouseRectangle.Intersects(pageRectangle);
            }

            // Create text and arrows.
            if (FadeTime >= FadeTimeMax && Active)
            {
                // 防止页数计算溢出
                if (Page < 0)
                    Page = 0;
                if (Page > TotalPages)
                    Page = TotalPages;

                // 绘制箭头
                DrawArrows(spriteBatch, xResolutionScale, yResolutionScale, yPageTop + 540 * yResolutionScale, mouseRectangle);
                // 请查看QolPanel
                PageDraw(spriteBatch);
            }
        }
        // 绘制箭头
        public void DrawArrows(SpriteBatch spriteBatch, float xResolutionScale, float yResolutionScale, float yPageBottom, Rectangle mouseRectangle)
        {

            float LeftarrowScale = 1f;

            float RightarrowScale = 1f;

            Texture2D leftArrowTexture = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/DraedonsLogArrow").Value;
            Texture2D rightArrowTexture = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/DraedonsLogArrow").Value;

            // 左箭头处理
            if (Page > 0)
            {
                //鼠标按下时缩小
                if (Main.mouseLeft && _leftArrowHovered)
                {
                    LeftarrowScale *= 0.9f;
                }
                // 绘制坐标
                Vector2 drawPosition = new Vector2(Main.screenWidth / 2 - 625f, yPageBottom);
                Rectangle arrowRect = new Rectangle(
                    (int)(drawPosition.X - leftArrowTexture.Width * xResolutionScale * LeftarrowScale / 2),
                    (int)(drawPosition.Y - leftArrowTexture.Height * yResolutionScale * LeftarrowScale / 2),
                    (int)(leftArrowTexture.Width * xResolutionScale * LeftarrowScale),
                    (int)(leftArrowTexture.Height * yResolutionScale * LeftarrowScale)
                );

                // 检测悬停
                bool isHovering = arrowRect.Intersects(mouseRectangle);
                _leftArrowHovered = isHovering;

                // 动态切换纹理
                if (isHovering)
                {
                    leftArrowTexture = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/DraedonsLogArrowHover").Value;
                    Main.blockMouse = true;
                }


                spriteBatch.Draw(leftArrowTexture, drawPosition, null, Color.White, 0f,
                    leftArrowTexture.Size() / 2,  // 改为中心锚点
                    new Vector2(xResolutionScale, yResolutionScale) * LeftarrowScale, SpriteEffects.FlipHorizontally,0f);


            }

            if (Page < TotalPages - 1)
            {
                //鼠标按下时缩小
                if (Main.mouseLeft && _rightArrowHovered)
                {
                    RightarrowScale *= 0.9f;
                }

                // 注释同上，只是复制了一遍，方向相反
                Vector2 drawPosition = new Vector2(Main.screenWidth / 2 + 625f, yPageBottom);
                Rectangle arrowRect = new Rectangle(
                    (int)(drawPosition.X - rightArrowTexture.Width * xResolutionScale * RightarrowScale / 2),
                    (int)(drawPosition.Y - rightArrowTexture.Height * yResolutionScale * RightarrowScale / 2),
                    (int)(rightArrowTexture.Width * xResolutionScale * RightarrowScale),
                    (int)(rightArrowTexture.Height * yResolutionScale * RightarrowScale)
                );

                bool isHovering = arrowRect.Intersects(mouseRectangle);
                _rightArrowHovered = isHovering;

                if (isHovering)
                {
                    rightArrowTexture = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/DraedonsLogArrowHover").Value;
                    Main.blockMouse = true;
                }
                spriteBatch.Draw(rightArrowTexture,drawPosition,null,Color.White,0f,rightArrowTexture.Size() / 2,new Vector2(xResolutionScale, yResolutionScale) * RightarrowScale, SpriteEffects.None,0f);
            }
        }

    }
}
