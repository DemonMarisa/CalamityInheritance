using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.NPCs.Boss.SCAL.Brother;
using CalamityInheritance.NPCs.Boss.SCAL.ArenaTile;
using CalamityInheritance.NPCs.Boss.SCAL.ScalWorm;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.Events;
using CalamityMod.Items.Mounts;
using CalamityMod.Particles;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.NPCs.Boss.SCAL.SoulSeeker;
using CalamityInheritance.NPCs.Boss.SCAL.Proj;
using CalamityInheritance.NPCs.TownNPC;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityMod.Items.Pets;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.Tools;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityInheritance.Content.Items.Placeables.Relic;
using CalamityMod.Items.Potions;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityMod.Projectiles.Magic;
using Terraria.GameContent.ItemDropRules;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Core;
using CalamityMod.Items.LoreItems;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Pets;
using log4net.Core;
using CalamityMod.NPCs;
using System.IO;
using CalamityInheritance.Content.Items.MiscItem;
using CalamityMod.Buffs.Potions;
using CalamityInheritance.Buffs.Potions;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Buffs.DamageOverTime;

namespace CalamityInheritance.NPCs.Boss.SCAL
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
        public int LifeMax = CalamityWorld.death ? 8800000 : CalamityWorld.revenge ? 8000000 : 5000000;
        // 免伤
        public float DR = CalamityWorld.death ? 0.75f : 0.7f;
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
        #region 音效
        //音效路径
        public static string CISoundPath => "CalamityInheritance/Sounds/Scal";
        public static string CalSoundPath => "CalamityMod/Sounds/Custom";
        public static string CalScalSoundPath => $"{CalSoundPath}/SCalSounds";
        //实际音效
        public static readonly SoundStyle SpawnSound                = new($"{CalSoundPath}/SupremeCalamitasSpawn") { Volume = 1.2f };
        public static readonly SoundStyle SepulcherSummonSound      = new($"{CalScalSoundPath}/SepulcherSpawn");
        public static readonly SoundStyle BrimstoneShotSound        = new($"{CalScalSoundPath}/BrimstoneShoot");
        public static readonly SoundStyle BrimstoneFireShotSound    = new($"{CalScalSoundPath}/BrimstoneFireblastImpact");

        public static readonly SoundStyle CatastropheSwing          = new($"{CalScalSoundPath}/CatastropheResonanceSlash1");
        public static readonly SoundStyle BrimstoneBigShotSound     = new($"{CalScalSoundPath}/BrimstoneBigShoot"); // DON'T YOU WANNA BE A [BIG SHOT]
        public static readonly SoundStyle DashSound                 = new($"{CalScalSoundPath}/SCalDash");
        public static readonly SoundStyle HellblastSound            = new($"{CalScalSoundPath}/BrimstoneHellblastSound");
        public static readonly SoundStyle BulletHellSound           = new($"{CalScalSoundPath}/SCalRumble");
        public static readonly SoundStyle BulletHellEndSound        = new($"{CalScalSoundPath}/SCalEndBH");
        public static readonly SoundStyle GiveUpSound               = new($"{CalScalSoundPath}/SupremeCalamitasGiveUp");
        public static readonly SoundStyle BrimstoneMonsterSpawn     = new($"{CalScalSoundPath}/BrimstoneMonsterSpawn");

        public static readonly SoundStyle ScalTra1 = new($"{CISoundPath}/SCalTra");
        public static readonly SoundStyle ScalTra2 = new($"{CISoundPath}/SCalTra2");
        #endregion
        public int dustType = (int)CalamityDusts.Brimstone;
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
        }
        #endregion
        #region SD
        public override void SetDefaults()
        {
            // 我不知道为什么修改NPCdamage就会导致boss属性翻倍，所以扔AI里面初始化了
            // 草拟吗难度增幅
            // NPC.damage = 350;
            NPC.npcSlots = 50f;

            NPC.width = NPC.height = 120;
            NPC.defense = 120;
            NPC.DR_NERD(DR);
            NPC.value = Item.buyPrice(platinum: 9999, gold: 99, silver: 99, copper: 99);
            
            NPC.lifeMax = LifeMax;
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);

            NPC.aiStyle = -1;
            AIType = -1;

            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            //梯凳驾到不免疫是理所当然
            NPC.buffImmune[ModContent.BuffType<StepToolDebuff>()] = false;
            //重新添加低温虹吸特判。因为寒冰神性已经够弱了
            NPC.buffImmune[ModContent.BuffType<CryoDrain>()] = false;
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

            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange([

				// You can add multiple elements if you really wanted to
				new FlavorTextBestiaryInfoElement($"{GenericNPC.GetNPCBestiaryLocal}.SupremeCalamitasLegacy")
            ]);
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
            writer.Write(NPC.CIMod().BossNewAI[6]);
            writer.Write(NPC.CIMod().BossNewAI[7]);
            writer.Write(NPC.CIMod().BossNewAI[8]);

            writer.Write(CIGlobalNPC.Arena.X);
            writer.Write(CIGlobalNPC.Arena.Y);
            writer.Write(CIGlobalNPC.Arena.Width);
            writer.Write(CIGlobalNPC.Arena.Height);
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
            NPC.CIMod().BossNewAI[6] = reader.ReadSingle();
            NPC.CIMod().BossNewAI[7] = reader.ReadSingle();
            NPC.CIMod().BossNewAI[8] = reader.ReadSingle();

            CIGlobalNPC.Arena.X = reader.ReadInt32();
            CIGlobalNPC.Arena.Y = reader.ReadInt32();
            CIGlobalNPC.Arena.Width = reader.ReadInt32();
            CIGlobalNPC.Arena.Height = reader.ReadInt32();
            dustType = reader.ReadInt32();
        }
        #endregion
        #region AI
        public override void AI()
        {
            if (NPC.rotation < 0f)
                NPC.rotation += MathHelper.TwoPi;
            else if  (NPC.rotation > MathHelper.TwoPi) 
                NPC.rotation -= MathHelper.TwoPi; //确保转角一直在2pi内
            // 获取目标
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest(true);

            Player target = Main.player[NPC.target];

            isWormAlive = NPC.AnyNPCs(ModContent.NPCType<SCalWormHead>());
            isSeekerAlive = NPC.AnyNPCs(ModContent.NPCType<SoulSeekerSupremeLegacy>());
            isBrotherAlive = NPC.AnyNPCs(ModContent.NPCType<SupremeCataclysmLegacy>()) || NPC.AnyNPCs(ModContent.NPCType<SupremeCatastropheLegacy>());

            if (!target.Hitbox.Intersects(CIGlobalNPC.Arena))
                Enraged = true;
            else
                Enraged = false;

            if (Main.slimeRain)
            {
                Main.StopSlimeRain(true);
                CalamityNetcode.SyncWorld();
            }

            Item targetSelectedItem = target.inventory[target.selectedItem];
            if (targetSelectedItem.CountsAsClass(ModContent.GetInstance<TrueMeleeDamageClass>()) || targetSelectedItem.CountsAsClass(ModContent.GetInstance<TrueMeleeNoSpeedDamageClass>()))
                isTrueMelee = true;
            else
                isTrueMelee = false;

            if (initialized == false)
            {
                // 多人模式是随机召唤位置，但是在第一帧传送到对应玩家身边
                NPC.Center = Main.player[NPC.target].Center + new Vector2(0f, -400f);
                Main.player[NPC.target].Calamity().GeneralScreenShakePower = 12;
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
            ref float currentPhase = ref NPC.CIMod().BossNewAI[6];
            ref float switchToDesperationPhase = ref NPC.CIMod().BossNewAI[7];
            ref float rotationSpeed = ref NPC.CIMod().BossNewAI[8];

            // 进入新的阶段
            float lifeRatio = NPC.life / (float)NPC.lifeMax;
            // 音乐
            HandleMusicVariables(lifeRatio);

            // Set the whoAmI variable.
            CIGlobalNPC.LegacySCal = NPC.whoAmI;
            //给予被针对的玩家BossZen
            target.AddBuffSafer<BossEffects>(1);
            // 激怒
            NPC.Calamity().CurrentlyEnraged = Enraged;
            
            // 生成场地
            if (!spawnArena)
            {
                SoundEngine.PlaySound(SpawnSound, NPC.position);
                logSpawnPos = new Vector2(target.Center.X , target.Center.Y);
                spawnArena = true;
                SpawnArena(ref attackType, target);
            }

            // 杀了星流飞椅
            if (target.mount?.Type == ModContent.MountType<DraedonGamerChairMount>())
                target.mount.Dismount(target);

            // 指定期间无敌
            NPC.dontTakeDamage = isSeekerAlive || isWormAlive || Enraged;

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
                dustType = (int)CalamityDusts.Brimstone;
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
                NPC.DR_NERD(0.9f);
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
                    DoBehavior_BulletHell(target,ref attackTimer, currentPhase, ref attackType);
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
            if(attackTimer < 10000)
                attackTimer++;

            // Main.NewText($"attackTimer : {attackTimer}");

            if (isWormAlive || isBrotherAlive  || isSeekerAlive)
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
                NPC.DR_NERD(10f);
            }
            else
            {
                vectorMultiplier = 1f;
                NPC.DR_NERD(DR);
            }

            if ((LegacySCalAttackType)attackType == LegacySCalAttackType.DesperationPhase)
            {
                NPC.damage = 0;
                NPC.DR_NERD(-1f);
                NPC.defense = 0;
            }
        }
        #endregion
        #region 音乐
        public void HandleMusicVariables(float lifeRatio)
        {
            CIGlobalNPC.LegacySCalGrief = -1;
            CIGlobalNPC.LegacySCalLament = -1;
            CIGlobalNPC.LegacySCalEpiphany = -1;
            CIGlobalNPC.LegacySCalAcceptance = -1;
            if (lifeRatio <= 0.01f)
            {
                CIGlobalNPC.LegacySCalAcceptance = NPC.whoAmI;
                return;
            }
            if (lifeRatio <= 0.3f)
            {
                CIGlobalNPC.LegacySCalEpiphany = NPC.whoAmI;
                return;
            }
            if (lifeRatio <= 0.5f)
            {
                CIGlobalNPC.LegacySCalLament = NPC.whoAmI;
                return;
            }
            else
                CIGlobalNPC.LegacySCalGrief = NPC.whoAmI;
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
        public void DoBehavior_BulletHell(Player target,ref float attacktimer, float currentPhaseHell, ref float attacktype)
        {
            Player player = Main.player[NPC.target];
            isContactDamage = false;
            NPC.velocity *= 0.95f;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (attacktimer == 500)
                    SoundEngine.PlaySound(BulletHellSound, NPC.position);
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
                    SoundEngine.PlaySound(BulletHellEndSound, NPC.position);
                    if(currentPhaseHell != 1f)
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
            CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, velocity, acceleration, true);

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
                        SoundEngine.PlaySound(BrimstoneFireShotSound, NPC.position);
                        canFireSplitingFireball = false;
                        Vector2 fireballDirection = new Vector2(player.Center.X - projectileOrigin.X, player.Center.Y - projectileOrigin.Y - 550f).SafeNormalize(Vector2.Zero) * baseSpeed;
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, fireballDirection, ModContent.ProjectileType<SCalBrimstoneFireblastLegacy>(), BrimstoneDarts, 0f, Main.myPlayer, 0f, 0f);
                    }
                    else
                    {
                        SoundEngine.PlaySound(BrimstoneShotSound, NPC.position);
                        canFireSplitingFireball = true;
                        // 多重弹幕散射
                        for (int i = 0; i < 8; i++)
                        {
                            Vector2 barrageDirection = (player.Center - projectileOrigin).SafeNormalize(Vector2.Zero);
                            float speedBoost = i > 3 ? -(i - 3) : i;
                            float speedFactor = 8f + speedBoost;

                            Vector2 projvelocity = barrageDirection * speedFactor + new Vector2(speedBoost);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileOrigin, projvelocity, ModContent.ProjectileType<BrimstoneBarrageLegacy>(), BrimstoneFireblast, 0f, Main.myPlayer );
                        }
                    }
                }
            }
            if(attacktimer == TotalAttackTime)
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
            NPC.CIMod().BossNewAI[8] = 0.04f;

            if (hasCharge == false)
            {
                float chargeVelocity = isWormAlive ? 26f : 30f;
                chargeVelocity += 1f * currentPhase;

                if(isSeekerAlive)
                    chargeVelocity += 5f;

                Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation + MathHelper.PiOver2) ;
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
                    NPC.CIMod().BossNewAI[8] += 0.15f;

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

            if(ChargeCount > totalCharge - 1)
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

            CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, velocity, acceleration, true);

            if (attacktimer % fireDelay == 0)
            {
                SoundEngine.PlaySound(HellblastSound, NPC.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // 使用旋转角度计算方向
                    Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation); // 基础方向根据旋转角度
                    direction = direction.SafeNormalize(Vector2.UnitX);

                    Vector2 offset = new Vector2(0, 60).RotatedBy(NPC.rotation);
                    Vector2 projectileVelocity = direction * 10f * vectorMultiplier;
                    Vector2 projectileSpawn = NPC.Center + offset;
                    projectileVelocity = projectileVelocity.RotatedBy(MathHelper.PiOver2);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, projectileVelocity, ModContent.ProjectileType<BrimstoneHellblastLegacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
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

            CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, velocity, acceleration, true);

            if (attacktimer % fireDelay == 0)
            {
                SoundEngine.PlaySound(BrimstoneBigShotSound, NPC.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // 使用旋转角度计算方向
                    Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation); // 基础方向根据旋转角度
                    direction = direction.SafeNormalize(Vector2.UnitX);

                    Vector2 projectileVelocity = direction * 5f * vectorMultiplier;
                    Vector2 projectileSpawn = NPC.Center/* + projectileVelocity * 8f*/;
                    projectileVelocity = projectileVelocity.RotatedBy(MathHelper.PiOver2);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, projectileVelocity, ModContent.ProjectileType<SCalBrimstoneGigablastLegacy>(), BrimstoneGigablast, 0f, Main.myPlayer, 0f, 2f);
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
            if(Skip == null)
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
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 4f * vectorMultiplier, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 600) //blasts from left and right
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 900) //blasts from above, left, and right
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 3f * vectorMultiplier, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), BulletHell, 0f, Main.myPlayer, 0f, 0f);
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
                    SoundEngine.PlaySound(BrimstoneFireShotSound, NPC.Center);
                    // 左右两侧
                    float distance = Main.rand.NextBool() ? -1000f : 1000f;
                    float velocity = (distance == -1000f ? 4f : -4f) * vectorMultiplier;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + distance, player.position.Y, velocity, 0f, ModContent.ProjectileType<SCalBrimstoneFireblastLegacy>(), BrimstoneFireblast, 0f, Main.myPlayer, 0f, 2f);
                }
                else
                {
                    SoundEngine.PlaySound(BrimstoneFireShotSound, NPC.Center);
                    // 上方
                    float spread = Main.rand.Next(-1000, 1000);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + spread, player.position.Y - 1000f, 0f, 5f * vectorMultiplier, ModContent.ProjectileType<SCalBrimstoneFireblastLegacy>(), BrimstoneFireblast, 0f, Main.myPlayer, 0f, 2f);
                }
            }
            if (attacktimer % BulletHell2SpawnDelay == 0)
            {
                if (attacktimer < 300) // 下方
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y + 1000f, 0f, -4f * vectorMultiplier, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 600) //左侧
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 900) //左侧与右侧
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
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
                SoundEngine.PlaySound(BrimstoneBigShotSound, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 5f * vectorMultiplier, ModContent.ProjectileType<SCalBrimstoneGigablastLegacy>(), BrimstoneGigablast, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer % SecondBulletHellblastsDelay == 0) // 发射无际裂变
            {
                SoundEngine.PlaySound(BrimstoneFireShotSound, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 10f * vectorMultiplier, ModContent.ProjectileType<SCalBrimstoneFireblastLegacy>(), BrimstoneFireblast, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer % BulletHell3SpawnDelay == 0)
            {
                if (attacktimer < 300) // 下方
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y + 1000f, 0f, -4f * vectorMultiplier, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 600) //右侧
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 900) //左侧与右侧
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
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
                SoundEngine.PlaySound(BrimstoneBigShotSound, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 5f * vectorMultiplier, ModContent.ProjectileType<SCalBrimstoneGigablastLegacy>(), BrimstoneGigablast, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer % SecondBulletHellblastsDelay == 0) // 发射深渊炙炎
            {
                SoundEngine.PlaySound(BrimstoneFireShotSound, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 10f * vectorMultiplier, ModContent.ProjectileType<SCalBrimstoneFireblastLegacy>(), BrimstoneFireblast, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer % BulletHell4SpawnDelay == 0)
            {
                if (attacktimer < 300) // 下方
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y + 1000f, 0f, -4f * vectorMultiplier, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 600) //左侧
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 890) //左侧与右侧
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
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
                SoundEngine.PlaySound(BrimstoneBigShotSound, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 5f * vectorMultiplier, ModContent.ProjectileType<SCalBrimstoneGigablastLegacy>(), BrimstoneGigablast, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer % SecondBulletHellblastsDelay == 0) // 发射深渊炙炎
            {
                SoundEngine.PlaySound(BrimstoneFireShotSound, NPC.Center);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 10f * vectorMultiplier, ModContent.ProjectileType<SCalBrimstoneFireblastLegacy>(), BrimstoneFireblast, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer % SecondBrimstoneWaveDelay == 0) //projectiles that move in wave pattern
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneWaveLegacy>(), BrimstoneDarts, 0f, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneWaveLegacy>(), BrimstoneDarts, 0f, Main.myPlayer, 0f, 0f);
            }

            if (attacktimer % BulletHell5SpawnDelay == 0)
            {
                if (attacktimer < 300) // 上方
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 4f * vectorMultiplier, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 600) //左侧与右侧
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
                else if (attacktimer < 900) //左侧与右侧与上方
                {
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1000), player.position.Y - 1000f, 0f, 4f * vectorMultiplier, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1000), -3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1000), 3.5f * vectorMultiplier, 0f, ModContent.ProjectileType<BrimstoneHellblast2Legacy>(), AbyssalSoul, 0f, Main.myPlayer, 0f, 0f);
                }
            }
        }
        #endregion
        #endregion
        #region 生成猩红圆月
        public void SpawnBloodMoon(Player player, float attacktimer)
        {
            int distanceX = -1000;
            int distanceY = 1000;
            int randomSpread = Main.rand.Next(-1000, 1000);
            int SpawnDelay = 200;
            if (attacktimer == SpawnDelay) // 上方
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + randomSpread, player.position.Y - distanceY, 0f, 1f * vectorMultiplier, ModContent.ProjectileType<BrimstoneMonsterLegacy>(), MoonDamage, 0f, Main.myPlayer, 0f, 0f);
            }
            if (attacktimer == SpawnDelay * 2) // 下方
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + randomSpread, player.position.Y - distanceX, 0f, 1f * vectorMultiplier, ModContent.ProjectileType<BrimstoneMonsterLegacy>(), MoonDamage, 0f, Main.myPlayer, 0f, 1f);
            }
            if (attacktimer == SpawnDelay * 3) // 左侧
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + distanceY, player.position.Y - randomSpread, 0f, 1f * vectorMultiplier, ModContent.ProjectileType<BrimstoneMonsterLegacy>(), MoonDamage, 0f, Main.myPlayer, 0f, 2f);
            }
            if (attacktimer == SpawnDelay * 4) // 右侧
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), player.position.X + distanceX, player.position.Y - randomSpread, 0f, 1f * vectorMultiplier, ModContent.ProjectileType<BrimstoneMonsterLegacy>(), MoonDamage, 0f, Main.myPlayer, 0f, 3f);
            }
        }
        #endregion
        #region 召唤灾坟虫
        public void DoBehavior_SummonSepulcher()
        {
            SoundEngine.PlaySound(SepulcherSummonSound, NPC.position);
            int spawnYAdd = CalamityWorld.death ? 100 : 125;
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
            float spawnY = logSpawnPos.Y - (CalamityWorld.death ? 1000 : 1150) + (CalamityWorld.death ? 200 : 250);

            int spawnXAdd = (CalamityWorld.death ? 200 : 250);

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int x = 0; x < 5; x++)
                {
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnX, (int)spawnY, ModContent.NPCType<SCalWormHeart>(), 0, 0f, 0f, 0f, 0f, 255);
                    spawnX += spawnXAdd;
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnX2, (int)spawnY, ModContent.NPCType<SCalWormHeart>(), 0, 0f, 0f, 0f, 0f, 255);
                    spawnX2 -= spawnXAdd;
                    spawnY += spawnYAdd;
                }
                NPC.SpawnOnPlayer(NPC.FindClosestPlayer(), ModContent.NPCType<SCalWormHead>());
            }
            SelectNextAttack();
            canNextPhase = true;
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
                    if (projectile.type == ModContent.ProjectileType<BrimstoneHellblast2Legacy>() ||
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
        #region 召唤兄弟
        public void DoBehavior_SummonBrother(Player target,ref float rotationSpeed,float attacktimer)
        {
            float spawnX = target.Center.X - 2000;
            float spawnX2 = target.Center.X + 2000;

            float spawnY = target.Center.Y - 1000;

            isContactDamage = false;
            NPC.velocity *= 0.95f;
            if(attacktimer == 1)
            {
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnX, (int)spawnY, ModContent.NPCType<SupremeCataclysmLegacy>());
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnX2, (int)spawnY, ModContent.NPCType<SupremeCatastropheLegacy>());
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
                    int FireEye = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.Center.X + (Math.Sin(I * 18) * 300)), (int)(NPC.Center.Y + (Math.Cos(I * 18) * 300)), ModContent.NPCType<SoulSeekerSupremeLegacy>(), NPC.whoAmI, 0, 0, 0, -1);
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
                SoundEngine.PlaySound(SpawnSound, NPC.position);
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
                float currentEased = CIFunction.EasingHelper.EaseInOutQuad(currentProgress);

                float previousProgress = (attacktimer - 1f) / phase1Duration;
                float previousEased = CIFunction.EasingHelper.EaseInOutQuad(previousProgress);

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
                    SoundEngine.PlaySound(BulletHellEndSound, NPC.position);
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
                        if (projectile.type == ModContent.ProjectileType<BrimstoneMonsterLegacy>())
                        {
                            if (projectile.timeLeft > 90)
                                projectile.timeLeft = 90;
                        }
                    }
                }
            }
            if(attacktimer == 450)
                SendBattleText(14);

            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.damage = 0;
            if(attacktimer < totaldesperationTime)
            {
                isContactDamage = false;
                NPC.velocity.X *= 0.95f;
            }
            if (attacktimer == totaldesperationTime)
            {
                SendBattleText(15);
                canDead = true;
                NPC.DR_NERD(0);
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
                if(dustType == CIDustID.DustMushroomSpray113)
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
        public void SendStartText()
        {

            if (CIDownedBossSystem.DownedLegacyScal)
            {
                if (Main.LocalPlayer.CIMod().LegacyScal_PlayerKillCount >= 4)
                    CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.KillScalMoreThan4", Color.OrangeRed);
                else if (Main.LocalPlayer.CIMod().LegacyScal_PlayerKillCount == 1)
                    CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.KillScalOnce", Color.OrangeRed);
            }
            else
            {
                if (Main.LocalPlayer.CIMod().LegacyScal_PlayerDeathCount == 50)
                    CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.Scal_PlayerDeathMoreThan50", Color.OrangeRed);
                else if (Main.LocalPlayer.CIMod().LegacyScal_PlayerDeathCount > 19)
                    CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.Scal_PlayerDeathMoreThan20", Color.OrangeRed);
                else if (Main.LocalPlayer.CIMod().LegacyScal_PlayerDeathCount > 4)
                    CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.Scal_PlayerDeathMoreThan4", Color.OrangeRed);
            }
        }
        public void SendBattleText(int ID)
        {
            CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.ScalEnterPhase" + ID, Color.OrangeRed);
        }
        #endregion
        #region 生成场地
        public void SpawnArena(ref float attackTypeChange, Player target)
        {
            float ArenaSize = CalamityWorld.death ? 129 : 159;
            // Define the arena.
            Vector2 arenaArea = new(ArenaSize, ArenaSize);
            CIGlobalNPC.Arena = Utils.CenteredRectangle(target.Center, arenaArea * 16f);
            int left = (int)(CIGlobalNPC.Arena.Center().X / 16 - arenaArea.X * 0.5f);
            int right = (int)(CIGlobalNPC.Arena.Center().X / 16 + arenaArea.X * 0.5f);
            int top = (int)(CIGlobalNPC.Arena.Center().Y / 16 - arenaArea.Y * 0.5f);
            int bottom = (int)(CIGlobalNPC.Arena.Center().Y / 16 + arenaArea.Y * 0.5f);
            int arenaTileType = ModContent.TileType<LegacyArenaTile>();

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
            cooldownSlot = ImmunityCooldownID.Bosses;

            bool cannotBeHurt = target.HasIFrames() || target.creativeGodMode;

            if (cannotBeHurt)
                return true;

            if (NPC.Center.Distance(target.Center) > NPC.width / 2)
                return false;

            GlowOrbParticle orb = new GlowOrbParticle(target.Center, new Vector2(6, 6).RotatedByRandom(360) * Main.rand.NextFloat(0.3f, 1.1f), false, 60, Main.rand.NextFloat(1.55f, 3.75f), Main.rand.NextBool() ? Color.Red : Color.Lerp(Color.Red, Color.Magenta, 0.5f), true, true);
            GeneralParticleHandler.SpawnParticle(orb);
            if (Main.rand.NextBool())
            {
                GlowOrbParticle orb2 = new GlowOrbParticle(target.Center, new Vector2(6, 6).RotatedByRandom(360) * Main.rand.NextFloat(0.3f, 1.1f), false, 60, Main.rand.NextFloat(1.55f, 3.75f), Color.Black, false, true, false);
                GeneralParticleHandler.SpawnParticle(orb2);
            }

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
            Texture2D ScalGlow = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/SupremeCalamitasLegacy_Glow").Value;
            // NPC.CIMod().BossNewAI[6]为阶段判定
            if(isSecondPhase == true)
            {
                Scal = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/SupremeCalamitasLegacy2").Value;
                ScalGlow = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/SupremeCalamitasLegacy2_Glow").Value;
            }
            // 判定使用蓝色贴图
            if(CIGlobalNPC.LegacySCalLament != -1)
            {
                Scal = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/SupremeCalamitasLegacy_Blue").Value;
                ScalGlow = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/SupremeCalamitasLegacy_Glow_Blue").Value;
                if (isSecondPhase == true)
                {
                    Scal = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/SupremeCalamitasLegacy2_Blue").Value;
                    ScalGlow = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/SupremeCalamitasLegacy2_Glow_Blue").Value;
                }
            }

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

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
                    vector41 -= new Vector2(Scal.Width, Scal.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
                    spriteBatch.Draw(Scal, vector41, NPC.frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
                }
            }

            Vector2 vector43 = NPC.Center - Main.screenPosition;
            vector43 -= new Vector2(Scal.Width, Scal.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
            vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
            spriteBatch.Draw(Scal, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

            Color color37 = Color.Lerp(Color.White, Color.Red, 0.5f);
            if (CalamityConfig.Instance.Afterimages)
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
                    int num622 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
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
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ModContent.ItemType<OmegaHealingPotion>();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            //yysy 20个都够你把所有魔影物品做完了
            npcLoot.Add(ModContent.ItemType<CalamitousEssence>(), 1, 20, 30);
            //魔影梯凳掉率为0.0005%
            npcLoot.Add(ModContent.ItemType<StepToolShadow>(), 2000);

            int[] weapons =
            [
                ModContent.ItemType<AngelicAlliance>(),
                ModContent.ItemType<Animus>(),
                ModContent.ItemType<Azathoth>(),
                ModContent.ItemType<AzathothLegacy>(),
                ModContent.ItemType<CrystylCrusher>(),
                ModContent.ItemType<DraconicDestruction>(),
                ModContent.ItemType<Earth>(),
                ModContent.ItemType<RedSun>(),
                ModContent.ItemType<IllustriousKnives>(),
                ModContent.ItemType<RogueTypeKnivesShadowspec>(),
                ModContent.ItemType<NanoblackReaper>(),
                ModContent.ItemType<MeleeTypeNanoblackReaper>(),
                ModContent.ItemType<RainbowPartyCannon>(),
                ModContent.ItemType<TriactisTruePaladinianMageHammerofMightMelee>(),
                ModContent.ItemType<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>(),
                ModContent.ItemType<SomaPrime>(),
                ModContent.ItemType<SomaPrimeOld>(),
                ModContent.ItemType<Svantechnical>(),
                ModContent.ItemType<SvantechnicalLegacy>(),
                ModContent.ItemType<Fabstaff>(),
                ModContent.ItemType<FabstaffOld>(),
                ModContent.ItemType<StaffofBlushie>(),
                ModContent.ItemType<Apotheosis>(),
                ModContent.ItemType<ApotheosisLegacy>(),
                ModContent.ItemType<TheDanceofLight>(),
                ModContent.ItemType<DanceofLightLegacy>(),
                ModContent.ItemType<ScarletDevil>(),
                ModContent.ItemType<Endogenesis>(),
                ModContent.ItemType<FlamsteedRing>(),
                ModContent.ItemType<Eternity>(),
                ModContent.ItemType<TemporalUmbrella>(),
                ModContent.ItemType<TemporalUmbrellaOld>(),
                // ModContent.ItemType<FlamsteedRing>(),
            ];
            // 随机掉一个
            var weaponDropRule = ItemDropRule.OneFromOptions(1, weapons);
            npcLoot.Add(weaponDropRule);
            //MAD模式击败下给羽毛
            npcLoot.Add(ItemDropRule.ByCondition(CIDropHelper.MADRule, ModContent.ItemType<DefiledFeather>()));
            // 爆裂与利锥
            npcLoot.AddIf(() => CalamityWorld.death, ModContent.ItemType<VehemencOld>());
            npcLoot.AddIf(() => CalamityWorld.death, ModContent.ItemType<Levi>());

            // 战利品纹章
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrimstoneJewel>()));
            npcLoot.Add(ModContent.ItemType<ScalTrophy>(), 10);

            // 圣物
            npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<ScalRelic>());
            // Lore
            npcLoot.AddConditionalPerPlayer(() => !CIDownedBossSystem.DownedLegacyScal, ModContent.ItemType<KnowledgeCalamitas>(), desc: DropHelper.FirstKillText);
        }

        public override void OnKill()
        {
            if (Main.LocalPlayer.CIMod().LegacyScal_PlayerKillCount > 0)
                SendBattleText(17);
            else if (Main.LocalPlayer.CIMod().LegacyScal_PlayerKillCount < 1)
                SendBattleText(18 + Main.LocalPlayer.CIMod().LegacyScal_PlayerDeathCount);
            else
                SendBattleText(16);

            DeathAshParticle.CreateAshesFromNPC(NPC);
            SoundEngine.PlaySound(GiveUpSound, NPC.position);
            // Increase the player's SCal kill count
            if (Main.player[NPC.target].CIMod().LegacyScal_PlayerKillCount < 5)
                Main.player[NPC.target].CIMod().LegacyScal_PlayerKillCount++;

            // Spawn the SCal NPC directly where the boss was
            if (!BossRushEvent.BossRushActive)
                NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y + 12, ModContent.NPCType<CalamitasNPCLegacy>());

            // Mark Calamitas as defeated
            CIDownedBossSystem.DownedLegacyScal = true;
            CalamityNetcode.SyncWorld();
        }
        #endregion
    }
}
