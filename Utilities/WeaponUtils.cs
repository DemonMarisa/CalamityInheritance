using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
    {
        //以向右开火/使用为准
        //调用
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

        public static void BetterSwing(Player player)
        {
            float xOffset = 6f;
            float yOffset = -10f;
            if (player.itemAnimation < player.itemAnimationMax * 0.333f)
                yOffset = 4f;
            else if (player.itemAnimation >= player.itemAnimationMax * 0.666f)
                xOffset = -4f;
            player.itemLocation.X = player.Center.X + xOffset * player.direction;
            player.itemLocation.Y = player.MountedCenter.Y + yOffset;
            if(player.gravDir < 0)
                player.itemLocation.Y = player.Center.Y + (player.position.Y - player.itemLocation.Y);
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
            target.AddBuff(ModContent.BuffType<GlacialState>(), 60);
        }
        public static void ScalDebuffs(this Player target, int AbyssalFlamesduration, int VulnerabilityHexLegacyduration, int Horrorduration)
        {
            target.AddBuff(ModContent.BuffType<AbyssalFlames>(), AbyssalFlamesduration, true);
            target.AddBuff(ModContent.BuffType<VulnerabilityHexLegacy>(), VulnerabilityHexLegacyduration, true);
            if (Horrorduration > 1)
                target.AddBuff(ModContent.BuffType<Horror>(), Horrorduration, true);
        }
        /// <summary>
        /// 让原版的手持也可以像手持弹幕一样旋转<br/>
        /// 随便找一个每帧调用的方法调用即可<br/>
        /// </summary>
        public static void NoHeldProjUpdateAim(Player player, float rotationOffset = 0f, float rotationSpeed = 1f)
        {
            player.ChangeDir(Math.Sign((player.LocalMouseWorld() - player.Center).X));

            Vector2 aimVect = player.LocalMouseWorld() - player.Center;
            aimVect.SafeNormalize(Vector2.UnitX);

            float targetRotation = aimVect.ToRotation();

            if(player.LocalMouseWorld().X < player.Center.X)
                player.itemRotation = player.itemRotation.AngleLerp(targetRotation - MathHelper.ToRadians(rotationOffset) + MathHelper.Pi, rotationSpeed);
            else
                player.itemRotation = player.itemRotation.AngleLerp(targetRotation + MathHelper.ToRadians(rotationOffset), rotationSpeed);
        }
    }
}
