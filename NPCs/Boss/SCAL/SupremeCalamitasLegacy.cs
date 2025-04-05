using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.Content.Items.Placeables.Vanity;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Dusts;
using CalamityMod.Events;
using CalamityMod.Items.Armor.Vanity;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Tiles;
using CalamityMod.UI;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static CalamityMod.NPCs.ExoMechs.Ares.AresBody;

namespace CalamityInheritance.NPCs.Boss.SCAL
{
    public class SupremeCalamitasLegacy : ModNPC
    {
        // 生成场地
        public bool spawnArena = false;
        // 是否可以生成
        public bool canDespawn = false;
        // 接触伤害
        public int ContactDamage = 2000;
        // 弹幕炼狱的伤害
        public int BulletHell = 190;
        // 深渊亡魂的伤害
        public int AbyssalSoul = 150;
        // 硫火飞镖的伤害
        public int BrimstoneDarts = 120;
        // 小型爆弹接触伤害
        public int BrimstoneFireblast = 140;
        // 大型爆弹接触伤害
        public int BrimstoneGigablast = 170;
        // 生命值
        public int LifeMax = CalamityWorld.death ? 8800000 : CalamityWorld.revenge ? 8000000 : 5000000;
        // 免伤
        public float DR = CalamityWorld.death ? 0.75f : 0.7f;
        // 防御
        public int Defense = 120;
        // 确认开始切换贴图
        public int SwitchTexture = 1;
        // 用于在AI的初始化
        // 草捏妈RE，谁叫你boss也自动吃差分的
        public bool initialized = false;
        // 转向玩家
        public float rotateToPlayer = 0;
        // 激怒弹幕乘数
        public float vectorMultiplier = 1f;
        // 是否造成碰撞上与无敌
        public bool isContactDamage = true;
        // 攻击类型枚举
        public enum LegacySCalAttackType
        {
            fireAbyssalSoul,
            fireGigablast,
            charge,
            fireDartsWallAndSmallblast,
            BulletHell,
            PhaseTransition,
            DesperationPhase,

            // 召唤招式
            SummonSepulcher,
            SummonBrother,
            SummonSoulSeeker,
        }

        // 帧图类型枚举
        public enum LegacySCalFrameType
        {
            Phase1Nor,
            Phase1OnlyGlow,
            Phase2Nor,
            Phase2OnlyGlow,
        }
        // 获取NPC实例
        public static NPC LegacySCal
        {
            get
            {
                if (CIGlobalNPC.LegacySCal == -1)
                    return null;

                return Main.npc[CIGlobalNPC.LegacySCal];
            }
        }

        // 激怒
        public static bool Enraged
        {
            get
            {
                if (LegacySCal is null)
                    return false;

                return !Main.player[LegacySCal.target].Hitbox.Intersects(LegacySCal.CIMod().Arena);
            }
        }
        // 終灾的攻击循环
        public static LegacySCalAttackType[] AttackCycle =>
            [
            LegacySCalAttackType.fireDartsWallAndSmallblast,
            LegacySCalAttackType.fireAbyssalSoul,
            LegacySCalAttackType.fireGigablast,
            LegacySCalAttackType.charge,

            LegacySCalAttackType.fireGigablast,
            LegacySCalAttackType.fireAbyssalSoul,
            LegacySCalAttackType.fireDartsWallAndSmallblast,
            LegacySCalAttackType.charge,

            LegacySCalAttackType.fireDartsWallAndSmallblast,
            LegacySCalAttackType.fireAbyssalSoul,
            LegacySCalAttackType.fireGigablast,
            LegacySCalAttackType.fireAbyssalSoul,

            LegacySCalAttackType.charge,
            LegacySCalAttackType.fireDartsWallAndSmallblast,
            LegacySCalAttackType.fireGigablast,
            LegacySCalAttackType.charge,

            LegacySCalAttackType.fireDartsWallAndSmallblast,
            LegacySCalAttackType.charge,
            LegacySCalAttackType.fireDartsWallAndSmallblast,
            LegacySCalAttackType.charge,
            LegacySCalAttackType.fireDartsWallAndSmallblast,
        ];

        // 阶段的血量百分比
        #region 阶段血量百分比
        public const float Phase1_SepulcherLifeRatio = 1f;

        public const float Phase2LifeRatio = 0.75f;

        public const float Phase3LifeRatio = 0.50f;

        public const float Phase4_brotherLifeRatio = 0.45f;

        public const float Phase5LifeRatio = 0.4f;

        public const float Phase6LifeRatio = 0.3f;

        public const float Phase7_SoulSeekerLifeRatio = 0.2f;

