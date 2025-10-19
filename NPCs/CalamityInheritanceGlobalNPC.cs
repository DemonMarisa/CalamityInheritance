using CalamityInheritance.Buffs.Statbuffs;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace CalamityInheritance.NPCs
{
    public partial class CIGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public ShizukuMoonlight.ClassType @moonClass;
        #region Debuff
        public bool ShizukuMoon = false;
        public bool SilvaStunDebuff = false;
        // 梯凳之怒
        public bool rageOfChair = false;
        // 深渊之火
        public bool abyssalFlamesNPC = false;
        // 恐惧
        public bool horrorNPC = false;
        // 孱弱巫咒
        public bool vulnerabilityHexLegacyNPC = false;
        // yanm刀
        public bool kamiFlu = false;
        #endregion

        public static int rageOfChairDoTDamage = 30000;
        public bool CryoDrainDoT = false;
        public const int CryoDrainDotDamage = 100;
        internal object newAI;

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (SilvaStunDebuff)
            {
                npc.velocity *= 0.95f;
            }
            if (rageOfChair)
            {
                if(npc.lifeRegen > 0)
                npc.lifeRegen = 0;
                npc.lifeRegen -= rageOfChairDoTDamage;
                if (damage < rageOfChairDoTDamage)
                    damage = 114514;
            }
            if (ShizukuMoon)
            {
                ApplyDPSDebuff(15000, 15000 / 5, ref npc.lifeRegen);
                damage = 15000 / 5;
            }
            if(abyssalFlamesNPC)
            {
                // 深渊之火
                ApplyDPSDebuff(12222, 12222 / 5, ref npc.lifeRegen);
                damage = 12222 / 5;
            }
            if(vulnerabilityHexLegacyNPC)
            {
                // 深渊之火
                ApplyDPSDebuff(6666, 6666 / 5, ref npc.lifeRegen);
                damage = 6666 / 5;
            }
            //不要试图修改这个dot伤害太多，因为这个会直接用于玩家生命恢复的计算。
            if (CryoDrainDoT)
            {
                npc.lifeRegen -= CryoDrainDotDamage; 
            }
            // Kami Debuff from Yanmei's Knife
            if (kamiFlu)
            {
                int baseKamiFluDoTValue = 250;
                ApplyDPSDebuff(baseKamiFluDoTValue, baseKamiFluDoTValue / 10, ref npc.lifeRegen);
            }
            void ApplyDPSDebuff(int lifeRegenValue, int damageValue, ref int lifeRegen)
            {
                if (lifeRegen > 0)
                    lifeRegen = 0;
                lifeRegen -= lifeRegenValue;
            }
        }
        public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
        {
            if (horrorNPC)
            {
                modifiers.FinalDamage *= 0.9f;
            }
            if (vulnerabilityHexLegacyNPC)
            {
                modifiers.FinalDamage *= 0.8f;
            }
        }
        // Incoming defense to this function is already affected by the vanilla debuffs Ichor (-10) and Betsy's Curse (-40), and cannot be below zero.
        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (horrorNPC)
            {
                modifiers.Defense.Flat -= 20;
                modifiers.FinalDamage *= 1.1f;
            }
            if (vulnerabilityHexLegacyNPC)
            {
                modifiers.Defense.Flat -= 30;
                modifiers.FinalDamage *= 1.5f;
            }
            if (kamiFlu)
            {
                //Avoid touching things that you probably aren't meant to damage
                if (modifiers.SuperArmor || npc.defense > 999 || npc.Calamity().DR >= 0.95f || npc.Calamity().unbreakableDR)
                    return;
                float impact = 0.2f;
                //Bypass defense
                modifiers.DefenseEffectiveness *= 0f;
                modifiers.FinalDamage *= 1f / (1f - impact);
            }
        }
        #region Reset Effects
        public override void ResetEffects(NPC npc)
        {
            SilvaStunDebuff = false;
            CryoDrainDoT = false;
            BossResetEffects(npc);
        }
        #endregion
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            Player plr = Main.player[projectile.owner];
            var mplr = plr.CIMod();
            modifiers.DefenseEffectiveness *= 0.5f;
        }
        #region Pre AI
        public override bool PreAI(NPC npc)
        {
            BeeAI(npc);
            return true;
        }
        public static bool BeeAI(NPC npc)
        {
            if (npc.target >= 0 && npc.target < Main.maxPlayers)
            {
                Player targetPlayer = Main.player[npc.target];

                // 确认玩家实例有效且活跃
                if (targetPlayer != null && targetPlayer.active)
                {
                    // 检查玩家的ModPlayer条件
                    if (targetPlayer.CIMod().LoreQueenBee || targetPlayer.CIMod().PanelsLoreQueenBee)
                    {
                        // NPC类型检查列表
                        int[] beeNPCs =
                        [
                    NPCID.Bee, NPCID.BeeSmall, NPCID.Hornet, NPCID.HornetFatty, NPCID.HornetHoney,
                    NPCID.HornetLeafy, NPCID.HornetSpikey, NPCID.HornetStingy, NPCID.BigHornetStingy, NPCID.LittleHornetStingy,
                    NPCID.BigHornetSpikey, NPCID.LittleHornetSpikey, NPCID.BigHornetLeafy, NPCID.LittleHornetLeafy,
                    NPCID.BigHornetHoney, NPCID.LittleHornetHoney, NPCID.BigHornetFatty, NPCID.LittleHornetFatty
                        ];

                        // 检查当前NPC类型是否在列表中
                        if (beeNPCs.Contains(npc.type))
                        {
                            CIGlobalAI.LoreQueenBeeEffect(npc);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        #endregion
        #region Edit Spawn Rate
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.CIMod().LoreHive)
            {
                spawnRate = (int)(spawnRate * 1.3);
                maxSpawns = (int)(maxSpawns * 0.6f);
            }
            if (player.CIMod().LorePerforator)
            {
                spawnRate = (int)(spawnRate * 0.7);
                maxSpawns = (int)(maxSpawns * 1.8f);
            }
            if (player.CIMod().MLG)
            {
                spawnRate = (int)(spawnRate * 1.25f);
                maxSpawns = (int)(maxSpawns * 1.25f);
            }
        }
        #endregion

        public override void OnKill(NPC npc)
        {
            Player player = Main.player[Main.myPlayer];
            if (npc.type == NPCID.EaterofWorldsHead)
                CIDownedBossSystem.DownedEOW = true;
            if (npc.type == NPCID.BrainofCthulhu)
                CIDownedBossSystem.DownedBOC = true;
            if (npc.type == NPCID.Mothron && CIDownedBossSystem.DownedLegacyYharonP1)
                CIDownedBossSystem.DownedBuffedSolarEclipse = true;

        }
    }
}
