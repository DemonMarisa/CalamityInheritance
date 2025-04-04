using CalamityInheritance.Buffs.StatDebuffs;
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
using CalamityMod.Tiles;
using CalamityMod.UI;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.SCAL
{
    public class SupremeCalamitasLegacy : ModNPC
    {
        // 生成场地
        public bool spawnArena = false;

        // 接触伤害
        public int ContactDamage = 805;
        // 弹幕炼狱的伤害
        public int BulletHell = 1150;
        // 深渊亡魂的伤害
        public int AbyssalSoul = 900;
        // 硫火飞镖的伤害
        public int BrimstoneDarts = 762;
        // 小型爆弹接触伤害
        public int BrimstoneFireblast = 862;
        // 大型爆弹接触伤害
        public int BrimstoneGigablast = 1162;
        // 生命值
        public int LifeMax = CalamityWorld.death ? 8800000 : CalamityWorld.revenge ? 8000000 : 5000000;
        // 免伤
        public float DR = CalamityWorld.death ? 0.75f : 0.7f;
        // 防御
        public int Defense = 120;
        // 攻击类型枚举
        public enum LegacySCalAttackType
        {
            fireAbyssalSoul,
            fireGigablast,
            charge,
            fireDartsWallAndSmallblast,
            BulletHell,

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
        public const float Phase1LifeRatio = 1f;

        public const float Phase2LifeRatio = 0.75f;

        public const float Phase3LifeRatio = 0.50f;

        public const float Phase4_brotherLifeRatio = 0.45f;

        public const float Phase5LifeRatio = 0.4f;

        public const float Phase6LifeRatio = 0.3f;

        public const float Phase7LifeRatio = 0.2f;

        public const float Phase8LifeRatio = 0.1f;

        public const float Phase9LifeRatio = 0.08f;

        public const float Phase10LifeRatio = 0.01f;
        #endregion
        #region 免伤
        public static float normalDR = 0.7f;
        public static float deathDR = 0.75f;
        public static float bossRushDR = 0.6f;
        public static float enragedDR = 0.99f;
        #endregion
        #region SSD
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Supreme Calamitas");
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }
        #endregion
        #region SD
        public override void SetDefaults()
        {
            NPC.damage = 350;
            NPC.npcSlots = 50f;
            NPC.width = 120;
            NPC.height = 120;
            NPC.defense = 120;
            NPC.DR_NERD(normalDR, normalDR, deathDR, bossRushDR, true);
            NPC.value = Item.buyPrice(10, 0, 0, 0);
            NPC.lifeMax = LifeMax;

            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;

            NPC.dontTakeDamage = false;
            NPC.chaseable = true;
            NPC.boss = true;
            NPC.canGhostHeal = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
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
            // 瞄准目标
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];

            if (Main.slimeRain)
            {
                Main.StopSlimeRain(true);
                CalamityNetcode.SyncWorld();
            }

            ref float attackType = ref NPC.ai[0];
            ref float attackTimer = ref NPC.ai[1];
            ref float attackDelay = ref NPC.ai[2];

            ref float frameChangeSpeed = ref NPC.localAI[1];
            ref float frameType = ref NPC.localAI[2];
            ref float currentPhase = ref NPC.CIMod().BossNewAI[6];
            ref float switchToDesperationPhase = ref NPC.CIMod().BossNewAI[7];

            // Set the whoAmI variable.
            CIGlobalNPC.LegacySCal = NPC.whoAmI;

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
            NPC.dontTakeDamage = NPC.AnyNPCs(ModContent.NPCType<SoulSeekerSupreme>()) || NPC.AnyNPCs(ModContent.NPCType<SepulcherHead>());


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
            /*
            // 开始战斗
            if (lifeRatio < Phase1LifeRatio && currentPhase == 0f)
            {
                attackTimer = 0f;
                attackType = (int)SCalAttackType.PhaseTransition;
                currentPhase++;
                NPC.netUpdate = true;
            }
            */
        }
        public void SpawnArena(ref float attackTypeChange)
        {
            spawnArena = true;
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
    }
}
