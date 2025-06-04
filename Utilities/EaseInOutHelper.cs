using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
    {
        /// <summary>
        /// 缓动函数工具类
        ///  变量 t 表示 0（动画开始）到 1（动画结束）范围内的值。
        /// 详见 https://easings.net/zh-cn
        /// </summary>
        public static class EasingHelper
        {
            // 二次缓入缓出
            public static float EaseInOutQuad(float t)
                => t < 0.5f ? 2f * t * t : 1f - (-2f * t + 2f) * (-2f * t + 2f) / 2f;

            // 指数缓出
            public static float EaseOutExpo(float t)
                => t == 1f ? 1f : 1f - MathF.Pow(2f, -10f * t);

            // 指数缓入缓出
            public static float EaseInOutExpo(float t)
                => t < 0.5f ? 2 * t * t : 1 - MathF.Pow(-2 * t + 2, 2) / 2;

            public static float EaseOutBack(float t)
            {
                if (t == 1)
                    return 1;
                const float c1 = 1.70158f;
                const float c3 = c1 + 1;

                return (float)(1 + c3 * Math.Pow(t - 1, 3) + c1 * Math.Pow(t - 1, 2));
            }

            public static float EaseInBack(float t)
            {
                if (t == 1)
                    return 1;
                const float c1 = 1.70158f;
                const float c3 = c1 + 1;

                return c3 * t * t * t - c1 * t * t;
            }
        }

    }
}
