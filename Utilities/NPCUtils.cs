using CalamityInheritance.NPCs;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
    {
        public static CIGlobalNPC CIMod(this NPC npc)
        {
            return npc.GetGlobalNPC<CIGlobalNPC>();
        }
        /// <summary>
        /// 这个用来去找玩家附近是否存在这个NPC
        /// </summary>
        /// <param name="npcType">NPC单位</param>
        /// <param name="player">玩家单位</param>
        /// <param name="range">最大检索距离</param>
        /// <returns>真:表示有这个npc</returns>
        public static bool IsThereNpcNearby(int npcType, Player player, float range)
        {
            if (npcType <= 0)
                return false;
            int npcIndex = NPC.FindFirstNPC(npcType);
            if (npcIndex != -1)
            {
                NPC npc = Main.npc[npcIndex];
                return npc.active && npc.Distance(player.Center) <= range;
            }
            return false;
        }
        /// <summary>
        /// 使原始单位能够追上你需要的目标单位
        /// 例：大部分boss的追逐AI。boss作为原始单位，玩家作为目标单位
        /// 则这个方法可以用于快速设置boss追赶的速度。
        /// !!!请务必确保这里面的变量是一定可变的
        /// </summary>
        /// <param name="npc">原始单位</param>
        /// <param name="tarSpeedX">目标单位的水平速度</param>
        /// <param name="tarSpeedY">目标单位的垂直速度</param>
        /// <param name="acceleration">当前单位具备的加速度</param>
        public static void TryCatchTraget(NPC npc, float tarSpeedX, float tarSpeedY, float acceleration)
        {
            if (npc.velocity.X < tarSpeedX)
            {
                npc.velocity.X += acceleration;
                if (npc.velocity.X < 0f && tarSpeedX > 0f)
                    npc.velocity.X += acceleration;
            }
            else if (npc.velocity.X > tarSpeedX)
            {
                npc.velocity.X -= acceleration;
                if (npc.velocity.X > 0f && tarSpeedX < 0f)
                    npc.velocity.X -= acceleration;
            }
            if (npc.velocity.Y < tarSpeedY)
            {
                npc.velocity.Y += acceleration;
                if (npc.velocity.Y < 0f && tarSpeedY > 0f)
                    npc.velocity.X += acceleration;
            }
            else if (npc.velocity.Y > tarSpeedY)
            {
                npc.velocity.Y -= acceleration;
                if (npc.velocity.Y > 0f && tarSpeedY < 0f)
                    npc.velocity.Y -= acceleration;
            }
        }
        /// <summary>
        /// 使你指定的npc单位发光R!G!B!
        /// </summary>
        /// <param name="npc">指定npc</param>
        /// <param name="red">R!</param>
        /// <param name="green">G!</param>
        /// <param name="blue">B!</param>
        public static void SetGlow(NPC npc, float red, float green, float blue)
        {
            float lightingPosX = (npc.position.X + npc.width/2)/16;
            float lightingPosY = (npc.position.Y + npc.width/2)/16;
            Lighting.AddLight(new Vector2(lightingPosX, lightingPosY), red, green, blue);
        }
        #region Smooth Movement
        /// <summary>
        /// 增强版平滑移动算法（基于灾厄代码改进）
        /// </summary>
        /// <param name="npc">要移动的NPC</param>
        /// <param name="movementDistanceGateValue">离目标足够近时停止的距离</param>
        /// <param name="distanceFromDestination">NPC离目标多远</param>
        /// <param name="baseVelocity">NPC移动过去的基础速度</param>
        /// <param name="maxLerpDistance">动态调整的最大lerp距离（原灾固定2400）</param>
        /// <param name="velocityMultiplier">最大速度倍率（原灾固定3）</param>
        /// <param name="minStopDistance">完全停止的最小距离</param>
        public static void BetterSmoothMovement(NPC npc, float movementDistanceGateValue, Vector2 distanceFromDestination, float baseVelocity, float acceleration
            , float maxLerpDistance = 2400f, float velocityMultiplier = 3f, float minStopDistance = 5f)
        {
            // 计算结果
            float distance = distanceFromDestination.Length();
            Vector2 direction = distanceFromDestination.SafeNormalize(Vector2.Zero);

            // 距离很近时会停止移动，避免抖动
            if (distance <= minStopDistance)
            {
                npc.velocity = Vector2.Zero;
                return;
            }

            // 速度曲线
            float lerpValue = 1f - (float)Math.Pow(Utils.GetLerpValue(movementDistanceGateValue, maxLerpDistance, distance, true), 0.5);

            // 最小速度：使用距离的平方根实现平滑减速
            float minSpeedCap = Math.Min(baseVelocity, (float)Math.Sqrt(distance * 2));

            // 最大速度：基于加速度动态调整
            float maxSpeedCap = baseVelocity * velocityMultiplier + acceleration * (1 / 60);
            Vector2 maxVelocity = distanceFromDestination / (24f - acceleration); // 加速度越大响应越快
            maxVelocity = maxVelocity.ClampMagnitude(minSpeedCap, maxSpeedCap);

            // 根据距离调整加速度强度（远距离时更强）
            float dynamicAcceleration = acceleration * MathHelper.Lerp(0.6f, 1.4f, lerpValue);

            // 使用三次缓动插值
            float smoothLerp = lerpValue * lerpValue * (3f - 2f * lerpValue);
            Vector2 desiredVelocity = Vector2.Lerp(direction * minSpeedCap, maxVelocity, smoothLerp);

            // 当速度方向与目标方向偏差过大时减速
            if (Vector2.Dot(npc.velocity.SafeNormalize(Vector2.Zero), direction) < 0.7f)
            {
                desiredVelocity *= 0.6f;
                dynamicAcceleration *= 1.5f;
            }

            // ========== 应用速度 ==========
            npc.velocity = Vector2.Lerp(npc.velocity, desiredVelocity, 0.3f);
        }
        #endregion
        #region Smooth Movement
        /// <summary>
        /// Smoother movement for NPCs
        /// </summary>
        /// <param name="npc">The NPC getting the movement change.</param>
        /// <param name="movementDistanceGateValue">The distance where the NPC should stop moving once it's close enough to its destination.</param>
        /// <param name="distanceFromDestination">How far the NPC is from its destination.</param>
        /// <param name="baseVelocity">How quickly the NPC moves towards its destination.</param>
        /// <param name="useSimpleFlyMovement">Whether the NPC should use SimpleFlyMovement to make the movement more affected by acceleration.</param>
        public static void SmoothMovement(NPC npc, float movementDistanceGateValue, Vector2 distanceFromDestination, float baseVelocity, float acceleration, bool useSimpleFlyMovement)
        {
            // Inverse lerp returns the percentage of progress between A and B
            float lerpValue = Utils.GetLerpValue(movementDistanceGateValue, 2400f, distanceFromDestination.Length(), true);

            // Min velocity
            float minVelocity = distanceFromDestination.Length();
            float minVelocityCap = baseVelocity;
            if (minVelocity > minVelocityCap)
                minVelocity = minVelocityCap;

            // Max velocity
            Vector2 maxVelocity = distanceFromDestination / 24f;
            float maxVelocityCap = minVelocityCap * 3f;
            if (maxVelocity.Length() > maxVelocityCap)
                maxVelocity = distanceFromDestination.SafeNormalize(Vector2.Zero) * maxVelocityCap;

            // Set the velocity
            Vector2 desiredVelocity = Vector2.Lerp(distanceFromDestination.SafeNormalize(Vector2.Zero) * minVelocity, maxVelocity, lerpValue);
            if (useSimpleFlyMovement)
                npc.SimpleFlyMovement(desiredVelocity, acceleration);
            else
                npc.velocity = desiredVelocity;
        }
        #endregion
    }
}
