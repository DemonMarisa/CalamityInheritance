using Microsoft.Xna.Framework;
using Terraria;

namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
    {
        /// <summary>
        /// 一个固定用来往外扩展出圆形粒子的函数(具体参考圣时之锤)
        /// <param name="pos">1:粒子原始生成位置.</param>
        /// <param name="dustCounts">2:粒子数量.</param>
        /// <param name="dustScale">3:粒子大小.</param>
        /// <param name="dustType">4:粒子类型.</param>
        /// <param name="dustGravity">5:粒子是否受到重力影响.</param>
        /// <param name="xPos">6:粒子水平位置偏移.</param>
        /// <param name="dustAlpha">7:粒子透明度, 默认255.</param>
        /// <param name="yPos">8:粒子垂直位置偏移, 默认0f.</param>
        /// <param name="xVel">9:粒子水平速度, 默认xVel = xPos.</param>
        /// <param name="yVel">10:粒子垂直速度, 默认0f.</param>
        /// </summary>
        public static void DustCircle(Vector2 pos, float dustCounts, float dustScale, int dustType, bool dustGravity,
                                      float xPos, int? dustAlpha = 255, float? yPos = 0f, float? xVel = null,
                                      float? yVel = 0f)
        {
            float xOffset = xPos;
            float yOffset = 0f;
            float xVelocity = xOffset;
            float yVelocity = 0f;
            int alphaVal = 255;
            if(dustAlpha.HasValue) alphaVal = dustAlpha.Value;
            if(yPos.HasValue) yOffset = yPos.Value;
            if(xVel.HasValue) xVelocity = xVel.Value;
            if(yVel.HasValue) yVelocity = yVel.Value;

            float rotArg = 360f / dustCounts;
            for(int i = 0; i < dustCounts; i++)
            {
                float rorate = MathHelper.ToRadians(i * rotArg);
                Vector2 dustPos = new Vector2(xPos, yOffset).RotatedBy(rorate);
                Vector2 dustVelocity = new Vector2(xVelocity, yVelocity).RotatedBy(rorate);
                Dust dust = Dust.NewDustPerfect(pos + dustPos, dustType, new Vector2(dustVelocity.X, dustVelocity.Y), alphaVal, default);
                dust.noGravity = dustGravity;
                dust.velocity = dustVelocity;
                dust.scale = dustScale;
            }
        }
    }
}