﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;
using CalamityMod.NPCs.Cryogen;
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityMod;
using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using Terraria.WorldBuilding;
using CalamityInheritance.System.DownedBoss;
using System.Collections.Generic;
using System.Linq;
using CalamityInheritance.CIPlayer;

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
        #region Debuff
        public bool SilvaStunDebuff = false;
        // 梯凳之怒
        public bool rageOfChair = false;
        // 深渊之火
        public bool abyssalFlamesNPC = false;
        // 恐惧
        public bool horrorNPC = false;
        // 孱弱巫咒
        public bool vulnerabilityHexLegacyNPC = false;
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
            if(abyssalFlamesNPC)
            {
                // 深渊之火
                ApplyDPSDebuff(12222, 12222 / 5, ref npc.lifeRegen);
            }
            if(vulnerabilityHexLegacyNPC)
            {
                // 深渊之火
                ApplyDPSDebuff(6666, 6666 / 5, ref npc.lifeRegen);
            }
            //不要试图修改这个dot伤害太多，因为这个会直接用于玩家生命恢复的计算。
            if (CryoDrainDoT)
            {
                npc.lifeRegen -= CryoDrainDotDamage; 
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
        }
        public void ApplyDPSDebuff(int lifeRegenValue, int damageValue, ref int lifeRegen)
        {
            if (lifeRegen > 0)
                lifeRegen = 0;

            lifeRegen -= lifeRegenValue;
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
        }
        #region Pre AI
        public override bool PreAI(NPC npc)
        {
            if (Main.player[npc.target].CIMod().LoreQueenBee || Main.player[npc.target].CIMod().PanelsLoreQueenBee)
            {
                if (npc.type == NPCID.Bee || npc.type == NPCID.BeeSmall || npc.type == NPCID.Hornet || npc.type == NPCID.HornetFatty || npc.type == NPCID.HornetHoney ||
                    npc.type == NPCID.HornetLeafy || npc.type == NPCID.HornetSpikey || npc.type == NPCID.HornetStingy || npc.type == NPCID.BigHornetStingy || npc.type == NPCID.LittleHornetStingy ||
                    npc.type == NPCID.BigHornetSpikey || npc.type == NPCID.LittleHornetSpikey || npc.type == NPCID.BigHornetLeafy || npc.type == NPCID.LittleHornetLeafy ||
                    npc.type == NPCID.BigHornetHoney || npc.type == NPCID.LittleHornetHoney || npc.type == NPCID.BigHornetFatty || npc.type == NPCID.LittleHornetFatty)
                {
                    CIGlobalAI.LoreQueenBeeEffect(npc);
                    return false;
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
