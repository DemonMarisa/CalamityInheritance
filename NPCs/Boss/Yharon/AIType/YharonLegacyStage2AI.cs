using CalamityInheritance.Utilities;
using CalamityMod.Projectiles.Boss;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.Yharon
{
    public partial class YharonLegacy : ModNPC
    {
        // 阶段划分
        public const float PostEclipse_Phase1LifeRatio = 1f;
        public const float PostEclipse_Phase2LifeRatio = 0.8f;
        public const float PostEclipse_Phase3LifeRatio = 0.6f;
        public const float PostEclipse_Phase4LifeRatio = 0.4f;
        public const float PostEclipse_Phase5LifeRatio = 0.2f;
        public void Stage2AI(float lifeRatio, ref float currentPhase, ref float attackType, ref float attackTimer, ref float circleCount, bool postEclipse)
        {
            if (lifeRatio <= PostEclipse_Phase1LifeRatio && currentPhase == 4f && isStage2)
            {
                attackType = (int)YharonAttacksType.PhaseTransition;
                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= PostEclipse_Phase2LifeRatio && currentPhase == 5f && isStage2)
            {
                attackType = (int)YharonAttacksType.PhaseTransition;
                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= PostEclipse_Phase3LifeRatio && currentPhase == 6f && isStage2)
            {
                attackType = (int)YharonAttacksType.PhaseTransition;
                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= PostEclipse_Phase4LifeRatio && currentPhase == 7f && isStage2)
            {
                attackType = (int)YharonAttacksType.PhaseTransition;
                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= PostEclipse_Phase5LifeRatio && currentPhase == 8f && isStage2)
            {
                attackType = (int)YharonAttacksType.PhaseTransition;
                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
        }
        #region 二阶段技能
        #region 释放龙炎弹
        public void DoBehavior_ReleaseYharonFireBall(Player target, ref float attacktimer, ref float frameType)
        {
            canLookTarget = false;
            int hoverX = 750;
            int hoverY = -300;
            float splittingMeteorBombingSpeed = 30f;
            // 走向目标的时间
            int splittingMeteorRiseTime = 90;
            int splittingMeteorBombTime = 72;
            // 应该在哪
            // 获取目标向量，让犽绒会一直以指定向量移动
            if (attacktimer < splittingMeteorRiseTime)
            {
                // 帧图采用默认飞行
                frameType = (float)YharonFrameType.Normal;
                // 向指定位置移动并准备释放龙炎弹
                Vector2 destination = target.Center + new Vector2(CIFunction.PlayerAndNPCDir(NPC, target) * hoverX, hoverY);
                Vector2 idealVelocity = NPC.SafeDirectionTo(destination) * splittingMeteorBombingSpeed;
                NPC.velocity = Vector2.Lerp(NPC.velocity, idealVelocity, 0.035f);
                // 平视
                NPC.rotation = NPC.rotation.AngleTowards(0f, 0.15f);
                NPC.spriteDirection = CIFunction.PlayerAndNPCDir(NPC, target);
            }
            // 开始释放龙炎弹
            else if (attacktimer == splittingMeteorRiseTime)
            {
                Vector2 velocity = NPC.SafeDirectionTo(target.Center);
                velocity.Y *= 0.3f;
                velocity = velocity.SafeNormalize(Vector2.UnitX * NPC.spriteDirection);

                frameType = (float)YharonFrameType.motionlessRoar;

                NPC.velocity = velocity * splittingMeteorBombingSpeed;
                /*
                if (morePowerfulMeteors)
                    NPC.velocity *= 1.45f;
                */
            }
            // 发射
            else
            {
                NPC.position.X += NPC.SafeDirectionTo(target.Center).X * 7f;
                NPC.position.Y += NPC.SafeDirectionTo(target.Center + Vector2.UnitY * -400f).Y * 6f;
                NPC.spriteDirection = (NPC.velocity.X < 0f).ToDirectionInt();
                NPC.rotation = NPC.velocity.ToRotation() + (NPC.spriteDirection == 1).ToInt() * MathHelper.Pi;

                // int fireballReleaseRate = morePowerfulMeteors ? 4 : 7;
                int fireballReleaseRate = 7;
                if (attacktimer % fireballReleaseRate == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + NPC.velocity * 3f, NPC.velocity, ModContent.ProjectileType<YharonFireball>(), 515, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer >= splittingMeteorRiseTime + splittingMeteorBombTime)
                SelectNextAttack();
        }
        #endregion
        #region 一到二阶段回血
        public void DoBehavior_ReBorn(Player target, ref float attacktimer, ref float frameType)
        {
            // 是的，这是丛林龙BGM的开头长度
            int healtimer = 1040;
            needhealLife = NPC.lifeMax - NPC.life;

            int healnum = needhealLife / healtimer;

            if (NPC.life > NPC.lifeMax)
                NPC.life = NPC.lifeMax;

            if (NPC.life < NPC.lifeMax)
                NPC.life += (int)(healnum * 1.2f);

            string Text = "+" + healnum;
            CIFunction.SendTextOnNPC(NPC, Text, CombatText.HealLife);
        }
        #endregion
        #endregion
    }
}
