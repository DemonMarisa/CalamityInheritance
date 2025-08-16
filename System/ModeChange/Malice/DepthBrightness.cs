using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.World;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.System.ModeChange.Malice
{
    public class DepthBrightnessSystem : ModSystem
    {
        public override void ModifyLightingBrightness(ref float brightness)
        {
            Player player = Main.player[Main.myPlayer];
            CIWorld world = ModContent.GetInstance<CIWorld>();

            if (player.ZoneUnderworldHeight)
                return;

            if (world.Malice || CIServerConfig.Instance.WeatherChange)
            {
                // 获取玩家当前Y坐标
                double playerDepth = Main.LocalPlayer.Center.Y;

                // 深度
                //double surfaceDepth = (float)(Main.worldSurface * 16.0f);   // 地表深度
                double cavernDepth = (float)(Main.rockLayer * 16.0f);      // 洞穴层开始
                float underworldDepth = Main.maxTilesY * 16.0f - 1600f; // 地狱层开始

                // 计算深度系数，0表示地表，1表示地狱
                float depthFactor = MathHelper.Clamp((float)((playerDepth - cavernDepth) / (underworldDepth - cavernDepth)), 0f, 1f);

                // 调整亮度衰减
                float brightnessMultiplier = 1f - MathHelper.SmoothStep(0f, 0.20f, (float)depthFactor);

                // 保留至少80%的亮度
                brightnessMultiplier = MathHelper.Clamp(brightnessMultiplier, 0.90f, 1f);

                // 应用亮度调整
                brightness *= brightnessMultiplier;
            }
        }

    }
}
