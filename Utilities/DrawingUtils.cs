using CalamityInheritance.CIPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

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
        /// <summary>
        /// 缓动函数工具类
        /// 详见 https://easings.net/zh-cn
        /// </summary>
        public static class EasingHelper
        {
            // 二次缓入缓出（更平滑的动画）
            public static float EaseInOutQuad(float t)
                => t < 0.5f ? 2f * t * t : 1f - (-2f * t + 2f) * (-2f * t + 2f) / 2f;
            // 指数缓出（适合弹窗出现）
            public static float EaseOutExpo(float t)
                => t == 1f ? 1f : 1f - MathF.Pow(2f, -10f * t);
        }
        /// <summary>
        /// 在屏幕上绘制可交互图片的方法
        /// 注：一定要手动操作buttonTexChange的值，方法只内置了悬停后必定切换为2，点击前的1与3的状态标记必须手动切换
        /// </summary>
        /// <param name="falseTexture">为假时的材质</param>
        /// <param name="falseHoveredTexture">为假时鼠标悬停上方时的材质</param>
        /// <param name="trueTexture">鼠标按下后的材质</param>
        /// <param name="trueHoveredTexture">鼠标按下后的材质</param>
        /// <param name="spriteBatch">绘制</param>
        /// <param name="Scale">缩放</param>
        /// <param name="xResolutionScale">X 缩放</param>
        /// <param name="yResolutionScale">Y 缩放</param>
        /// <param name="xPageBottom">以屏幕中心为锚点的 X 偏移</param>
        /// <param name="yPageBottom">以屏幕中心为锚点的 Y 偏移</param>
        /// <param name="buttonCount">标记使用哪个贴图的部分，当为1时，是默认贴图，为2时，是鼠标悬浮的贴图，为3时，是点击后的贴图</param>
        /// <param name="mouseRectangle">点击判定</param>
        /// <summary>

        // 这一段接受数量也太多了，我只能这么写了（
        public static void DrawButtom(
          SpriteBatch spriteBatch,
          Texture2D falseTexture,
          Texture2D falseHoveredTexture,
          Texture2D trueTexture,
          Texture2D trueHoveredTexture,
          Texture2D unavailableTexture,
          float Scale,
          float xResolutionScale,
          float yResolutionScale,
          float xPageBottom,
          float yPageBottom,
          ref int buttonCount,
          ref int UIID,
          ref bool available,
          Rectangle mouseRectangle
        )
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
                if (cIPlayer.wasMouseDown)
                {
                    // 释放瞬间：切换永久状态
                    if (!isMouseDown && buttonCount == 2)
                    {
                        buttonCount = 3;
                        cIPlayer.wasMouseDown = false;
                    }
                    if (!isMouseDown && buttonCount == 4)
                    {
                        // 在1和3之间切换
                        buttonCount = 1;
                        cIPlayer.wasMouseDown = false;
                    }
                }
            }
            
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
            spriteBatch.Draw(targetTexture, drawPosition, null, Color.White, 0f,targetTexture.Size() / 2,new Vector2(xResolutionScale, yResolutionScale) * scale, SpriteEffects.FlipHorizontally, 0f);
        }

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
            ref bool available
            )
        {
            float scale = Scale;
            Texture2D targetTexture = texture;

            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer cIPlayer = player.CIMod();

            // 绘制坐标
            Vector2 drawPosition = new Vector2(Main.screenWidth / 2 + xPageBottom, Main.screenHeight / 2 + yPageBottom);

            if (!available)
                targetTexture = unavailableTexture;

            // 改为中心锚点
            spriteBatch.Draw(targetTexture, drawPosition, null, Color.White, 0f, targetTexture.Size() / 2, new Vector2(xResolutionScale, yResolutionScale) * scale, SpriteEffects.FlipHorizontally, 0f);
        }
    }
}
