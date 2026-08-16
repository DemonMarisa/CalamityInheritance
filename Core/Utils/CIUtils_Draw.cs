using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        public static Color MultiplyRGB(this Color firstColor, Color secondColor) => new Color((byte)((float)(firstColor.R * secondColor.R) / 255f), (byte)((float)(firstColor.G * secondColor.G) / 255f), (byte)((float)(firstColor.B * secondColor.B) / 255f));
        /// <summary>         
        /// Returns a color lerp that allows for smooth transitioning between two given colors           
        /// </summary>           
        /// <param name="firstColor">The first color you want it to switch between</param>           
        /// <param name="secondColor">The second color you want it to switch between</param>          
        /// <param name="seconds">How long you want it to take to swap between colors</param>
        public static Color ColorSwap(Color firstColor, Color secondColor, float seconds)
        {
            double timeMult = (double)(MathHelper.TwoPi / seconds);
            float colorMePurple = (float)((Math.Sin(timeMult * Main.GlobalTimeWrappedHourly) + 1) * 0.5f);
            return Color.Lerp(firstColor, secondColor, colorMePurple);
        }
        public static Color MulticolorLerp(float increment, params Color[] colors)
        {
            increment %= 0.999f;
            int currentColorIndex = (int)(increment * colors.Length);
            Color currentColor = colors[currentColorIndex];
            Color nextColor = colors[(currentColorIndex + 1) % colors.Length];
            return Color.Lerp(currentColor, nextColor, increment * colors.Length % 1f);
        }
    }
}
