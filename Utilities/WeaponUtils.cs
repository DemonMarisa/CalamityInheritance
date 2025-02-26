using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
    {
        //以向右开火/使用为准
        //调用
        //Vector2 targetPosition = Main.MouseWorld;
        //player.itemRotation = CIFunction.CalculateItemRotation(player, targetPosition, 7);
        //还有这一段jb纯屎山，以后有机会要重构
        public static float CalculateItemRotation(Player player, Vector2 mouseWorld, float rotationOffsetDegrees)
        {
            // 获取玩家位置
            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            float xPos = mouseWorld.X - playerCenter.X;
            float yPos = mouseWorld.Y - playerCenter.Y;

            // 计算鼠标相对于玩家的向量
            Vector2 direction = mouseWorld - playerCenter;
            direction = Vector2.Normalize(direction);

            // 更新玩家朝向
            player.direction = mouseWorld.X >= playerCenter.X ? 1 : -1;

            // 计算基础角度
            float baseRotation = (float)Math.Atan2(direction.Y, direction.X);

            // 转换偏移量为弧度
            float offsetRadians = MathHelper.ToRadians(rotationOffsetDegrees);

            player.direction = mouseWorld.X >= player.position.X ? 1 : -1;
            Vector2 directionToMouse = new Vector2(xPos, yPos).SafeNormalize(Vector2.UnitX);
            float rotationOffset = MathHelper.ToRadians(rotationOffsetDegrees);

            if (player.direction == 1) // 向右
            {
                return baseRotation + offsetRadians;
            }
            else // 向左
            {
                if (direction.Y >= 0) // 向左下
                {
                    float flippedRotation = directionToMouse.ToRotation() - MathHelper.Pi;
                    return flippedRotation - rotationOffset;
                }
                else // 向左上
                {
                    return MathHelper.Pi + baseRotation - offsetRadians;
                }
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
