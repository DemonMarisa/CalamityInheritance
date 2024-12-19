using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Utilities
{
    public static partial class CalamityInheritanceUtils
    {
        //以向右开火/使用为准
        //调用
        //Vector2 targetPosition = Main.MouseWorld;
        //player.itemRotation = CalamityInheritanceUtils.CalculateItemRotation(player, targetPosition, 7);
        public static float CalculateItemRotation(Player player, Vector2 mouseWorld, float rotationOffsetDegrees)
        {
            Vector2 playerPos = player.RotatedRelativePoint(player.MountedCenter, true);
            float xPos = mouseWorld.X - playerPos.X;
            float yPos = mouseWorld.Y - playerPos.Y;
            player.direction = mouseWorld.X >= player.position.X ? 1 : -1;
            Vector2 directionToMouse = new Vector2(xPos, yPos).SafeNormalize(Vector2.UnitX);
            float rotationOffset = MathHelper.ToRadians(rotationOffsetDegrees);
            if (player.direction == 1)
            {
                return directionToMouse.ToRotation() + rotationOffset;
            }
            else
            {
                float flippedRotation = directionToMouse.ToRotation() - MathHelper.Pi;
                return flippedRotation - rotationOffset;
            }
        }
        public static void ExoDebuffs(this NPC target, float multiplier = 1f)
        {
            target.AddBuff(BuffID.Frostburn, 300);
            target.AddBuff(BuffID.OnFire, 300);
            target.AddBuff(BuffID.CursedInferno, 300);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 300);
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
            target.AddBuff(ModContent.BuffType<Plague>(), 300);
        }
    }
}
