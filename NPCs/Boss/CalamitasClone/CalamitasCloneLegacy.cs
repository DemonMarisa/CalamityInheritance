using CalamityInheritance.Buffs.StatDebuffs;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Dusts;
using MonoMod.Core.Utils;
using CalamityInheritance.Buffs.Potions;
using static CalamityInheritance.NPCs.Boss.Yharon.YharonLegacy;
using Terraria.Audio;
using CalamityInheritance.Utilities;
using static CalamityInheritance.NPCs.Boss.SCAL.SupremeCalamitasLegacy;
using CalamityInheritance.NPCs.Boss.SCAL.Proj;
using CalamityInheritance.Content.Items;
using CalamityInheritance.NPCs.Boss.CalamitasClone.Projectiles;
using Terraria.GameContent;
using CalamityInheritance.NPCs.TownNPC;
using CalamityInheritance.System.DownedBoss;
using CalamityMod.Events;
using CalamityMod.Particles;
using CalamityInheritance.NPCs.Boss.CalamitasClone.Brothers;
using CalamityInheritance.NPCs.Boss.SCAL.Brother;
using CalamityInheritance.NPCs.Boss.SCAL.SoulSeeker;
using CalamityInheritance.NPCs.Boss.CalamitasClone.LifeSeeker;
using CalamityMod.World;
using CalamityInheritance.Content.Items.Placeables.Vanity;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs;
using CalamityMod.Projectiles.Boss;
using CalamityMod.UI;
using static System.Net.Mime.MediaTypeNames;
using CalamityMod.Sounds;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Vanity;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture.DevPaintings;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.TreasureBags;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Summon;
using Terraria.GameContent.ItemDropRules;
using CalamityInheritance.Content.Items.Placeables.Relic;
using Terraria.GameContent.Bestiary;
using CalamityInheritance.Core;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.NPCs.Boss.CalamitasClone
{
    [AutoloadBossHead]
    public class CalamitasCloneLegacy : ModNPC
    {
        #region 杂项初始化
        public enum LegacyCCloneAttackType
        {
            fireAbyssalLaser,
            fireFireBall,
            charge,
            BulletHell,

            PhaseTransition,

            // 召唤招式
            SummonBrother,
            SummonTwoBrother,
            SummonSoulSeeker,
        }
        // 普灾的攻击循环
        public static LegacyCCloneAttackType[] AttackCycle =>
            [
            LegacyCCloneAttackType.fireAbyssalLaser,// 这是用来标记的，因为调用的时候会+1，取不到第一个，得取一遍回来才能取到
            LegacyCCloneAttackType.fireAbyssalLaser,
            LegacyCCloneAttackType.charge,
            LegacyCCloneAttackType.fireFireBall,
            LegacyCCloneAttackType.charge,
            ];
        // 普灾兄弟存活期间的攻击循环
        public static LegacyCCloneAttackType[] AttackCycleBrother =>
            [
            LegacyCCloneAttackType.fireAbyssalLaser,// 这是用来标记的，因为调用的时候会+1，取不到第一个，得取一遍回来才能取到
            LegacyCCloneAttackType.fireFireBall,
            ];
        // 最终阶段的攻击
        public static LegacyCCloneAttackType[] AttackCycleFinal =>
            [
            LegacyCCloneAttackType.charge,// 这是用来标记的，因为调用的时候会+1，取不到第一个，得取一遍回来才能取到
            LegacyCCloneAttackType.fireAbyssalLaser,
            LegacyCCloneAttackType.fireFireBall,
            LegacyCCloneAttackType.charge,
            LegacyCCloneAttackType.fireFireBall,
            ];
        #endregion
        #region 阶段
        public const float stage1LifeRatio = 1f;
        public const float stage2LifeRatio = 0.7f;
        public const float stage2_P1_LifeRatio = 0.7f;
        public const float stage2_P2_LifeRatio = 0.5f;
        public const float stage2_P3_LifeRatio = 0.3f;
        public const float stage2_P4_LifeRatio = 0.1f;

        public const float finalPhase = 5f;
        #endregion
        #region 数据
        public int laserDamage = 30;
        public int fireDamage = 40;
        #endregion
        #region 杂项bool
        // 用于在AI的初始化
        public bool initialized = false;
        // 判定是否进入二阶段
        public bool isStage2 = false;
        // 转阶段无敌
        public bool invincible = false;
        public bool isCloneSeekerAlive = false;
        public bool isCloneBrotherAlive = false;
        // 用于进入弹幕炼狱判定
        public bool canBulletHell = false;
        public bool OnlyGlow = false;
        // 随机召唤一个兄弟，但是保证不会同一场战斗召唤两个
        public bool SpawnWho = false;
        #endregion
        #region SSD
        public string Gen = "CalamityInheritance/NPCs/Boss/CalamitasClone";
        public static Asset<Texture2D> P1GlowTexture;
        public static Asset<Texture2D> P2Texture;
        public static Asset<Texture2D> P2GlowTexture;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Calamitas");
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            if (!Main.dedServ)
            {
                P1GlowTexture = ModContent.Request<Texture2D>($"{Gen}/CalamitasCloneLegacy_Glow", AssetRequestMode.AsyncLoad);
                P2Texture = ModContent.Request<Texture2D>($"{Gen}/CalamitasCloneLegacy_Phase2", AssetRequestMode.AsyncLoad);
                P2GlowTexture = ModContent.Request<Texture2D>($"{Gen}/CalamitasCloneLegacy_Phase2_Glow", AssetRequestMode.AsyncLoad);
            }

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
            NPC.damage = 70;
            NPC.npcSlots = 14f;
            NPC.width = NPC.height = 116;
            NPC.defense = 15;

            NPC.value = 0f;

            NPC.DR_NERD((CalamityWorld.death || BossRushEvent.BossRushActive) ? 0.075f : 0.15f);
            NPC.LifeMaxNERB(37500, 45000, 520000);

            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;

            NPC.knockBackResist = 0f;

            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToWater = true;

            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit4;
        }
        #endregion
        #region 图鉴
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {

            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange([
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				// You can add multiple elements if you really wanted to
				new FlavorTextBestiaryInfoElement($"{GenericNPC.GetNPCBestiaryLocal}.CalamitasCloneLegacy")
            ]);
        }
        #endregion
        #region AI
        public override void AI()
        {
            if (NPC.rotation < 0f)
                NPC.rotation += MathHelper.TwoPi;
            else if (NPC.rotation > MathHelper.TwoPi)
                NPC.rotation -= MathHelper.TwoPi; //确保转角一直在2pi内

            isCloneSeekerAlive = NPC.AnyNPCs(ModContent.NPCType<LifeSeekerLegacy>());
            isCloneBrotherAlive = NPC.AnyNPCs(ModContent.NPCType<CataclysmLegacy>()) || NPC.AnyNPCs(ModContent.NPCType<CatastropheLegacy>());

            if (initialized == false)
            {
                SpawnWho = Main.rand.NextBool();
                Main.player[NPC.target].Calamity().GeneralScreenShakePower = 4;
                SpawnDust();
                initialized = true;
            }
            // 获取目标
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest(true);

            Player target = Main.player[NPC.target];

            if (Main.slimeRain)
            {
                Main.StopSlimeRain(true);
                CalamityNetcode.SyncWorld();
            }

            // 目标死亡后消失
            if (!target.active || target.dead || Main.IsItDay())
            {
                NPC.Opacity = Math.Clamp(NPC.Opacity - 0.1f, 0f, 1f);

                for (int i = 0; i < 2; i++)
                {
                    Dust fire = Dust.NewDustPerfect(NPC.Center, (int)CalamityDusts.Brimstone);
                    fire.position += Main.rand.NextVector2Circular(36f, 36f);
                    fire.velocity = Main.rand.NextVector2Circular(8f, 8f);
                    fire.noGravity = true;
                    fire.scale *= Main.rand.NextFloat(1f, 1.2f);
                }

                if (NPC.Opacity <= 0f)
                    NPC.active = false;
                return;
            }

            ref float attackType = ref NPC.ai[0];
            ref float attackTimer = ref NPC.ai[1];
            ref float currentPhase = ref NPC.ai[2];
            // NPC.ai[3]用于招式选择
            ref float rotationSpeed = ref NPC.CIMod().BossNewAI[1];

            // 进入新的阶段
            float lifeRatio = NPC.life / (float)NPC.lifeMax;

            // 处理音乐
            HandleMusicVariables();

            if (lifeRatio <= stage1LifeRatio && currentPhase == 0f)
            {
                attackTimer = 0;
                SelectNextAttack();
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= stage2LifeRatio && currentPhase == 1f)
            {
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.CalamitasCloneSpawn", Color.OrangeRed);
                attackTimer = 0;
                attackType = (float)LegacyCCloneAttackType.PhaseTransition;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= stage2_P1_LifeRatio && currentPhase == 2f && isStage2)
            {
                attackTimer = 0;
                attackType = (float)LegacyCCloneAttackType.SummonBrother;
                // 反转一下，防止一场战斗出两次同样的兄弟
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= stage2_P2_LifeRatio && currentPhase == 3f && isStage2)
            {
                SpawnWho = !SpawnWho;
                attackTimer = 0;
                attackType = (float)LegacyCCloneAttackType.SummonBrother;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= stage2_P3_LifeRatio && currentPhase == 4f && isStage2)
            {
                attackTimer = 0;
                attackType = (float)LegacyCCloneAttackType.SummonSoulSeeker;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= stage2_P4_LifeRatio && currentPhase == finalPhase && isStage2)
            {
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.CalamitasCloneSummonBrother", Color.OrangeRed);
                attackTimer = 0;
                attackType = (float)LegacyCCloneAttackType.SummonTwoBrother;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            attackTimer++;
            // 重置旋转速度
            rotationSpeed = 0.4f;
            // 重置无敌状态
            invincible = false;

            CanIntoBH(ref attackType, ref attackTimer);

            switch ((LegacyCCloneAttackType)attackType)
            {
                case LegacyCCloneAttackType.fireAbyssalLaser:
                    DoBehavior_FireAbyssaLaser(target, attackTimer, currentPhase);
                    break;
                case LegacyCCloneAttackType.charge:
                    DoBehavior_Charge(target,ref attackTimer, currentPhase,ref rotationSpeed);
                    break;
                case LegacyCCloneAttackType.fireFireBall:
                    DoBehavior_FireBall(target, attackTimer, currentPhase);
                    break;
                case LegacyCCloneAttackType.PhaseTransition:
                    DoBehavior_PhaseTransition(target, attackTimer);
                    break;
                case LegacyCCloneAttackType.SummonBrother:
                    SummonBrother(false);
                    break;
                case LegacyCCloneAttackType.SummonTwoBrother:
                    SummonBrother(true);
                    break;
                case LegacyCCloneAttackType.SummonSoulSeeker:
                    SummonLifeSeeker();
                    break;
                case LegacyCCloneAttackType.BulletHell:
                    BulletHell1(target, attackTimer, currentPhase);
                    break;
            }

            if (invincible == true || isCloneSeekerAlive || isCloneBrotherAlive)
            {
                if(!isCloneSeekerAlive)
                    NPC.damage = 0;
                NPC.dontTakeDamage = true;
                NPC.chaseable = false;
            }
            else
            {
                NPC.damage = 200;
                NPC.dontTakeDamage = false;
                NPC.chaseable = true;
            }

            LookAtTarget(target, rotationSpeed);
        }
        #endregion
        #region 音乐
        public void HandleMusicVariables()
        {
            CIGlobalNPC.LegacyCalamitasClone = -1;
            CIGlobalNPC.LegacyCalamitasCloneP2 = -1;
            if (isStage2)
            {
                CIGlobalNPC.LegacyCalamitasCloneP2 = NPC.whoAmI;
                return;
            }
            else
                CIGlobalNPC.LegacyCalamitasClone = NPC.whoAmI;
        }
        #endregion
        #region 技能
        #region 看向目标
        public void LookAtTarget(Player player, float rotationSpeed)
        {
            NPC.rotation = NPC.rotation.AngleLerp(NPC.AngleTo(player.Center) - MathHelper.PiOver2, rotationSpeed);
        }
        #endregion
        #region 发射激光
        public void DoBehavior_FireAbyssaLaser(Player target, float attacktimer, float crphase)
        {
            // 移动速度
            float velocity = 10f;
            float acceleration = 0.18f;
            int distanceX = 600;
            int distanceY = 180;

            int totalFireTime = crphase > finalPhase ? 180 : 360;
            int fireDelay = crphase > finalPhase ? 15 : 30;
            // 如果玩家手持真近战武器，那么降低加速度

            int posX = 1;
            if (NPC.Center.X < target.Center.X)
                posX = -1;

            // 尝试悬停在玩家左上方或右上方
            Vector2 destination = new Vector2(target.Center.X + posX * distanceX, target.Center.Y - distanceY);
            // 大于一半时间后向下移动
            if(attacktimer > totalFireTime / 2)
                destination = new Vector2(target.Center.X + posX * distanceX, target.Center.Y + distanceY);

            // 应该在哪
            Vector2 distanceFromDestination = destination - NPC.Center;

            CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, velocity, acceleration, true);

            if (attacktimer % fireDelay == 0)
            {
                SoundEngine.PlaySound(CISoundID.SoundLaserDestroyer, NPC.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // 使用旋转角度计算方向
                    Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation); // 基础方向根据旋转角度
                    direction = direction.SafeNormalize(Vector2.UnitX);

                    int projType = ModContent.ProjectileType<BrimstoneLaser>();
                    // 偏移向量
                    Vector2 offset = new Vector2(0, 50).RotatedBy(NPC.rotation);
                    Vector2 projectileVelocity = direction * 12.5f;
                    Vector2 projectileSpawn = NPC.Center + offset;
                    projectileVelocity = projectileVelocity.RotatedBy(MathHelper.PiOver2);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, projectileVelocity, projType, laserDamage, 0f, Main.myPlayer, 0f, 0f);
                }
            }

            if (attacktimer > totalFireTime)
                SelectNextAttack();
        }
        #endregion
        #region 冲刺
        public bool hasCharge = false;
        public int ChargeCount = 0;
        public void DoBehavior_Charge(Player target, ref float attacktimer, float currentPhase, ref float rotationacc)
        {
            int totalCharge = currentPhase > finalPhase ? 4 : 2;
            int chargeCount = 25;
            int chargeCooldown = currentPhase > finalPhase ? 50 : 70;
            int dashFireCD = 4;
            if (attacktimer < chargeCount)
            {
                rotationacc = 0f;
                if (Main.netMode != NetmodeID.MultiplayerClient && currentPhase > finalPhase && isCloneBrotherAlive == false)
                {
                    if (attacktimer % dashFireCD == 0)
                    {
                        int type = ModContent.ProjectileType<BrimstoneHellblastLegacy>();
                        Vector2 fireballVelocity = NPC.velocity * 0.005f;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, fireballVelocity, type, fireDamage, 0f, Main.myPlayer, 0f, 0f, 1f);
                    }
                }
            }

            if (hasCharge == false)
            {
                float chargeVelocity = 26f;
                chargeVelocity += 1f * currentPhase;

                Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation + MathHelper.PiOver2);
                direction = direction.SafeNormalize(Vector2.UnitX);
                NPC.velocity = direction * chargeVelocity;

                NPC.netUpdate = true;
                SoundEngine.PlaySound(DashSound, NPC.Center);
                hasCharge = true;
            }
            else
            {
                if (attacktimer > chargeCount)
                {
                    NPC.velocity *= 0.96f;

                    if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                        NPC.velocity.X = 0f;
                    if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
                        NPC.velocity.Y = 0f;
                }
                if (attacktimer >= chargeCooldown)
                {
                    hasCharge = false;
                    attacktimer = 0;
                    ChargeCount++;
                }
                NPC.netUpdate = true;
            }

            if (ChargeCount > totalCharge - 1)
                SelectNextAttack();
        }
        #endregion
        #region 火球
        public void DoBehavior_FireBall(Player target, float attacktimer, float crphase)
        {
            int fireBallCount = crphase > finalPhase ? 5 : 4;
            int fireBallDelay = crphase > finalPhase ? 20 : 45;

            // Scal的加速度和速度
            float velocity = 12f;
            float acceleration = 0.18f;

            // 終灾应该在哪
            Vector2 destination = new Vector2(target.Center.X, target.Center.Y - 550f);
            // 离应该在哪的距离
            Vector2 distanceFromDestination = destination - NPC.Center;
            // 移动
            CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, velocity, acceleration, true);


            if (attacktimer % fireBallDelay == 0)
            {
                SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // 使用旋转角度计算方向
                    Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation); // 基础方向根据旋转角度
                    direction = direction.SafeNormalize(Vector2.UnitX);
                    int projType = ModContent.ProjectileType<HellfireballReborn>();
                    // 偏移向量
                    Vector2 offset = new Vector2(0, 50).RotatedBy(NPC.rotation);
                    Vector2 projectileVelocity = direction * 12.5f;
                    Vector2 projectileSpawn = NPC.Center + offset;
                    projectileVelocity = projectileVelocity.RotatedBy(MathHelper.PiOver2);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, projectileVelocity, projType, fireDamage, 0f, Main.myPlayer, 0f, 0f);
                }
            }

            if (attacktimer > fireBallCount * fireBallDelay)
                SelectNextAttack();
        }
        #endregion
        #region 转阶段
        public int needhealLife = 500;
        public void DoBehavior_PhaseTransition(Player target, float attacktimer)
        {
            NPC.velocity *= 0.95f;
            float phase1Duration = 240;// 持续时间
            int healtimer = 200;
            invincible = true;
            NPC.velocity *= 0.95f;

            // 初始化随机偏移
            if (attacktimer == 1)
            {
                needhealLife = NPC.lifeMax - NPC.life;
                SoundEngine.PlaySound(SpawnSound, NPC.position);
            }
            if (attacktimer < healtimer)
            {
                int healnum = needhealLife / healtimer;

                if (NPC.life > NPC.lifeMax)
                    NPC.life = NPC.lifeMax;

                if (NPC.life < NPC.lifeMax)
                    NPC.life += (int)(healnum * 1.2f);

                string Text = "+" + healnum;
                CIFunction.SendTextOnNPC(NPC, Text, Color.ForestGreen);
                for (int i = 0; i < (int)(attacktimer * 0.01f + 1); i++)
                {
                    PulseEffect();
                }
            }
            if (attacktimer > phase1Duration)
            {
                SoundEngine.PlaySound(BulletHellEndSound, NPC.position);
                NPC.life = NPC.lifeMax;
                isStage2 = true;
                NPC.netUpdate = true;
                SpawnDust();
                SelectNextAttack();
            }
        }
        #endregion
        #region 选择下一个攻击
        public void SelectNextAttack(int? Skip = null)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            // ai1对应的啥
            // attackType = NPC.ai[0];
            // attackTimer = NPC.ai[1];
            float currentPhase = NPC.ai[2];

            // 置零攻击计时器和选择攻击类型
            NPC.ai[1] = 0f;
            hasCharge = false;
            ChargeCount = 0;
            // 获取当前索引（不重置）
            int currentIndex = (int)NPC.ai[3];

            LegacyCCloneAttackType[] attackCycle = AttackCycle;

            if (currentPhase > finalPhase)
                attackCycle = AttackCycleFinal;

            if (isCloneBrotherAlive)
                attackCycle = AttackCycleBrother;

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
        #region 召唤兄弟
        public void SummonBrother(bool summonTwoBrother)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (summonTwoBrother == true)
                {
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + NPC.width, (int)NPC.Center.Y, ModContent.NPCType<CataclysmLegacy>(), NPC.whoAmI);
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X - NPC.width, (int)NPC.Center.Y, ModContent.NPCType<CatastropheLegacy>(), NPC.whoAmI, 0, 15f);
                }
                else
                {
                    if (SpawnWho)
                        NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + NPC.width, (int)NPC.Center.Y + NPC.height, ModContent.NPCType<CataclysmLegacy>(), NPC.whoAmI);
                    else
                        NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X - NPC.width, (int)NPC.Center.Y - NPC.height, ModContent.NPCType<CatastropheLegacy>(), NPC.whoAmI, 0, 15f);
                }
            }
            SpawnDust();
            SelectNextAttack();
        }
        #endregion
        #region 召唤探魂眼
        public void SummonLifeSeeker()
        {
            SoundEngine.PlaySound(SoundID.Item72, NPC.Center);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int seekerAmt = 10;
                int seekerSpread = 360 / seekerAmt;
                int seekerDistance = 180;
                for (int i = 0; i < seekerAmt; i++)
                {
                    int spawn = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.Center.X + (Math.Sin(i * seekerSpread) * seekerDistance)), (int)(NPC.Center.Y + (Math.Cos(i * seekerSpread) * seekerDistance)), ModContent.NPCType<LifeSeekerLegacy>(), NPC.whoAmI, 0, 0, 0, -1);
                    Main.npc[spawn].ai[0] = i * seekerSpread;
                }
            }
            SelectNextAttack();
            /*
            string key = "Mods.CalamityMod.Status.Boss.CalamitasBossText3";
            Color messageColor = Color.Orange;
            CalamityUtils.DisplayLocalizedText(key, messageColor);
            */
        }
        #endregion
        #region 弹幕炼狱
        public void BulletHell1(Player player, float attacktimer, float currentPhase)
        {
            invincible = true;
            // Scal的加速度和速度
            float NPCvelocity = 12f;
            float acceleration = 0.18f;
            int BulletHellblastsDelay = 180;
            // 終灾应该在哪
            Vector2 destination = new Vector2(player.Center.X, player.Center.Y - 500f);
            // 离应该在哪的距离
            Vector2 distanceFromDestination = destination - NPC.Center;
            // 移动
            CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, NPCvelocity, acceleration, true);

            int BulletHell1SpawnDelay = currentPhase > 4f ? 12 : currentPhase > 2f ? 9 : 12;
            if (attacktimer == 1)
            {
                if (currentPhase > 4f)
                    CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.CalamitasCloneFinalPhase", Color.OrangeRed);

                SpawnDust();
                OnlyGlow = true;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int type = ModContent.ProjectileType<BrimstoneHellblast2>();

                float projSpeed = 4f;
                if (currentPhase > 4f)
                {
                    // 发射小爆弹
                    if (attacktimer % BulletHellblastsDelay == 0)
                    {
                        if (Main.rand.NextBool())
                        {
                            SoundEngine.PlaySound(BrimstoneFireShotSound, NPC.Center);
                            // 左右两侧
                            float distance = Main.rand.NextBool() ? -1000f : 1000f;
                            float velocity = (distance == -1000f ? 4f : -4f);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + distance, player.position.Y, velocity, 0f, ModContent.ProjectileType<SCalBrimstoneFireblastLegacy>(), fireDamage, 0f, Main.myPlayer, 0f, 2f, 1f);
                        }
                        else
                        {
                            SoundEngine.PlaySound(BrimstoneFireShotSound, NPC.Center);
                            // 上方
                            float spread = Main.rand.Next(-1000, 1000);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + spread, player.position.Y - 1000f, 0f, 5f, ModContent.ProjectileType<SCalBrimstoneFireblastLegacy>(), fireDamage, 0f, Main.myPlayer, 0f, 2f, 1f);
                        }
                    }
                }

                if (attacktimer % BulletHell1SpawnDelay == 0)
                {
                    if (attacktimer < 300f) // Blasts from above
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, projSpeed, type, fireDamage, 0f, Main.myPlayer, 2f, 0f);
                    }
                    else if (attacktimer < 600f) // Blasts from left and right
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -(projSpeed - 0.5f), 0f, type, fireDamage, 0f, Main.myPlayer, 2f, 0f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), projSpeed - 0.5f, 0f, type, fireDamage, 0f, Main.myPlayer, 2f, 0f);
                    }
                    else // Blasts from above, left, and right
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, projSpeed - 1f, type, fireDamage, 0f, Main.myPlayer, 2f, 0f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -(projSpeed - 1f), 0f, type, fireDamage, 0f, Main.myPlayer, 2f, 0f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), projSpeed - 1f, 0f, type, fireDamage, 0f, Main.myPlayer, 2f, 0f);
                    }
                }
            }

            if (attacktimer == 500)
                SoundEngine.PlaySound(BulletHellSound, NPC.position);
            if (attacktimer == 901)
                DeSpawn();
            if (attacktimer == 902)
            {
                OnlyGlow = false;
                SpawnDust();
                SoundEngine.PlaySound(BulletHellEndSound, NPC.position);
                SelectNextAttack();
            }
        }
        #endregion
        #region 删除所有弹幕
        public void DeSpawn()
        {
            for (int x = 0; x < Main.maxProjectiles; x++)
            {
                Projectile projectile = Main.projectile[x];
                if (projectile.active)
                {
                    if (projectile.type == ModContent.ProjectileType<BrimstoneHellblast2>() ||
                        projectile.type == ModContent.ProjectileType<BrimstoneBarrageLegacy>() ||
                        projectile.type == ModContent.ProjectileType<BrimstoneWaveLegacy>())
                    {
                        if (projectile.timeLeft > 60)
                            projectile.timeLeft = 60;
                    }
                    else if (projectile.type == ModContent.ProjectileType<SCalBrimstoneFireblastLegacy>() || projectile.type == ModContent.ProjectileType<SCalBrimstoneGigablastLegacy>())
                    {
                        projectile.ai[1] = 1f;

                        if (projectile.timeLeft > 15)
                            projectile.timeLeft = 15;
                    }
                }
            }
        }
        #endregion
        #region 可以进入弹幕炼狱
        public void CanIntoBH(ref float attacktype, ref float attacktimer)
        {
            if (isCloneBrotherAlive)
                canBulletHell = true;

            if (canBulletHell == true && isCloneBrotherAlive == false)
            {
                canBulletHell = false;
                attacktype = (float)LegacyCCloneAttackType.BulletHell;
                attacktimer = 0;
            }
        }
        #endregion
        #endregion
        #region 召唤粒子环
        public void SpawnDust()
        {
            int dustAmt = 50;
            int random = 10;

            for (int j = 0; j < 10; j++)
            {
                random += j;
                int dustAmtSpawned = 0;
                int scale = random * 12;
                float dustPositionX = NPC.Center.X - (scale / 2);
                float dustPositionY = NPC.Center.Y - (scale / 2);
                while (dustAmtSpawned < dustAmt)
                {
                    float dustVelocityX = Main.rand.Next(-random, random);
                    float dustVelocityY = Main.rand.Next(-random, random);
                    float dustVelocityScalar = random * 2f;
                    float dustVelocity = (float)Math.Sqrt(dustVelocityX * dustVelocityX + dustVelocityY * dustVelocityY);
                    dustVelocity = dustVelocityScalar / dustVelocity;
                    dustVelocityX *= dustVelocity;
                    dustVelocityY *= dustVelocity;
                    int dust = Dust.NewDust(new Vector2(dustPositionX, dustPositionY), scale, scale, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].position.X = NPC.Center.X;
                    Main.dust[dust].position.Y = NPC.Center.Y;
                    Main.dust[dust].position.X += Main.rand.Next(-10, 11);
                    Main.dust[dust].position.Y += Main.rand.Next(-10, 11);
                    Main.dust[dust].velocity.X = dustVelocityX;
                    Main.dust[dust].velocity.Y = dustVelocityY;
                    Main.dust[dust].scale = 3f;
                    dustAmtSpawned++;
                }
            }
        }
        #endregion
        #region 脉冲粒子
        public void PulseEffect()
        {
            if (!Main.dedServ)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 spawnPosition = NPC.Center + angle.ToRotationVector2() * (300f + Main.rand.NextFloat(100f, 100f));
                Vector2 velocity = (angle - (float)Math.PI).ToRotationVector2() * Main.rand.NextFloat(20f, 35f);
                Dust dust = Dust.NewDustPerfect(spawnPosition, (int)CalamityDusts.Brimstone, velocity);
                dust.scale = 0.9f;
                dust.fadeIn = 1.25f;
                dust.noGravity = true;
                dust.velocity *= 0.99f;
            }
        }
        #endregion
        #region 绘制
        public override Color? GetAlpha(Color drawColor)
        {
            if (OnlyGlow)
                return new Color(0, 0, 0, NPC.alpha);

            return null;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // 在图鉴中使用默认绘制
            if (NPC.IsABestiaryIconDummy)
                return true;

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Texture2D texture = isStage2 ? P2Texture.Value : TextureAssets.Npc[NPC.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount[NPC.type] / 2);
            Color white = Color.White;
            float colorLerpAmt = 0.5f;
            int afterimageAmt = 7;

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int i = 1; i < afterimageAmt; i += 2)
                {
                    Color afterimageColor = drawColor;
                    afterimageColor = Color.Lerp(afterimageColor, white, colorLerpAmt);
                    afterimageColor = NPC.GetAlpha(afterimageColor);
                    afterimageColor *= (afterimageAmt - i) / 15f;
                    Vector2 offset = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                    offset -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    offset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture, offset, NPC.frame, afterimageColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                }
            }

            Vector2 npcOffset = NPC.Center - screenPos;
            npcOffset -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
            npcOffset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
            spriteBatch.Draw(texture, npcOffset, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0f);

            texture = isStage2 ? P2GlowTexture.Value : P1GlowTexture.Value;
            Color color = Color.Lerp(Color.White, Color.Red, 0.5f);

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int i = 1; i < afterimageAmt; i++)
                {
                    Color extraAfterimageColor = color;
                    extraAfterimageColor = Color.Lerp(extraAfterimageColor, white, colorLerpAmt);
                    extraAfterimageColor *= (afterimageAmt - i) / 15f;
                    Vector2 offset = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                    offset -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    offset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture, offset, NPC.frame, extraAfterimageColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                }
            }

            spriteBatch.Draw(texture, npcOffset, NPC.frame, color, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);

            return false;
        }
        #endregion
        #region 击杀
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (hurtInfo.Damage > 0)
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 180, true);
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    int brimDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[brimDust].velocity *= 3f;
                    if (Main.rand.NextBool())
                    {
                        Main.dust[brimDust].scale = 0.5f;
                        Main.dust[brimDust].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int j = 0; j < 70; j++)
                {
                    int brimDust2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[brimDust2].noGravity = true;
                    Main.dust[brimDust2].velocity *= 5f;
                    brimDust2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[brimDust2].velocity *= 2f;
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<CalamitasCloneBag>()));

            var normalOnly = npcLoot.DefineNormalOnlyDropSet();
            {
                // Items
                int[] items = new int[]
                {
                    ModContent.ItemType<Oblivion>(),
                    ModContent.ItemType<Animosity>(),
                    ModContent.ItemType<LashesofChaos>(),
                    ModContent.ItemType<EntropysVigil>()
                };
                normalOnly.Add(DropHelper.CalamityStyle(DropHelper.NormalWeaponDropRateFraction, items));

                // Equipment
                normalOnly.Add(DropHelper.PerPlayer(ModContent.ItemType<VoidofCalamity>()));
                normalOnly.Add(ModContent.ItemType<ChaosStone>(), DropHelper.NormalWeaponDropRateFraction);
                normalOnly.Add(ModContent.ItemType<Regenator>(), 10);

                // Materials
                normalOnly.Add(ModContent.ItemType<EssenceofHavoc>(), 1, 8, 10);
                normalOnly.Add(ModContent.ItemType<AshesofCalamity>(), 1, 25, 30);

                // Vanity
                normalOnly.Add(ModContent.ItemType<CalamitasCloneMask>(), 7);
                var calVanity = ItemDropRule.Common(ModContent.ItemType<HoodOfCalamity>(), 10);
                calVanity.OnSuccess(ItemDropRule.Common(ModContent.ItemType<RobesOfCalamity>()));
                normalOnly.Add(calVanity);
                normalOnly.Add(ModContent.ItemType<ThankYouPainting>(), ThankYouPainting.DropInt);
            }

            npcLoot.Add(ModContent.ItemType<CalamitasCloneTrophy>(), 10);

            // Relic
            npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<CalCloneRelic>());

            // GFB Ashes of Annihilation drop
            npcLoot.DefineConditionalDropSet(DropHelper.GFB).Add(ModContent.ItemType<AshesofAnnihilation>(), 1, 6, 9, true);

            // Lore
            npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCalamitasClone, ModContent.ItemType<LoreCalamitasClone>(), desc: DropHelper.FirstKillText);
        }

        public override void OnKill()
        {
            if (CIServerConfig.Instance.CalExtraDrop)
                PingDownedLevi();
                
            //无论如何标记这个为真。
            CIDownedBossSystem.DownedCalClone = true;
            DeathAshParticle.CreateAshesFromNPC(NPC);
            CalamityNetcode.SyncWorld();
        }
        public static void PingDownedLevi()
        {
            string key = "Mods.CalamityMod.Status.Progression.AbyssDropsText";
            Color messageColor = Color.RoyalBlue;
            // 不用标记了，我给原灾利维坦掉落挂了个钩子，现在他会同时判定普灾和利维坦了
            if (!CIDownedBossSystem.DownedCalClone)
            {
                if (!Main.player[Main.myPlayer].dead && Main.player[Main.myPlayer].active)
                    SoundEngine.PlaySound(CommonCalamitySounds.WyrmScreamSound, Main.player[Main.myPlayer].Center);

                CalamityUtils.DisplayLocalizedText(key, messageColor);
            }
        }
        #endregion
    }
}
