using CalamityInheritance.Utilities;
using CalamityMod.Projectiles.Boss;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static CalamityInheritance.NPCs.Boss.Yharon.YharonLegacy;
using CalamityInheritance.NPCs.Boss.CalamitasClone.Brothers;
using CalamityInheritance.NPCs.Boss.Yharon.Proj;

namespace CalamityInheritance.NPCs.Boss.Yharon
{
    public partial class YharonLegacy : ModNPC
    {
        // 阶段划分
        public const float stage2LifeRatio = 0.1f;
        public const float PreEclipse_Phase1LifeRatio = 1f;

        public const float PreEclipse_Phase2LifeRatio = 0.7f;

        public const float PreEclipse_Phase3LifeRatio = 0.4f;
        public void Stage1AI(float lifeRatio,ref float currentPhase,ref float attackType,ref float attackTimer,ref float circleCount, bool postEclipse)
        {
            #region 阶段判定
            // 进入新阶段
            // 用于开局的攻击
            #region 第一大阶段
            if (lifeRatio <= PreEclipse_Phase1LifeRatio && currentPhase == 0f)
            {
                attackType = (int)YharonAttacksType.PhaseTransition;
                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= PreEclipse_Phase2LifeRatio && currentPhase == 1f)
            {
                attackType = (int)YharonAttacksType.PhaseTransition;
                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= PreEclipse_Phase3LifeRatio && currentPhase == 2f)
            {
                attackType = (int)YharonAttacksType.PhaseTransition;
                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= stage2LifeRatio && currentPhase == 3f)
            {
                if (postEclipse == true)
                    attackType = (int)YharonAttacksType.PhaseTransition;
                else
                    attackType = (int)YharonAttacksType.FlyAway;

                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                isStage2 = true;
                NPC.netUpdate = true;
                return;
            }
            #endregion
            #endregion
        }
        #region 技能
        #region 看向目标
        public void LookAtTarget(Player target, float rotationSpeed)
        {
            // 贴图朝向改动
            // -1时在左边，1时在右边
            int playerFacingDirection = Math.Sign(target.Center.X - NPC.Center.X);
            if (playerFacingDirection != 0)
            {
                NPC.direction = playerFacingDirection;
                NPC.spriteDirection = -NPC.direction;
            }
            NPC.rotation = NPC.rotation.AngleLerp(NPC.AngleTo(target.Center), rotationSpeed);

        }
        #endregion
        #region 取消生成
        public void DeSpawn(Player target)
        {
            // Despawn
            if (target.dead || !target.active)
            {
                NPC.velocity.Y -= 0.4f;
                if (NPC.timeLeft > 60)
                    NPC.timeLeft = 60;
            }
            else if (NPC.timeLeft < 1800)
                NPC.timeLeft = 1800;
        }
        #endregion
        #region 冲刺
        public bool hasCharge = false;
        public int ChargeCount = 0;
        public void DoBehavior_Charge(Player target, ref float attacktimer, ref float frametype, ref float crrotAcc, bool skipHover)
        {
            float currentPhase = NPC.ai[2];
            canLookTarget = false;
            int totalCharge = 2;
            int chargeCount = 20;
            int chargeCd = currentPhase > 4 ? 45 : 60;
            int chargeCooldown = chargeCd + chargeCount;
            int hoverTimer = 90;

            int hoverDistanceX = 500;
            int hoverDistanceY = 200;

            if (attacktimer < hoverTimer)
            {
                if (skipHover)
                    attacktimer += hoverTimer;
                if (attacktimer == 3)
                    SoundEngine.PlaySound(RoarSound, NPC.Center);

                float closeVelocity = 8f;
                float closeVelocityAcc = 0.4f;

                // 犽绒应该在的地方
                Vector2 destination = new Vector2(target.Center.X + NPC.spriteDirection * hoverDistanceX, target.Center.Y - hoverDistanceY);
                // 与目标位置的差距
                Vector2 distanceFromDestination = destination - NPC.Center;
                // 移动
                CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, closeVelocity, closeVelocityAcc, true);

                // 旋转
                float progress = attacktimer / hoverTimer;
                // float apply = MathHelper.Lerp(0, 1f, progress);
                // Main.NewText($"progress : {progress}");
                LookAtTarget(target, progress);

                frametype = (float)YharonFrameType.Roar;
            }
            if (attacktimer > hoverTimer)
            {
                if (hasCharge == false)
                {
                    float chargeVelocity = currentPhase > 4 ? 48f : 28f;
                    float fastChargeVelocityMultiplier = 1.5f;

                    Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation);
                    direction = direction.SafeNormalize(Vector2.UnitX);
                    NPC.velocity = direction * chargeVelocity * fastChargeVelocityMultiplier;
                    frametype = (float)YharonFrameType.motionless;
                    NPC.netUpdate = true;
                    hasCharge = true;

                    SoundEngine.PlaySound(ShortRoarSound, NPC.Center);
                }
                else
                {
                    if (attacktimer < chargeCount + hoverTimer)
                        ChargeDust(7);
                    if (attacktimer > chargeCount + hoverTimer)
                    {
                        NPC.velocity *= currentPhase > 4 ? 0.94f : 0.96f;

                        crrotAcc = 0.2f;

                        LookAtTarget(target, crrotAcc);

                        if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                            NPC.velocity.X = 0f;
                        if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
                            NPC.velocity.Y = 0f;

                        frametype = (float)YharonFrameType.PlayOnce;
                    }
                    if (attacktimer > chargeCooldown + hoverTimer)
                    {
                        hasCharge = false;
                        attacktimer = hoverTimer;
                        ChargeCount++;
                    }
                    NPC.netUpdate = true;
                }
            }
            if (ChargeCount > totalCharge - 1)
                SelectNextAttack();
        }
        #endregion
        #region 旋转火球
        public void DoBehavior_CircleFlareBombs(Player target, ref float attackTimer, ref float frametype)
        {
            canLookTarget = false;
            if (attackTimer == 0)
            {
                NPC.velocity = new(6f, 6f);
            }

            int flareDustPhaseTimer = 100;
            float spinTime = flareDustPhaseTimer;
            float spinPhaseRotation = MathHelper.TwoPi * 3 / spinTime;

            int flareDustPhaseTimer2 = 100;
            int flareDustSpawnDivisor2 = flareDustPhaseTimer2 / 30;

            if (attackTimer % flareDustSpawnDivisor2 == 0f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 projectileVelocity = NPC.velocity;
                    projectileVelocity.Normalize();
                    int type = ModContent.ProjectileType<FlareDust2>();
                    int damage = NPC.GetProjectileDamage(type);
                    float finalVelocity = 12f;
                    float projectileAcceleration = 1.1f;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, projectileVelocity, type, damage, 0f, Main.myPlayer, finalVelocity, projectileAcceleration);
                }
            }

            NPC.velocity = NPC.velocity.RotatedBy(-(double)spinPhaseRotation * NPC.direction);
            NPC.rotation = NPC.velocity.ToRotation();

            frametype = (float)YharonFrameType.PlayOnce;

            if (attackTimer > flareDustPhaseTimer)
                SelectNextAttack();
        }
        #endregion
        #region 发射跟踪火球
        public void DoBehavior_FireFlareBombs(Player target, ref float attacktimer, ref float frameType)
        {
            frameType = (float)YharonFrameType.motionlessRoar;
            // 用来说明可以使用默认的看向目标，因为每帧都会重置，不需要再重置一遍
            // canLookTarget = true;
            if (attacktimer == 2f)
                SoundEngine.PlaySound(RoarSound, NPC.Center);

            int playerFacingDirection = Math.Sign(target.Center.X - NPC.Center.X);
            Vector2 offset = new Vector2(160, -30 * playerFacingDirection).RotatedBy(NPC.rotation);
            Vector2 projectileSpawn = NPC.Center + offset;

            int fireDelay = 4;
            int totalTimer = 60;

            float closeVelocity = 12f;
            float closeVelocityAcc = 0.4f;

            // 与目标位置的差距
            Vector2 distanceFromDestination = target.Center - NPC.Center;
            // 移动
            CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, closeVelocity, closeVelocityAcc, true);

            if (attacktimer % fireDelay == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int type = ModContent.ProjectileType<FlareBomb>();
                    int damage = NPC.GetProjectileDamage(type);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, Vector2.Zero, type, damage, 0f, Main.myPlayer, NPC.target, 1f);
                }
            }

            if (attacktimer > totalTimer)
                SelectNextAttack();
        }
        #endregion
        #region 转换阶段
        public void DoBehavior_PhaseTransition(Player target, ref float attacktimer, ref float frameType, float phase)
        {
            invincible = true;
            int p1Timer = 120;
            int totalTimer = 180;

            if (attacktimer < p1Timer)
            {
                frameType = (float)YharonFrameType.Normal;
                NPC.velocity *= 0.97f;
            }
            else if (attacktimer < totalTimer)
            {
                if (attacktimer == 121)
                {
                    SoundEngine.PlaySound(RoarSound, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, NPC.direction * 4, 8f, ModContent.ProjectileType<Flare>(), 0, 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, -(float)NPC.direction * 4, 8f, ModContent.ProjectileType<Flare>(), 0, 0f, Main.myPlayer, 0f, 0f);
                    }
                }
                playerP2PEffect = true;
                frameType = (float)YharonFrameType.Roar;
                NPC.velocity *= 0.97f;
            }

            if (attacktimer > totalTimer)
            {
                SelectNextAttack(0);
            }
        }
        #endregion
        #region 发射弹幕炼狱
        public Vector2 logVector2 = Vector2.Zero;
        public void DoBehavior_FlareBombsHell(Player target, ref float attacktimer, ref float frameType, int AttackStyle)
        {
            frameType = (float)YharonFrameType.PlayOnce;
            float currentPhase = NPC.ai[2];
            int spinPhaseTimer = 180;
            int flareDustSpawnDivisor = currentPhase > 5 ? 8 : 12;
            int fireNPC = 40;
            int circleCounter = currentPhase > 5 ? 4 : 3;
            float spinPhaseRotation = MathHelper.TwoPi * circleCounter / spinPhaseTimer;

            if (attacktimer == 1)
            {
                NPC.velocity = currentPhase > 5 ? new(9f, 9f) : new(6f, 6f);
                logVector2 = target.Center + new Vector2(Main.rand.NextFloat(-500f, 500f), -300f);
                NPC.Center = logVector2;
                NPC.Opacity = 0f;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (attacktimer < spinPhaseTimer)
                {
                    canLookTarget = false;
                    if (attacktimer % fireNPC == 0f)
                        NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<DetonatingFlare>(), NPC.whoAmI);
                    if (attacktimer % flareDustSpawnDivisor == 0f)
                    {
                        if (AttackStyle == 0)
                        {
                            int ringReduction = (int)MathHelper.Lerp(0f, 14f, attacktimer / spinPhaseTimer);
                            int totalProjectiles = 34 - ringReduction; // 36 for first ring, 22 for last ring
                            DoFlareDustBulletHell(0, flareDustSpawnDivisor, 100, totalProjectiles, 0f, 0f, NPC.Center);
                        }
                        else
                            DoFlareDustBulletHell(1, spinPhaseTimer, 100, 12, 12f, 3.6f, NPC.Center);
                    }
                    NPC.velocity = NPC.velocity.RotatedBy(-(double)spinPhaseRotation * NPC.direction);
                    NPC.rotation = NPC.velocity.ToRotation();
                }

                if (attacktimer > spinPhaseTimer && attacktimer < spinPhaseTimer + 60)
                    NPC.velocity *= 0.97f;
            }

            if (attacktimer > spinPhaseTimer + 60)
                SelectNextAttack();
        }
        #endregion
        #region 弹幕地狱发射函数
        public void DoFlareDustBulletHell(int attackType, int timer, int projectileDamage, int totalProjectiles, float projectileVelocity, float radialOffset, Vector2 firePos)
        {
            SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
            // ai1是攻击计时器
            float aiVariableUsed = NPC.ai[1];
            switch (attackType)
            {
                case 0:
                    float offsetAngle = 360 / totalProjectiles;
                    int totalSpaces = totalProjectiles / 5;
                    int spaceStart = Main.rand.Next(totalProjectiles - totalSpaces);
                    float ai0 = aiVariableUsed % (timer * 2) == 0f ? 1f : 0f;

                    int spacesMade = 0;
                    for (int i = 0; i < totalProjectiles; i++)
                    {
                        if (i >= spaceStart && spacesMade < totalSpaces)
                            spacesMade++;
                        else
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), firePos, Vector2.Zero, ModContent.ProjectileType<FlareDust>(), projectileDamage, 0f, Main.myPlayer, ai0, i * offsetAngle);
                    }
                    break;

                case 1:
                    double radians = MathHelper.TwoPi / totalProjectiles;
                    Vector2 spinningPoint = Vector2.Normalize(new Vector2(-NPC.localAI[2], -projectileVelocity));

                    for (int i = 0; i < totalProjectiles; i++)
                    {
                        Vector2 fireSpitFaceDirection = spinningPoint.RotatedBy(radians * i) * projectileVelocity;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), firePos, fireSpitFaceDirection, ModContent.ProjectileType<FlareDust>(), projectileDamage, 0f, Main.myPlayer, 2f, 0f);
                    }

                    float newRadialOffset = (int)aiVariableUsed / (timer / 4) % 2f == 0f ? radialOffset : -radialOffset;
                    NPC.localAI[2] += newRadialOffset;
                    break;

                default:
                    break;
            }
        }
        #endregion
        #region 传送冲刺
        public void DoBehavior_TelephoneCharge(Player target, ref float attacktimer, ref float frameType)
        {
            float currentPhase = NPC.ai[2];
            float closeVelocity = 8f;
            float closeVelocityAcc = 0.4f;
            int TotalHover = 30;
            frameType = (float)YharonFrameType.PlayOnce;
            float distance = 250 * Math.Sign((NPC.Center - target.Center).X);

            if (attacktimer <= 1)
            {
                SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                Vector2 center = target.Center + new Vector2(-distance, Main.rand.NextBool() ? 500 : -500);
                NPC.Center = center;
                NPC.Opacity = 0f;
            }
            else if (attacktimer <= TotalHover)
            {
                int hoverDistanceX = 500;
                int hoverDistanceY = 200;
                // 犽绒应该在的地方
                Vector2 destination = new Vector2(target.Center.X + NPC.spriteDirection * hoverDistanceX, target.Center.Y - hoverDistanceY);
                // 与目标位置的差距
                Vector2 distanceFromDestination = destination - NPC.Center;
                // 移动
                CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, closeVelocity, closeVelocityAcc, true);

                NPC.velocity *= 0.96f;
            }
            if (hasCharge == false && attacktimer > TotalHover)
            {
                canLookTarget = false;
                float chargeVelocity = currentPhase > 5 ? 48f : 28f;
                float fastChargeVelocityMultiplier = 1.5f;

                Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation);
                direction = direction.SafeNormalize(Vector2.UnitX);
                NPC.velocity = direction * chargeVelocity * fastChargeVelocityMultiplier;
                frameType = (float)YharonFrameType.motionless;
                NPC.netUpdate = true;
                hasCharge = true;

                SoundEngine.PlaySound(ShortRoarSound, NPC.Center);
            }
            if (attacktimer > TotalHover + 15)
                NPC.velocity *= currentPhase > 5 ? 0.94f : 0.96f;
            if (attacktimer > 80)
                SelectNextAttack();
        }
        #endregion
        #region 飞走
        public void DoBehavior_FlyAway(float attacktimer, ref float frameType)
        {
            invincible = true;
            //奶奶的不要走之前创思我好不好
            NPC.damage = 0;
            if (Main.zenithWorld)
                NPC.damage = 114514;
            if (attacktimer < 90)
            {
                if (attacktimer == 8)
                    SoundEngine.PlaySound(RoarSound, NPC.Center);
                NPC.velocity *= 0.96f;
                if (attacktimer < 30)
                    frameType = (float)YharonFrameType.Roar;
                else
                    frameType = (float)YharonFrameType.Normal;
            }
            else
            {
                frameType = (float)YharonFrameType.Normal;
                NPC.velocity.X *= 0.96f;
                NPC.velocity.Y -= 0.4f;
                if (attacktimer == 160)
                {
                    FirstDown();
                    NPC.active = false;
                }
                NPC.Opacity -= 0.04f;
            }
        }
        #endregion
        #region 透明度变化
        public void DoBehavior_OpacityToZero(float attacktimer, ref float frameType)
        {
            frameType = (float)YharonFrameType.Normal;
            NPC.velocity *= 0.99f;
            NPC.Opacity -= 0.053f;

            if (attacktimer > 30)
                SelectNextAttack();
        }
        #endregion
        #endregion
    }
}
