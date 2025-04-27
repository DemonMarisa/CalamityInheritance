using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using static CalamityInheritance.Utilities.CIFunction;

namespace CalamityInheritance.UI
{
    public abstract class CalPopupGUI
    {
        // 当前淡入/淡出进度帧数
        public int FadeTime;
        // 是否处于激活状态
        public bool Active;
        // 淡入淡出总帧数（默认30帧）
        public virtual int FadeTimeMax { get; set; } = 30;

        // 原有代码
        public virtual void Update()
        {
            // 淡入逻辑
            if (Active)
            {
                if (FadeTime < FadeTimeMax)
                    FadeTime++;
            }
            // 淡出逻辑
            else if (FadeTime > 0)
                FadeTime--;

            // 点击外部区域关闭弹窗
            if (Main.mouseLeft && Main.mouseLeftRelease && !Main.blockMouse && FadeTime >= FadeTimeMax)
            {
                Main.mouseLeftRelease = false;
                Main.mouseLeft = false;
                Active = false;
            }
        }
        // 获取弹窗顶部的Y坐标（带滑动动画）
        // DemonMarisa: 改进了淡入淡出动画的实现方式，使其更加平滑。
        // public float GetYTop() => EasingHelper.EaseInOutQuad(Main.screenHeight * 2, Main.screenHeight * 0.25f, FadeTime / (float)FadeTimeMax);
        public float GetYTop()
        {
            float progress = EasingHelper.EaseInOutQuad(FadeTime / (float)FadeTimeMax);
            return MathHelper.Lerp(Main.screenHeight * 2, Main.screenHeight * 0.25f, progress);
        }

        // 计算分辨率适配的缩放比例
        public Vector2 GetScreenAdjustedScale(float textureScale, Texture2D drawTexture)
        {
            float progress = EasingHelper.EaseOutExpo(FadeTime / (float)FadeTimeMax);
            float xScale = MathHelper.Lerp(0.004f, 1f, progress);
            // 同上，改进了淡入淡出动画的实现方式，使其更加平滑。
            Vector2 scale = new Vector2(xScale, 1f) * new Vector2(Main.screenWidth, Main.screenHeight) / drawTexture.Size();
            scale *= 0.25f;
            scale *= textureScale;
            return scale;
        }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
