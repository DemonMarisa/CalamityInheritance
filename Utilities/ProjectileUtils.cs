using System;
using System.Reflection.PortableExecutable;
using CalamityMod;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using Terraria;
using Terraria.DataStructures;

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
        public static Vector2 GiveVelocity(float directionMult)
        {
            Vector2 velocity = new Vector2(directionMult , directionMult);
            velocity.Normalize();
            return velocity;
        }
        

        /// <summary>
        /// 用于使射弹在任意起始位置发射至任意目标位置, 经典例子就是天降射弹
        /// </summary>
        /// <param name="src">发射源</param>
        /// <param name="whoAmI">射弹的归属者</param>
        /// <param name="pType">射弹类型</param>
        /// <param name="pSpeed">射弹速度</param>
        /// <param name="pDamage">射弹伤害</param>
        /// <param name="srcX">射弹起始x坐标</param>
        /// <param name="srcY">射弹起始y坐标</param>
        /// <param name="tarX">目标x坐标</param>
        /// <param name="tarY">目标y坐标</param>
        /// <param name="xVelocityOffset">默认取0f，距离向量的水平偏移</param>
        /// <param name="yVelocityOffset">默认取0f，距离向量的垂直偏移</param>
        /// <param name="pKB">默认取5f，击退力量</param>
        /// <param name="ai0">默认取0f，ai0</param>
        /// <param name="ai1">默认取0f，ai1</param>
        /// <param name="ai2">默认取0f，ai2</param>
        public static void RainDownProj(ref EntitySource_ItemUse_WithAmmo src, ref int whoAmI, int pType, float pSpeed, int pDamage,
                                        float srcX, float srcY, float tarX, float tarY,
                                        float? xVelocityOffset = 0f, float? yVelocityOffset = 0f,
                                        float? pKB = 5f, float? ai0 = 0f, float? ai1 = 0f, float? ai2 = 0f)
        {
            //目标坐标
            Vector2 tarPos = new (tarX, tarY);            
            //起始坐标
            Vector2 srcPos = new (srcX, srcY);
            //目标坐标与起始坐标的距离向量
            Vector2 srcTar = tarPos - srcPos;
            //距离向量一个随机度
            if (xVelocityOffset.HasValue)
                srcTar.X += xVelocityOffset.Value;
            if (yVelocityOffset.HasValue)
                srcTar.Y += yVelocityOffset.Value;
            //取向量模
            float srcTarDist = srcTar.Length();
            //修正距离向量为实际射弹的速度向量
            //这里相当于是 速度/距离 得到时间的倒数 (1/T)
            srcTarDist = pSpeed / srcTarDist;
            //时间的倒数分别与距离向量的水平距离和垂直距离相乘即可将其修正为速度向量
            srcTar.X *= srcTarDist;
            srcTar.Y *= srcTarDist;
            //发射射弹
            Projectile.NewProjectile(src, srcPos, srcTar, pType, pDamage, pKB.Value, whoAmI, ai0.Value, ai1.Value, ai2.Value);

        }
        ///<summary>
        ///用于手持射弹的使用判定
        ///</summary>
        public static bool CantUseHoldout(this Player player, bool needsToHold = true)
        {
            if (player != null && player.active && !player.dead && !(!player.channel && needsToHold) && !player.CCed)
            {
                return player.noItems;
            }

            return true;
        }
    }
}
