using CalamityInheritance.Buffs.Potions;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.Content.Items.Placeables.Vanity;
using CalamityInheritance.Content.Items.TreasureBags;
using CalamityInheritance.NPCs.TownNPC;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Events;
using CalamityMod.NPCs;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Boss;
using CalamityMod.UI;
using CalamityMod.World;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.Yharon
{
    [AutoloadBossHead]
    public class YharonLegacy : ModNPC
    {
        #region 杂项初始化
        #region 攻击枚举
        public enum YharonAttacksType
        {
            Hover,
            Charge,
            ChargeNoRoar,
            FasterCharge,
            SpawnTornado,
            SpawnDetonatingFlame,
            FlareBombs,
            FlareBombsLine,
            FlareBombsCircle,
            FlareBombsHell1,
            FlareBombsHell2,
            SpawnFlaresRing,
            TeleportCharge,
            DragonFireballs,

            OpacityToZero,
            PhaseTransition,
            FlyAway
        }
        #endregion
        #region 帧图枚举
        public enum YharonFrameType
        {
            Normal,
            motionless,
            Roar,
            motionlessRoar,
            PlayOnce
        }
        #endregion
        #region 攻击循环
        public static YharonAttacksType[] AttackCycle =>
            [
            YharonAttacksType.FlareBombs,// 这是用来标记的，因为调用的时候会+1，取不到第一个，得取一遍回来才能取到
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.FlareBombsCircle,
            YharonAttacksType.FlareBombs,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.Charge,
            YharonAttacksType.FlareBombs,
            YharonAttacksType.FlareBombsCircle
            ];

        public static YharonAttacksType[] P2AttackCycle =>
            [
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.TeleportCharge,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.FlareBombsHell2,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.TeleportCharge,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.FlareBombs,
            YharonAttacksType.FlareBombsCircle,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.FlareBombsHell1,
            YharonAttacksType.FlareBombs,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.TeleportCharge,
            ];

        public static YharonAttacksType[] P3AttackCycle => 
            [
            YharonAttacksType.FlareBombs,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.FlareBombsHell2,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.TeleportCharge,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.FlareBombsHell1,
            YharonAttacksType.FlareBombsCircle,
            YharonAttacksType.FlareBombs,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.Charge,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.TeleportCharge,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.FlareBombsHell2,
            YharonAttacksType.FlareBombs,
            YharonAttacksType.FlareBombsCircle,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.FlareBombsHell1,
            ];
        #endregion
        #region 获取NPC实例
        public static NPC LegacyYharon
        {
            get
            {
                if (CIGlobalNPC.LegacyYharon == -1)
                    return null;

                return Main.npc[CIGlobalNPC.LegacyYharon];
            }
        }
        #endregion
        #endregion
        #region 音效
        public static readonly SoundStyle RoarSound = new("CalamityMod/Sounds/Custom/Yharon/YharonRoar");
        public static readonly SoundStyle ShortRoarSound = new("CalamityMod/Sounds/Custom/Yharon/YharonRoarShort");
        public static readonly SoundStyle FireSound = new("CalamityMod/Sounds/Custom/Yharon/YharonFire");
        public static readonly SoundStyle OrbSound = new("CalamityMod/Sounds/Custom/Yharon/YharonFireOrb");
        public static readonly SoundStyle HitSound = new("CalamityMod/Sounds/NPCHit/YharonHurt");
        public static readonly SoundStyle DeathSound = new("CalamityMod/Sounds/NPCKilled/YharonDeath");
        #endregion
        #region 数据
        public static float Phase1_DR = 0.24f;
        public static float Phase2_DR = 0.26f;
        // 生命值
        public int LifeMax = CalamityWorld.death ? 4040000 : CalamityWorld.revenge ? 2525000 : 2275000;
        // 免伤
        public float DR = CalamityWorld.death ? 0.75f : 0.7f;
        // 阶段划分
        public const float stage2LifeRatio = 0.1f;
        public const float PreEclipse_Phase1LifeRatio = 1f;

        public const float PreEclipse_Phase2LifeRatio = 0.8f;

        public const float PreEclipse_Phase3LifeRatio = 0.5f;
        #endregion
        #region 杂项bool
        // 用于在AI的初始化
        public bool initialized = false;
        // 用于控制改变朝向
        public bool canChangeDir = false;
        // 用于控制是否朝向目标
        public bool canLookTarget = false;
        // 转阶段无敌
        public bool invincible = false;
        // 转阶段效果控制
        public bool playerP2PEffect = false;
        // 判定释放过了日食
        public bool postEclipse = false;
        #endregion
        #region SSD
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 7;
            NPCID.Sets.TrailingMode[NPC.type] = 1;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                SpriteDirection = 1,
                PortraitScale = 0.7f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
        }
        #endregion
        #region SD
        public override void SetDefaults()
        {
            NPC.npcSlots = 50f;
            NPC.width = 200;
            NPC.height = 200;
            NPC.defense = 120;

            NPC.value = Item.buyPrice(platinum: 100, gold: 0, silver: 0, copper: 0);
            NPC.lifeMax = LifeMax;
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            AIType = -1;

            NPC.boss = true;
            NPC.DR_NERD(Phase1_DR);

            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;

            NPC.DeathSound = DeathSound;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = true;
        }
        #endregion
        #region AI
        public override void AI()
        {
            // 生命百分比
            float lifeRatio = NPC.life / (float)NPC.lifeMax;

            //确保转角一直在2pi内
            if (NPC.rotation < 0f)
                NPC.rotation += MathHelper.TwoPi;
            else if (NPC.rotation > MathHelper.TwoPi)
                NPC.rotation -= MathHelper.TwoPi;

            if (initialized == false)
            {
                NPC.damage = 760;
                initialized = true;
            }
            // 瞄准目标
            if (NPC.target < 0 || NPC.target == Main.maxPlayers || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest();

            // Set the whoAmI variable.
            CIGlobalNPC.LegacyYharon = NPC.whoAmI;

            #region ai[]可读性
            ref float attackType = ref NPC.ai[0];
            ref float attackTimer = ref NPC.ai[1];
            ref float currentPhase = ref NPC.ai[2];
            ref float circleCount = ref NPC.ai[3];
            ref float frameType = ref NPC.localAI[1];
            #endregion
            Player target = Main.player[NPC.target];

            #region 阶段判定
            // 进入新阶段
            // 用于开局的攻击
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
                if(postEclipse == true)
                    attackType = (int)YharonAttacksType.PhaseTransition;
                else
                    attackType = (int)YharonAttacksType.FlyAway;

                attackTimer = 0;
                circleCount = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            #endregion

            float rotationAcc = 0.2f;

            attackTimer++;
            #region 重置效果
            canLookTarget = true;
            invincible = false;
            playerP2PEffect = false;

            if (NPC.Opacity < 1f)
                NPC.Opacity += 0.02f;

            #endregion

            switch ((YharonAttacksType)attackType)
            {
                case YharonAttacksType.Charge:
                    DoBehavior_Charge(target, ref attackTimer, ref frameType,ref rotationAcc, false);
                    break;
                case YharonAttacksType.ChargeNoRoar:
                    DoBehavior_Charge(target, ref attackTimer, ref frameType,ref rotationAcc, true);
                    break;
                case YharonAttacksType.FlareBombsCircle:
                    DoBehavior_FlareBombsCircle(target, ref attackTimer, ref frameType);
                    break;
                case YharonAttacksType.FlareBombs:
                    DoBehavior_FireFlareBombs(target, ref attackTimer, ref frameType);
                    break;
                case YharonAttacksType.PhaseTransition:
                    DoBehavior_PhaseTransition(target, ref attackTimer, ref frameType);
                    break;
                case YharonAttacksType.FlareBombsHell1:
                    DoBehavior_FlareBombsHell(target, ref attackTimer, ref frameType, 0);
                    break;
                case YharonAttacksType.FlareBombsHell2:
                    DoBehavior_FlareBombsHell(target, ref attackTimer, ref frameType, 1);
                    break;
                case YharonAttacksType.TeleportCharge:
                    DoBehavior_TelephoneCharge(target, ref attackTimer, ref frameType);
                    break;
                case YharonAttacksType.FlyAway:
                    DoBehavior_FlyAway(attackTimer, ref frameType);
                    break;
                case YharonAttacksType.OpacityToZero:
                    DoBehavior_OpacityToZero(attackTimer, ref frameType);
                    break;
                default:
                    NPC.velocity *= 0.95f;
                    LookAtTarget(target, rotationAcc);
                    break;
            }

            // 独立的rot判定
            if (canLookTarget)
                LookAtTarget(target, rotationAcc);

            if (invincible == true)
            {
                NPC.dontTakeDamage = true;
                NPC.chaseable = false;
            }
            else
            {
                NPC.dontTakeDamage = false;
                NPC.chaseable = true;
            }

            // Main.NewText($"attackTimer : {attackTimer}");
        }
        #endregion
        #region 技能
        #region 看向目标
        public void LookAtTarget(Player target, float rotationSpeed)
        {
            // 贴图朝向改动
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
        #region 选择下一个攻击
        // 选择下一个攻击
        public void SelectNextAttack(int? Skip = null)
        {

            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            // ai1对应的啥
            // float attackTimer = NPC.ai[1];

            // 置零攻击计时器和选择攻击类型
            float currentPhase = NPC.ai[2];
            NPC.ai[1] = 0f;
            ChargeCount = 0;
            NPC.localAI[2] = 0f;
            // 获取当前索引（不重置）
            int currentIndex = (int)NPC.ai[3];

            YharonAttacksType[] attackCycle = AttackCycle;

            if (currentPhase == 2)
                attackCycle = P2AttackCycle;
            if (currentPhase == 3)
                attackCycle = P3AttackCycle;
            if (currentPhase == 4)
                attackCycle = P3AttackCycle;

            if (Skip == null)
            {
                // 递增索引
                currentIndex++;
                if (currentIndex >= attackCycle.Length)
                    currentIndex = 0;
            }
            else
            {
                currentIndex = (int)Skip;
                if (currentIndex >= attackCycle.Length)
                    currentIndex = 0;
            }

            // 更新索引和攻击类型
            NPC.ai[3] = currentIndex;
            NPC.ai[0] = (int)attackCycle[currentIndex];

            // 多人游戏同步
            if (Main.netMode == NetmodeID.Server)
                NPC.netUpdate = true;
        }
        #endregion
        #region 冲刺
        public bool hasCharge = false;
        public int ChargeCount = 0;
        public void DoBehavior_Charge(Player target, ref float attacktimer, ref float frametype,ref float crrotAcc, bool skipHover)
        {
            canLookTarget = false;
            int totalCharge = 2;
            int chargeCount = 20;
            int chargeCooldown = 60 + chargeCount;
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
                    float chargeVelocity = 28f;
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
                        NPC.velocity *= 0.97f;

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
        public void DoBehavior_FlareBombsCircle(Player target, ref float attackTimer, ref float frametype)
        {
            canLookTarget = false;
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

            int hoverDistanceX = 500;
            int hoverDistanceY = 200;

            float closeVelocity = 12f;
            float closeVelocityAcc = 0.4f;

            // 犽绒应该在的地方
            Vector2 destination = new Vector2(target.Center.X + NPC.spriteDirection * hoverDistanceX, target.Center.Y - hoverDistanceY);
            // 与目标位置的差距
            Vector2 distanceFromDestination = target.Center - NPC.Center;
            // 移动
            CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, closeVelocity, closeVelocityAcc, true);

            if(attacktimer % fireDelay == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int type = ModContent.ProjectileType<FlareBomb>();
                    int damage = NPC.GetProjectileDamage(type);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, Vector2.Zero, type, damage, 0f, Main.myPlayer, NPC.target, 1f);
                }
            }

            if(attacktimer > totalTimer)
                SelectNextAttack();
        }
        #endregion
        #region 转换阶段
        public void DoBehavior_PhaseTransition(Player target, ref float attacktimer, ref float frameType)
        {
            invincible = true;
            int p1Timer = 120;
            int totalTimer = 180;

            if (attacktimer < p1Timer)
            {
                frameType = (float)YharonFrameType.Normal;
                NPC.velocity *= 0.97f;
            }
            else
            {
                if(attacktimer == 121)
                    SoundEngine.PlaySound(RoarSound, NPC.Center);
                playerP2PEffect = true;
                frameType = (float)YharonFrameType.Roar;
                NPC.velocity *= 0.97f;
            }

            if(attacktimer > totalTimer)
                SelectNextAttack();
        }
        #endregion
        #region 发射弹幕炼狱
        public Vector2 logVector2 = Vector2.Zero;
        public void DoBehavior_FlareBombsHell(Player target, ref float attacktimer, ref float frameType, int AttackStyle)
        {
            frameType = (float)YharonFrameType.PlayOnce;

            int spinPhaseTimer = 150;
            int flareDustSpawnDivisor = spinPhaseTimer / 15;
            float spinPhaseRotation = MathHelper.TwoPi * 3 / spinPhaseTimer;

            if (attacktimer == 1)
            {
                NPC.velocity = new( 6f, 6f);
                logVector2 = target.Center + new Vector2(Main.rand.NextFloat(-500f, 500f), -300f);
                NPC.Center = logVector2;
                NPC.Opacity = 0f;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (attacktimer < 150)
                {
                    canLookTarget = false;
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

                if (attacktimer > 150 && attacktimer < 210)
                    NPC.velocity *= 0.97f;
            }

            if (attacktimer > 210)
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
        public int teleportLocation = 0;
        public void DoBehavior_TelephoneCharge(Player target, ref float attacktimer, ref float frameType)
        {
            int TotalHover = 60;
            frameType = (float)YharonFrameType.PlayOnce;
            float distance = 450 * Math.Sign((NPC.Center - target.Center).X);

            teleportLocation = Main.rand.NextBool() ? 600 : -600;
            if(attacktimer <= 1)
            {
                SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                Vector2 center = target.Center + new Vector2(-distance, teleportLocation);
                NPC.Center = center;
                NPC.Opacity = 0f;
            }

            if (hasCharge == false && attacktimer > TotalHover)
            {
                canLookTarget = false;
                float chargeVelocity = 28f;
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
                NPC.velocity *= 0.98f;
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
        #region 视觉效果
        #region Charge Dust
        public void ChargeDust(int dustAmt)
        {
            for (int i = 0; i < dustAmt; i++)
            {
                Vector2 dustRotate = Vector2.Normalize(NPC.velocity) * new Vector2((NPC.width + 50) / 2f, NPC.height) * 0.75f;
                dustRotate = dustRotate.RotatedBy((i - (dustAmt / 2 - 1)) * (double)Math.PI / (float)dustAmt) + NPC.Center;
                Vector2 dustVel = ((float)(Main.rand.NextDouble() * Math.PI) - MathHelper.PiOver2).ToRotationVector2() * Main.rand.Next(3, 8);
                int chargeDust = Dust.NewDust(dustRotate + dustVel, 0, 0, DustID.CopperCoin, dustVel.X * 2f, dustVel.Y * 2f, 100, default, 1.4f);
                Main.dust[chargeDust].noGravity = true;
                Main.dust[chargeDust].noLight = true;
                Main.dust[chargeDust].velocity /= 4f;
                Main.dust[chargeDust].velocity -= NPC.velocity;
            }
        }
        #endregion
        #endregion
        #region 绘制
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.FlipHorizontally;
            float drawRotation = NPC.rotation;
            if (NPC.spriteDirection == 1)
                drawRotation = NPC.rotation + MathHelper.Pi;
            else
                spriteEffects = SpriteEffects.None;

            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Texture2D Glowtexture = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/Yharon/YharonLegacyGlow").Value;

            Vector2 halfSizeTexture = new(texture.Width / 2, texture.Height / Main.npcFrameCount[NPC.type] / 2);
            Vector2 drawLocation = NPC.Center - screenPos;
            drawLocation -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
            drawLocation += halfSizeTexture * NPC.scale + new Vector2(0f, NPC.gfxOffY);

            Color color = drawColor;
            Color invincibleColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
            color = CalamityGlobalNPC.buffColor(color, 0.9f, 0.7f, 0.3f, 1f);

            // 残影数据
            int afterimageAmt = 10;
            int afterimageIncrement = 2;

            // 颜色修改数据
            float lerpInterpolateValue = 0f;
            Color lerpEndColor = Color.White;

            float attackType = NPC.ai[0];
            float attackTimer = NPC.ai[1];

            if (invincible)
                lerpEndColor = invincibleColor;

            else if (((YharonAttacksType)attackType) == YharonAttacksType.Charge || ((YharonAttacksType)attackType) == YharonAttacksType.ChargeNoRoar)
            {
                lerpEndColor = Color.Red;
                lerpInterpolateValue = 0.5f;
            }
            else
                color = drawColor;

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int i = 1; i < afterimageAmt; i += afterimageIncrement)
                {
                    Color afterimageColor = color;
                    afterimageColor = Color.Lerp(afterimageColor, lerpEndColor, lerpInterpolateValue);
                    afterimageColor = NPC.GetAlpha(afterimageColor);
                    afterimageColor *= (afterimageAmt - i) / 15f;
                    Vector2 afterimagePos = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                    afterimagePos -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    afterimagePos += halfSizeTexture * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture, afterimagePos, NPC.frame, afterimageColor, drawRotation, halfSizeTexture, NPC.scale, spriteEffects, 0f);
                }
            }

            int additionalAfterimageAmt = 0;
            float additionalAfterimageOpacity = 0f;
            float afterimageScale = 0f;
            int afterimageColorDivisor = 60;

            if (playerP2PEffect)
            {
                additionalAfterimageAmt = 6;
                additionalAfterimageOpacity = 1f - (float)Math.Cos(attackTimer / afterimageColorDivisor * MathHelper.TwoPi);
                additionalAfterimageOpacity /= 3f;
                afterimageScale = 60f;
            }

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int k = 0; k < additionalAfterimageAmt; k++)
                {
                    Color additionalAfterimageColor = drawColor;
                    additionalAfterimageColor = Color.Lerp(additionalAfterimageColor, lerpEndColor, lerpInterpolateValue);
                    additionalAfterimageColor = NPC.GetAlpha(additionalAfterimageColor);
                    additionalAfterimageColor *= 1f - additionalAfterimageOpacity;
                    Vector2 additionalAfterimagePos = NPC.Center + (k / (float)additionalAfterimageAmt * MathHelper.TwoPi + NPC.rotation).ToRotationVector2() * afterimageScale * additionalAfterimageOpacity - screenPos;
                    additionalAfterimagePos -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    additionalAfterimagePos += halfSizeTexture * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture, additionalAfterimagePos, NPC.frame, additionalAfterimageColor, drawRotation, halfSizeTexture, NPC.scale, spriteEffects, 0f);
                }
            }

            // 常规贴图绘制
            spriteBatch.Draw(texture, drawLocation, NPC.frame, NPC.GetAlpha(drawColor), drawRotation, halfSizeTexture, NPC.scale, spriteEffects, 0f);
            // 绘制发光贴图
            spriteBatch.Draw(Glowtexture, drawLocation, NPC.frame, NPC.GetAlpha(drawColor), drawRotation, halfSizeTexture, NPC.scale, spriteEffects, 0f);

            return false;
        }
        #endregion
        #region 帧图控制
        public override void FindFrame(int frameHeight)
        {
            float frameType = NPC.localAI[1];
            switch ((YharonFrameType)frameType)
            {
                case YharonFrameType.Normal:
                    FrameType_Normal();
                    break;
                case YharonFrameType.Roar:
                    DoBehavior_Roar();
                    break;
                case YharonFrameType.motionless:
                    DoBehavior_motionless();
                    break;
                case YharonFrameType.motionlessRoar:
                    DoBehavior_motionlessRoar();
                    break;
                case YharonFrameType.PlayOnce:
                    DoBehavior_PlayOnce();
                    break;
                default:
                    FrameType_Normal();
                    break;
            }
            void FrameType_Normal()
            {
                NPC.frameCounter += 1D;
                if (NPC.frameCounter > 5D)
                {
                    NPC.frameCounter = 0D;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y >= frameHeight * 5)
                    NPC.frame.Y = 0;
            }
            void DoBehavior_Roar()
            {
                NPC.frameCounter += 1D;
                if (NPC.frameCounter < 10D)
                    NPC.frame.Y = frameHeight * 4;
                if (NPC.frameCounter < 20D)
                    NPC.frame.Y = frameHeight * 6;
                else
                    NPC.frame.Y = frameHeight * 5;
            }
            void DoBehavior_motionless()
            {
                NPC.frame.Y = 0;
            }
            void DoBehavior_motionlessRoar()
            {
                NPC.frame.Y = frameHeight * 5;
            }
            void DoBehavior_PlayOnce()
            {
                NPC.frameCounter += 1D;
                if (NPC.frameCounter > 5D)
                {
                    NPC.frameCounter = 0D;
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y >= frameHeight * 5)
                {
                    NPC.frame.Y = 0;
                    NPC.frameCounter -= 1D;
                }
            }
        }
        #endregion
        #region 预防死亡
        // 防止没过完演出就似了
        public override bool CheckDead()
        {
            NPC.life = 1;
            NPC.active = true;
            NPC.dontTakeDamage = true;
            NPC.netUpdate = true;
            return false;
        }
        #endregion
        #region 死亡
        
        public void FirstDown()
        {
            Player player = Main.LocalPlayer;

            if (!CIDownedBossSystem.DownedLegacyYharonP1)
            {
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.YharonPreEclipse", Color.Orange);
                CIFunction.SendTextOnPlayer("Boss.Text.YharonPreEclipse", Color.Orange);
            }

            SoundEngine.PlaySound(CISoundID.SoundCurseFlamesAttack, NPC.position);

            // Spawn the SCal NPC directly where the boss was
            if (!BossRushEvent.BossRushActive)
                player.QuickSpawnItem(player.GetSource_GiftOrReward(), ModContent.ItemType<YharonTreasureBagsLegacy>(), 1);

            // Mark Calamitas as defeated
            CIDownedBossSystem.DownedLegacyYharonP1 = true;
            CalamityNetcode.SyncWorld();
        }
        /*
        public override void OnKill()
        {
            Player player = Main.LocalPlayer;
            CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.YharonPreEclipse", Color.Orange);
            CIFunction.SendTextOnPlayer("Boss.Text.YharonPreEclipse", Color.Orange);

            SoundEngine.PlaySound(CISoundID.SoundCurseFlamesAttack, NPC.position);

            // Spawn the SCal NPC directly where the boss was
            if (!BossRushEvent.BossRushActive)
                player.QuickSpawnItem(player.GetSource_GiftOrReward(), ModContent.ItemType<YharonTreasureBagsLegacy>(), 1);

            // Mark Calamitas as defeated
            CIDownedBossSystem.DownedLegacyScal = true;
            CalamityNetcode.SyncWorld();
        }
        */
        #endregion
    }
}
