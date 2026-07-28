using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {        /// <summary>
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
    }
}
