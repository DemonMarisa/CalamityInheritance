using CalamityInheritance.NPCs.Boss.Yharon.Proj;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
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
                doRebornEffect = true;
                isStage2 = true;
                attackType = (int)YharonAttacksType.PhaseTransition;
                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= PostEclipse_Phase2LifeRatio && currentPhase == 5f && isStage2 && !doRebornEffect)
            {
                attackType = (int)YharonAttacksType.PhaseTransition;
                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= PostEclipse_Phase3LifeRatio && currentPhase == 6f && isStage2 && !doRebornEffect)
            {
                attackType = (int)YharonAttacksType.PhaseTransition;
                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= PostEclipse_Phase4LifeRatio && currentPhase == 7f && isStage2 && !doRebornEffect)
            {
                attackType = (int)YharonAttacksType.PhaseTransition;
                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= PostEclipse_Phase5LifeRatio && currentPhase == 8f && isStage2 && !doRebornEffect)
            {
                attackType = (int)YharonAttacksType.PhaseTransition;
                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                canDie = true;
                NPC.netUpdate = true;
                return;
            }
        }
        #region 二阶段技能
        #region 一到二阶段回血
        public int needhealLife = 500;
        public void DoBehavior_ReBorn(Player target, ref float attacktimer, ref float frameType, ref float attacktype, ref float RebornTimer)
        {
            invincible = true;
            stage2SPDraw = true;
            // 是的，这是丛林龙BGM的开头长度
            int healtimer = 960;
            RebornTimer++;

            if (RebornTimer == 1)
                needhealLife = NPC.lifeMax - NPC.life;

            int healnum = needhealLife / healtimer;

            if (NPC.life > NPC.lifeMax)
                NPC.life = NPC.lifeMax;

            if (NPC.life < NPC.lifeMax)
                NPC.life += (int)(healnum * 1.1f);

            string Text = "+" + healnum * 5;

            if (CalamityConfig.Instance.BossHealthBoost == 0f)
                Text = "+4040";

            if (RebornTimer % 5 == 0)
                CIFunction.SendTextOnNPC(NPC, Text, CombatText.HealLife);

            if (RebornTimer > healtimer)
            {
                doRebornEffect = false;
                attacktype = (float)YharonAttacksType.PhaseTransition;
                stage2SPDraw = false;
            }
        }
        #endregion
        #region 释放龙炎弹
        public void DoBehavior_ReleaseYharonFireBall(Player target, ref float attacktimer, ref float frameType)
        {
            float currentPhase = NPC.ai[2];
            canLookTarget = false;
            DrawRotate = true;
            int hoverX = currentPhase > 5 ? 900 : 750;
            int hoverY = -300;
            float splittingMeteorBombingSpeed = 38f;
            // 走向目标的时间
            int splittingMeteorRiseTime = 90;
            int splittingMeteorBombTime = 90;
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
                NPC.rotation = NPC.rotation.AngleTowards(0f, 0.15f);
                // 平视
                NPC.spriteDirection = -CIFunction.PlayerAndNPCDir(NPC, target);
            }
            // 开始释放龙炎弹
            else if (attacktimer == splittingMeteorRiseTime)
            {
                Vector2 velocity = NPC.SafeDirectionTo(target.Center);
                velocity.Y *= 0.3f;
                velocity = velocity.SafeNormalize(Vector2.UnitX * NPC.spriteDirection);

                frameType = (float)YharonFrameType.motionlessRoar;

                NPC.velocity = velocity * splittingMeteorBombingSpeed;
                
                if (currentPhase > 5)
                    NPC.velocity *= 1.45f;
                
            }
            // 发射
            else
            {
                frameType = (float)YharonFrameType.motionlessRoar;
                NPC.position.X += NPC.SafeDirectionTo(target.Center).X * 7f;
                NPC.position.Y += NPC.SafeDirectionTo(target.Center + Vector2.UnitY * -400f).Y * 6f;

                // int fireballReleaseRate = morePowerfulMeteors ? 4 : 7;
                int fireballReleaseRate = currentPhase > 5 ? 3 : 7;
                if (attacktimer % fireballReleaseRate == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + NPC.velocity * 3f, NPC.velocity, ModContent.ProjectileType<YharonFireballLegacy>(), 515, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer >= splittingMeteorRiseTime + splittingMeteorBombTime)
                SelectNextAttack();
        }
        #endregion
        #region 冲刺并旋转发射跟踪火球
        public void DoBehavior_SpinCharge(Player target, ref float attacktimer, ref float frameType)
        {
            int slowDownTime = 30;
            int upwardFlyTime = 90;
            int spinTime = upwardFlyTime + 42;
            int secondDashTime = 45;
            int fireDely = 5;
            // 开始时减速
            // 小于30
            if (attacktimer < slowDownTime)
            {
                // 帧图为正常的挥动
                frameType = (float)YharonFrameType.Normal;
                NPC.velocity *= 0.97f;
            }
            // 向前冲刺，并沿途释放火球
            // 30 - 90
            if (attacktimer > slowDownTime && attacktimer < upwardFlyTime)
            {
                canLookTarget = false;
                if (attacktimer == slowDownTime + 1)
                {
                    // 向前根据旋转冲刺
                    Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation);
                    direction = direction.SafeNormalize(Vector2.UnitX);

                    // 帧图为静止
                    frameType = (float)YharonFrameType.motionlessRoar;
                    // 冲刺速度
                    float chargeVelocity = 28f;
                    // 向前冲刺
                    NPC.velocity = direction * chargeVelocity;
                    NPC.netUpdate = true;
                    SoundEngine.PlaySound(ShortRoarSound, NPC.Center);
                }
                DashFire(ref attacktimer);
            }
            // 旋转
            // 90 - 132
            if (attacktimer > upwardFlyTime && attacktimer < spinTime)
            {
                canLookTarget = false;
                // 转一圈
                NPC.velocity = NPC.velocity.RotatedBy(MathHelper.TwoPi / 42);
                NPC.rotation = NPC.velocity.ToRotation();
                // 算朝向
                Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation);
                direction = direction.SafeNormalize(Vector2.UnitX);
                // 确保是转圈的同时发射八个
                if (attacktimer % fireDely == 0)
                    ChangeProjSD(NPC.Center, direction);
            }
            // 132 - 177
            if (attacktimer > spinTime && attacktimer < spinTime + secondDashTime)
            {
                canLookTarget = false;
                if (attacktimer == spinTime + 1)
                {
                    // 直接向玩家冲刺
                    Vector2 directionToPlayer = target.Center - NPC.Center;
                    directionToPlayer = directionToPlayer.SafeNormalize(Vector2.UnitX);
                    float chargeVelocity = 32f;
                    NPC.velocity = directionToPlayer * chargeVelocity;
                    NPC.rotation = NPC.velocity.ToRotation();
                    NPC.netUpdate = true;
                    SoundEngine.PlaySound(ShortRoarSound, NPC.Center);
                }
                DashFire(ref attacktimer);
            }
            if (attacktimer > spinTime + secondDashTime)
                SelectNextAttack();
            #region 冲刺封装
            void DashFire(ref float attacktimer)
            {
                // 开火的偏移，大约为嘴部
                int playerFacingDirection = Math.Sign(target.Center.X - NPC.Center.X);
                Vector2 offset = new Vector2(160, -30 * playerFacingDirection).RotatedBy(NPC.rotation);
                Vector2 projectileSpawn = NPC.Center + offset;
                // 向前根据旋转冲刺
                Vector2 direction2 = Vector2.UnitX.RotatedBy(NPC.rotation);
                direction2 = direction2.SafeNormalize(Vector2.UnitX);
                if (attacktimer % fireDely == 0)
                    ChangeProjSD(projectileSpawn, direction2);
            }
            void ChangeProjSD(Vector2 projectileSpawn, Vector2 direction2)
            {
                int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, direction2 * 12.5f, ProjectileID.CultistBossFireBall, fireballDamage, 0f);
                Main.projectile[p].tileCollide = false;
            }
            #endregion
        }
        #endregion
        #region 发射圆环火球
        public void DoBehavior_RingFlareBombs(Player target, ref float attacktimer, ref float frameType)
        {
            canLookTarget = false;
            DrawRotate = true;
            int followPlayerTime = 180;
            int hoverDistanceY = 200;
            float closeVelocity = 18f;
            float closeVelocityAcc = 2.4f;

            int firstfiretime = 20;
            int firedelay = 60;
            // 算一下整体的时间
            int totalfire = followPlayerTime + firedelay * 2 + firstfiretime + 30;
            if (attacktimer < followPlayerTime)
            {
                // 平视
                NPC.rotation = NPC.rotation.AngleTowards(0f, 0.15f);
                NPC.spriteDirection = -CIFunction.PlayerAndNPCDir(NPC, target);

                // 犽绒应该在的地方
                Vector2 destination = new Vector2(target.Center.X, target.Center.Y - hoverDistanceY);
                // 与目标位置的差距
                Vector2 distanceFromDestination = destination - NPC.Center;
                // 移动
                CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, closeVelocity, closeVelocityAcc, true);
                // 如果提前接近，则直接进入下一步
                if (NPC.Distance(target.Center) < 600f && NPC.rotation == 0f)
                {
                    attacktimer = followPlayerTime;
                }
            }
            if (attacktimer > followPlayerTime)
            {
                if (NPC.ai[1] < followPlayerTime + firstfiretime)//75
                    NPC.velocity *= 0.95f;
                else
                    NPC.velocity *= 0.98f;

                // 好吧我又偷懒了
                if (attacktimer == followPlayerTime + firstfiretime || attacktimer == followPlayerTime + firedelay || attacktimer == followPlayerTime + firedelay * 2)
                {
                    if (NPC.velocity.Y > 0f)
                        NPC.velocity.Y /= 3f;
                    NPC.velocity.Y -= 3f;
                    SoundEngine.PlaySound(ShortRoarSound, NPC.Center);
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<DetonatingFlare>(), NPC.whoAmI);
                    if (Main.rand.NextBool())
                        NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<DetonatingFlare>(), NPC.whoAmI);
                    DoFireRing(300, NPC.GetProjectileDamage(ModContent.ProjectileType<FlareBomb>()), NPC.target, 1f);
                }
                if (attacktimer == followPlayerTime + firedelay)
                {
                    // 扔一个龙卷
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<BigFlare2>(), 0, 0f, Main.myPlayer, 1f, NPC.target + 1);
                }
            }
            if (attacktimer > totalfire)
                SelectNextAttack();
        }
        #region 火球圆环封装
        public void DoFireRing(int timeLeft, int damage, float ai0, float ai1)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                float velocity = ai1 == 0f ? 10f : 5f;
                int totalProjectiles = 50;
                float radians = MathHelper.TwoPi / totalProjectiles;
                for (int i = 0; i < totalProjectiles; i++)
                {
                    Vector2 flareRotationAmt = new Vector2(0f, -velocity).RotatedBy(radians * i);
                    int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, flareRotationAmt, ModContent.ProjectileType<FlareBomb>(), damage, 0f, Main.myPlayer, ai0, ai1);
                    Main.projectile[proj].timeLeft = timeLeft;
                }
            }
        }
        #endregion
        #endregion
        #region 横向飞行并留下火球
        public void DoBehavior_SpawnLineFireBall(Player target, ref float attacktimer, ref float frameType)
        {
            float currentPhase = NPC.ai[2];
            // 走向目标的时间
            int splittingMeteorRiseTime = 90;
            int hoverX = currentPhase > 5 ? 1500 : 1200;
            int hoverY = -200;
            float splittingMeteorBombingSpeed = 38f;
            int TotalTimer = splittingMeteorRiseTime + 180;
            if (attacktimer < splittingMeteorRiseTime)
            {
                // 帧图采用默认飞行
                frameType = (float)YharonFrameType.Normal;
                // 向指定位置移动并准备冲刺
                Vector2 destination = target.Center + new Vector2(CIFunction.PlayerAndNPCDir(NPC, target) * hoverX, hoverY);
                Vector2 idealVelocity = NPC.SafeDirectionTo(destination) * splittingMeteorBombingSpeed;
                NPC.velocity = Vector2.Lerp(NPC.velocity, idealVelocity, 0.035f);
            }
            else if (attacktimer == splittingMeteorRiseTime)
            {
                canLookTarget = false;
                // 向前根据旋转冲刺
                Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation);
                direction = direction.SafeNormalize(Vector2.UnitX);
                // 帧图为静止
                frameType = (float)YharonFrameType.motionlessRoar;
                // 冲刺速度
                float chargeVelocity = 38f;
                // 向前冲刺
                NPC.velocity = direction * chargeVelocity;
                NPC.netUpdate = true;
                SoundEngine.PlaySound(ShortRoarSound, NPC.Center);
            }
            // 发射
            else if (attacktimer < TotalTimer)
            {
                canLookTarget = false;
                frameType = (float)YharonFrameType.motionlessRoar;
                // int fireballReleaseRate = morePowerfulMeteors ? 4 : 7;
                int playerFacingDirection = Math.Sign(target.Center.X - NPC.Center.X);
                Vector2 offset = new Vector2(160, -30 * playerFacingDirection).RotatedBy(NPC.rotation);
                Vector2 projectileSpawn = NPC.Center + offset;

                int fireballReleaseRate = 6;
                if (attacktimer % fireballReleaseRate == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, Vector2.Zero, ModContent.ProjectileType<FlareDust2>(), 515, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer > TotalTimer)
                SelectNextAttack();
        }
        #endregion
        #region 召唤火龙卷
        public void DoBehavior_SpawnFlareTor(Player target, ref float attacktimer, ref float frameType)
        {
            canLookTarget = false;
            DrawRotate = true;
            int followPlayerTime = 180;
            int hoverDistanceY = 200;
            float closeVelocity = 18f;
            float closeVelocityAcc = 2.4f;

            int firstfiretime = 20;
            int firedelay = 60;
            // 算一下整体的时间
            int totalfire = followPlayerTime + firedelay;
            if (attacktimer < followPlayerTime)
            {
                // 平视
                NPC.rotation = NPC.rotation.AngleTowards(0f, 0.15f);
                NPC.spriteDirection = -CIFunction.PlayerAndNPCDir(NPC, target);

                // 犽绒应该在的地方
                Vector2 destination = new Vector2(target.Center.X, target.Center.Y - hoverDistanceY);
                // 与目标位置的差距
                Vector2 distanceFromDestination = destination - NPC.Center;
                // 移动
                CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, closeVelocity, closeVelocityAcc, true);
                // 如果提前接近，则直接进入下一步
                if (NPC.Distance(target.Center) < 600f && NPC.rotation == 0f)
                {
                    attacktimer = followPlayerTime;
                }
            }
            if (attacktimer > followPlayerTime)
            {
                if (NPC.ai[1] < followPlayerTime + firstfiretime)//75
                    NPC.velocity *= 0.95f;
                else
                    NPC.velocity *= 0.98f;

                // 好吧我又偷懒了
                if (attacktimer == followPlayerTime + firedelay)
                {
                    if (NPC.velocity.Y > 0f)
                        NPC.velocity.Y /= 3f;
                    NPC.velocity.Y -= 3f;
                    SoundEngine.PlaySound(ShortRoarSound, NPC.Center);
                    // 扔一个龙卷
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<BigFlare2>(), 0, 0f, Main.myPlayer, 1f, NPC.target + 1);
                        NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<DetonatingFlare>(), NPC.whoAmI);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<BigFlare2>(), 0, 0f, Main.myPlayer, 1f, NPC.target + 1);
                        NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<DetonatingFlare>(), NPC.whoAmI);
                        NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<DetonatingFlare>(), NPC.whoAmI);
                    }
                }
            }
            if (attacktimer > totalfire)
                SelectNextAttack();
        }
        #endregion
        #region X形龙炎弹
        public void DoBehavior_SpawnYharonFireBall(Player target, ref float attackTimer, ref float frameType)
        {
            canLookTarget = false;
            DrawRotate = true;
            int totalFlameVortices = 3;
            int totalFlameWaves = 7;
            float flameVortexSpawnDelay = 30f;
            // 平视
            NPC.rotation = NPC.rotation.AngleTowards(0f, 0.15f);
            NPC.spriteDirection = -CIFunction.PlayerAndNPCDir(NPC, target);

            if (attackTimer == 1)
            {
                frameType = (float)YharonFrameType.PlayOnce;
                NPC.Center = target.Center + new Vector2(0, -300f);
                NPC.Opacity = 0f;
                NPC.rotation = 0;
            }
            if (attackTimer == flameVortexSpawnDelay)
            {
                NPC.velocity = Vector2.Zero;
                SoundEngine.PlaySound(OrbSound);
                for (int i = 0; i < totalFlameVortices; i++)
                {
                    float angle = MathHelper.TwoPi * i / totalFlameVortices;
                    Vector2 SpawnPos = target.Center + angle.ToRotationVector2() * 1780f;
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)SpawnPos.X, (int)SpawnPos.Y, ModContent.NPCType<DetonatingFlare>());
                }
            }
            if (attackTimer > flameVortexSpawnDelay && attackTimer % 7 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                frameType = (float)YharonFrameType.Normal;
                float horizontalOffset = (attackTimer - flameVortexSpawnDelay) / 7f * 205f + 260f;
                Vector2 fireballSpawnPosition = NPC.Center + new Vector2(horizontalOffset, -90f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), fireballSpawnPosition, Vector2.UnitY.RotatedBy(-0.18f) * -20f, ModContent.ProjectileType<YharonFireballLegacy>(), 525, 0f, Main.myPlayer);

                fireballSpawnPosition = NPC.Center + new Vector2(-horizontalOffset, -90f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), fireballSpawnPosition, Vector2.UnitY.RotatedBy(0.18f) * -20f, ModContent.ProjectileType<YharonFireballLegacy>(), 525, 0f, Main.myPlayer);
            }
            if (attackTimer > flameVortexSpawnDelay + totalFlameWaves * 7)
                SelectNextAttack();
        }
        #endregion
        #endregion
    }
}
