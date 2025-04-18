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
                //敌方有涂油的话梯凳驾到的dot伤害+20000
                if(npc.HasBuff(BuffID.Oiled))
                npc.lifeRegen -= rageOfChairDoTDamage+20000;
                npc.lifeRegen -= rageOfChairDoTDamage;
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
        #region 等级的伤害池子
        public int meleeDamage = 0;
        public int rangedDamage = 0;
        public int magicDamage = 0;
        public int summonDamage = 0;
        public int rogueDamage = 0;
        #endregion
        public override void OnHitNPC(NPC npc, NPC target, NPC.HitInfo modifiers)
        {
            bool isMelee = modifiers.DamageType.CountsAsClass<MeleeDamageClass>() || modifiers.DamageType.CountsAsClass<TrueMeleeDamageClass>();
            // 射手
            bool isRanged = modifiers.DamageType.CountsAsClass<RangedDamageClass>();
            // 法师
            bool isMagic = modifiers.DamageType.CountsAsClass<MagicDamageClass>();
            // 召唤
            bool isWhip = modifiers.DamageType.CountsAsClass<SummonMeleeSpeedDamageClass>();
            bool isSummon = modifiers.DamageType.CountsAsClass<SummonDamageClass>() || isWhip;
            // 盗贼
            bool isRogue = modifiers.DamageType.CountsAsClass<RogueDamageClass>();

            if (isMelee)
                meleeDamage += modifiers.Damage;
            if (isRanged)
                rangedDamage += modifiers.Damage;
            if (isMagic)
                magicDamage += modifiers.Damage;
            if (isSummon)
                summonDamage += modifiers.Damage;
            if (isRogue)
                rogueDamage += modifiers.Damage;
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
            //拥有失温虹吸的敌怪将会被汲取大部分的数值
            if (projectile.type == ModContent.ProjectileType<CryogenPtr>() && mplr.ColdDivityTier3 && npc.HasBuff(ModContent.BuffType<CryoDrain>()))
            {
                //伤害、dr和防御 
                npc.damage /= 2;
                npc.Calamity().DR /= 2;
                npc.defense /= 2;
                //敌对单位的……血量
                npc.life *= (int)0.99f;
                int buffDef = npc.defense;
                float buffDR = npc.Calamity().DR;
                if (plr.HasBuff(ModContent.BuffType<CryoDrain>()) && plr.ActiveItem().type == ModContent.ItemType<CyrogenLegendary>())
                {
                    plr.statDefense += buffDef;
                    plr.endurance += buffDR;
                    //固定只给1.1f乘算
                    plr.GetDamage<SummonDamageClass>() *= 1.1f; 
                }
                //这些效果只有手持冰寒神性且玩家本身具备失温虹吸效果时才会提供
            }
        }
        #region Pre AI
        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.Bee || npc.type == NPCID.BeeSmall || npc.type == NPCID.Hornet || npc.type == NPCID.HornetFatty || npc.type == NPCID.HornetHoney ||
                npc.type == NPCID.HornetLeafy || npc.type == NPCID.HornetSpikey || npc.type == NPCID.HornetStingy || npc.type == NPCID.BigHornetStingy || npc.type == NPCID.LittleHornetStingy ||
                npc.type == NPCID.BigHornetSpikey || npc.type == NPCID.LittleHornetSpikey || npc.type == NPCID.BigHornetLeafy || npc.type == NPCID.LittleHornetLeafy ||
                npc.type == NPCID.BigHornetHoney || npc.type == NPCID.LittleHornetHoney || npc.type == NPCID.BigHornetFatty || npc.type == NPCID.LittleHornetFatty)
            {
                if (Main.player[npc.target].CIMod().LoreQueenBee)
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
        }
        #endregion

        public override void OnKill(NPC npc)
        {
            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer cIPlayer = player.CIMod();
            if (npc.type == NPCID.EaterofWorldsHead)
                CIDownedBossSystem.DownedEOW = true;
            if (npc.type == NPCID.BrainofCthulhu)
                CIDownedBossSystem.DownedBOC = true;

            int[] damageType = 
            { 
                meleeDamage,
                rangedDamage,
                magicDamage,
                summonDamage,
                rogueDamage
            };
            int maxValue = 0;
            int maxIndex = -1;
            // 寻找最高伤害值
            for (int i = 0; i < damageType.Length; i++)
            {
                if (damageType[i] > maxValue)
                {
                    maxValue = damageType[i];
                    maxIndex = i;
                }
            }
            if (maxIndex != -1)
            {
                if (maxIndex == 0)
                    cIPlayer.meleePool += (int)(maxValue * 0.1f);
                if (maxIndex == 1)
                    cIPlayer.rangePool += (int)(maxValue * 0.1f);
                if (maxIndex == 2)
                    cIPlayer.magicPool += (int)(maxValue * 0.1f);
                if (maxIndex == 3)
                    cIPlayer.summonPool += (int)(maxValue * 0.1f);
                if (maxIndex == 4)
                    cIPlayer.roguePool += (int)(maxValue * 0.1f);
            }

            // 重置伤害统计
            meleeDamage = 0;
            rangedDamage = 0;
            magicDamage = 0;
            summonDamage = 0;
            rogueDamage = 0;
        }
    }
}
