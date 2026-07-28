using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        public static float TryGetVectorMud(float distanceX, float distanceY)
        {
            return (float)Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
        }
        public static void DustCircle(Vector2 pos, float dustCounts, float dustScale, int dustType, bool dustGravity,
                                      float xPos, int? dustAlpha = 255, float? yPos = 0f, float? xVel = null,
                                      float? yVel = 0f)
        {
            float xOffset = xPos;
            float yOffset = 0f;
            float xVelocity = xOffset;
            float yVelocity = 0f;
            int alphaVal = 255;
            if (dustAlpha.HasValue) alphaVal = dustAlpha.Value;
            if (yPos.HasValue) yOffset = yPos.Value;
            if (xVel.HasValue) xVelocity = xVel.Value;
            if (yVel.HasValue) yVelocity = yVel.Value;

            float rotArg = 360f / dustCounts;
            for (int i = 0; i < dustCounts; i++)
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
