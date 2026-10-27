using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.Buff.Buffs;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityInheritance.Content.NPCs.Boss.Scal.ArenaTile;
using CalamityInheritance.Content.NPCs.Boss.Scal.Brother;
using CalamityInheritance.Content.NPCs.Boss.Scal.Proj;
using CalamityInheritance.Content.NPCs.Boss.Scal.ScalWorm;
using CalamityInheritance.Content.NPCs.Boss.Scal.Sky;
using CalamityInheritance.Content.NPCs.Boss.Scal.SoulSeeker;
using CalamityInheritance.Core.CIConfigs;
using CalamityInheritance.Core.CIWorlds;
using CalamityInheritance.Core.Misc;
using CalamityInheritance.Core.Path;
using CalamityInheritance.Core.Utils;
using CalamityInheritance.Music;
using LAP.Content.Configs;
using LAP.Core.MusicEvent;
using LAP.Core.SpecificEffectManagers;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.NPCs.Boss.Scal
{
    [AutoloadBossHead]
    public class SupremeCalamitasLegacy : ModNPC
    {
        // 生成场地
        public bool spawnArena = false;
        // 是否可以生成
        public bool canDespawn = false;
        // 接触伤害
        public int ContactDamage = 2000;
        // 红月伤害
        public int MoonDamage = 220;
        // 弹幕炼狱的伤害
        public int BulletHell = 190;
        // 深渊亡魂的伤害
        public int AbyssalSoul = 170;
        // 硫火飞镖的伤害
        public int BrimstoneDarts = 150;
        // 小型爆弹接触伤害
        public int BrimstoneFireblast = 180;
        // 大型爆弹接触伤害
        public int BrimstoneGigablast = 200;
        // 生命值
        public int LifeMax = 8800000;
        // 免伤
        public float DR = 0.25f;
        // 防御
        public int Defense = 120;
        // 确认开始切换贴图
        public bool OnlyGlow = false;
        // 用于在AI的初始化
        // 草捏妈RE，谁叫你boss也自动吃差分的
        public bool initialized = false;
        // 转向玩家
        public float rotateToPlayer = 0;
        // 激怒弹幕乘数
        public float vectorMultiplier = 1f;
        // 是否造成碰撞上与无敌
        public bool isContactDamage = true;
        // 是否存活小弟
        public bool isBrotherAlive = false;
        public bool isSeekerAlive = false;
        public bool isWormAlive = false;
        // 目标是否手持真近战武器
        public bool isTrueMelee = false;
        // 记录召唤时的位置
        public Vector2 logSpawnPos;
        // 是否绘制二阶段贴图
        public bool isSecondPhase = false;
        // 是否可以死亡
        public bool canDead = false;
        // 是否可以进入下一个阶段
        // 必须在对应招式手动切换状态
        public bool canNextPhase = true;
        public int dustType = DustID.LifeDrain;
        #region 杂项初始化
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

        // 激怒
        public bool Enraged = false;
        public Rectangle Arena;
        // 終灾的攻击循环
        public static LegacySCalAttackType[] AttackCycle =>
            [
            LegacySCalAttackType.fireDartsWallAndSmallblast,// 这是用来标记的，因为调用的时候会+1，取不到第一个，得取一遍回来才能取到
            LegacySCalAttackType.fireDartsWallAndSmallblast, // OnlyGlow = false  1
            LegacySCalAttackType.fireAbyssalSoul,
            LegacySCalAttackType.fireGigablast,// OnlyGlow = true  3
            LegacySCalAttackType.charge,

            LegacySCalAttackType.fireGigablast,// OnlyGlow = false  5
            LegacySCalAttackType.fireAbyssalSoul,
            LegacySCalAttackType.fireDartsWallAndSmallblast,// OnlyGlow = true  7
            LegacySCalAttackType.charge,

            LegacySCalAttackType.fireDartsWallAndSmallblast,// OnlyGlow = false  9
            LegacySCalAttackType.fireAbyssalSoul,
            LegacySCalAttackType.fireGigablast,
            LegacySCalAttackType.fireGigablast,
            LegacySCalAttackType.fireAbyssalSoul,// OnlyGlow = true   13

            LegacySCalAttackType.charge,
            LegacySCalAttackType.fireDartsWallAndSmallblast,// OnlyGlow = false    15
            LegacySCalAttackType.fireGigablast,// OnlyGlow = true    16
            LegacySCalAttackType.fireGigablast,
            LegacySCalAttackType.charge,

            LegacySCalAttackType.fireDartsWallAndSmallblast,
            LegacySCalAttackType.charge,
            LegacySCalAttackType.fireDartsWallAndSmallblast,
            LegacySCalAttackType.charge,
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
        public const float FirstBulletHellPhase = 0f;
        public const float SecondBulletHellPhase = 1f;
        public const float ThirdBulletHellPhase = 2f;
        public const float SpawnBrothersPhase = 3f;
        public const float Transiting = 4f;
        public const float FourthBulletHellPhase = 5f;
        public const float SoulSeekerPhase = 6f;
        public const float FinalBulletHellPhase = 7f;
        public const float SummonSepulcherPhase = 8f;
        public const float DesPhase = 12f;

        public const int Grief = 1;
        public const int Lament = 2;
        public const int Epiphany = 3;
        public const int Acceptance = 4;
        #endregion
        #endregion
        #region SSD
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 1;

            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                SpriteDirection = 1,
                PortraitScale = 0.7f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
            NPCID.Sets.MPAllowedEnemies[Type] = true;

            NPC.AddToNoModeStateScaleNPC();
        }
        #endregion
        #region SD
        public override void SetDefaults()
        {
            // 我不知道为什么修改NPCdamage就会导致boss属性翻倍，所以扔AI里面初始化了
            // 草拟吗难度增幅
            // 在LAP改了这个问题了
            NPC.damage = 2000;
            NPC.npcSlots = 50f;

            NPC.width = NPC.height = 120;
            NPC.defense = 120;
            NPC.SetDR(DR);
            NPC.value = Item.buyPrice(platinum: 9999, gold: 99, silver: 99, copper: 99);

            NPC.lifeMax = LifeMax;

            NPC.aiStyle = -1;
            AIType = -1;

            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.OnFire3] = false;
            NPC.buffImmune[BuffID.OnFire] = false;

            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = false;
            NPC.chaseable = true;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit4;
        }
        #endregion
        #region 怪物图鉴
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {

            bestiaryEntry.Info.AddRange([new FlavorTextBestiaryInfoElement($"{LocalizationPath.NPCBestiary}.SupremeCalamitasLegacy")]);
        }
        #endregion
        #region 接受发送AI
        //多人同步的二三事
        public override void SendExtraAI(BinaryWriter writer)
        {
            BitsByte net1 = new BitsByte();
            //一个比特=8个字节，如果有部分字节暂时用不上，这些字节是一定得用各种方法占用掉让其形成一个完整的比特的
            //不然发送的时候会有点问题
            net1[0] = initialized;
            net1[1] = spawnArena;
            net1[2] = OnlyGlow;
            net1[3] = hasCharge;
            net1[4] = Enraged;
            net1[5] = canDead;
            net1[6] = isContactDamage;
            net1[7] = isSecondPhase;
            writer.Write(net1);

            BitsByte net2 = new BitsByte();
            net2[0] = isBrotherAlive;
            net2[1] = isSeekerAlive;
            net2[2] = isWormAlive;
            net2[3] = isTrueMelee;
            net2[4] = canDespawn;
            net2[5] = canNextPhase;
            net2[6] = false;
            net2[7] = false;
            writer.Write(net2);

            writer.Write(NPC.lifeMax);
            writer.Write(NPC.life);

            writer.Write(LifeMax);
            writer.Write(DR);
            writer.Write(NPC.localAI[1]);
            writer.Write(NPC.localAI[2]);
            writer.Write(NPC.CI().BossNewAI[6]);
            writer.Write(NPC.CI().BossNewAI[7]);
            writer.Write(NPC.CI().BossNewAI[8]);

            writer.Write(Arena.X);
            writer.Write(Arena.Y);
            writer.Write(Arena.Width);
            writer.Write(Arena.Height);
            writer.Write(dustType);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            BitsByte net1 = reader.ReadByte();
            initialized = net1[0];
            spawnArena = net1[1];
            OnlyGlow = net1[2];
            hasCharge = net1[3];
            Enraged = net1[4];
            canDead = net1[5];
            isContactDamage = net1[6];
            isSecondPhase = net1[7];

            BitsByte net2 = reader.ReadByte();
            isBrotherAlive = net2[0];
            isSeekerAlive = net2[1];
            isWormAlive = net2[2];
            isTrueMelee = net2[3];
            canDespawn = net2[4];
            canNextPhase = net2[5];
            _ = net2[6];
            _ = net2[7];

            NPC.lifeMax = reader.ReadInt32();
            NPC.life = reader.ReadInt32();

            LifeMax = reader.ReadInt32();
            DR = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
            NPC.CI().BossNewAI[6] = reader.ReadSingle();
            NPC.CI().BossNewAI[7] = reader.ReadSingle();
            NPC.CI().BossNewAI[8] = reader.ReadSingle();

            Arena.X = reader.ReadInt32();
            Arena.Y = reader.ReadInt32();
            Arena.Width = reader.ReadInt32();
            Arena.Height = reader.ReadInt32();
            dustType = reader.ReadInt32();
        }
        #endregion
        #region AI
        public override void AI()
        {
            NPC.netOffset = Vector2.Zero;

            LAPContent.ActiveSky<ScalSky>();
            LAPContent.ActivePostProgress<ScalPP>();

            if (NPC.rotation < 0f)
                NPC.rotation += MathHelper.TwoPi;
            else if (NPC.rotation > MathHelper.TwoPi)
                NPC.rotation -= MathHelper.TwoPi; //确保转角一直在2pi内
            // 获取目标
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest(true);

            Player target = Main.player[NPC.target];

            isWormAlive = NPC.AnyNPCs(NPCType<SCalWormHead>());
            isSeekerAlive = NPC.AnyNPCs(NPCType<SoulSeekerSupremeLegacy>());
            isBrotherAlive = NPC.AnyNPCs(NPCType<SupremeCataclysmLegacy>()) || NPC.AnyNPCs(NPCType<SupremeCatastropheLegacy>());

            if (!target.Hitbox.Intersects(Arena))
                Enraged = true;
            else
                Enraged = false;

            if (Main.slimeRain)
            {
                Main.StopSlimeRain(true);
                if (Main.dedServ)
                    NetMessage.SendData(MessageID.WorldData);
            }

            Item targetSelectedItem = target.inventory[target.selectedItem];
            if (targetSelectedItem.DamageType.IsTrueMelee())
                isTrueMelee = true;
            else
                isTrueMelee = false;

            if (initialized == false)
            {
                // 多人模式是随机召唤位置，但是在第一帧传送到对应玩家身边
                NPC.Center = Main.player[NPC.target].Center + new Vector2(0f, -400f);
                ScreenShakeSystem.AddScreenShakes(NPC.Center, 12, 12, 0);
                SpawnDust();
                SpawnDust();
                NPC.damage = 2000;
                initialized = true;
            }

            ref float attackType = ref NPC.ai[0];
            ref float attackTimer = ref NPC.ai[1];

            // NPC.ai[3]用于招式选择
            ref float frameChangeSpeed = ref NPC.localAI[1];
            ref float frameType = ref NPC.localAI[2];
            ref float currentPhase = ref NPC.CI().BossNewAI[6];
            ref float switchToDesperationPhase = ref NPC.CI().BossNewAI[7];
            ref float rotationSpeed = ref NPC.CI().BossNewAI[8];

            // 进入新的阶段
            float lifeRatio = NPC.life / (float)NPC.lifeMax;
            // 音乐
            HandleMusicVariables(lifeRatio);

            // Set the whoAmI variable.
            CIBossAliveFlag.LegacySCal = NPC.whoAmI;
            //给予被针对的玩家BossZen
            target.AddBuff(BuffType<BossEffects>(), 2);

            // 生成场地
            if (!spawnArena)
            {
                SoundEngine.PlaySound(CISounds.SupremeCalamitasSpawn, NPC.position);
                logSpawnPos = new Vector2(target.Center.X, target.Center.Y);
                spawnArena = true;
                SpawnArena(ref attackType, target);
            }
            if (HasCalamity())
            {           
                // 杀了星流飞椅
                if (target.mount?.Type == CalMount.DraedonGamerChairMount)
                    target.mount.Dismount(target);
            }

            // 指定期间无敌
            NPC.dontTakeDamage = isSeekerAlive || isWormAlive || Enraged;

            // 目标死亡后消失
            if (!target.active || target.dead)
            {
                NPC.Opacity = Math.Clamp(NPC.Opacity - 0.1f, 0f, 1f);

                for (int i = 0; i < 2; i++)
                {
                    Dust fire = Dust.NewDustPerfect(NPC.Center, DustID.LifeDrain);
                    fire.position += Main.rand.NextVector2Circular(36f, 36f);
                    fire.velocity = Main.rand.NextVector2Circular(8f, 8f);
                    fire.noGravity = true;
                    fire.scale *= Main.rand.NextFloat(1f, 1.2f);
                }
                if (NPC.Opacity <= 0f)
                    NPC.active = false;
                return;
            }
            #region 阶段判定
            // 进入新阶段
            // 用于开局的攻击
            if (lifeRatio == Phase1_SepulcherLifeRatio && currentPhase == FirstBulletHellPhase && canNextPhase)
            {
                SendStartText();
                attackTimer = 0;
                attackType = (int)LegacySCalAttackType.BulletHell;
                currentPhase++;
                NPC.netUpdate = true;
                canNextPhase = false;
                return;
            }
            // 第二阶段，75% - 50%
            if (lifeRatio <= Phase2LifeRatio && currentPhase == SecondBulletHellPhase && canNextPhase)
            {
                SendBattleText(2);
                attackTimer = 0;
                attackType = (int)LegacySCalAttackType.BulletHell;
                currentPhase++;
                NPC.netUpdate = true;
                canNextPhase = false;
                return;
            }
            // 第三阶段，50% - 45%
            if (lifeRatio <= Phase3LifeRatio && currentPhase == ThirdBulletHellPhase && canNextPhase)
            {
                dustType = CIDustID.DustMushroomSpray113;
                SendBattleText(3);
                attackTimer = 0;
                attackType = (int)LegacySCalAttackType.BulletHell;
                currentPhase++;
                NPC.netUpdate = true;
                canNextPhase = false;
                return;
            }
            // 第四阶段，45% - 40%
            if (lifeRatio <= Phase4_brotherLifeRatio && currentPhase == SpawnBrothersPhase && canNextPhase)
            {
                SendBattleText(4);
                attackTimer = 0;
                attackType = (int)LegacySCalAttackType.SummonBrother;
                currentPhase++;
                NPC.netUpdate = true;
                canNextPhase = false;
                return;
            }
            // 第五阶段，40% - 30%
            if (lifeRatio <= Phase5LifeRatio && currentPhase == Transiting && canNextPhase)
            {
                attackTimer = 0;
                attackType = (int)LegacySCalAttackType.PhaseTransition;
                currentPhase++;
                NPC.netUpdate = true;
                canNextPhase = false;
                return;
            }
            // 第六阶段，30% - 20%
            if (lifeRatio <= Phase6LifeRatio && currentPhase == FourthBulletHellPhase && canNextPhase)
            {
                dustType = DustID.LifeDrain;
                SendBattleText(6);
                attackTimer = 0;
                attackType = (int)LegacySCalAttackType.BulletHell;
                currentPhase++;
                NPC.netUpdate = true;
                canNextPhase = false;
                return;
            }
            // 第七阶段，20% - 10%
            if (lifeRatio <= Phase7_SoulSeekerLifeRatio && currentPhase == SoulSeekerPhase && canNextPhase)
            {
                SendBattleText(7);
                attackTimer = 0;
                attackType = (int)LegacySCalAttackType.SummonSoulSeeker;
                currentPhase++;
                NPC.netUpdate = true;
                canNextPhase = false;
                return;
            }
            // 第八阶段，10% - 8%
            if (lifeRatio <= Phase8LifeRatio && currentPhase == FinalBulletHellPhase && canNextPhase)
            {
                SendBattleText(8);
                attackTimer = 0;
                NPC.SetDR(0.1f);
                attackType = (int)LegacySCalAttackType.BulletHell;
                currentPhase++;
                NPC.netUpdate = true;
                canNextPhase = false;
                return;
            }
            // 第九阶段，8% - 1%
            if (lifeRatio <= Phase9_SepulcherLifeRatio && currentPhase == SummonSepulcherPhase && canNextPhase)
            {
                SendBattleText(9);
                attackTimer = 0;
                attackType = (int)LegacySCalAttackType.SummonSepulcher;
                currentPhase++;
                NPC.netUpdate = true;
                canNextPhase = false;
                return;
            }
            // 用于发送文字的标记
            if (lifeRatio <= 0.06f && currentPhase == 9f && canNextPhase)
            {
                SendBattleText(10);
                attackTimer = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            // 用于发送文字的标记
            if (lifeRatio <= 0.04f && currentPhase == 10f && canNextPhase)
            {
                SendBattleText(11);
                attackTimer = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            // 用于发送文字的标记
            if (lifeRatio <= 0.02f && currentPhase == 11f && canNextPhase)
            {
                SendBattleText(12);
                attackTimer = 0;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            // 第十阶段，1% - 0%
            if (lifeRatio <= Phase10LifeRatio && currentPhase == DesPhase && canNextPhase)
            {
                SendBattleText(13);
                attackTimer = 0;
                attackType = (int)LegacySCalAttackType.DesperationPhase;
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            #endregion
            // 开始时重设为真
            isContactDamage = true;
            // 重置旋转速度
            rotationSpeed = 0.5f;

            switch ((LegacySCalAttackType)attackType)
            {
                case LegacySCalAttackType.BulletHell:
                    rotationSpeed = 0.08f;
                    DoBehavior_BulletHell(target, ref attackTimer, currentPhase, ref attackType);
                    break;
                case LegacySCalAttackType.fireDartsWallAndSmallblast:
                    DoBehavior_FireDartsWallAndSmallblast(attackTimer);
                    break;
                case LegacySCalAttackType.charge:
                    DoBehavior_Charge(target, ref attackTimer, currentPhase);
                    break;
                case LegacySCalAttackType.fireAbyssalSoul:
                    DoBehavior_FireAbyssalSoul(target, attackTimer);
                    break;
                case LegacySCalAttackType.fireGigablast:
                    DoBehavior_FireGigablast(target, attackTimer);
                    break;
                case LegacySCalAttackType.SummonSepulcher:
                    DoBehavior_SummonSepulcher();
                    break;
                case LegacySCalAttackType.SummonBrother:
                    DoBehavior_SummonBrother(target, ref rotationSpeed, attackTimer);
                    break;
                case LegacySCalAttackType.SummonSoulSeeker:
                    DoBehavior_SummonSoulSeeker();
                    break;
                case LegacySCalAttackType.PhaseTransition:
                    DoBehavior_PhaseTransition(target, attackTimer, 0.06f);
                    break;
                case LegacySCalAttackType.DesperationPhase:
                    DoBehavior_DesperationPhase(attackTimer);
                    break;
                default:
                    NPC.velocity *= 0.95f;
                    break;
            }

            if ((LegacySCalAttackType)attackType != LegacySCalAttackType.PhaseTransition)
                LookAtTarget(target, rotationSpeed);

            // 防止有人兄弟阶段拖太久了
            if (attackTimer < 1000000)
                attackTimer++;

            // Main.NewText($"attackTimer : {attackTimer}");

            if (isWormAlive || isBrotherAlive || isSeekerAlive)
                isContactDamage = false;

            // 手动重置伤害和是否无敌
            if (!isContactDamage)
            {
                if ((LegacySCalAttackType)attackType != LegacySCalAttackType.PhaseTransition && !isSeekerAlive)
                    NPC.damage = 0;

                rotationSpeed = 0.08f;
                NPC.dontTakeDamage = true;
                NPC.chaseable = false;
            }
            else
            {
                if ((LegacySCalAttackType)attackType != LegacySCalAttackType.DesperationPhase)
                    NPC.damage = ContactDamage;
                NPC.dontTakeDamage = false;
                NPC.chaseable = true;
            }

            if (Enraged)
            {
                vectorMultiplier += 2f;
                NPC.SetDR(0.01f);
            }
            else
            {
                vectorMultiplier = 1f;
                NPC.SetDR(DR);
            }

            if ((LegacySCalAttackType)attackType == LegacySCalAttackType.DesperationPhase)
            {
                NPC.damage = 0;
                NPC.SetDR(1f);
                NPC.defense = 0;
            }
        }
        #endregion
        #region 音乐
        public void HandleMusicVariables(float lifeRatio)
        {
            if (lifeRatio <= 0.01f)
            {
                CIBossAliveFlag.LegacySCalBGM = Acceptance;
                return;
            }
            if (lifeRatio <= 0.3f)
            {
                CIBossAliveFlag.LegacySCalBGM = Epiphany;
                return;
            }
            if (lifeRatio <= 0.5f)
            {
                CIBossAliveFlag.LegacySCalBGM = Lament;
                return;
            }
            CIBossAliveFlag.LegacySCalBGM = Grief;
        }
        #endregion
        #region 技能
        #region 看向目标
        public void LookAtTarget(Player player, float rotationSpeed)
        {
            NPC.rotation = NPC.rotation.AngleLerp(NPC.AngleTo(player.Center) - MathHelper.PiOver2, rotationSpeed);
        }
        #endregion
        #region 弹幕炼狱管理
        public void DoBehavior_BulletHell(Player target, ref float attacktimer, float currentPhaseHell, ref float attacktype)
        {
            Player player = Main.player[NPC.target];
            isContactDamage = false;
            NPC.velocity *= 0.95f;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (attacktimer == 500)
                    SoundEngine.PlaySound(CISounds.SCalRumble, NPC.position);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // 第一轮
                    if (currentPhaseHell == 1f)
                        BulletHell1(target, attacktimer);
                    // 第二轮
                    if (currentPhaseHell == 2f)
                        BulletHell2(target, attacktimer);
                    // 第三轮
                    if (currentPhaseHell == 3f)
                        BulletHell3(target, attacktimer);
                    // 第四轮
                    if (currentPhaseHell == 6f)
                        BulletHell4(target, attacktimer);
                    // 第五轮
                    if (currentPhaseHell == 8f)
                        BulletHell5(target, attacktimer);
                }
                if (attacktimer > 500 && attacktimer < 860)
                {
                    for (int i = 0; i < (int)((attacktimer - 400) * 0.01f); i++)
                    {
                        PulseEffect();
                    }
                }
                if (attacktimer == 901)
                    DeSpawn();
                if (attacktimer == 902)
                {
                    SpawnDust();
                    SoundEngine.PlaySound(CISounds.SCalEndBH, NPC.position);
                    if (currentPhaseHell != 1f)
                        SelectNextAttack();
                    else
                        DoBehavior_SummonSepulcher();
                    canNextPhase = true;
                }

            }
        }
        #endregion
        #region 硫火飞镖墙
        public bool canFireSplitingFireball = true;
        public void DoBehavior_FireDartsWallAndSmallblast(float attacktimer)
        {
            int BarrageAttackDelay = isWormAlive ? 180 : 90;
            int TotalAttackTime = 300;
            Player player = Main.player[NPC.target];

            // Scal的加速度和速度
            float velocity = 12f;
            float acceleration = 0.12f;

            // 如果玩家手持真近战武器，那么降低加速度
            if (isTrueMelee)
                acceleration *= 0.5f;

            // 終灾应该在哪
            Vector2 destination = new Vector2(player.Center.X, player.Center.Y - 550f);
            // 离应该在哪的距离
            Vector2 distanceFromDestination = destination - NPC.Center;
            // 移动
            SmoothMovement(NPC, 0f, distanceFromDestination, velocity, acceleration, true);

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
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        if (Main.rand.Next(6) < 3 && canFireSplitingFireball)
                        {
                            SoundEngine.PlaySound(CISounds.BrimstoneFireblastImpact, NPC.position);
                            canFireSplitingFireball = false;
                            Vector2 fireballDirection = new Vector2(player.Center.X - projectileOrigin.X, player.Center.Y - projectileOrigin.Y - 550f).SafeNormalize(Vector2.Zero) * baseSpeed;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, fireballDirection, ProjectileType<SCalBrimstoneFireblastLegacy>(), BrimstoneDarts, 0f, Main.myPlayer, 0f, 0f);
                        }
                        else
                        {
                            SoundEngine.PlaySound(CISounds.BrimstoneShoot, NPC.position);
                            canFireSplitingFireball = true;
                            // 多重弹幕散射
                            for (int i = 0; i < 8; i++)
                            {
                                Vector2 barrageDirection = (player.Center - projectileOrigin).SafeNormalize(Vector2.Zero);
                                float speedBoost = i > 3 ? -(i - 3) : i;
                                float speedFactor = 8f + speedBoost;

                                Vector2 projvelocity = barrageDirection * speedFactor + new Vector2(speedBoost);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileOrigin, projvelocity, ProjectileType<BrimstoneBarrageLegacy>(), BrimstoneFireblast, 0f, Main.myPlayer);
                            }
                        }
                    }
                }
            }
            if (attacktimer == TotalAttackTime)
                SelectNextAttack();
        }
        #endregion
        #region 冲刺
        public bool hasCharge = false;
        public int ChargeCount = 0;
        public void DoBehavior_Charge(Player target, ref float attacktimer, float currentPhase)
        {
            int totalCharge = 4;
            int chargeCount = 25;
            int chargeCooldown = 70;

            // 冲刺时转头的速度
            NPC.CI().BossNewAI[8] = 0.04f;

            if (hasCharge == false)
            {
                float chargeVelocity = isWormAlive ? 26f : 30f;
                chargeVelocity += 1f * currentPhase;

                if (isSeekerAlive)
                    chargeVelocity += 5f;

                Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation + MathHelper.PiOver2);
                direction = direction.SafeNormalize(Vector2.UnitX);
                NPC.velocity = direction * chargeVelocity;

                NPC.netUpdate = true;
                SoundEngine.PlaySound(CISounds.SCalDash, NPC.Center);
                hasCharge = true;
            }
            else
            {
                if (attacktimer > chargeCount)
                {
                    NPC.CI().BossNewAI[8] += 0.15f;

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
                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType, 0f, 0f, 0, default, 1.25f);
                }
                NPC.netUpdate = true;
            }

            if (ChargeCount > totalCharge - 1)
                SelectNextAttack();
        }
        #endregion
        #region 平射深渊亡魂
        public void DoBehavior_FireAbyssalSoul(Player target, float attacktimer)
        {
            // How fast SCal moves to the destination
            float velocity = 32;
            float acceleration = 1.2f;
            int distance = 600;

            int totalFireTime = 480;
            int fireDelay = isWormAlive ? 40 : 20;
            // 如果玩家手持真近战武器，那么降低加速度
            if (isTrueMelee)
                acceleration *= 0.5f;

            int posX = 1;
            if (NPC.Center.X < target.Center.X)
                posX = -1;

            // This is where SCal should be
            Vector2 destination = new Vector2(target.Center.X + posX * distance, target.Center.Y);

            // How far SCal is from where she's supposed to be
            Vector2 distanceFromDestination = destination - NPC.Center;

            SmoothMovement(NPC, 0f, distanceFromDestination, velocity, acceleration, true);

            if (attacktimer % fireDelay == 0)
            {
                SoundEngine.PlaySound(CISounds.BrimstoneHellblastSound, NPC.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // 使用旋转角度计算方向
                    Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation); // 基础方向根据旋转角度
                    direction = direction.SafeNormalize(Vector2.UnitX);

                    Vector2 offset = new Vector2(0, 60).RotatedBy(NPC.rotation);
                    Vector2 projectileVelocity = direction * 10f * vectorMultiplier;
                    Vector2 projectileSpawn = NPC.Center + offset;
                    projectileVelocity = projectileVelocity.RotatedBy(MathHelper.PiOver2);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, projectileVelocity, ProjectileType<BrimstoneHellblastLegacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
            }
            if (attacktimer > totalFireTime)
                SelectNextAttack();
        }
        #endregion
        #region 平射无际裂变
        public void DoBehavior_FireGigablast(Player target, float attacktimer)
        {
            // How fast SCal moves to the destination
            float velocity = 32f;
            float acceleration = 1.2f;
            int distance = 750;

            int totalFireTime = 300;
            int fireDelay = isWormAlive ? 280 : 140;
            // 如果玩家手持真近战武器，那么降低加速度
            if (isTrueMelee)
                acceleration *= 0.5f;

            int posX = 1;
            if (NPC.Center.X < target.Center.X)
                posX = -1;

            // This is where SCal should be
            Vector2 destination = new Vector2(target.Center.X + posX * distance, target.Center.Y);

            // How far SCal is from where she's supposed to be
            Vector2 distanceFromDestination = destination - NPC.Center;

            SmoothMovement(NPC, 0f, distanceFromDestination, velocity, acceleration, true);

            if (attacktimer % fireDelay == 0)
            {
                SoundEngine.PlaySound(CISounds.BrimstoneBigShoot, NPC.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // 使用旋转角度计算方向
                    Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation); // 基础方向根据旋转角度
                    direction = direction.SafeNormalize(Vector2.UnitX);

                    Vector2 projectileVelocity = direction * 5f * vectorMultiplier;
                    Vector2 projectileSpawn = NPC.Center/* + projectileVelocity * 8f*/;
                    projectileVelocity = projectileVelocity.RotatedBy(MathHelper.PiOver2);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, projectileVelocity, ProjectileType<SCalBrimstoneGigablastLegacy>(), BrimstoneGigablast, 0f, Main.myPlayer, 0f, 2f);
                }
            }
            if (attacktimer > totalFireTime)
                SelectNextAttack();
        }
        #endregion
        #region 选择下一个攻击
        public int AttackTypeCount = 0;
        // 选择下一个攻击
        public void SelectNextAttack(int? Skip = null)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            // ai1对应的啥
            // float attackTimer = NPC.ai[1];
            // int attackType = (int)NPC.ai[0];
            // int currentPhase = (int)NPC.CIMod().BossNewAI[6];

            // 置零攻击计时器和选择攻击类型
            NPC.ai[1] = 0f;
            ChargeCount = 0;

            // 获取当前索引（不重置）
            int currentIndex = (int)NPC.ai[3];

            LegacySCalAttackType[] attackCycle = AttackCycle;
            if (Skip == null)
            {
                // 递增索引
                currentIndex++;
                if (currentIndex >= AttackCycle.Length)
                    currentIndex = 0;
            }
            else
            {
                currentIndex = (int)Skip;
                if (currentIndex >= AttackCycle.Length)
                    currentIndex = 0;
            }
            // 更新索引和攻击类型
            NPC.ai[3] = currentIndex;
            NPC.ai[0] = (int)AttackCycle[currentIndex];

            // 多人游戏同步
            if (Main.netMode == NetmodeID.Server)
                NPC.netUpdate = true;
        }
        #endregion
        #region 弹幕炼狱
        #region 第一轮
        public void BulletHell1(Player player, float attacktimer)
        {
            int BulletHell1SpawnDelay = Enraged ? 4 : 6;

            if (attacktimer % BulletHell1SpawnDelay == 0)
            {
                if (attacktimer < 300) //blasts from above
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 4f * vectorMultiplier, ProjectileType<BrimstoneHellblast2Legacy>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 600) //blasts from left and right
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3.5f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 900) //blasts from above, left, and right
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 3f * vectorMultiplier, ProjectileType<BrimstoneHellblast2Legacy>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                }
            }
        }
        #endregion
        #region 第二轮
        public void BulletHell2(Player player, float attacktimer)
        {
            int SecondBulletHellblastsDelay = 180;
            int BulletHell2SpawnDelay = Enraged ? 7 : 9;

            // 发射小爆弹
            if (attacktimer % SecondBulletHellblastsDelay == 0)
            {
                if (Main.rand.NextBool())
                {
                    SoundEngine.PlaySound(CISounds.BrimstoneBigShoot, NPC.Center);
                    // 左右两侧
                    float distance = Main.rand.NextBool() ? -1000f : 1000f;
                    float velocity = (distance == -1000f ? 4f : -4f) * vectorMultiplier;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + distance, player.position.Y, velocity, 0f, ProjectileType<SCalBrimstoneFireblastLegacy>(), BrimstoneFireblast, 0f, Main.myPlayer, 0f, 2f);
                }
                else
                {
                    SoundEngine.PlaySound(CISounds.BrimstoneFireblastImpact, NPC.Center);
                    // 上方
                    float spread = Main.rand.Next(-1000, 1000);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + spread, player.position.Y - 1000f, 0f, 5f * vectorMultiplier, ProjectileType<SCalBrimstoneFireblastLegacy>(), BrimstoneFireblast, 0f, Main.myPlayer, 0f, 2f);
                }
            }
            if (attacktimer % BulletHell2SpawnDelay == 0)
            {
                if (attacktimer < 300) // 下方
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y + 1000f, 0f, -4f * vectorMultiplier, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 600) //左侧
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 900) //左侧与右侧
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
            }
        }
        #endregion
        #region 第三轮
        public void BulletHell3(Player player, float attacktimer)
        {
            int SecondBulletHellblastsDelay = 240;
            int SecondBulletHellGigablastsDelay = 180;
            int BulletHell3SpawnDelay = Enraged ? 9 : 11;

            // 发射大小爆弹
            if (attacktimer % SecondBulletHellGigablastsDelay == 0) // 发射小爆弹
            {
                SoundEngine.PlaySound(CISounds.BrimstoneBigShoot, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 5f * vectorMultiplier, ProjectileType<SCalBrimstoneGigablastLegacy>(), BrimstoneGigablast, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer % SecondBulletHellblastsDelay == 0) // 发射无际裂变
            {
                SoundEngine.PlaySound(CISounds.BrimstoneFireblastImpact, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 10f * vectorMultiplier, ProjectileType<SCalBrimstoneFireblastLegacy>(), BrimstoneFireblast, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer % BulletHell3SpawnDelay == 0)
            {
                if (attacktimer < 300) // 下方
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y + 1000f, 0f, -4f * vectorMultiplier, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 600) //右侧
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3.5f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 900) //左侧与右侧
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3.5f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
            }
            if (attacktimer == 890)
                DeSpawn();
        }
        #endregion
        #region 第四轮
        public void BulletHell4(Player player, float attacktimer)
        {
            int SecondBulletHellblastsDelay = 240;
            int SecondBulletHellGigablastsDelay = 180;
            int BulletHell4SpawnDelay = Enraged ? 10 : 12;

            SpawnBloodMoon(player, attacktimer);
            // 发射大小爆弹
            if (attacktimer % SecondBulletHellGigablastsDelay == 0) // 发射无际裂变
            {
                SoundEngine.PlaySound(CISounds.BrimstoneBigShoot, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 5f * vectorMultiplier, ProjectileType<SCalBrimstoneGigablastLegacy>(), BrimstoneGigablast, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer % SecondBulletHellblastsDelay == 0) // 发射深渊炙炎
            {
                SoundEngine.PlaySound(CISounds.BrimstoneFireblastImpact, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 10f * vectorMultiplier, ProjectileType<SCalBrimstoneFireblastLegacy>(), BrimstoneFireblast, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer % BulletHell4SpawnDelay == 0)
            {
                if (attacktimer < 300) // 下方
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y + 1000f, 0f, -4f * vectorMultiplier, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 600) //左侧
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 890) //左侧与右侧
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3.5f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
            }
        }
        #endregion
        #region 第五轮
        public void BulletHell5(Player player, float attacktimer)
        {
            int SecondBulletHellblastsDelay = 360;
            int SecondBulletHellGigablastsDelay = 240;
            int SecondBrimstoneWaveDelay = 30;
            int BulletHell5SpawnDelay = Enraged ? 12 : 14;

            // 发射大小爆弹
            if (attacktimer % SecondBulletHellGigablastsDelay == 0) // 发射无际裂变
            {
                SoundEngine.PlaySound(CISounds.BrimstoneBigShoot, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 5f * vectorMultiplier, ProjectileType<SCalBrimstoneGigablastLegacy>(), BrimstoneGigablast, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer % SecondBulletHellblastsDelay == 0) // 发射深渊炙炎
            {
                SoundEngine.PlaySound(CISounds.BrimstoneFireblastImpact, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 10f * vectorMultiplier, ProjectileType<SCalBrimstoneFireblastLegacy>(), BrimstoneFireblast, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer % SecondBrimstoneWaveDelay == 0) //projectiles that move in wave pattern
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -5f * vectorMultiplier, 0f, ProjectileType<BrimstoneWaveLegacy>(), BrimstoneDarts, 0f, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 5f * vectorMultiplier, 0f, ProjectileType<BrimstoneWaveLegacy>(), BrimstoneDarts, 0f, Main.myPlayer, 0f, 0f);
            }

            if (attacktimer % BulletHell5SpawnDelay == 0)
            {
                if (attacktimer < 300) // 上方
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 4f * vectorMultiplier, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 600) //左侧与右侧
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3.5f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 900) //左侧与右侧与上方
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 4f * vectorMultiplier, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3.5f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
            }
        }
        #endregion
        #endregion
        #region 生成猩红圆月
        public void SpawnBloodMoon(Player player, float attacktimer)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
                int distanceX = -1000;
            int distanceY = 1000;
            int randomSpread = Main.rand.Next(-1000, 1000);
            int SpawnDelay = 200;
            if (attacktimer == SpawnDelay) // 上方
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + randomSpread, player.position.Y - distanceY, 0f, 1f * vectorMultiplier, ProjectileType<BrimstoneMonsterLegacy>(), MoonDamage, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer == SpawnDelay * 2) // 下方
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + randomSpread, player.position.Y - distanceX, 0f, 1f * vectorMultiplier, ProjectileType<BrimstoneMonsterLegacy>(), MoonDamage, 0f, Main.myPlayer, 0f, 1f);
            }
            if (attacktimer == SpawnDelay * 3) // 左侧
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + distanceY, player.position.Y - randomSpread, 0f, 1f * vectorMultiplier, ProjectileType<BrimstoneMonsterLegacy>(), MoonDamage, 0f, Main.myPlayer, 0f, 2f);
            }
            if (attacktimer == SpawnDelay * 4) // 右侧
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + distanceX, player.position.Y - randomSpread, 0f, 1f * vectorMultiplier, ProjectileType<BrimstoneMonsterLegacy>(), MoonDamage, 0f, Main.myPlayer, 0f, 3f);
            }
        }
        #endregion
        #region 召唤灾坟虫
        public void DoBehavior_SummonSepulcher()
        {
            SoundEngine.PlaySound(CISounds.SepulcherSpawn, NPC.position);
            int spawnYAdd = CIWorld.Death ? 100 : 125;
            /*
            string key = "Mods.CalamityMod.SupremeBossText3";
            Color messageColor = Color.DarkRed;
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(Language.GetTextValue(key), messageColor);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
            }
            */
            float spawnX = logSpawnPos.X - 1000 + 50;
            float spawnX2 = logSpawnPos.X + 1000 - 50;
            float spawnY = logSpawnPos.Y - (CIWorld.Death ? 1000 : 1150) + (CIWorld.Death ? 200 : 250);

            int spawnXAdd = CIWorld.Death ? 200 : 250;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int x = 0; x < 5; x++)
                {
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnX, (int)spawnY, NPCType<SCalWormHeart>(), 0, 0f, 0f, 0f, 0f, 255);
                    spawnX += spawnXAdd;
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnX2, (int)spawnY, NPCType<SCalWormHeart>(), 0, 0f, 0f, 0f, 0f, 255);
                    spawnX2 -= spawnXAdd;
                    spawnY += spawnYAdd;
                }
                NPC.SpawnOnPlayer(NPC.FindClosestPlayer(), NPCType<SCalWormHead>());
            }
            SelectNextAttack();
            canNextPhase = true;
        }
        #endregion
        #region 删除所有弹幕
        public static void DeSpawn()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            for (int x = 0; x < Main.maxProjectiles; x++)
            {
                Projectile projectile = Main.projectile[x];
                if (projectile.active)
                {
                    if (projectile.type == ProjectileType<BrimstoneHellblast2Legacy>() ||
                        projectile.type == ProjectileType<BrimstoneBarrageLegacy>() ||
                        projectile.type == ProjectileType<BrimstoneWaveLegacy>())
                    {
                        if (projectile.timeLeft > 60)
                            projectile.timeLeft = 60;
                    }
                    else if (projectile.type == ProjectileType<SCalBrimstoneFireblastLegacy>() || projectile.type == ProjectileType<SCalBrimstoneGigablastLegacy>())
                    {
                        projectile.ai[1] = 1f;

                        if (projectile.timeLeft > 15)
                            projectile.timeLeft = 15;
                    }
                }
            }
        }
        #endregion
        #region 召唤兄弟
        public void DoBehavior_SummonBrother(Player target, ref float rotationSpeed, float attacktimer)
        {
            float spawnX = target.Center.X - 2000;
            float spawnX2 = target.Center.X + 2000;

            float spawnY = target.Center.Y - 1000;

            isContactDamage = false;
            NPC.velocity *= 0.95f;
            if (attacktimer == 1)
            {
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnX, (int)spawnY, NPCType<SupremeCataclysmLegacy>());
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnX2, (int)spawnY, NPCType<SupremeCatastropheLegacy>());
                canNextPhase = true;
            }

            // 防止一出来就选择了
            if (!isBrotherAlive && attacktimer > 5)
            {
                SelectNextAttack();
            }
        }
        #endregion
        #region 召唤探魂眼
        public void DoBehavior_SummonSoulSeeker()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                SoundEngine.PlaySound(SoundID.Item74, NPC.position);
                for (int I = 0; I < 20; I++)
                {
                    int FireEye = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.Center.X + Math.Sin(I * 18) * 300), (int)(NPC.Center.Y + Math.Cos(I * 18) * 300), NPCType<SoulSeekerSupremeLegacy>(), NPC.whoAmI, 0, 0, 0, -1);
                    NPC Eye = Main.npc[FireEye];
                    Eye.ai[0] = I * 18;
                    Eye.ai[3] = I * 18;
                }
                SpawnDust();
            }
            OnlyGlow = true;
            // 召唤探魂眼时立刻转到冲刺
            SelectNextAttack(18);
            canNextPhase = true;
        }
        #endregion
        #endregion
        #region 演出
        #region 转阶段转圈
        public float roatationSpeed = 0f;
        public void DoBehavior_PhaseTransition(Player target, float attacktimer, float rotationSpeed)
        {
            float phase1Duration = 180;    // 正向段持续时间
            float spinCount = 8f;              // 旋转圈数
            isContactDamage = false;
            NPC.velocity *= 0.95f;
            // 初始化随机偏移
            if (attacktimer == 1)
            {
                spinCount += MathHelper.Pi;
                SoundEngine.PlaySound(CISounds.SupremeCalamitasSpawn, NPC.position);
            }
            // 旋转
            if (attacktimer < phase1Duration)
            {
                for (int i = 0; i < (int)(attacktimer * 0.01f); i++)
                {
                    PulseEffect();
                }
                // 计算当前帧和上一帧的缓动进度
                float currentProgress = attacktimer / phase1Duration;
                float currentEased = EasingHelper.EaseInOutQuad(currentProgress);

                float previousProgress = (attacktimer - 1f) / phase1Duration;
                float previousEased = EasingHelper.EaseInOutQuad(previousProgress);

                float totalRotation = spinCount * MathHelper.TwoPi;
                float rotationIncrement = (currentEased - previousEased) * totalRotation;
                // 应用旋转
                NPC.rotation += rotationIncrement;
                NPC.rotation += MathHelper.Pi / phase1Duration;

            }
            // 转向玩家
            else
            {
                roatationSpeed += 0.0005f;
                // 转向玩家
                NPC.rotation = NPC.rotation.AngleLerp(NPC.AngleTo(target.Center) - MathHelper.PiOver2, roatationSpeed);
                // 特效和音效
                if (attacktimer == phase1Duration)
                {
                    SendBattleText(5);
                    isSecondPhase = true;
                    SpawnDust();
                    SpawnDust();
                    SoundEngine.PlaySound(CISounds.SCalEndBH, NPC.position);
                }
            }

            if (attacktimer == 360)
            {
                canNextPhase = true;
                SelectNextAttack(19);
                OnlyGlow = true;
            }
        }
        #endregion
        #region 最终对话阶段
        public void DoBehavior_DesperationPhase(float attacktimer)
        {
            OnlyGlow = false;
            int totaldesperationTime = 900;
            Vector2 FinaPos = new(logSpawnPos.X, logSpawnPos.Y - 200);
            if (attacktimer == 2)
                Dust.QuickDustLine(NPC.Center, FinaPos, 500f, Color.Gray);
            if (attacktimer == 3)
            {
                NPC.position = FinaPos;
                for (int x = 0; x < Main.maxProjectiles; x++)
                {
                    Projectile projectile = Main.projectile[x];
                    if (projectile.active)
                    {
                        if (projectile.type == ProjectileType<BrimstoneMonsterLegacy>())
                        {
                            if (projectile.timeLeft > 90)
                                projectile.timeLeft = 90;
                        }
                    }
                }
            }
            if (attacktimer == 450)
                SendBattleText(14);

            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.damage = 0;
            if (attacktimer < totaldesperationTime)
            {
                isContactDamage = false;
                NPC.velocity.X *= 0.95f;
            }
            if (attacktimer == totaldesperationTime)
            {
                SendBattleText(15);
                canDead = true;
                NPC.SetDR(1f);
            }
        }
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
                float dustPositionX = NPC.Center.X - scale / 2;
                float dustPositionY = NPC.Center.Y - scale / 2;
                while (dustAmtSpawned < dustAmt)
                {
                    float dustVelocityX = Main.rand.Next(-random, random);
                    float dustVelocityY = Main.rand.Next(-random, random);
                    float dustVelocityScalar = random * 2f;
                    float dustVelocity = (float)Math.Sqrt(dustVelocityX * dustVelocityX + dustVelocityY * dustVelocityY);
                    dustVelocity = dustVelocityScalar / dustVelocity;
                    dustVelocityX *= dustVelocity;
                    dustVelocityY *= dustVelocity;
                    int dust = Dust.NewDust(new Vector2(dustPositionX, dustPositionY), scale, scale, dustType, 0f, 0f, 100, default, 2f);
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
                if (dustType == CIDustID.DustMushroomSpray113)
                {
                    float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                    Vector2 spawnPosition = NPC.Center + angle.ToRotationVector2() * (300f + Main.rand.NextFloat(100f, 100f));
                    Vector2 velocity = (angle - (float)Math.PI).ToRotationVector2() * Main.rand.NextFloat(20f, 35f);
                    Dust dust = Dust.NewDustPerfect(spawnPosition, dustType, velocity);
                    dust.scale = 0.9f;
                    dust.fadeIn = 1.25f;
                    dust.noGravity = true;
                    dust.velocity *= 0.96f;
                }
                else
                {
                    float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                    Vector2 spawnPosition = NPC.Center + angle.ToRotationVector2() * (300f + Main.rand.NextFloat(100f, 100f));
                    Vector2 velocity = (angle - (float)Math.PI).ToRotationVector2() * Main.rand.NextFloat(50f, 80f);
                    Dust dust = Dust.NewDustPerfect(spawnPosition, dustType, velocity);
                    dust.scale = 0.9f;
                    dust.fadeIn = 1.25f;
                    dust.noGravity = true;
                }
            }
        }
        #endregion
        #endregion
        #region 文本
        public static void SendStartText()
        {
            if (CIDownedBossSystem.DownedLegacyScal)
            {
                if (Main.LocalPlayer.CI().LegacyScal_PlayerKillCount >= 4)
                    CIUtils.BroadcastLocalizedText($"{LocalizationPath.BossText}.KillScalMoreThan4", Color.OrangeRed);
                else if (Main.LocalPlayer.CI().LegacyScal_PlayerKillCount == 1)
                    CIUtils.BroadcastLocalizedText($"{LocalizationPath.BossText}.KillScalOnce", Color.OrangeRed);
            }
            else
            {
                if (Main.LocalPlayer.CI().LegacyScal_PlayerDeathCount == 50)
                    CIUtils.BroadcastLocalizedText($"{LocalizationPath.BossText}.Scal_PlayerDeathMoreThan50", Color.OrangeRed);
                else if (Main.LocalPlayer.CI().LegacyScal_PlayerDeathCount > 19)
                    CIUtils.BroadcastLocalizedText($"{LocalizationPath.BossText}.Scal_PlayerDeathMoreThan20", Color.OrangeRed);
                else if (Main.LocalPlayer.CI().LegacyScal_PlayerDeathCount > 4)
                    CIUtils.BroadcastLocalizedText($"{LocalizationPath.BossText}.Scal_PlayerDeathMoreThan4", Color.OrangeRed);
            }
        }
        public void SendBattleText(int ID)
        {
            CIUtils.BroadcastLocalizedText($"{LocalizationPath.BossText}.ScalEnterPhase" + ID, Color.OrangeRed);
        }
        #endregion
        #region 生成场地
        public void SpawnArena(ref float attackTypeChange, Player target)
        {
            float ArenaSize = 129;
            // Define the arena.
            Vector2 arenaArea = new(ArenaSize, ArenaSize);
            Arena = Utils.CenteredRectangle(target.Center, arenaArea * 16f);
            int left = (int)(Arena.Center().X / 16 - arenaArea.X * 0.5f);
            int right = (int)(Arena.Center().X / 16 + arenaArea.X * 0.5f);
            int top = (int)(Arena.Center().Y / 16 - arenaArea.Y * 0.5f);
            int bottom = (int)(Arena.Center().Y / 16 + arenaArea.Y * 0.5f);
            int arenaTileType = TileType<LegacyArenaTile>();

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
            return true;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.ScalDebuffs(300, 480, 480);
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
            if (NPC.ai[3] == 1)
                OnlyGlow = false;

            if (NPC.ai[3] == 3)
                OnlyGlow = true;

            if (NPC.ai[3] == 5)
                OnlyGlow = false;

            if (NPC.ai[3] == 7)
                OnlyGlow = true;

            if (NPC.ai[3] == 9)
                OnlyGlow = false;

            if (NPC.ai[3] == 13)
                OnlyGlow = true;

            if (NPC.ai[3] == 15)
                OnlyGlow = false;

            if (NPC.ai[3] == 16)
                OnlyGlow = true;

            if (OnlyGlow)
                return new Color(0, 0, 0, NPC.alpha);
            return null;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // 在图鉴中使用默认绘制
            if (NPC.IsABestiaryIconDummy)
                return true;

            Texture2D Scal = TextureAssets.Npc[NPC.type].Value;
            Texture2D ScalGlow = Request<Texture2D>($"{Texture}_Glow").Value;
            // NPC.CIMod().BossNewAI[6]为阶段判定
            if (isSecondPhase == true)
            {
                Scal = Request<Texture2D>($"{Texture}2").Value;
                ScalGlow = Request<Texture2D>($"{Texture}2_Glow").Value;
            }
            // 判定使用蓝色贴图
            if (CIBossAliveFlag.LegacySCalBGM == Lament)
            {
                Scal = Request<Texture2D>($"{Texture}_Blue").Value;
                ScalGlow = Request<Texture2D>($"{Texture}_Glow_Blue").Value;
                if (isSecondPhase == true)
                {
                    Scal = Request<Texture2D>($"{Texture}2_Blue").Value;
                    ScalGlow = Request<Texture2D>($"{Texture}2_Glow_Blue").Value;
                }
            }

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
            Color color36 = Color.White;
            float amount9 = 0.5f;
            int num153 = 7;

            if (LAPConfig.Instance.PerformanceMode)
            {
                for (int num155 = 1; num155 < num153; num155 += 2)
                {
                    Color color38 = drawColor;
                    color38 = Color.Lerp(color38, color36, amount9);
                    color38 = NPC.GetAlpha(color38);
                    color38 *= (num153 - num155) / 15f;
                    Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
                    vector41 -= new Vector2(Scal.Width, Scal.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
                    spriteBatch.Draw(Scal, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
                }
            }

            Vector2 vector43 = NPC.Center - Main.screenPosition;
            vector43 -= new Vector2(Scal.Width, Scal.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
            vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
            spriteBatch.Draw(Scal, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

            Color color37 = Color.Lerp(Color.White, CIBossAliveFlag.LegacySCalBGM != Lament ? Color.Red : Color.White, 0.5f);
            if (LAPConfig.Instance.PerformanceMode)
            {
                for (int num163 = 1; num163 < num153; num163++)
                {
                    Color color41 = color37;
                    color41 = Color.Lerp(color41, color36, amount9);
                    color41 *= (num153 - num163) / 15f;
                    Vector2 vector44 = NPC.oldPos[num163] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
                    vector44 -= new Vector2(ScalGlow.Width, Scal.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    vector44 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
                    spriteBatch.Draw(ScalGlow, vector44, NPC.frame, color41, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
                }
            }

            spriteBatch.Draw(ScalGlow, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LifeDrain, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LifeDrain, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LifeDrain, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }
        #endregion
        #region 预防死亡
        // Prevent the player from accidentally killing SCal instead of having her turn into a town NPC.
        public override bool CheckDead()
        {
            // 防止最后对话前就打死了終灾
            if (canDead == false)
            {
                NPC.life = 1;
                NPC.active = true;
                NPC.dontTakeDamage = true;
                NPC.netUpdate = true;
                return false;
            }
            else
                return true;
        }
        #endregion
        #region 击杀与掉落
        //public override void BossLoot(ref string name, ref int potionType)
        //{
        //    potionType = ItemType<OmegaHealingPotion>();
        //}

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //npcLoot.Add(ItemType<CalamitousEssence>(), 1, 30, 40);
            ////魔影梯凳掉率为0.0005%
            //npcLoot.Add(ItemType<StepToolShadows>(), 2000);

            //int[] weapons =
            //[
            //    ItemType<AngelicAlliance>(),
            //    ItemType<Animus>(),
            //    ItemType<Ozzathoth>(),
            //    ItemType<AzathothLegacy>(),
            //    ItemType<CrystylCrusher>(),
            //    ItemType<DraconicDestruction>(),
            //    ItemType<Earth>(),
            //    ItemType<RedSun>(),
            //    ItemType<IllustriousKnives>(),
            //    ItemType<RogueShadowspecKnives>(),
            //    ItemType<NanoblackReaper>(),
            //    ItemType<MeleeNanoblackReaper>(),
            //    ItemType<RainbowPartyCannon>(),
            //    ItemType<TriactisTruePaladinianMageHammerofMight>(),
            //    ItemType<RogueTriactisHammer>(),
            //    ItemType<SomaPrime>(),
            //    ItemType<SomaPrimeOld>(),
            //    ItemType<Svantechnical>(),
            //    ItemType<SvantechnicalLegacy>(),
            //    ItemType<Sylvestaff>(),
            //    ItemType<FabstaffOld>(),
            //    ItemType<StaffofBlushie>(),
            //    ItemType<Apotheosis>(),
            //    ItemType<ApotheosisLegacy>(),
            //    ItemType<TheDanceofLight>(),
            //    ItemType<DanceofLightLegacy>(),
            //    ItemType<ScarletDevil>(),
            //    ItemType<Endogenesis>(),
            //    ItemType<FlamsteedRing>(),
            //    ItemType<Eternity>(),
            //    ItemType<TemporalUmbrella>(),
            //    ItemType<TemporalUmbrellaOld>(),
            //];
            //// 随机掉一个
            //var weaponDropRule = ItemDropRule.OneFromOptions(1, weapons);
            //npcLoot.Add(weaponDropRule);
            //// 爆裂与利锥
            //npcLoot.AddIf(() => CalamityWorld.death, ItemType<VehemencOld>());
            //npcLoot.AddIf(() => CalamityWorld.death, ItemType<Levi>());

            //// 战利品纹章
            //npcLoot.Add(ItemDropRule.Common(ItemType<BrimstoneJewel>()));
            //npcLoot.Add(ItemType<ScalTrophy>(), 10);

            //// 圣物
            //npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ItemType<ScalRelic>());
            //// Lore
            //npcLoot.AddConditionalPerPlayer(() => !CIDownedBossSystem.DownedLegacyScal, ItemType<KnowledgeCalamitas>(), desc: DropHelper.FirstKillText);
        }

        public override void OnKill()
        {
            if (Main.LocalPlayer.CI().LegacyScal_PlayerKillCount > 0)
                SendBattleText(17);
            else if (Main.LocalPlayer.CI().LegacyScal_PlayerKillCount < 1)
                SendBattleText(18 + Main.LocalPlayer.CI().LegacyScal_PlayerDeathCount);
            else
                SendBattleText(16);
            SoundEngine.PlaySound(CISounds.SupremeCalamitasGiveUp, NPC.position);
            if (Main.player[NPC.target].CI().LegacyScal_PlayerKillCount < 5)
                Main.player[NPC.target].CI().LegacyScal_PlayerKillCount++;
            // NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y + 12, NPCType<CalamitasNPCLegacy>());
            if (!CIDownedBossSystem.DownedLegacyScal)
            {
                MusicEventManger.AddMusicEventEntry(MusicLoader.GetMusicSlot(Mod, CIMusicRegister.Interlude3), TimeSpan.FromSeconds(295.532d), () => CIConfig.Instance.Scal, TimeSpan.FromSeconds(5d));
            }
            CIDownedBossSystem.DownedLegacyScal = true;
            if (Main.dedServ)
                NetMessage.SendData(MessageID.WorldData);
        }
        #endregion
    }
}