        public const float Phase8LifeRatio = 0.1f;

        public const float Phase9_SepulcherLifeRatio = 0.08f;

        public const float Phase10LifeRatio = 0.01f;
        #endregion
        #region SSD
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }
        #endregion
        #region SD
        public override void SetDefaults()
        {
            // 我不知道为什么修改NPCdamage就会导致boss属性翻倍，所以扔AI里面初始化了
            // 草拟吗难度增幅
            NPC.damage = 350;
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.npcSlots = 50f;

            NPC.width = NPC.height = 120;
            NPC.defense = 120;
            NPC.DR_NERD(DR);
            NPC.value = Item.buyPrice(9999, 0, 0, 0);
            
            NPC.lifeMax = LifeMax;
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);

            NPC.lifeMax /= 2;

            NPC.aiStyle = -1;
            AIType = -1;

            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;

            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = false;
            NPC.chaseable = true;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit4;

            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToElectricity = false;
            NPC.Calamity().VulnerableToWater = false;
            
        }
        #endregion
        #region 怪物图鉴
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new FlavorTextBestiaryInfoElement("Mods.CalamityMod.Bestiary.SupremeCalamitas")
            });
        }
        
        #endregion

        public override void AI()
        {
            if (Enraged)
                vectorMultiplier = 2f;
            else
                vectorMultiplier = 1f;

            if (initialized == false)
            {
                initialized = true;
                NPC.lifeMax = LifeMax;
                NPC.life = LifeMax;
                double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
                NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
                NPC.life += (int)(NPC.lifeMax * HPBoost);
            }
            // 开始时重设为真
            isContactDamage = true;

            // 获取目标
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest(true);

            Player target = Main.player[NPC.target];

            if (Main.slimeRain)
            {
                Main.StopSlimeRain(true);
                CalamityNetcode.SyncWorld();
            }

            ref float attackType = ref NPC.ai[0];
            ref float attackTimer = ref NPC.ai[1];

            ref float stageAttackType = ref NPC.ai[2];
            // NPC.ai[3]用于招式选择
            ref float frameChangeSpeed = ref NPC.localAI[1];
            ref float frameType = ref NPC.localAI[2];
            ref float currentPhase = ref NPC.CIMod().BossNewAI[6];
            ref float switchToDesperationPhase = ref NPC.CIMod().BossNewAI[7];

            // Set the whoAmI variable.
            CIGlobalNPC.LegacySCal = NPC.whoAmI;

            // Main.NewText($"currentPhase : {currentPhase}");

            // 激怒
            NPC.Calamity().CurrentlyEnraged = Enraged;

            // 生成场地
            if (!spawnArena)
            {
                spawnArena = true;
                SpawnArena(ref attackType);
            }
            // 音乐控制
            // HandleMusicVariables(npc);

            // 杀了星流飞椅
            if (target.mount?.Type == ModContent.MountType<DraedonGamerChairMount>())
                target.mount.Dismount(target);

            // 指定期间无敌
            NPC.dontTakeDamage = NPC.AnyNPCs(ModContent.NPCType<SoulSeekerSupreme>()) || NPC.AnyNPCs(ModContent.NPCType<SepulcherHead>()) || Enraged;

            // 目标死亡后消失
            if (!target.active || target.dead)
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

            // 进入新的阶段
            float lifeRatio = NPC.life / (float)NPC.lifeMax;

            // 进入新阶段
            // 用于开局的攻击
            if (lifeRatio == Phase1_SepulcherLifeRatio && currentPhase == 0f)
            {
                attackType = (int)LegacySCalAttackType.BulletHell;
                currentPhase++;
                NPC.netUpdate = true;
            }
            // 第二阶段，75% - 50%
            if (lifeRatio <= Phase2LifeRatio && currentPhase == 1f)
            {
                attackType = (int)LegacySCalAttackType.BulletHell;
                currentPhase++;
                NPC.netUpdate = true;
            }
            // 第三阶段，50% - 45%
            if (lifeRatio <= Phase3LifeRatio && currentPhase == 2f)
            {
                attackType = (int)LegacySCalAttackType.BulletHell;
                currentPhase++;
                NPC.netUpdate = true;
            }
            // 第四阶段，45% - 40%
            if (lifeRatio <= Phase4_brotherLifeRatio && currentPhase == 3f)
            {
                attackType = (int)LegacySCalAttackType.SummonBrother;
                currentPhase++;
                NPC.netUpdate = true;
            }
            // 第五阶段，40% - 30%
            if (lifeRatio <= Phase5LifeRatio && currentPhase == 4f)
            {
                attackType = (int)LegacySCalAttackType.PhaseTransition;
                currentPhase++;
                NPC.netUpdate = true;
            }
            // 第六阶段，30% - 20%
            if (lifeRatio <= Phase6LifeRatio && currentPhase == 5f)
            {
                attackType = (int)LegacySCalAttackType.BulletHell;
                currentPhase++;
                NPC.netUpdate = true;
            }
            // 第七阶段，20% - 10%
            if (lifeRatio <= Phase7_SoulSeekerLifeRatio && currentPhase == 6f)
            {
                attackType = (int)LegacySCalAttackType.SummonSoulSeeker;
                currentPhase++;
                NPC.netUpdate = true;
            }
            // 第八阶段，10% - 8%
            if (lifeRatio <= Phase8LifeRatio && currentPhase == 7f)
            {
                attackType = (int)LegacySCalAttackType.BulletHell;
                currentPhase++;
                NPC.netUpdate = true;
            }
            // 第九阶段，8% - 1%
            if (lifeRatio <= Phase9_SepulcherLifeRatio && currentPhase == 8f)
            {
                attackType = (int)LegacySCalAttackType.SummonSepulcher;
                currentPhase++;
                NPC.netUpdate = true;
            }
            // 第十阶段，1% - 0%
            if (lifeRatio <= Phase10LifeRatio && currentPhase == 9f)
            {
                attackType = (int)LegacySCalAttackType.DesperationPhase;
                currentPhase++;
                NPC.netUpdate = true;
            }

            LookAtTarget(target);

            switch ((LegacySCalAttackType)attackType)
            {
                case LegacySCalAttackType.BulletHell:
                    DoBehavior_BulletHell(attackTimer, currentPhase, ref attackType);
                    break;
                case LegacySCalAttackType.fireDartsWallAndSmallblast:
                    DoBehavior_FireDartsWallAndSmallblast(attackTimer);
                    break;
                default:
                    frameType = (int)LegacySCalFrameType.Phase2Nor;
                    frameChangeSpeed = 0.2f;
                    break;
            }

            attackTimer++;
            Main.NewText($"attackTimer : {attackTimer}");
            #region 文本
            SendStartText();
            #endregion

            // 手动重置伤害和是否无敌
            if (!isContactDamage)
            {
                NPC.damage = 0;
                NPC.dontTakeDamage = true;
                NPC.chaseable = false;
            }
            else
            {
                NPC.damage = ContactDamage;
                NPC.dontTakeDamage = false;
                NPC.chaseable = true;
            }
        }
        #region 技能
        public int baseBulletHellProjectileGateValue = (Enraged ? 4 : 6);
        
        public void LookAtTarget(Player player)
        {
            NPC.rotation = NPC.rotation.AngleLerp(NPC.AngleTo(player.Center) - MathHelper.PiOver2, 0.08f);
        }

        public void DoBehavior_BulletHell(float attacktimer, float currentPhaseHell, ref float attacktype)
        {

            Player player = Main.player[NPC.target];

            isContactDamage = false;

            if (!canDespawn)
                NPC.velocity *= 0.95f;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if(attacktimer % baseBulletHellProjectileGateValue == 0)
                {
                    if (attacktimer < 300) //blasts from above
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 4f * vectorMultiplier, ModContent.ProjectileType<BrimstoneHellblast2>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                    }
                    else if (attacktimer < 600) //blasts from left and right
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                    }
                    else if (attacktimer < 900) //blasts from above, left, and right
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 3f * vectorMultiplier, ModContent.ProjectileType<BrimstoneHellblast2>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                    }
                }
            }
            if (attacktimer == 900)
                attacktype = (float)LegacySCalAttackType.fireDartsWallAndSmallblast;
        }
        public bool canFireSplitingFireball = true;
        public void DoBehavior_FireDartsWallAndSmallblast(float attacktimer)
        {
            int BarrageAttackDelay = 90;
            Player player = Main.player[NPC.target];

            // 灾坟虫存在时射击减缓
            if (NPC.AnyNPCs(ModContent.NPCType<SepulcherHead>()))
                attacktimer -= 0.5f;

            // Scal的加速度和速度
            float velocity = 12f;
            float acceleration = 0.12f;

            // 如果玩家手持真近战武器，那么降低加速度
            Item targetSelectedItem = player.inventory[player.selectedItem];
            if (targetSelectedItem.CountsAsClass(ModContent.GetInstance<TrueMeleeDamageClass>()) || targetSelectedItem.CountsAsClass(ModContent.GetInstance<TrueMeleeNoSpeedDamageClass>()))
                acceleration *= 0.5f;

            // 終灾应该在哪
            Vector2 destination = new Vector2(player.Center.X, player.Center.Y - 550f);
            // 离应该在哪的距离
            Vector2 distanceFromDestination = destination - NPC.Center;

            CIFunction.BetterSmoothMovement(NPC, 0f, distanceFromDestination, velocity, acceleration);

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (attacktimer % BarrageAttackDelay == 0)
                {
                    Vector2 projectileVelocity = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
                    Vector2 projectileSpawn = NPC.Center + projectileVelocity * 8f;

                    float baseSpeed = 10f * vectorMultiplier;

                    // 计算基础弹道轨迹
                    Vector2 directionToPlayer = NPC.Center - player.Center;
                    directionToPlayer.Y -= Math.Abs(directionToPlayer.X) * 0.1f; // Y轴补偿
                    directionToPlayer = directionToPlayer.SafeNormalize(Vector2.Zero) * baseSpeed;

                    Vector2 projectileOrigin = NPC.Center + directionToPlayer;

                    if (Main.rand.Next(6) < 3 && canFireSplitingFireball)
                    {
                        canFireSplitingFireball = false;
                        Vector2 fireballDirection = new Vector2(player.Center.X - projectileOrigin.X, player.Center.Y - projectileOrigin.Y - 550f).SafeNormalize(Vector2.Zero) * baseSpeed;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, fireballDirection, ModContent.ProjectileType<SCalBrimstoneFireblast>(), BrimstoneDarts, 0f, Main.myPlayer, 0f, 0f);
                    }
                    else
                    {
                        canFireSplitingFireball = true;
                        // 多重弹幕散射
                        for (int i = 0; i < 8; i++)
                        {
                            Vector2 barrageDirection = (player.Center - projectileOrigin).SafeNormalize(Vector2.Zero);
                            float speedBoost = i > 3 ? -(i - 3) : i;
                            float speedFactor = 8f + speedBoost;

                            Vector2 projvelocity = barrageDirection * speedFactor + new Vector2(speedBoost);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileOrigin, projvelocity, ModContent.ProjectileType<BrimstoneBarrage>(), BrimstoneFireblast, 0f, Main.myPlayer );
                        }
                    }
                }
            }
            if(attacktimer == 300)
                SelectNextAttack();
        }
        public void DoBehavior_FireAbyssalSoul(float attacktimer, float currentPhaseHell)
        {
        }
        public int AttackTypeCount = 0;
        // 选择下一个攻击
        public void SelectNextAttack()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            // ai1对应的啥
            // float attackTimer = NPC.ai[1];
            // int attackType = (int)NPC.ai[0];
            // int currentPhase = (int)NPC.CIMod().BossNewAI[6];

            // 置零攻击计时器和选择攻击类型
            NPC.ai[1] = 0f;

            // 获取当前索引（不要重置）
            int currentIndex = (int)NPC.ai[3];

            LegacySCalAttackType[] attackCycle = AttackCycle;

            // 递增索引
            currentIndex++;
            if (currentIndex >= AttackCycle.Length)
                currentIndex = 0;

            // 更新索引和攻击类型
            NPC.ai[3] = currentIndex;
            NPC.ai[0] = (int)AttackCycle[currentIndex];

            // 多人游戏同步
            if (Main.netMode == NetmodeID.Server)
                NPC.netUpdate = true;
        }
        #endregion

        #region 文本
        public bool startText = false;
        public void SendStartText()
        {

            if (!startText)
            {
                if(CIDownedBossSystem.DownedLegacyScal)
                {
                    if (Main.LocalPlayer.CIMod().LegacyScal_PlayerKillCount >= 4)
                        CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.KillScalMoreThan4", Color.DarkRed);
                    else if (Main.LocalPlayer.CIMod().LegacyScal_PlayerKillCount == 1)
                        CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.KillScalOnce", Color.DarkRed);
                }
                else
                {
                    if (Main.LocalPlayer.CIMod().LegacyScal_PlayerDeathCount == 50)
                        CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.Scal_PlayerDeathMoreThan50", Color.DarkRed);
                    else if (Main.LocalPlayer.CIMod().LegacyScal_PlayerDeathCount > 19)
                        CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.Scal_PlayerDeathMoreThan20", Color.DarkRed);
                    else if (Main.LocalPlayer.CIMod().LegacyScal_PlayerDeathCount > 4)
                        CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.Scal_PlayerDeathMoreThan4", Color.DarkRed);
                }
                startText = true;
            }
        }
        #endregion
        #region 生成场地
        public void SpawnArena(ref float attackTypeChange)
        {
            // Define the arena.
            Vector2 arenaArea = new(145f, 145f);
            LegacySCal.CIMod().Arena = Utils.CenteredRectangle(NPC.Center, arenaArea * 16f);
            int left = (int)(LegacySCal.CIMod().Arena.Center().X / 16 - arenaArea.X * 0.5f);
            int right = (int)(LegacySCal.CIMod().Arena.Center().X / 16 + arenaArea.X * 0.5f);
            int top = (int)(LegacySCal.CIMod().Arena.Center().Y / 16 - arenaArea.Y * 0.5f);
            int bottom = (int)(LegacySCal.CIMod().Arena.Center().Y / 16 + arenaArea.Y * 0.5f);
            int arenaTileType = ModContent.TileType<ArenaTile>();

            for (int i = left; i <= right; i++)
            {
                for (int j = top; j <= bottom; j++)
                {
                    if (!WorldGen.InWorld(i, j))
                        continue;

                    // Create arena tiles.
                    if ((i == left || i == right || j == top || j == bottom) && !Main.tile[i, j].HasTile)
                    {
                        Main.tile[i, j].TileType = (ushort)arenaTileType;
                        Main.tile[i, j].Get<TileWallWireStateData>().HasTile = true;
                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendTileSquare(-1, i, j, 1, TileChangeType.None);
                        else
                            WorldGen.SquareTileFrame(i, j, true);
                    }
                }
            }

            attackTypeChange = (int)LegacySCalAttackType.BulletHell;
            NPC.netUpdate = true;
        }
        #endregion
        #region 杂项与绘制
        public override bool CheckActive()
        {
            return canDespawn;
        }
        
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;
            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            float attackTypeDraw = NPC.ai[0];

            if (attackTypeDraw == (float)LegacySCalAttackType.charge)
                return new Color(0, 0, 0, NPC.alpha);
            return null;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D ScalPhase1 = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/SupremeCalamitasLegacy").Value;
            Texture2D ScalPhase2 = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/SupremeCalamitasLegacy2").Value;

            Texture2D ScalPhase1Glow = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/SupremeCalamitasLegacyGlow").Value;
            Texture2D ScalPhase2Glow = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/SupremeCalamitasLegacy2Glow").Value;

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            float currentPhase = NPC.CIMod().BossNewAI[6];
            Texture2D texture2D15 = currentPhase > 4f ? ScalPhase2 : TextureAssets.Npc[NPC.type].Value;
            Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
            Color color36 = Color.White;
            float amount9 = 0.5f;
            int num153 = 7;
            if (CalamityConfig.Instance.Afterimages)
            {
                for (int num155 = 1; num155 < num153; num155 += 2)
                {
                    Color color38 = drawColor;
                    color38 = Color.Lerp(color38, color36, amount9);
                    color38 = NPC.GetAlpha(color38);
                    color38 *= (num153 - num155) / 15f;
                    Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
                    vector41 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
                    spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
                }
            }

            Vector2 vector43 = NPC.Center - Main.screenPosition;
            vector43 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
            vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
            spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);


            texture2D15 = SwitchTexture == 2 ? ScalPhase2Glow : ScalPhase1Glow;
            Color color37 = Color.Lerp(Color.White, Color.Red, 0.5f);
            if (CalamityConfig.Instance.Afterimages)
            {
                for (int num163 = 1; num163 < num153; num163++)
                {
                    Color color41 = color37;
                    color41 = Color.Lerp(color41, color36, amount9);
                    color41 *= (num153 - num163) / 15f;
                    Vector2 vector44 = NPC.oldPos[num163] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
                    vector44 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    vector44 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
                    spriteBatch.Draw(texture2D15, vector44, NPC.frame, color41, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
                }
            }

            spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

            return false;
        }
        
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.width = 100;
                NPC.height = 100;
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }
        #endregion
        #region 击杀与掉落
        public override void OnKill()
        {
            // Increase the player's SCal kill count
            if (Main.player[NPC.target].CIMod().LegacyScal_PlayerKillCount < 5)
                Main.player[NPC.target].CIMod().LegacyScal_PlayerKillCount++;

            /*
            // Spawn the SCal NPC directly where the boss was
            if (!BossRushEvent.BossRushActive)
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y + 12, cirrus ? ModContent.NPCType<FAP>() : ModContent.NPCType<WITCH>());
            */

            // Mark Calamitas as defeated
            CIDownedBossSystem.DownedLegacyScal = true;
            CalamityNetcode.SyncWorld();
        }
        #endregion
    }
}
