using System;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;

namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
    {
        /// <summary>
        /// 从灾厄那里搬过来的跟踪算法，加了一个角度限制.
        /// <param name="projectile">弹幕类型.</param>
        /// <param name="ignoreTiles">是否无视墙壁.</param>
        /// <param name="distanceRequired">跟踪范围.</param>
        /// <param name="homingVelocity">跟踪速度.</param>
        /// <param name="inertia">惯性.</param>
        /// <param name="HomeInCD">扫描CD（不写默认30帧）.</param>
        /// <param name="maxAngleChange">角度限制（不写默认不限制）.</param>
        /// </summary>
        public static void HomeInOnNPC(Projectile projectile, bool ignoreTiles, float distanceRequired, float homingVelocity, float inertia, float? maxAngleChange = null, int? HomeInCD = 30)
        {
            if (!projectile.friendly)
                return;

            HomeInCD++;

            // Set amount of extra updates.
            if (projectile.Calamity().defExtraUpdates == -1)
                projectile.Calamity().defExtraUpdates = projectile.extraUpdates;

            Vector2 destination = projectile.Center;
            float maxDistance = distanceRequired;
            bool locatedTarget = false;

            //寻找距离最近的目标
            float npcDistCompare = 25000f;
            int targetIndex = -1;

            if(locatedTarget == false && HomeInCD.Value % HomeInCD.Value == 0)
            {
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    float extraDistance = (npc.width / 2) + (npc.height / 2);
                    if (!npc.CanBeChasedBy(projectile, false) || !projectile.WithinRange(npc.Center, maxDistance + extraDistance))
                        continue;

                    float currentNPCDist = Vector2.Distance(npc.Center, projectile.Center);
                    if ((currentNPCDist < npcDistCompare) && (ignoreTiles || Collision.CanHit(projectile.Center, 1, 1, npc.Center, 1, 1)))
                    {
                        npcDistCompare = currentNPCDist;
                        targetIndex = npc.whoAmI;
                    }
                }
            }

            if (targetIndex != -1)
            {
                destination = Main.npc[targetIndex].Center;
                locatedTarget = true;
            }

            if (locatedTarget)
            {
                // Increase amount of extra updates to greatly increase homing velocity.
                projectile.extraUpdates = projectile.Calamity().defExtraUpdates + 1;

                // 计算制导向量
                Vector2 homeDirection = (destination - projectile.Center).SafeNormalize(Vector2.UnitY);
                Vector2 newVelocity = (projectile.velocity * inertia + homeDirection * homingVelocity) / (inertia + 1f);

                // If maxAngleChange is provided, limit the angle change
                if (maxAngleChange.HasValue)
                {
                    float currentAngle = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
                    float targetAngle = (float)Math.Atan2(newVelocity.Y, newVelocity.X);
                    float angleDifference = MathHelper.WrapAngle(targetAngle - currentAngle);

                    // 添加了转换角度为弧度，以便进行比较和限制。如果角度差大于最大允许的角度变化，则将速度限制在最大角度变化的范围内。
                    // 不需要手动转换了
                    float maxChangeRadians = MathHelper.ToRadians(maxAngleChange.Value);
                    if (Math.Abs(angleDifference) > maxChangeRadians)
                    {
                        float clampedAngle = currentAngle + Math.Sign(angleDifference) * maxChangeRadians;
                        float speed = newVelocity.Length();
                        newVelocity = new Vector2((float)Math.Cos(clampedAngle), (float)Math.Sin(clampedAngle)) * speed;
                    }
                }

                projectile.velocity = newVelocity;
            }
            else
            {
                // Set amount of extra updates to default amount.
                projectile.extraUpdates = projectile.Calamity().defExtraUpdates;
            }
        }
        ///<summary>
        ///用于回旋镖的返程AI.
        ///注:这一函数不会实现回旋镖返程至玩家身上时消失的效果
        ///<param name="player">玩家本身</param>
        ///<param name="boomerang">回旋镖本身.</param>
        ///<param name="rSpeed">返程速度.</param>
        ///<param name="acceleration">返程加速度.</param>
        ///<param name="minKillRangeBoomerangToPlr">使回旋镖执行kill()之前回旋镖与玩家之间的最小距离.默认3000f</param>
        ///</summary>
        public static void BoomerangReturningAI(Player player, Projectile boomerang, float rSpeed, float acceleration, float? minKillRangeBoomerangToPlr = 3000f)
        {
            //返厂的回旋镖应当取消不可穿墙
            boomerang.tileCollide = false;
            Vector2 playerCenter = player.Center;
            float xDist = playerCenter.X - boomerang.Center.X;
            float yDist = playerCenter.Y - boomerang.Center.Y;
            float dist = TryGetVectorMud(xDist, yDist);

            //超出这个距离，直接干掉回旋镖而非返程
            if(minKillRangeBoomerangToPlr.HasValue)
            {
                if(dist>minKillRangeBoomerangToPlr.Value)
                boomerang.Kill();
            }
            else
            {
                if(dist > 3000f)
                boomerang.Kill();
            }

            dist = rSpeed / dist;
            xDist *= dist;
            yDist *= dist;
            
            //提供加速度
            if (boomerang.velocity.X < xDist)
            {
                boomerang.velocity.X += acceleration;
                if (boomerang.velocity.X < 0f && xDist > 0f)
                    boomerang.velocity.X += acceleration;
            }
            else if (boomerang.velocity.X > xDist)
            {
                boomerang.velocity.X -= acceleration;
                if (boomerang.velocity.X > 0f && xDist < 0f)
                    boomerang.velocity.X -= acceleration;
            }
            if (boomerang.velocity.Y < yDist)
            {
                boomerang.velocity.Y += acceleration;
                if (boomerang.velocity.Y < 0f && yDist > 0f)
                    boomerang.velocity.Y += acceleration;
            }
            else if (boomerang.velocity.Y > yDist)
            {
                boomerang.velocity.Y -= acceleration;
                if (boomerang.velocity.Y > 0f && yDist < 0f)
                    boomerang.velocity.Y -= acceleration;
            }
        } 
    }
}
