using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.NPCs.Boss.Calamitas.Brothers;
using CalamityInheritance.NPCs.Boss.Calamitas.Projectiles;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.Calamitas
{
    /// <summary>
    /// 头都要裂了，根本无从下手
    /// </summary>
    public class TestReborn
    {
        #region AI枚举
        public enum RebornAttacks
        {
            //悬浮头上，发射火球
            IsShootFireballs,
            IsShootFireballsSlow,
            //水平两侧发射激光
            IsShootLasers,
            //冲刺，就是冲刺
            IsCharging,
            IsChargingReverse,
            //生成
            IsSpawnBros,
            IsSpawnSeekers,
        }
        #endregion
        #region AI顺序
        public static RebornAttacks[] BeforeRebornBrothersAttackCycle =>
        [
            //只用于标记首次攻击
            RebornAttacks.IsShootFireballs,
            RebornAttacks.IsShootFireballs,
            RebornAttacks.IsShootLasers,
        ];
        public static RebornAttacks[] AfterRebornBrothersAttackCycle =>
        [
            RebornAttacks.IsShootFireballs,
            RebornAttacks.IsShootLasers,
            RebornAttacks.IsCharging,
        ];
        #endregion
        #region 别名
        const float DoFrieball = 0f;
        const float DoFrieballAlt = 1f;
        const float DoLaser = 2f;
        const float DoCharging = 3f;
        const float DoFireballSlow = 4f;
        const float DoChargingNoDmg = 5f;
        const float DoSpawnBros = 6f;
        const float DoSpawnBrosTwo = 7f;
        const float DoSpawnBrosThree = 8f;
        const float DoSpawnSeekers = 9f;
        #endregion
        #region 射弹伤害,统一写死不做修改
        const short FireballDamage = 120;
        const short LasersDamage = 120;
        const short ChargeDamage = 240;
        #endregion
        #region AI刻
        const float Phase2Life = 0.7f;
        const float Phase3Life = 0.4f;
        const float Phase4Life = 0.3f;
        const float Phase5Life = 0.1f;
        const float NextAttackTimer = 250f;
        #endregion
        #region 一些别的判定
        public bool CanSwitchAttack = false;
        #endregion
        public static void TestAI(NPC npc)
        {
            //玩家，或者说目标，看你咋想.
            if (npc.target < 0 || npc.target == Main.maxPlayers || Main.player[npc.target].dead || !Main.player[npc.target].active)
                npc.TargetClosest();
            Player target = Main.player[npc.target];

            //标记普灾
            CIGlobalNPC.ThisCalamitasRebornP2 = npc.whoAmI;

            //普灾的退场动画，这里是一个线性往上递增的速度,`以及没有设置脱战距离，这是有意为之的
            if (!target.active || target.dead)
            {
                //曲线上升
                npc.velocity = Vector2.Lerp(npc.velocity, Vector2.UnitX * -24f, 0.05f);
                if (!npc.WithinRange(target.Center, 1100f))
                {
                    npc.life = 0;
                    npc.active = false;
                    npc.netUpdate = true;
                }
                return;
            }
            //兄弟在场与否
            bool ifBrothersExist = NPC.AnyNPCs(ModContent.NPCType<CataclysmReborn>()) ||
                                   NPC.AnyNPCs(ModContent.NPCType<CatastropheReborn>());
            //阶段搜索
            float lifeRatio = npc.life / (float)npc.lifeMax;

            //四个血量阶段，第三个血量阶段生成探魂眼
            bool ifPhase2 = lifeRatio < Phase2Life;
            bool ifPhase3 = lifeRatio < Phase3Life;
            bool ifPhase4 = lifeRatio < Phase4Life;
            bool ifPhase5 = lifeRatio < Phase5Life;

            Vector2 tarCenter = new(target.position.X - target.width / 2, target.position.Y - target.height / 2);
            Vector2 npcCenter = new(npc.Center.X, npc.Center.Y + npc.height - 59f);
            Vector2 npcToTar = npcCenter - tarCenter;

            //使普灾的转角一直朝向玩家 
            float angleToTar = (float)Math.Atan2(npcToTar.Y, npcToTar.X) + MathHelper.PiOver2;
            BrothersGeneric.TryKeeping(npc, 0.1f, angleToTar);
            ref float attackType = ref npc.ai[0];
            ref float attackTypeChangeTimer = ref npc.ai[1];
            ref float curAttack = ref npc.ai[2];
            ref float chargeMark = ref npc.ai[3];
            ref float movementSwitchMark = ref npc.localAI[0];
            ref float projShootTimer = ref npc.localAI[1];
            #region 判定
            //兄弟重生 第一波
            if (lifeRatio <= Phase2Life && curAttack == DoSpawnBros)
            {
                attackTypeChangeTimer = 0;
                attackType = (int)RebornAttacks.IsSpawnBros;
                curAttack++;
                npc.netUpdate = true;
            }
            if (lifeRatio <= Phase3Life && curAttack == DoSpawnBrosTwo)
            {
                attackTypeChangeTimer = 0;
                attackType = (int)RebornAttacks.IsSpawnBros;
                curAttack++;
                npc.netUpdate = true;
            }
            if (lifeRatio <= Phase4Life && curAttack == DoSpawnSeekers)
            {
                attackTypeChangeTimer = 0;
                attackType = (int)RebornAttacks.IsSpawnSeekers;
                curAttack++;
                npc.netUpdate = true;
            }
            if (lifeRatio <= Phase5Life && curAttack == DoSpawnBrosThree)
            {
                attackTypeChangeTimer = 0;
                attackType = (int)RebornAttacks.IsSpawnSeekers;
                curAttack++;
                npc.netUpdate = true;
            }
            #endregion

            //写入AI, 我完全不会写只能照猫画虎
            switch ((RebornAttacks)attackType)
            {
                case RebornAttacks.IsShootFireballs:
                    DoShooFireballAI(npc, target, ref attackTypeChangeTimer, ref curAttack, ref attackType, ref projShootTimer, ifBrothersExist , lifeRatio, ref movementSwitchMark);
                    break;
                case RebornAttacks.IsShootLasers:
                    DoLaserAI(npc, target, ref attackTypeChangeTimer, ref curAttack, ref attackType, ref projShootTimer, ifBrothersExist , lifeRatio, ref movementSwitchMark);
                    break;
                case RebornAttacks.IsCharging:
                    DoChargingAI(npc, target, ref attackType, ifBrothersExist , angleToTar, ref attackTypeChangeTimer);
                    break;
                case RebornAttacks.IsChargingReverse:
                    DoChargingReverseAI(npc, ref attackTypeChangeTimer,  ref attackType, ref movementSwitchMark);
                    break;
            }
        }

        private static void DoChargingReverseAI(NPC npc, ref float attackTypeChangeTimer,  ref float attackType, ref float movementSwitchMark)
        {
            attackTypeChangeTimer += 1f;
            if (attackTypeChangeTimer > 30f)
            {
                //执行反冲AI
                attackType = (int)RebornAttacks.IsCharging;
                attackTypeChangeTimer = 0f;
                //这个用于……我也不知道。
                movementSwitchMark = 0f;
                npc.netUpdate = true;
            }
        }

        private static void DoChargingAI(NPC npc, Player target, ref float attackType,  bool ifBrothersExist, float rot, ref float attackTypeChangeTimer)
        {
            //这下面一帧内可以全部完成，无需担心。
            if (attackTypeChangeTimer == 0f)
            {
                npc.damage = ifBrothersExist ? 0 : npc.defDamage;
                //音效
                SoundEngine.PlaySound(SoundID.Roar, npc.Center);
                //转角
                npc.rotation = rot;
                //冲刺速度
                float chargingSpeed = 42f;
                //设置冲刺向量
                Vector2 chargeVelo = target.Center - npc.Center;
                float chargeDist = chargeVelo.Length();
                chargeDist = chargingSpeed / chargeDist;
                chargeVelo.X *= chargeDist;
                chargeVelo.Y *= chargeDist;
                //提供冲刺
                npc.velocity = chargeVelo;
                //提供冲刺后，将其置为新的冲刺方法
                attackType = (int)RebornAttacks.IsChargingReverse;
                attackTypeChangeTimer = 1f;
            }
            //调用这个Timer
            attackTypeChangeTimer += 1f;
            //在接下来的这个Timer内我们使其速度逐渐降低
            if (attackTypeChangeTimer >= 30f)
            {
                npc.velocity.Y *= 0.9f;
                npc.velocity.X *= 0.9f;
                if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
                    npc.velocity.X = 0f;
                if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
                    npc.velocity.Y = 0f;
            }
            else npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - MathHelper.PiOver2;
            //Timer > 40f的时候我们开始执行另外一个程式
            if (attackTypeChangeTimer > 40f)
            {
                //重新刷新这个Timer
                attackTypeChangeTimer = 0f;
                //转角修正，
                npc.rotation = rot;
                //这个用于标记冲刺次数
                npc.ai[3] += 1f;
                //执行反冲AI
                attackType = (int)RebornAttacks.IsChargingReverse;
                //查看冲刺次数是否大于2 如果是，重置为最开始的火球AI
                if (npc.ai[3] >= 2f)
                {
                    npc.TargetClosest();
                    attackType = (int)RebornAttacks.IsShootFireballs;
                    npc.ai[3] = 0f;
                    return;
                }
            }
        }

        private static void DoLaserAI(NPC npc, Player target, ref float attackTypeChangeTimer, ref float curAttack, ref float attackType, ref float projShootTimer, bool ifBrothersExist, float lifeRatio, ref float movementSwitchMark)
        {
            GenericMovement(npc, target, ref attackType, ref movementSwitchMark);
            attackTypeChangeTimer += 1f;
            if (attackTypeChangeTimer >= 180f)
            {
                attackType = npc.life >= npc.lifeMax * 0.7f ? (int)RebornAttacks.IsShootFireballs : (int)RebornAttacks.IsCharging;
                attackTypeChangeTimer = 0f;
                movementSwitchMark = 1f;
                npc.netUpdate = true;
                return;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                projShootTimer += ifBrothersExist ? 1f : 2f;
                if (projShootTimer >= 50f)
                {
                    projShootTimer = 0f;
                    int pType = ifBrothersExist ? ModContent.ProjectileType<HellfireballReborn>() : ModContent.ProjectileType<BrimstoneLaser>();
                    Vector2 pVel = Vector2.Normalize(target.Center - npc.Center) * 13f;
                    Vector2 pPos = Vector2.Normalize(pVel) * 40f;
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + pPos, pVel, pType, LasersDamage, 0f, Main.myPlayer, target.position.X, target.position.Y);
                }
            }
        }
        private static void GenericMovement(NPC npc, Player target, ref float attackType,ref float movementSwitchMark)
        {
            //重新计算普灾的位置
            ref float npcMovementX = ref npc.localAI[2];
            ref float npcMovementY = ref npc.localAI[3];
            float setMovingDistGate = 100f;
            float speed = 18f;
            float accele = 0.2f;
            Item selectedItem = target.inventory[target.selectedItem];
            bool isTrueMelee = selectedItem.CountsAsClass<TrueMeleeDamageClass>() || selectedItem.CountsAsClass<TrueMeleeNoSpeedDamageClass>();
            if (isTrueMelee) accele *= 0.5f;

            int side = 1;
            if (npc.Center.X < target.Center.X)
                side = -1;
            
            //距离？
            float floatingDist = 400f;
            Vector2 floatingVector = new (target.Center.X + floatingDist * side, target.Center.Y);
            if (movementSwitchMark == 1f) 
            {
                npcMovementX = Main.rand.Next(-49, 52);
                npcMovementY = Main.rand.Next(-299, 302);
                npc.netUpdate = true;
                movementSwitchMark = 0f;
            }
            floatingVector.X += attackType == (int)RebornAttacks.IsShootLasers ? npcMovementY : npcMovementX;
            floatingVector.Y += attackType == (int)RebornAttacks.IsShootLasers ? npcMovementX : npcMovementY;
            //实际向量cen
            Vector2 distTar = floatingVector - npc.Center;
            //movement
            CIFunction.BetterSmoothMovement(npc, setMovingDistGate, distTar, speed, accele);
        }
        private static void DoShooFireballAI(NPC npc, Player target, ref float attackTypeChangeTimer, ref float curAttack, ref float attackType, ref float projShootTimer, bool anyBrothers, float lifePercent, ref float movementSwitchMark)
        {

            GenericMovement(npc, target, ref attackType, ref movementSwitchMark);
            #region 以下用于切换攻击AI状态的计时
            attackTypeChangeTimer += 1f;
            if (attackTypeChangeTimer>= 300f)
            {
                attackType = (int)RebornAttacks.IsShootLasers;
                projShootTimer = 0f;
                attackTypeChangeTimer = 0f;
                movementSwitchMark = 1f;
                npc.netUpdate = true;
                
            }
            #endregion
            //用于生成火球的AI
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                projShootTimer += 1f;
                if (!anyBrothers)
                    projShootTimer += 2f * (1f - lifePercent);
            }

            if (projShootTimer >= (anyBrothers ? 150f : 100f))
            {
                projShootTimer = 0f;
                float pSpeed = 15f;
                int pType = ModContent.ProjectileType<HellfireballReborn>();
                int pDamage = FireballDamage;
                //火球预判？不过我们默认其有预判
                bool pPreddvtive = Main.rand.NextBool();
                Vector2 pPredVec = pPreddvtive ? target.velocity * 20f : Vector2.Zero;
                Vector2 pVelocity = Vector2.Normalize(target.Center + pPredVec - npc.Center) * pSpeed;
                Vector2 pOffset = Vector2.Normalize(pVelocity) * 40f;
                int p = Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + pOffset, pVelocity, pType, pDamage, 0f, Main.myPlayer, target.position.X, target.position.Y);
                Main.projectile[p].netUpdate = true;
            }
        }
        /// <summary>
        /// 这个用于兄弟在场的冲刺时生成粒子
        /// </summary>
        /// <param name="boss"></param>
        public static void VisualDust(NPC boss)
        {
            if (Main.rand.NextBool())
            {
                Vector2 offset = new Vector2(12, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 realOffset = new Vector2(4, 0).RotatedBy(offset.ToRotation());
                Vector2 getDustVelocity = new(boss.velocity.X * 0.4f + realOffset.X, boss.velocity.Y * 0.4f + realOffset.Y);
                float dustScale = 1.2f;
                Dust d = Dust.NewDustPerfect(new Vector2(boss.Center.X, boss.Center.Y) + offset, CIDustID.DustBlood, getDustVelocity, 100, Color.Red, dustScale);
                d.noGravity = true;
            }

            if (Main.rand.NextBool(6))
            {
                Vector2 offset = new Vector2(12, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 realOffset = new Vector2(4, 0).RotatedBy(offset.ToRotation());
                Vector2 getDustVelocity = new(boss.velocity.X * 0.5f + realOffset.X, boss.velocity.Y * 0.5f + realOffset.Y);
                float dustScale = 1.2f;
                int getDust = CIDustID.DustHeatRay;
                Dust d = Dust.NewDustPerfect(new Vector2(boss.Center.X, boss.Center.Y) + offset, getDust, getDustVelocity, 100, Color.Red, dustScale);
                d.noGravity = true;
            }
        }

    }
}
