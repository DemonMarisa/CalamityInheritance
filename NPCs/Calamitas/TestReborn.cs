using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.NPCs.Calamitas.Brothers;
using CalamityInheritance.NPCs.Calamitas.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Calamitas
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
            FloatingShootFireball,
            //水平两侧发射激光
            VerticalShootLasers,
            //冲刺，就是冲刺
            Charge,
        }
        #endregion
        #region 射弹伤害,统一写死不做修改
        public static readonly int FireballDamage = 120;
        public static readonly int LasersDamage = 120;
        public static readonly int ChargeDamage = 240;
        #endregion
        #region AI刻
        public const float Phase2Life = 0.7f;
        public const float Phase3Life = 0.4f;
        public const float Phase4Life = 0.3f;
        public const float Phase5Life = 0.1f;
        public const float NextAttackTimer = 250f;
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

            bool ifBrothersExist = NPC.AnyNPCs(ModContent.NPCType<CataclysmReborn>()) ||
                                   NPC.AnyNPCs(ModContent.NPCType<CatastropheReborn>());
            float lifeRatio = npc.life / (float)npc.lifeMax;

            //四个血量阶段，第三个血量阶段生成探魂眼
            bool ifPhase2 = lifeRatio < Phase2Life;
            bool ifPhase3 = lifeRatio < Phase3Life;
            bool ifPhase4 = lifeRatio < Phase4Life;
            bool ifPhase5 = lifeRatio < Phase5Life;
 
            Vector2 tarCenter = new(target.position.X - target.width/2, target.position.Y - target.height/2);
            Vector2 npcCenter = new(npc.Center.X, npc.Center.Y + npc.height - 59f);
            Vector2 npcToTar = npcCenter - tarCenter;

            //使普灾的转角一直朝向玩家 
            float angleToTar = (float)Math.Atan2(npcToTar.Y, npcToTar.X) + MathHelper.PiOver2;
            BrothersGeneric.TryKeeping(npc, 0.1f, angleToTar);
            ref float switchAttackTimer = ref npc.ai[1];
            ref float chargeTimer = ref npc.ai[2];
            //写入AI, 我完全不会写只能照猫画虎
            switch ((RebornAttacks)npc.ai[0])
            {
                case RebornAttacks.FloatingShootFireball:
                    TryShootFireball(npc, target, npcCenter, 24f, ModContent.ProjectileType<HellfireballReborn>(), FireballDamage, ref switchAttackTimer);
                    break;
                case RebornAttacks.VerticalShootLasers:
                    TryShootLaser(npc, target, npcCenter, 24f, ModContent.ProjectileType<BrimstoneLaser>(), LasersDamage);
                    break;
                case RebornAttacks.Charge:
                    ChargeAI(npc, target, ifBrothersExist, 40f);
                    break;
            }
            switchAttackTimer++;
        }
        /// <summary>
        /// 发射火球
        /// </summary>
        /// <param name="npc">普灾</param>
        /// <param name="target">玩家</param>
        /// <param name="npcCenter">普灾中心</param>
        /// <param name="pSpeed">射弹速度</param>
        /// <param name="pType">射弹类型</param>
        /// <param name="pDmg">射弹伤害</param>
        public static void TryShootFireball(NPC npc, Player target, Vector2 npcCenter, float pSpeed, int pType, int pDmg, ref float switchAttackTimer)
        {
            //火球预判
            Vector2 pPredice = Main.rand.NextBool() ? target.velocity * 15f : Vector2.Zero;
            Vector2 pVel = Vector2.Normalize(target.Center + pPredice - npc.Center) * pSpeed;
            Vector2 pPos = Vector2.Normalize(pVel) * 40f;
            int p = Projectile.NewProjectile(npc.GetSource_FromAI(), npcCenter + pPos, pVel, pType, pDmg, 0f, Main.myPlayer, target.position.X, target.position.Y);
            Main.projectile[p].netUpdate = true;
        }
        /// <summary>
        /// 发射激光
        /// </summary>
        /// <param name="npc">普灾</param>
        /// <param name="target">玩家</param>
        /// <param name="npcCenter">普灾中心</param>
        /// <param name="pSpeed">射弹速度</param>
        /// <param name="pType">射弹类型</param>
        /// <param name="pDmg">射弹伤害</param>
        public static void TryShootLaser(NPC npc, Player target, Vector2 npcCenter, float pSpeed, int pType, int pDmg)
        {
            Vector2 pVel = Vector2.Normalize(target.Center - npc.Center) * pSpeed;
            Vector2 pPos = Vector2.Normalize(pVel) * 40f;
            if (!Collision.CanHit(npc.position,npc.width,npc.height,npc.position,npc.width,npc.height))
                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + pPos, pVel, ModContent.ProjectileType<BrimstoneLaser>(), pDmg, 0f, Main.myPlayer, target.position.X, target.position.Y);
            else Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + pPos, pVel, pType, pDmg, 0f, Main.myPlayer, target.position.X, target.position.Y);
        }
        /// <summary>
        /// 冲刺
        /// </summary>
        /// <param name="npc">普灾</param>
        /// <param name="target">玩家</param>
        /// <param name="ifBrothers">兄弟是否在场，如果在场冲刺没有伤害</param>
        /// <param name="baseChargeSpeed">冲刺速度</param>
        public static void ChargeAI(NPC npc, Player target, bool ifBrothers, float baseChargeSpeed)
        {
            npc.damage = npc.defDamage;
            if(ifBrothers)
            {
                npc.damage = 0;
                VisualDust(npc);
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
                Vector2 offset = new Vector2(12,0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 realOffset = new Vector2(4,0).RotatedBy(offset.ToRotation());
                Vector2 getDustVelocity = new (boss.velocity.X * 0.4f + realOffset.X, boss.velocity.Y * 0.4f + realOffset.Y);
                float dustScale = 1.2f;
                Dust d = Dust.NewDustPerfect(new Vector2(boss.Center.X, boss.Center.Y) + offset, CIDustID.DustBlood, getDustVelocity, 100, Color.Red, dustScale);
                d.noGravity = true;
            }

            if (Main.rand.NextBool(6))
            {
                Vector2 offset = new Vector2(12,0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 realOffset = new Vector2(4,0).RotatedBy(offset.ToRotation());
                Vector2 getDustVelocity = new (boss.velocity.X * 0.5f + realOffset.X, boss.velocity.Y * 0.5f + realOffset.Y);
                float dustScale = 1.2f;
                int getDust = CIDustID.DustHeatRay;
                Dust d = Dust.NewDustPerfect(new Vector2(boss.Center.X, boss.Center.Y) + offset, getDust, getDustVelocity, 100, Color.Red, dustScale);
                d.noGravity = true;
            }
        }

    }
}
