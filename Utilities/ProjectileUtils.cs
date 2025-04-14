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

                // 限制角度
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
        public static Vector2 RandomVelocity(float X, float Y)
        {
            Vector2 velocity = new Vector2(Main.rand.NextFloat(X, -X), Main.rand.NextFloat(Y, -Y));
            velocity.Normalize();
            return velocity;
        }
        public static Vector2 GiveVelocity(float num)
        {
            Vector2 velocity = new Vector2(num, num);
            velocity.Normalize();
            return velocity;
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
        /// <summary>
        /// 新的追踪方法，这个会指定一个NPC, 且可以自定义输入额外更新，以及强制速度不受距离影响
        /// 目前没有角度限制等一类的东西，如果需要则可以补上。
        /// </summary>
        /// <param name="proj">射弹</param>
        /// <param name="target">射弹目标</param>
        /// <param name="distRequired">最大范围</param>
        /// <param name="speed">射弹速度</param>
        /// <param name="inertia">惯性</param>
        /// <param name="giveExtraUpdate">给予额外更新，默认1</param>
        /// <param name="forceSpeed">指定射弹无视距离，使射弹使用你输入的速度。这个效果有一个距离特判，即距离比你输入的射弹速度还短的时候才会生效, 一般可无视。</param>
        public static void HomingNPCBetter(Projectile proj, NPC target, float distRequired, float speed, float inertia, int giveExtraUpdate = 1, float? forceSpeed = null)
        {
            //一般来说你用这个方法就说明target理论上应当可以被追，但……just in case
            if (!proj.friendly || target == null || !target.active)
                return;
            bool canHome;

            float curDist = Vector2.Distance(target.Center, proj.Center);
            //存储射弹当前额外更新
            if (proj.CalamityInheritance().StoreEU == -1)
                proj.CalamityInheritance().StoreEU = proj.extraUpdates;

            if (!target.CanBeChasedBy(proj) || !target.chaseable || curDist > distRequired) 
                canHome = false;
            else canHome = true;
            if (canHome)
            {
                //给予额外更新
                proj.extraUpdates = proj.CalamityInheritance().StoreEU + giveExtraUpdate;
                //开始追踪target
                Vector2 home = (target.Center - proj.Center).SafeNormalize(Vector2.UnitY);
                Vector2 velo = (proj.velocity * inertia + home * speed) / (inertia + 1f);

                //TODO: 这个写法不太好，可能得考虑重置一下
                //除非你当前距离比射弹速度还少, 我们才会重新设定速度
                if (forceSpeed.HasValue && curDist < speed)
                    velo = proj.velocity + home * forceSpeed.Value;
                //设定速度
                proj.velocity = velo;
            }
            //否则返回射弹原本的额外更新
            else proj.extraUpdates = proj.CalamityInheritance().StoreEU;
        }
        /// <summary>
        /// 使你的射弹追随你的鼠标。有使其判定鼠标是否在墙体内与给予额外更新的输入
        /// </summary>
        /// <param name="projectile">射弹</param>
        /// <param name="homingSpeed">追踪速度</param>
        /// <param name="inertia">惯性</param>
        /// <param name="giveExtraUpdate">附加多少额外更新，默认为1</param>
        /// <param name="ignoreTiles">是否无视物块</param>
        /// <param name="stopHomingDist">如果距离鼠标位置有一定距离时停止跟随</param>
        public static void HomeInOnMouseBetter(Projectile projectile, float homingSpeed, float inertia, int giveExtraUpdate = 1, bool ignoreTiles = false, float? stopHomingDist = null)
        {
            Vector2 des = Main.MouseWorld;
            //一般情况下……鼠标是不会大于4k屏幕的，对吧？
            float distCompared = 6400f;
            if (projectile.CalamityInheritance().StoreEU == -1)
                projectile.CalamityInheritance().StoreEU = projectile.extraUpdates;

            if ((Vector2.Distance(projectile.Center, des) > distCompared || Main.player[projectile.owner].dead) && (ignoreTiles == false || !Collision.CanHit(projectile.Center, 1, 1, des, 1, 1)))
            {
                projectile.extraUpdates = projectile.CalamityInheritance().StoreEU;
                projectile.netUpdate = true;
                return;
            }
            if (stopHomingDist.HasValue)
            {
                if (Vector2.Distance(projectile.Center, des) < stopHomingDist.Value)
                {
                    projectile.extraUpdates = projectile.CalamityInheritance().StoreEU;
                    projectile.netUpdate = true;
                    return;
                }
            }
            //本质上不需要做什么，因为如果要跟随鼠标位置的话, 鼠标肯定是一直存在的, 因此直接给速度就行了
            //计算向量
            projectile.extraUpdates = projectile.CalamityInheritance().StoreEU + giveExtraUpdate;
            Vector2 homing = (des - projectile.Center).SafeNormalize(Vector2.UnitY);
            Vector2 velocity = (projectile.velocity * inertia + homing * homingSpeed) / (inertia + 1f);
            projectile.velocity = velocity;
        }
        /// <summary>
        /// 用于搜索距离玩家最近的npc单位，并返回NPC实例。通常情况下与上方的追踪方法配套
        /// 这个方法会同时实现穿墙、数组、boss优先度的搜索。不过只能用于射弹。但也足够
        /// 这里Boss优先度的实现逻辑是如果我们但凡搜索到一个Boss，就把这个Boss临时存储，在返回实例的时候优先使用
        /// </summary>
        /// <param name="p">玩家</param>
        /// <param name="maxDist">最大搜索距离</param>
        /// <param name="bossFirst">boss优先度，这个还没实现好逻辑，所以填啥都没用（</param>
        /// <param name="ignoreTiles">穿墙搜索, 默认为</param>
        /// <param name="arrayFirst">数组优先, 这个将会使射弹优先针对数组内第一个单位,默认为否</param>
        /// <returns>返回一个NPC实例</returns>
        public static NPC FindClosestTarget(Projectile p, float maxDist, bool bossFirst = false, bool ignoreTiles = true, bool arrayFirst = false)
        {
            //bro我真的要遍历整个NPC吗？
            float distStoraged = maxDist;
            NPC tryGetBoss = null;
            NPC acceptableTarget = null;
            bool alreadyGetBoss = false;
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (!npc.active || npc.friendly || npc.lifeMax < 5)
                    continue;
                float exDist = npc.width + npc.height;
                //如果优先搜索boss单位，且当前敌怪不是一个boss，直接跳过
                //单位不可被追踪 或者 超出索敌距离则continue
                if (!npc.CanBeChasedBy(p.Center, false) || Vector2.Distance(p.Center, npc.Center) > distStoraged + exDist)
                    continue;

                //补: 如果优先搜索Boss单位, 且附近至少有一个。我们直接存储这个Boss单位
                //已经获取到的会被标记，使其不会再跑一遍搜索.
                if (npc.boss && bossFirst && !alreadyGetBoss)
                {
                    tryGetBoss = npc;
                    alreadyGetBoss = true;
                }
                
                //搜索符合条件的敌人, 准备返回这个NPC实例
                float curNpcDist = Vector2.Distance(npc.Center, p.Center);
                if (curNpcDist < distStoraged && (ignoreTiles || Collision.CanHit(p.Center, 1, 1, npc.Center, 1, 1)))
                {
                    distStoraged = curNpcDist;
                    acceptableTarget = npc;
                    if (tryGetBoss != null & bossFirst)
                        acceptableTarget = tryGetBoss;
                    //如果是数组优先，直接在这返回实例
                    if (arrayFirst)
                        return acceptableTarget;
                }
            }
            //返回这个NPC实例
            return acceptableTarget;      
        }
    }
}
