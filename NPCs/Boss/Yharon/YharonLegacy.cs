using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Accessories.Wings;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.Content.Items.TreasureBags;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityInheritance.NPCs.Boss.Yharon.Arena;
using CalamityInheritance.System;
using CalamityInheritance.System.Configs;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityInheritance.World;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.CalPlayer;
using CalamityMod.Events;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Vanity;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Pets;
using CalamityMod.Items.Placeables.Furniture.BossRelics;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.Potions;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.TreasureBags;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs;
using CalamityMod.Particles;
using CalamityMod.Tiles.Ores;
using CalamityMod.World;
using LAP.Content.Configs;
using LAP.Core.MusicEvent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.Yharon
{
    [AutoloadBossHead]
    public partial class YharonLegacy : ModNPC
    {
        #region 杂项初始化
        //全局传递的别名
        //1f -> 已生成, 0f -> 未生成
        //获取这个值是否==1f，如果是，set这个值为1f，否则set为0f
        
        #region 攻击枚举
        public enum YharonAttacksType
        {
            Hover,// 悬停
            Charge,// 有吼声提示的冲刺
            ChargeNoRoar,// 没有吼声提示的冲刺
            SpawnFlareTornado,// 召唤火焰龙卷风

            FlareBombs,// 发射跟踪火球
            FlareBombsCircle,// 转圈发射火球
            FlareBombsHell1,// 第一样式的弹幕炼狱 发射数个扇形的
            FlareBombsHell2,// 第二样式的弹幕炼狱 为比较规整的
            TeleportCharge,// 传送冲刺
            // 二阶段独有
            SpawnDetonatingFlame,// 召唤爆炸火焰(NPC)
            SpawnFlaresRing,// 召唤火焰环
            YharonFireballs,// 释放龙炎弹
            SpinCharge,// 冲刺后旋转发射火球
            LineFireBall,// 直线发射滞留火球
            SpawnXYharonFireBall,// 炼狱同款召唤X龙炎弹

            OpacityToZero,// 透明度渐变
            PhaseTransition,// 转阶段
            ReBornPhase,// 回血转阶段
            FlyAway// 飞走
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
        #region 一阶段
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
        #region 二阶段
        // 100 - 80
        public static YharonAttacksType[] Stage2P1AttackCycle =>
            [
            YharonAttacksType.SpawnFlaresRing,// 这是用来标记的，因为调用的时候会+1，取不到第一个，得取一遍回来才能取到
            YharonAttacksType.Charge,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.TeleportCharge,
            YharonAttacksType.YharonFireballs,
            YharonAttacksType.LineFireBall,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.TeleportCharge,
            YharonAttacksType.Charge,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.SpinCharge,
            YharonAttacksType.FlareBombs,
            YharonAttacksType.LineFireBall,
            YharonAttacksType.YharonFireballs,
            YharonAttacksType.SpinCharge,
            YharonAttacksType.SpawnFlaresRing,
            ];
        // 80 -60
        public static YharonAttacksType[] Stage2P2AttackCycle =>
            [
            YharonAttacksType.SpawnFlaresRing,// 这是用来标记的，因为调用的时候会+1，取不到第一个，得取一遍回来才能取到
            YharonAttacksType.Charge,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.SpinCharge,
            YharonAttacksType.Charge,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.TeleportCharge,
            YharonAttacksType.LineFireBall,
            YharonAttacksType.YharonFireballs,
            YharonAttacksType.SpawnFlareTornado,
            YharonAttacksType.Charge,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.SpinCharge,
            YharonAttacksType.Charge,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.YharonFireballs,
            YharonAttacksType.LineFireBall,
            ];
        // 60 - 40

        public static YharonAttacksType[] Stage2P3AttackCycle =>
            [
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.FlareBombsHell2,// 这是用来标记的，因为调用的时候会+1，取不到第一个，得取一遍回来才能取到
            YharonAttacksType.SpawnFlareTornado,
            YharonAttacksType.SpinCharge,
            YharonAttacksType.Charge,
            YharonAttacksType.LineFireBall,
            YharonAttacksType.YharonFireballs,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.FlareBombsHell1,
            YharonAttacksType.FlareBombsCircle,
            YharonAttacksType.FlareBombs,
            YharonAttacksType.Charge,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.YharonFireballs,
            YharonAttacksType.SpawnFlareTornado,
            YharonAttacksType.LineFireBall,
            YharonAttacksType.SpinCharge,
            YharonAttacksType.Charge,
            YharonAttacksType.YharonFireballs,
            YharonAttacksType.LineFireBall,
            ];
        // 40 - 20
        public static YharonAttacksType[] Stage2P4AttackCycle =>
            [
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.FlareBombsHell1,// 这是用来标记的，因为调用的时候会+1，取不到第一个，得取一遍回来才能取到
            YharonAttacksType.LineFireBall,
            YharonAttacksType.YharonFireballs,
            YharonAttacksType.SpinCharge,
            YharonAttacksType.YharonFireballs,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.FlareBombsHell1,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.SpawnXYharonFireBall,
            YharonAttacksType.SpawnFlareTornado,
            YharonAttacksType.SpawnFlaresRing,
            YharonAttacksType.FlareBombs,
            YharonAttacksType.YharonFireballs,
            YharonAttacksType.SpawnFlareTornado,
            YharonAttacksType.LineFireBall,
            YharonAttacksType.SpawnFlaresRing,
            YharonAttacksType.Charge,
            YharonAttacksType.ChargeNoRoar,
            YharonAttacksType.YharonFireballs,
            YharonAttacksType.SpinCharge,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.FlareBombsHell2,
            YharonAttacksType.SpawnFlareTornado,
            YharonAttacksType.SpawnXYharonFireBall,
            ];
        // 20 - 0
        public static YharonAttacksType[] Stage2P5AttackCycle =>
            [
            YharonAttacksType.YharonFireballs,// 这是用来标记的，因为调用的时候会+1，取不到第一个，得取一遍回来才能取到
            YharonAttacksType.SpawnFlaresRing,
            YharonAttacksType.SpawnFlareTornado,
            YharonAttacksType.SpinCharge,
            YharonAttacksType.SpawnXYharonFireBall,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.FlareBombsHell1,
            YharonAttacksType.YharonFireballs,
            YharonAttacksType.FlareBombs,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.SpawnXYharonFireBall,
            YharonAttacksType.SpawnFlareTornado,
            YharonAttacksType.SpawnFlaresRing,
            YharonAttacksType.LineFireBall,
            YharonAttacksType.YharonFireballs,
            YharonAttacksType.SpawnFlareTornado,
            YharonAttacksType.SpawnFlaresRing,
            YharonAttacksType.SpinCharge,
            YharonAttacksType.Charge,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.YharonFireballs,
            YharonAttacksType.FlareBombsHell2,
            YharonAttacksType.SpawnFlareTornado,
            YharonAttacksType.OpacityToZero,
            YharonAttacksType.SpawnXYharonFireBall,
            YharonAttacksType.LineFireBall,
            ];
        #endregion
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
        // 生命值
        public int LifeMax = CalamityWorld.death ? 4040000 : CalamityWorld.revenge ? 2525000 : 2275000;
        // 免伤
        public float DR = CalamityWorld.death ? 0.65f : 0.46f;
        public int fireballDamage = 130;
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
        // 激怒
        public bool Enraged = false;
        // 第二面
        public bool isStage2 = false;
        // 转阶段特殊绘制
        public bool stage2SPDraw = false;
        // 开始二阶段的判定
        public bool doRebornEffect = false;
        // 控制绘制的旋转
        public bool DrawRotate = false;
        // 是否可以死亡
        public bool canDie = false;
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
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }
        #endregion
        #region 怪物图鉴
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {

            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange([

				// You can add multiple elements if you really wanted to
				new FlavorTextBestiaryInfoElement($"{GenericNPC.GetNPCBestiaryLocal}.YharonLegacy")
            ]);
        }
        #endregion
        #region SD
        public override void SetDefaults()
        {
            NPC.npcSlots = 50f;
            NPC.width = 200;
            NPC.height = 200;
            NPC.defense = 150;

            NPC.value = Item.buyPrice(platinum: 100, gold: 0, silver: 0, copper: 0);
            NPC.lifeMax = LifeMax;
            double HPBoost = CalamityServerConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);


            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            AIType = -1;

            NPC.boss = true;
            NPC.DR_NERD(DR);

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
        //调用了公用的boss栏位，手动发送AI
        public override void SendExtraAI(BinaryWriter writer)
        {
            BitsByte net1 = new BitsByte();
            //一个比特=8个字节，如果有部分字节暂时用不上，这些字节是一定得用各种方法占用掉让其形成一个完整的比特的
            //不然发送的时候会有点问题
            net1[0] = initialized;
            net1[1] = canChangeDir;
            net1[2] = canLookTarget;
            net1[3] = invincible;
            net1[4] = playerP2PEffect;
            net1[5] = false;
            net1[6] = hasCharge;
            net1[7] = Enraged;
            writer.Write(net1);

            BitsByte net2 = new BitsByte();
            //一个比特=8个字节，如果有部分字节暂时用不上，这些字节是一定得用各种方法占用掉让其形成一个完整的比特的
            //不然发送的时候会有点问题
            net2[0] = isStage2;
            net2[1] = stage2SPDraw;
            net2[2] = doRebornEffect;
            net2[3] = DrawRotate;
            net2[4] = canDie;
            net2[5] = false;
            net2[6] = false;
            net2[7] = false;
            writer.Write(net2);

            writer.Write(needhealLife);
            writer.Write(NPC.localAI[1]);
            writer.Write(NPC.CIMod().BossNewAI[0]);
            writer.Write(NPC.CIMod().BossNewAI[1]);

            writer.Write(CIGlobalNPC.Arena.X);
            writer.Write(CIGlobalNPC.Arena.Y);
            writer.Write(CIGlobalNPC.Arena.Width);
            writer.Write(CIGlobalNPC.Arena.Height);

            writer.Write(NPC.lifeMax);
            writer.Write(NPC.life);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            BitsByte net1 = reader.ReadByte();
            initialized = net1[0];
            canChangeDir = net1[1];
            canLookTarget = net1[2];
            invincible = net1[3];
            playerP2PEffect = net1[4];
            _ = net1[5];
            hasCharge = net1[6];
            Enraged = net1[7];

            BitsByte net2 = reader.ReadByte();
            isStage2 = net2[0];
            stage2SPDraw = net2[1];
            doRebornEffect = net2[2];
            DrawRotate = net2[3];
            canDie = net2[4];
            _ = net2[5];
            _ = net2[6];
            _ = net2[7];

            needhealLife= reader.ReadInt32();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.CIMod().BossNewAI[0] = reader.ReadSingle();
            NPC.CIMod().BossNewAI[1] = reader.ReadSingle();

            CIGlobalNPC.Arena.X = reader.ReadInt32();
            CIGlobalNPC.Arena.Y = reader.ReadInt32();
            CIGlobalNPC.Arena.Width = reader.ReadInt32();
            CIGlobalNPC.Arena.Height = reader.ReadInt32();

            NPC.lifeMax = reader.ReadInt32();
            NPC.life = reader.ReadInt32();
        }
        public override void AI()
        {
            MiscFlagReset.YharonSkyActive = true;
            // 生命百分比
            float lifeRatio = NPC.life / (float)NPC.lifeMax;

            //确保转角一直在2pi内
            if (NPC.rotation < 0f)
                NPC.rotation += MathHelper.TwoPi;
            else if (NPC.rotation > MathHelper.TwoPi)
                NPC.rotation -= MathHelper.TwoPi;

            // 瞄准目标
            if (NPC.target < 0 || NPC.target == Main.maxPlayers || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest();

            Player target = Main.player[NPC.target];
            // 目标无限飞
            foreach (var player in Main.ActivePlayers)
                player.Calamity().infiniteFlight = true;

            CalamityPlayer.areThereAnyDamnBosses = true;

            if (!target.Hitbox.Intersects(CIGlobalNPC.Arena))
                Enraged = true;
            else
                Enraged = false;

            if (initialized == false)
            {
                initialized = true;
                // 场地生成
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int width = 9000;
                    CIGlobalNPC.Arena.X = (int)(target.Center.X - width * 0.5f);
                    CIGlobalNPC.Arena.Y = (int)(target.Center.Y - 160000f);
                    CIGlobalNPC.Arena.Width = width;
                    CIGlobalNPC.Arena.Height = 320000;

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), target.Center.X + width * 0.5f, target.Center.Y + 100f, 0f, 0f, ModContent.ProjectileType<YharonArenaProj>(), 1000, 0f, Main.myPlayer);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), target.Center.X - width * 0.5f, target.Center.Y + 100f, 0f, 0f, ModContent.ProjectileType<YharonArenaProj>(), 1000, 0f, Main.myPlayer);
                }
                NPC.netUpdate = true;
            }

            // Set the whoAmI variable.
            CIGlobalNPC.LegacyYharon = NPC.whoAmI;
            // 音乐控制
            HandleMusicVariables();

            #region ai[]可读性
            ref float attackType = ref NPC.ai[0];
            ref float attackTimer = ref NPC.ai[1];
            ref float currentPhase = ref NPC.ai[2];
            ref float circleCount = ref NPC.ai[3];
            ref float frameType = ref NPC.localAI[1];
            ref float RebornTimer = ref NPC.CIMod().BossNewAI[2];
            #endregion
            //给BossZen
            target.AddBuffSafer<BossEffects>(1);
            #region 阶段判定
            // 第一大阶段
            Stage1AI(lifeRatio,ref currentPhase, ref attackType, ref attackTimer, ref circleCount);
            // 第二大阶段
            if (CIDownedBossSystem.DownedBuffedSolarEclipse || !CIServerConfig.Instance.SolarEclipseChange)
                Stage2AI(lifeRatio, ref currentPhase, ref attackType, ref attackTimer, ref circleCount);
            #endregion

            // 目标死亡后消失
            if (!target.active || target.dead)
            {
                DoBehavior_FlyAway(attackTimer, ref frameType, false);
                return;
            }

            float rotationAcc = 0.2f;

            attackTimer++;
            #region 重置效果
            canLookTarget = true;
            invincible = false;
            playerP2PEffect = false;
            DrawRotate = false;

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
                    DoBehavior_CircleFlareBombs(target, ref attackTimer, ref frameType);
                    break;
                case YharonAttacksType.FlareBombs:
                    DoBehavior_FireFlareBombs(target, ref attackTimer, ref frameType);
                    break;
                case YharonAttacksType.PhaseTransition:
                    DoBehavior_PhaseTransition(target, ref attackTimer, ref frameType, currentPhase);
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
                    DoBehavior_FlyAway(attackTimer, ref frameType, true);
                    break;
                case YharonAttacksType.OpacityToZero:
                    DoBehavior_OpacityToZero(attackTimer, ref frameType);
                    break;
                    // 二阶段的招式
                case YharonAttacksType.YharonFireballs:
                    DoBehavior_ReleaseYharonFireBall(target, ref attackTimer, ref frameType);
                    break;
                case YharonAttacksType.SpinCharge:
                    DoBehavior_SpinCharge(target, ref attackTimer, ref frameType);
                    break;
                case YharonAttacksType.SpawnFlaresRing:
                    DoBehavior_RingFlareBombs(target, ref attackTimer, ref frameType);
                    break;
                case YharonAttacksType.LineFireBall:
                    DoBehavior_SpawnLineFireBall(target, ref attackTimer, ref frameType);
                    break;
                case YharonAttacksType.SpawnFlareTornado:
                    DoBehavior_SpawnFlareTor(target, ref attackTimer, ref frameType);
                    break;
                case YharonAttacksType.SpawnXYharonFireBall:
                    DoBehavior_SpawnYharonFireBall(target, ref attackTimer, ref frameType);
                    break;
                default:
                    NPC.velocity *= 0.95f;
                    LookAtTarget(target, rotationAcc);
                    break;
            }

            if (doRebornEffect)
                DoBehavior_ReBorn(target, ref attackTimer, ref frameType, ref attackType,ref RebornTimer);

            // 独立的rot判定
            if (canLookTarget)
                LookAtTarget(target, rotationAcc);

            // 激怒
            NPC.Calamity().CurrentlyEnraged = Enraged;

            if (Enraged)
            {
                NPC.damage = 760 * 114;
                NPC.DR_NERD(10f);
                NPC.Calamity().canBreakPlayerDefense = true;
            }
            else
            {
                NPC.damage = 760;
                NPC.DR_NERD(DR);
                NPC.Calamity().canBreakPlayerDefense = false;
            }

            if (invincible == true)
            {
                NPC.dontTakeDamage = true;
                NPC.chaseable = false;
                NPC.netUpdate = true;
            }
            else
            {
                NPC.dontTakeDamage = false;
                NPC.chaseable = true;
                NPC.netUpdate = true;
            }
        }
        #region 音乐
        public void HandleMusicVariables()
        {
            CIGlobalNPC.LegacyYharonStage2FadeIn = -1;
            CIGlobalNPC.LegacyYharonStage2 = -1;
            if (doRebornEffect)
            {
                CIGlobalNPC.LegacyYharonStage2FadeIn = NPC.whoAmI;
                return;
            }
            if (isStage2)
            {
                CIGlobalNPC.LegacyYharonStage2 = NPC.whoAmI;
                return;
            }
        }
        #endregion
        public static int ConvertTileWidthToInt(int num) => num * 16;
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
            if (currentPhase == 5)
                attackCycle = Stage2P1AttackCycle;
            if (currentPhase == 6)
                attackCycle = Stage2P2AttackCycle;
            if (currentPhase == 7)
                attackCycle = Stage2P3AttackCycle;
            if (currentPhase == 8)
                attackCycle = Stage2P4AttackCycle;
            if (currentPhase == 9)
                attackCycle = Stage2P5AttackCycle;
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
            float attackType = NPC.ai[0];

            // 朝向左侧时的特判
            SpriteEffects spriteEffects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            float drawRotation = NPC.rotation + (NPC.spriteDirection == 1 ? MathHelper.Pi : 0);

            if(DrawRotate)
            {
                spriteEffects = NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                drawRotation = NPC.rotation;
            }

            // 材质
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Texture2D glowTexture = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/Yharon/YharonLegacyGlow").Value;
            // 绘制中心
            Vector2 halfSize = new(texture.Width / 2, texture.Height / Main.npcFrameCount[NPC.type] / 2);

            Vector2 baseDrawPos = NPC.Center - screenPos - new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f + halfSize * NPC.scale + new Vector2(0f, NPC.gfxOffY);
            // 颜色逻辑重构
            Color baseColor = drawColor;
            Color effectColor = invincible ? new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0) :
                attackType is (float)YharonAttacksType.Charge or (float)YharonAttacksType.ChargeNoRoar ? Color.White : Color.Red;

            baseColor = CalamityGlobalNPC.buffColor(baseColor, 0.9f, 0.7f, 0.3f, 1f);
            float lerpStrength = invincible ? 1f : (attackType is (float)YharonAttacksType.Charge or (float)YharonAttacksType.ChargeNoRoar ? 0.5f : 0f);

            // 残影绘制
            if (!LAPConfig.Instance.PerformanceMode)
            {
                DrawAfterimages(spriteBatch, texture, baseDrawPos, halfSize, drawRotation, spriteEffects,
                    baseColor, effectColor, lerpStrength, drawColor);
            }

            // 主体绘制
            spriteBatch.Draw(texture, baseDrawPos, NPC.frame, NPC.GetAlpha(baseColor), drawRotation, halfSize, NPC.scale, spriteEffects, 0f);
            // 发光绘制
            spriteBatch.Draw(glowTexture, baseDrawPos, NPC.frame, NPC.GetAlpha(Color.White), drawRotation, halfSize, NPC.scale, spriteEffects, 0f);
            if(stage2SPDraw)
            {
                // 主体绘制
                spriteBatch.Draw(texture, baseDrawPos, NPC.frame, NPC.GetAlpha(effectColor), drawRotation, halfSize, NPC.scale, spriteEffects, 0f);
                // 发光绘制
                spriteBatch.Draw(glowTexture, baseDrawPos, NPC.frame, NPC.GetAlpha(effectColor), drawRotation, halfSize, NPC.scale, spriteEffects, 0f);
            }

            return false;
        }
        public void DrawAfterimages(SpriteBatch spriteBatch, Texture2D texture, Vector2 basePos, Vector2 halfSize, 
            float rotation, SpriteEffects effects, Color baseColor, Color effectColor, float lerpStrength, Color drawColor)
        {
            // 常规残影
            for (int i = 1; i < 10; i += 2)
            {
                Color color = Color.Lerp(baseColor, effectColor, lerpStrength) * ((10 - i) / 15f);
                Vector2 afterimagePos = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
                afterimagePos -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                afterimagePos += halfSize * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                spriteBatch.Draw(texture, afterimagePos, NPC.frame, color, rotation, halfSize, NPC.scale, effects, 0f);
            }

            // 特殊残影
            if (playerP2PEffect)
            {
                float additionalAfterimageOpacity = 0f;
                int afterimageColorDivisor = 60;
                additionalAfterimageOpacity = 1f - (float)Math.Cos(NPC.ai[1] / afterimageColorDivisor * MathHelper.TwoPi);
                additionalAfterimageOpacity /= 3f;

                for (int k = 0; k < 6; k++)
                {
                    Color additionalAfterimageColor = drawColor;
                    additionalAfterimageColor = Color.Lerp(additionalAfterimageColor, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0), 0.5f);
                    additionalAfterimageColor = NPC.GetAlpha(additionalAfterimageColor);
                    additionalAfterimageColor *= 1f - additionalAfterimageOpacity;
                    Vector2 additionalAfterimagePos = NPC.Center + (k / (float)6 * MathHelper.TwoPi + NPC.rotation).ToRotationVector2() * 60f * additionalAfterimageOpacity - Main.screenPosition;

                    spriteBatch.Draw(texture, additionalAfterimagePos, NPC.frame,
                        additionalAfterimageColor,
                        rotation, halfSize, NPC.scale, effects, 0f);
                }
            }
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
            if (canDie)
                return true;

            NPC.life = 1;
            NPC.active = true;
            NPC.dontTakeDamage = true;
            NPC.netUpdate = true;
            return false;
        }
        #endregion
        #region 死亡
        #region 日蚀前击败
        public void FirstDown()
        {
            Player player = Main.LocalPlayer;

            if (!CIDownedBossSystem.DownedLegacyYharonP1)
            {
                string key = Language.GetTextValue("Mods.CalamityInheritance.Boss.Text.YharonPreEclipse");
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.YharonPreEclipse", Color.Orange);
                CIFunction.SendTextOnPlayer(key, Color.Orange);
            }
            else
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.YharonPreEclipse2", Color.Orange);

            SoundEngine.PlaySound(CISoundID.SoundCurseFlamesAttack, NPC.position);

            // Spawn the SCal NPC directly where the boss was
            if (!BossRushEvent.BossRushActive)
                player.QuickSpawnItem(player.GetSource_GiftOrReward(), ModContent.ItemType<YharonTreasureBagsLegacy>(), 1);
            CIWorld world = ModContent.GetInstance<CIWorld>();
            if(world.Armageddon)
                player.QuickSpawnItem(player.GetSource_GiftOrReward(), ModContent.ItemType<YharonTreasureBagsLegacy>(), 5);

            CIDownedBossSystem.DownedLegacyYharonP1 = true;
            CalamityNetcode.SyncWorld();
        }
        #endregion
        #region 龙二击败
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Boss bag
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<YharonBag>()));

            // Normal drops: Everything that would otherwise be in the bag
            var normalOnly = npcLoot.DefineNormalOnlyDropSet();
            {
                // Weapons
                int[] weapons = new int[]
                {
                    ModContent.ItemType<DragonRage>(),
                    ModContent.ItemType<TheBurningSky>(),
                    ModContent.ItemType<DragonsBreath>(),
                    ModContent.ItemType<ChickenCannon>(),
                    ModContent.ItemType<PhoenixFlameBarrage>(),
                    ModContent.ItemType<YharonsKindleStaff>(), // Yharon Kindle Staff
                    ModContent.ItemType<Wrathwing>(), // Infernal Spear
                    ModContent.ItemType<TheFinalDawn>(),

                    ModContent.ItemType<DragonSword>(),
                    ModContent.ItemType<BurningSkyLegacy>(),
                    ModContent.ItemType<AncientDragonsBreath>(),
                    ModContent.ItemType<ChickenCannonLegacy>(),
                    ModContent.ItemType<DragonStaff>(),
                    ModContent.ItemType<DragonSpear>(),
                    ModContent.ItemType<YharonSonStaff>(),
                };
                normalOnly.Add(DropHelper.CalamityStyle(DropHelper.NormalWeaponDropRateFraction, weapons));
                normalOnly.Add(ModContent.ItemType<YharimsCrystalLegendary>(), 1);

                // Vanity
                normalOnly.Add(ModContent.ItemType<YharonMask>(), 7);
                normalOnly.Add(ModContent.ItemType<ForgottenDragonEgg>(), 10);
                normalOnly.Add(ModContent.ItemType<McNuggets>(), 10);
            }

            // 随机1000-2000龙魂碎片
            npcLoot.Add(DropHelper.PerPlayer(ModContent.ItemType<YharonSoulFragment>(), 1, 1000, 2000));

            // Equipment
            npcLoot.Add(DropHelper.PerPlayer(ModContent.ItemType<YharimsGift>()));
            npcLoot.Add(DropHelper.PerPlayer(ModContent.ItemType<DrewsWings>()));

            // Trophy (always directly from boss, never in bag)
            npcLoot.Add(ModContent.ItemType<YharonTrophy>(), 10);

            // Relic
            npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(ModContent.ItemType<YharonRelic>());

            // GFB Egg drop
            // He is the dragon of rebirth afterall
            var GFBOnly = npcLoot.DefineConditionalDropSet(DropHelper.GFB);
            {
                GFBOnly.Add(ModContent.ItemType<YharonEgg>(), hideLootReport: true);
            }

            // Lore
            npcLoot.AddConditionalPerPlayer(() => !CIDownedBossSystem.DownedLegacyYharonP2, ModContent.ItemType<KnowledgeYharon>(), desc: DropHelper.FirstKillText);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ModContent.ItemType<OmegaHealingPotion>();
        }
        public override void OnKill()
        {
            Player player = Main.LocalPlayer;

            SoundEngine.PlaySound(CISoundID.SoundCurseFlamesAttack, NPC.position);

            // If Yharon has not been killed yet, notify players of Auric Ore
            if (!CIDownedBossSystem.DownedLegacyYharonP2)
            {
                CalamityUtils.SpawnOre(ModContent.TileType<AuricOre>(), 2E-05, 0.75f, 0.9f, 10, 20);

                string key = "Mods.CalamityInheritance.Boss.Text.DownedYharon";
                Color messageColor = Color.Gold;
                CIFunction.BroadcastLocalizedText(key, messageColor);

                if (!CIDownedBossSystem.DownedLegacyScal)
                {
                    MusicEventManger.AddMusicEventEntry("CalamityInheritance/Music/Tyrant", TimeSpan.FromSeconds(110d), () => CIConfig.Instance.Tyrant1, TimeSpan.FromSeconds(5d));
                }
            }
            if (CIServerConfig.Instance.MarkSameBossDown)
            {
                DownedBossSystem.downedYharon = true;
            }
            CIDownedBossSystem.DownedBuffedSolarEclipse = true;
            CIDownedBossSystem.DownedLegacyYharonP1 = true;
            CIDownedBossSystem.DownedLegacyYharonP2 = true;
            CalamityNetcode.SyncWorld();

            DoFireRing(300, 99999, -1f, 0f);
            NPC.position.X = NPC.position.X + (NPC.width / 2);
            NPC.position.Y = NPC.position.Y + (NPC.height / 2);
            NPC.width = 300;
            NPC.height = 280;
            NPC.position.X = NPC.position.X - (NPC.width / 2);
            NPC.position.Y = NPC.position.Y - (NPC.height / 2);
            for (int i = 0; i < 40; i++)
            {
                int fieryDust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CopperCoin, 0f, 0f, 100, default, 2f);
                Main.dust[fieryDust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[fieryDust].scale = 0.5f;
                    Main.dust[fieryDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 70; j++)
            {
                int fieryDust2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CopperCoin, 0f, 0f, 100, default, 3f);
                Main.dust[fieryDust2].noGravity = true;
                Main.dust[fieryDust2].velocity *= 5f;
                fieryDust2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CopperCoin, 0f, 0f, 100, default, 2f);
                Main.dust[fieryDust2].velocity *= 2f;
            }

            // Turn into dust on death.
            if (NPC.life <= 0)
                DeathAshParticle.CreateAshesFromNPC(NPC, NPC.velocity);
        }
        #endregion
        #endregion
        #region Hit Effect
        public override void HitEffect(NPC.HitInfo hit)
        {
            // hit sound
            if (NPC.soundDelay == 0)
            {
                NPC.soundDelay = Main.rand.Next(16, 20);
                SoundEngine.PlaySound(HitSound, NPC.Center);
            }

            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f, 0, default, 1f);
            }
        }
        #endregion
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (hurtInfo.Damage > 0)
                target.AddBuff(ModContent.BuffType<Dragonfire>(), 180, true);
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }
    }
}
