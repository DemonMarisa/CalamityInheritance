using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

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
        // 缓动函数工具类
        // 详见 https://easings.net/zh-cn
        public static class EasingHelper
        {
            // 二次缓入缓出（更平滑的动画）
            public static float EaseInOutQuad(float t)
                => t < 0.5f ? 2f * t * t : 1f - (-2f * t + 2f) * (-2f * t + 2f) / 2f;
            // 指数缓出（适合弹窗出现）
            public static float EaseOutExpo(float t)
                => t == 1f ? 1f : 1f - MathF.Pow(2f, -10f * t);
        }
    }
}
