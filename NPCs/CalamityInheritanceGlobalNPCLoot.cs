﻿using System;
using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Accessories.DashAccessories;
using CalamityInheritance.Content.Items.Accessories.Magic;
using CalamityInheritance.Content.Items.Accessories.Melee;
using CalamityInheritance.Content.Items.Accessories.Ranged;
using CalamityInheritance.Content.Items.Accessories.Rogue;
using CalamityInheritance.Content.Items.Accessories.Summon;
using CalamityInheritance.Content.Items.Accessories.Wings;
using CalamityInheritance.Content.Items.Armor.AncientAstral;
using CalamityInheritance.Content.Items.Armor.AncientAuric;
using CalamityInheritance.Content.Items.Armor.AncientBloodflare;
using CalamityInheritance.Content.Items.Armor.AncientGodSlayer;
using CalamityInheritance.Content.Items.Armor.AncientSilva;
using CalamityInheritance.Content.Items.Armor.AncientTarragon;
using CalamityInheritance.Content.Items.Armor.Wulfum;
using CalamityInheritance.Content.Items.Armor.Xeroc;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Placeables.Furniture.CraftingStations;
using CalamityInheritance.Content.Items.Placeables.Vanity;
using CalamityInheritance.Content.Items.Potions;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityInheritance.Content.Items.Weapons.Typeless;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.TreasureBags;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.AcidRain;
using CalamityMod.NPCs.AquaticScourge;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.CeaselessVoid;
using CalamityMod.NPCs.Crabulon;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.Ravager;
using CalamityMod.NPCs.Signus;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.StormWeaver;
using CalamityMod.NPCs.SunkenSea;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.Yharon;
using CalamityMod.World;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using BadgeofBravery = CalamityInheritance.Content.Items.Accessories.Melee.BadgeofBravery;
using ElementalQuiver = CalamityInheritance.Content.Items.Accessories.Ranged.ElementalQuiver;
using StatisVoidSash = CalamityInheritance.Content.Items.Accessories.DashAccessories.StatisVoidSash;

namespace CalamityInheritance.NPCs
{
    public partial class CIGlobalNPC : GlobalNPC
    {
        public static bool ShouldNotDropThings(NPC npc) => npc.Calamity().newAI[0] == 0f || ((CalamityWorld.death || BossRushEvent.BossRushActive) && npc.Calamity().newAI[0] != 3f);
        public static bool LastAnLStanding()
        {
            int count = NPC.CountNPCS(ModContent.NPCType<Anahita>()) + NPC.CountNPCS(ModContent.NPCType<Leviathan>());
            return count <= 1;
        }
        public static bool ExoCanDropLoot()
        {
            int count = NPC.CountNPCS(ModContent.NPCType<ThanatosHead>()) + NPC.CountNPCS(ModContent.NPCType<AresBody>()) + NPC.CountNPCS(ModContent.NPCType<AresBody>());
            return count <= 1;
        }
        #region Modify NPC Loot Main Hook
        public override void ModifyNPCLoot(NPC npc, NPCLoot Loot)
        {
            LeadingConditionRule postDoG = Loot.DefineConditionalDropSet(DropHelper.PostDoG());
            bool dropSoul = CIServerConfig.Instance.CalBossesCanDropSoul;
            if (dropSoul)
            {
                if (npc.CheckNPCMod<BrimstoneElemental>())
                    DropSoul(ItemID.SoulofFright);
                if (npc.CheckNPCMod<Cryogen>())
                    DropSoul(ItemID.SoulofMight);
                if (npc.CheckNPCMod<AquaticScourgeHead>())
                DropSoul(ItemID.SoulofSight);
            }
            void DropSoul(int soulType, int dropRate = 1, int dropMin = 35, int dropMax = 45)
            {
                Loot.DropCommonVanilla(soulType, dropRate, dropMin, dropMax);
            }
            if (npc.CheckNPCMod<IrradiatedSlime>())
                Loot.Add(ItemMod<LeadCore>(), 3);

            if (npc.CheckNPCMod<GammaSlime>())
                Loot.Add(ItemMod<LeadCore>(), 3);

            if (npc.CheckNPCMod<CragmawMire>())
                Loot.Add(ItemMod<LeadCore>(), 2);

            if (npc.CheckNPCMod<Mauler>())
                Loot.Add(ItemMod<LeadCore>(), 2);

            if (npc.CheckNPCMod<NuclearTerror>())
                Loot.Add(ItemMod<LeadCore>(), 1);

            if (npc.CheckNPCMod<EutrophicRay>())
                Loot.Add(ItemMod<EutrophicShank>(), 3);
            if (npc.CheckNPCMod<IceClasper>())
                Loot.Add(ItemMod<AncientAncientIceChunk>(), 3);

            LeadingConditionRule postPolter = Loot.DefineConditionalDropSet(DropHelper.PostPolter());

            if (npc.CheckNPCMod<EidolonWyrmHead>())
                postPolter.Add(ItemMod<SoulEdge>(), 1);
            const int wulfrumArmorDropRate = 100;
            int[] wulfurm =
            [
                ModContent.NPCType<WulfrumDrone>(),
                ModContent.NPCType<WulfrumGyrator>(),
                ModContent.NPCType<WulfrumHovercraft>(),
                ModContent.NPCType<WulfrumAmplifier>(),
                ModContent.NPCType<WulfrumRover>()
            ];
            foreach (var wulfrumEnemy in wulfurm)
            {
                if (npc.CheckNPCID(wulfrumEnemy))
                {
                    Loot.DropCommonMod<MageWulfrumHoodLegacy>(wulfrumArmorDropRate);
                    Loot.DropCommonMod<MeleeWulfrumHelmLegacy>(wulfrumArmorDropRate);
                    Loot.DropCommonMod<RangedWulfrumHeadgearLegacy>(wulfrumArmorDropRate);
                    Loot.DropCommonMod<SummonerWulfrumHelmetLegacy>(wulfrumArmorDropRate);
                    Loot.DropCommonMod<ThrowerWulfrumMaskLegacy>(wulfrumArmorDropRate);
                    Loot.DropCommonMod<WulfrumLeggingsLegacy>(wulfrumArmorDropRate);
                    Loot.DropCommonMod<WulfrumArmorLegacy>(wulfrumArmorDropRate);
                }

            }
            // if (npc.CheckNPCMod<WulfrumDrone>())
            // {
            //     Loot.Add(ItemMod<MageWulfrumHoodLegacy>(), 100);
            //     Loot.Add(ItemMod<MeleeWulfrumHelmLegacy>(), 100);
            //     Loot.Add(ItemMod<RangedWulfrumHeadgearLegacy>(), 100);
            //     Loot.Add(ItemMod<SummonerWulfrumHelmetLegacy>(), 100);
            //     Loot.Add(ItemMod<ThrowerWulfrumMaskLegacy>(), 100);
            //     Loot.Add(ItemMod<WulfrumArmorLegacy>(), 100);
            //     Loot.Add(ItemMod<WulfrumLeggingsLegacy>(), 100);
            // }

            // if (npc.CheckNPCMod<WulfrumGyrator>())
            // {
            //     Loot.Add(ItemMod<MageWulfrumHoodLegacy>(), 100);
            //     Loot.Add(ItemMod<MeleeWulfrumHelmLegacy>(), 100);
            //     Loot.Add(ItemMod<RangedWulfrumHeadgearLegacy>(), 100);
            //     Loot.Add(ItemMod<SummonerWulfrumHelmetLegacy>(), 100);
            //     Loot.Add(ItemMod<ThrowerWulfrumMaskLegacy>(), 100);
            //     Loot.Add(ItemMod<WulfrumArmorLegacy>(), 100);
            //     Loot.Add(ItemMod<WulfrumLeggingsLegacy>(), 100);
            // 

            // if (npc.CheckNPCMod<WulfrumHovercraft>())
            // {
            //     Loot.Add(ItemMod<MageWulfrumHoodLegacy>(), 100);
            //     Loot.Add(ItemMod<MeleeWulfrumHelmLegacy>(), 100);
            //     Loot.Add(ItemMod<RangedWulfrumHeadgearLegacy>(), 100);
            //     Loot.Add(ItemMod<SummonerWulfrumHelmetLegacy>(), 100);
            //     Loot.Add(ItemMod<ThrowerWulfrumMaskLegacy>(), 100);
            //     Loot.Add(ItemMod<WulfrumArmorLegacy>(), 100);
            //     Loot.Add(ItemMod<WulfrumLeggingsLegacy>(), 100);
            // }

            // if (npc.CheckNPCMod<WulfrumAmplifier>())
            // {
            //     Loot.Add(ItemMod<MageWulfrumHoodLegacy>(), 100);
            //     Loot.Add(ItemMod<MeleeWulfrumHelmLegacy>(), 100);
            //     Loot.Add(ItemMod<RangedWulfrumHeadgearLegacy>(), 100);
            //     Loot.Add(ItemMod<SummonerWulfrumHelmetLegacy>(), 100);
            //     Loot.Add(ItemMod<ThrowerWulfrumMaskLegacy>(), 100);
            //     Loot.Add(ItemMod<WulfrumArmorLegacy>(), 100);
            //     Loot.Add(ItemMod<WulfrumLeggingsLegacy>(), 100);
            // }

            // if (npc.CheckNPCMod<WulfrumRover>())
            // {
            //     Loot.Add(ItemMod<MageWulfrumHoodLegacy>(), 100);
            //     Loot.Add(ItemMod<MeleeWulfrumHelmLegacy>(), 100);
            //     Loot.Add(ItemMod<RangedWulfrumHeadgearLegacy>(), 100);
            //     Loot.Add(ItemMod<SummonerWulfrumHelmetLegacy>(), 100);
            //     Loot.Add(ItemMod<ThrowerWulfrumMaskLegacy>(), 100);
            //     Loot.Add(ItemMod<WulfrumArmorLegacy>(), 100);
            //     Loot.Add(ItemMod<WulfrumLeggingsLegacy>(), 100);
            // }
            if (npc.CheckNPCMod<Cnidrion>())
            {
                if (CIServerConfig.Instance.CalExtraDrop == true)
                {
                    Loot.Add(ItemMod<PearlShard> (), 1, 6, 12);
                }
            }
            #region 深渊掉落
            var postCalClone = Loot.DefineConditionalDropSet(CIDropHelper.CIPostCalClone());
            // 魔鬼鱼
            if (npc.CheckNPCMod<DevilFish>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ItemMod<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ItemMod<Lumenyl>(), 2);
            }
            // 蝰蛇鱼
            if (npc.CheckNPCMod<Viperfish>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ItemMod<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ItemMod<Lumenyl>(), 2);
            }
            // 流光石首鱼
            if (npc.CheckNPCMod<LuminousCorvina>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ItemMod<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ItemMod<Lumenyl>(), 2);
            }
            // 剧毒米诺鱼
            if (npc.CheckNPCMod<ToxicMinnow>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ItemMod<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ItemMod<Lumenyl>(), 2);
            }
            // 巨大乌贼
            if (npc.CheckNPCMod<GiantSquid>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ItemMod<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ItemMod<Lumenyl>(), 2);
            }
            // 桨鱼
            if (npc.CheckNPCMod<OarfishHead>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ItemMod<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ItemMod<Lumenyl>(), 2);
            }
            // 幻海水母
            if (npc.CheckNPCMod<MirageJelly>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ItemMod<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ItemMod<Lumenyl>(), 2);
            }
            // 肿胀翻车鱼
            if (npc.CheckNPCMod<Bloatfish>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ItemMod<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ItemMod<Lumenyl>(), 2);
            }
            // 大嘴鳗
            if (npc.CheckNPCMod<GulperEelHead>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ItemMod<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ItemMod<Lumenyl>(), 2);
            }
            #endregion
            #region ModBoss

            if (npc.CheckNPCMod<DesertScourgeHead>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedDesertScourge, ItemMod<KnowledgeDesertScourge>(), desc: DropHelper.FirstKillText);
                Loot.Add(ItemMod<AeroStoneLegacy>(),1);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<DesertScourgeBag>());
            }
            if (npc.CheckNPCMod<Crabulon>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCrabulon, ItemMod<KnowledgeCrabulon>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<CrabulonBag>());
            }
            if (npc.CheckNPCMod<HiveMind>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedHiveMind, ItemMod<KnowledgeHiveMind>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<HiveMindBag>());
                Loot.DropCommonMod<ShadethrowerLegacy>();
                Loot.DropCommonMod<ShadowdropStaff>();
            }
            if (npc.CheckNPCMod<PerforatorHive>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedPerforator, ItemMod<KnowledgePerforators>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<PerforatorBag>());
                Loot.DropCommonMod<BloodClotStaff>();
            }
            if (npc.CheckNPCMod<SlimeGodCore>())
            {
                Loot.Add(ItemMod<PurifiedJam>(), 1);
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedSlimeGod, ItemMod<KnowledgeSlimeGod>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<SlimeGodBag>());
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<PurifiedJam>());
                Loot.DropCommonMod<OverloadedBlasterLegacy>();
            }

            if (npc.CheckNPCMod<Cryogen>())
            {
                LegendaryDropHelper(ItemMod<CyrogenLegendary>(), ref Loot);
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCryogen, ItemMod<KnowledgeCryogen>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<CryogenBag>());
                //新东西
                Loot.DropCommonMod<CryoBar>(3, 10, 20);
                Loot.DropCommonMod<GlacialCrusher>();
                Loot.DropCommonMod<BittercoldStaff>();
                if (CIServerConfig.Instance.CalBossesCanDropSoul)
                    Loot.DropCommonVanilla(ItemID.SoulofMight, 1, 35, 45);
            }
            if (npc.CheckNPCMod<BrimstoneElemental> ())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedBrimstoneElemental, ItemMod<KnowledgeBrimstoneElemental>(), desc: DropHelper.FirstKillText);
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedBrimstoneElemental, ItemMod<KnowledgeBrimstoneCrag>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<BrimstoneWaifuBag>());
            }
            if (npc.CheckNPCMod<AquaticScourgeHead>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedAquaticScourge, ItemMod<KnowledgeAquaticScourge>(), desc: DropHelper.FirstKillText);
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedAquaticScourge, ItemMod<KnowledgeSulphurSea>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<AquaticScourgeBag>());
            }
            if (npc.CheckNPCMod<CalamitasClone>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCalamitasClone, ItemMod<KnowledgeCalamitasClone>(), desc: DropHelper.FirstKillText);
                if (CIServerConfig.Instance.CalExtraDrop == true)
                {
                    Loot.Add(ItemID.BrokenHeroSword, 3, 2, 3);
                }
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<CalamitasCloneBag>());
            }
            if (npc.CheckNPCMod<Cataclysm>() )
                Loot.Add(ItemMod<HavocsBreathLegacy>(), 4);
            if (npc.CheckNPCMod<Catastrophe>())
                Loot.Add(ItemMod<BrimstoneFlameblaster>(), 4);
            if (npc.CheckNPCMod<Anahita>() || npc.CheckNPCMod<Leviathan>())
            {
                bool shouldDropLore(DropAttemptInfo info) => (!DownedBossSystem.downedLeviathan || !DownedBossSystem.downedCalamitasClone) && LastAnLStanding();
                Loot.AddConditionalPerPlayer(shouldDropLore, ItemMod<KnowledgeLeviathanAnahita>(), desc: DropHelper.FirstKillText);
                Loot.AddConditionalPerPlayer(shouldDropLore, ItemMod<KnowledgeOcean>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<LeviathanBag>());
                
            }
            if (npc.CheckNPCMod<AstrumAureus>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedAstrumAureus, ItemMod<KnowledgeAstrumAureus>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<AstrumAureusBag>());
                Loot.DropCommonMod<AuroraBlazerLegacy>();
            }
            if (npc.CheckNPCMod<PlaguebringerGoliath>())
            {
                LegendaryDropHelper(ItemMod<PBGLegendary>(), ref Loot);
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedPlaguebringer, ItemMod<KnowledgePlaguebringerGoliath>(), desc: DropHelper.FirstKillText);
                var onlyMasterDeath = ItemDropRule.ByCondition(CIDropHelper.MasterDeath, ItemMod<PBGLegendary>(), 1);
                onlyMasterDeath.OnFailedConditions(ItemDropRule.ByCondition(new Conditions.NotMasterMode(), ItemMod<PBGLegendary>(), 100), true);
                Loot.Add(onlyMasterDeath);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<PlaguebringerGoliathBag>());
                Loot.DropCommonMod<BlightSpewerLegacy>();
            }
            if (npc.CheckNPCMod<RavagerBody>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedRavager, ItemMod<KnowledgeRavager>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<RavagerBag>());
                Loot.DropCommonMod<BloodPactLegacy>();
                Loot.DropCommonMod<MeleeTypeCorpusAvertor>();
            }
            if (npc.CheckNPCMod<AstrumDeusHead>())
            {
                bool firstDeusKill(DropAttemptInfo info) => !DownedBossSystem.downedAstrumDeus && !ShouldNotDropThings(info.npc);
                Loot.AddConditionalPerPlayer( firstDeusKill, ItemMod<KnowledgeAstrumDeus>(), desc: DropHelper.FirstKillText);
                Loot.AddConditionalPerPlayer( firstDeusKill, ItemMod<KnowledgeAstralInfection>(), desc: DropHelper.FirstKillText);
                Loot.Add(ItemMod<ConclaveCrossfire>(), 10);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<AstrumDeusBag>());
            }
            if (npc.CheckNPCMod<ProfanedGuardianCommander> ())
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedGuardians, ItemMod<KnowledgeProfanedGuardians>(), desc: DropHelper.FirstKillText);
            if (npc.CheckNPCMod<Bumblefuck>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedDragonfolly, ItemMod<KnowledgeDragonfolly>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<DragonfollyBag>());
            }
            if (npc.CheckNPCMod<Providence>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedProvidence, ItemMod<KnowledgeProvidence>(), desc: DropHelper.FirstKillText);
                Loot.Add(ItemMod<PristineFuryLegacy>(), 10);
                Loot.AddConditionalPerPlayer(() => Condition.InUnderworld.IsMet(), ItemMod<ElysianAegisold>());
                Loot.DropCommonMod<PristineFuryLegacy>(4);
                Loot.DropCommonMod<SamuraiBadge>(10);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<ProvidenceBag>());
            }
            bool downedVoid = DownedBossSystem.downedCeaselessVoid;
            bool downedSW = DownedBossSystem.downedStormWeaver;
            bool downedSig = DownedBossSystem.downedSignus;
            bool shouldDropTheirLore = (downedVoid && downedSW && !downedSig) || (downedVoid && !downedSW && downedSig) || (!downedVoid && downedSW && downedSig);
            if (npc.CheckNPCMod<StormWeaverHead>())
            {
                Loot.AddConditionalPerPlayer(() => shouldDropTheirLore, ItemMod<KnowledgeSentinels>());
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<StormWeaverBag>());
            }
            if (npc.CheckNPCMod<CeaselessVoid>())
            {
                Loot.AddConditionalPerPlayer(() => shouldDropTheirLore, ItemMod<KnowledgeSentinels>());
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<CeaselessVoidBag>());
            }
            if (npc.CheckNPCMod<Signus>())
            {
                Loot.AddConditionalPerPlayer(() => shouldDropTheirLore, ItemMod<KnowledgeSentinels>());
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<SignusBag>());
            }
            if (npc.CheckNPCMod<Polterghast>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedPolterghast, ItemMod<KnowledgePolterghast>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<PolterghastBag>());
            }
            if (npc.CheckNPCMod<OldDuke>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedBoomerDuke, ItemMod<KnowledgeOldDuke>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<OldDukeBag>());
            }
            if (npc.CheckNPCMod<DevourerofGodsHead>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedDoG, ItemMod<KnowledgeDevourerofGods>(), desc: DropHelper.FirstKillText);
                Loot.Add(ItemDropRule.ByCondition(DropHelper.RevAndMaster, ItemMod<AncientMurasama>(), 1));
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<DevourerofGodsBag>());
                Loot.DropCommonMod<MeleeTypeEradicator>();
            }
            if (npc.CheckNPCMod<Yharon>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedYharon, ItemMod<KnowledgeYharon>(), desc: DropHelper.FirstKillText);
                LegendaryDropHelper(ItemMod<YharimsCrystalLegendary>(), ref Loot);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<YharonBag>());
                Loot.DropCommonVanilla(ItemMod<DrewsWings>());
                Loot.DropCommonMod<DragonsBreathold>();
                Loot.DropCommonMod<VoidVortexLegacy>(10);
                Loot.DropCommonMod<YharimsGiftLegacy>(1);
            }
            if (npc.CheckNPCMod<AresBody>() || npc.CheckNPCMod<ThanatosHead>() || npc.CheckNPCMod<Apollo>())
            {
                bool shouldDropLore(DropAttemptInfo info) => !DownedBossSystem.downedExoMechs && ExoCanDropLoot();
                Loot.AddConditionalPerPlayer(shouldDropLore, ItemMod<KnowledgeExoMechs>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<DraedonBag>());
            }
            if (npc.CheckNPCMod<SupremeCalamitas>())
            {
                Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCalamitas, ItemMod<KnowledgeCalamitas>(), desc: DropHelper.FirstKillText);
                Loot.Add(ItemMod<VehemencOld>(), 1);
                Loot.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(),ItemMod<Armageddon>(), 1, 3000, 9999));
                CIFunction.ArmageddonBagDrop(Loot, ItemMod<CalamitasCoffer>());
            }
            #endregion
            #region 掉落lore的条件
            // Internal function to determine whether this NPC is the second Twin killed in a fight, regardless of which Twin it is.
            bool IsLastTwinStanding(DropAttemptInfo info)
            {
                NPC npc = info.npc;
                if (npc is null)
                    return false;

                if (npc.type == NPCID.Retinazer)
                    return !NPC.AnyNPCs(NPCID.Spazmatism);
                else if (npc.type == NPCID.Spazmatism)
                    return !NPC.AnyNPCs(NPCID.Retinazer);

                return false;
            }

            // Internal function to determine whether this NPC should drop the Mechanical Bosses combined lore item
            // Drops on the first mech boss killed (so the 2nd twin, Destroyer, or Skeletron Prime)
            bool ShouldDropMechLore(DropAttemptInfo info)
            {
                NPC npc = info.npc;
                if (npc is null)
                    return false;
                bool lastTwinStanding = IsLastTwinStanding(info);
                return !NPC.downedMechBossAny && (lastTwinStanding || npc.type == NPCID.TheDestroyer || npc.type == NPCID.SkeletronPrime);
            }

            #endregion
            switch (npc.type)
            {
                #region NPC
                // AnomuraFungus
                // FungalCarapace @ 14.29% Normal, 25% Expert+
                case NPCID.AnomuraFungus:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<FungalCarapace>(), 7, 4));
                    break;
                case NPCID.PossessedArmor:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<PsychoticAmulet>(), 40, 20));
                    break;
                case NPCID.PirateCrossbower:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<RaidersGlory>(), 25, 15));
                    break;
                case NPCID.PirateDeadeye:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<ProporsePistol>(), 25, 15));
                    break;
                case NPCID.IchorSticker:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<IchorSpearLegacy>(), 100, 50));
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<SpearofDestinyLegacy>(), 200, 100));
                    break;
                case NPCID.VortexRifleman:
                    //我能问个问题吗，为啥交叉集火是星璇小怪掉的？
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<ConclaveCrossfire>(), 100, 50));
                    break;
                case NPCID.SeaSnail:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<SeaShell>(), 2, 1));
                    break;
                case NPCID.DarkCaster:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<AncientShiv>(),25,15));
                    break;
                //礼物宝箱怪掉节日矛
                case NPCID.PresentMimic:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<HolidayHalberd>(), 7, 5));
                    break;
                case NPCID.MartianWalker:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<WingmanLegacy>(), 7, 5));
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<ACTWingman>(), 7, 5));
                    break;
                //蚁狮掉弓和爪子
                case NPCID.Antlion:
                case NPCID.FlyingAntlion:
                case NPCID.WalkingAntlion:
                case NPCID.GiantWalkingAntlion:
                case NPCID.GiantFlyingAntlion:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<AntlionBow>(), 50, 33));
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<MandibleClaws>(), 50, 33));
                    break;
                case NPCID.GoblinWarrior:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<Warblade>(), 25, 20));
                    break;
                //骷髅掉战斧
                case NPCID.Skeleton:
                case NPCID.ArmoredSkeleton:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemMod<Waraxe>(), 20, 15));
                    break;
                //符文法师掉马格努斯眼
                case NPCID.RuneWizard:
                    Loot.Add(ItemDropRule.Common(ItemMod<MagnusEye>(), 10));
                    break;
                #endregion
                #region LoreItems
                // Dreadnautilus drops the Blood Moon lore
                case NPCID.BloodNautilus:
                    Loot.AddConditionalPerPlayer(() => !DownedBossSystem.downedDreadnautilus, ItemMod<KnowledgeBloodMoon>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.DD2Betsy:
                    LegendaryDropHelper(ItemMod<RavagerLegendary>(), ref Loot);
                    break;
                case NPCID.KingSlime:
                    // More gel is not dropped on Expert because he has more minions, which increases the amount of gel provided.
                    Loot.AddConditionalPerPlayer(() => !NPC.downedSlimeKing, ItemMod<KnowledgeKingSlime>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.EyeofCthulhu:
                    // Lore
                    Loot.AddConditionalPerPlayer(() => !NPC.downedBoss1, ItemMod<KnowledgeEyeofCthulhu>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                    // Corruption World OR Drunk World: Corruption Lore
                    LeadingConditionRule eowLoreCorruption = new(DropHelper.If((info) => info.npc.boss && (!WorldGen.crimson || WorldGen.drunkWorldGen) && !NPC.downedBoss2, desc: DropHelper.FirstKillText));
                    eowLoreCorruption.Add(ItemMod<KnowledgeCorruption>(), hideLootReport: WorldGen.crimson && !WorldGen.drunkWorldGen);
                    eowLoreCorruption.Add(ItemMod<KnowledgeEaterofWorlds>(), hideLootReport: WorldGen.crimson && !WorldGen.drunkWorldGen);
                    Loot.Add(eowLoreCorruption);

                    // Crimson World OR Drunk World: Crimson Lore
                    LeadingConditionRule eowLoreCrimson = new(DropHelper.If((info) => info.npc.boss && (WorldGen.crimson || WorldGen.drunkWorldGen) && !NPC.downedBoss2, desc: DropHelper.FirstKillText));
                    eowLoreCrimson.Add(ItemMod<KnowledgeCrimson>(), hideLootReport: !WorldGen.crimson && !WorldGen.drunkWorldGen);
                    eowLoreCrimson.Add(ItemMod<KnowledgeBrainofCthulhu>(), hideLootReport: !WorldGen.crimson && !WorldGen.drunkWorldGen);
                    Loot.Add(eowLoreCrimson);
                    break;
                case NPCID.BrainofCthulhu:
                    // Corruption World OR Drunk World: Corruption Lore
                    LeadingConditionRule bocLoreCorruption = new(DropHelper.If(() => (!WorldGen.crimson || WorldGen.drunkWorldGen) && !NPC.downedBoss2, desc: DropHelper.FirstKillText));
                    bocLoreCorruption.Add(ItemMod<KnowledgeCorruption>(), hideLootReport: WorldGen.crimson && !WorldGen.drunkWorldGen);
                    bocLoreCorruption.Add(ItemMod<KnowledgeEaterofWorlds>(), hideLootReport: WorldGen.crimson && !WorldGen.drunkWorldGen);
                    Loot.Add(bocLoreCorruption);

                    // Crimson World OR Drunk World: Crimson Lore
                    LeadingConditionRule bocLoreCrimson = new(DropHelper.If(() => (WorldGen.crimson || WorldGen.drunkWorldGen) && !NPC.downedBoss2, desc: DropHelper.FirstKillText));
                    bocLoreCrimson.Add(ItemMod<KnowledgeCrimson>(), hideLootReport: !WorldGen.crimson && !WorldGen.drunkWorldGen);
                    bocLoreCrimson.Add(ItemMod<KnowledgeBrainofCthulhu>(), hideLootReport: !WorldGen.crimson && !WorldGen.drunkWorldGen);
                    Loot.Add(bocLoreCrimson);
                    break;
                case NPCID.QueenBee:
                    // Lore
                    Loot.AddConditionalPerPlayer(() => !NPC.downedQueenBee, ItemMod<KnowledgeQueenBee>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.SkeletronHead:
                    // Lore
                    Loot.AddConditionalPerPlayer(() => !NPC.downedBoss3, ItemMod<KnowledgeSkeletron>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.WallofFlesh:
                    // Lore
                    Loot.AddConditionalPerPlayer(() => !Main.hardMode, ItemMod<KnowledgeUnderworld>(), desc: DropHelper.FirstKillText);
                    Loot.AddConditionalPerPlayer(() => !Main.hardMode, ItemMod<KnowledgeWallofFlesh>(), desc: DropHelper.FirstKillText);
                    Loot.AddConditionalPerPlayer(() => !Main.hardMode, ItemMod<MLGRune>(), desc: DropHelper.FirstKillText);
                    Loot.Add(ItemMod<MeowthrowerLegacy>(), 4);
                    break;
                case NPCID.TheDestroyer:
                    // Lore
                    Loot.AddConditionalPerPlayer(() => !NPC.downedMechBoss1, ItemMod<KnowledgeDestroyer>(), desc: DropHelper.FirstKillText);
                    Loot.AddConditionalPerPlayer(ShouldDropMechLore, ItemMod<KnowledgeMechs>(), desc: DropHelper.MechBossText);
                    //三个机械boss都会掉，有意为之的
                    LegendaryDropHelper(ItemMod<DestroyerLegendary>(), ref Loot);
                    break;
                case NPCID.Retinazer:
                case NPCID.Spazmatism:
                    // Lore
                    Loot.AddConditionalPerPlayer((info) => !NPC.downedMechBoss2 && IsLastTwinStanding(info), ItemMod<KnowledgeTwins>(), desc: DropHelper.FirstKillText);
                    Loot.AddConditionalPerPlayer(ShouldDropMechLore, ItemMod<KnowledgeMechs>(), desc: DropHelper.MechBossText);
                    break;
                case NPCID.SkeletronPrime:
                    // Lore
                    Loot.AddConditionalPerPlayer(() => !NPC.downedMechBoss3, ItemMod<KnowledgeSkeletronPrime>(), desc: DropHelper.FirstKillText);
                    Loot.AddConditionalPerPlayer(ShouldDropMechLore, ItemMod<KnowledgeMechs>(), desc: DropHelper.MechBossText);
                    Loot.Add(ItemDropRule.MasterModeCommonDrop(ItemMod<SpearofDestinyLegacy>()));
                    break;
                case NPCID.Plantera:
                    // Lore
                    Loot.AddConditionalPerPlayer(() => !NPC.downedPlantBoss, ItemMod<KnowledgePlantera>(), desc: DropHelper.FirstKillText);
                    LegendaryDropHelper(ItemMod<PlanteraLegendary>(), ref Loot);
                    break;
                case NPCID.Golem:
                    // Lore
                    Loot.AddConditionalPerPlayer(() => !NPC.downedGolemBoss, ItemMod<KnowledgeGolem>(), desc: DropHelper.FirstKillText);
                    LegendaryDropHelper(ItemMod<DefenseBlade>(), ref Loot);
                    break;
                case NPCID.DukeFishron:
                    // Lore
                    Loot.AddConditionalPerPlayer(() => !NPC.downedFishron, ItemMod<KnowledgeDukeFishron>(), desc: DropHelper.FirstKillText);
                    LegendaryDropHelper(ItemMod<DukeLegendary>(), ref Loot);
                    break;
                case NPCID.CultistBoss:
                    // Lore
                    Loot.AddConditionalPerPlayer(() => !NPC.downedAncientCultist, ItemMod<KnowledgeLunaticCultist>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.MoonLordCore:
                    // Lore
                    Loot.AddConditionalPerPlayer(() => !NPC.downedMoonlord, ItemMod<KnowledgeMoonLord>(), desc: DropHelper.FirstKillText);
                    break;

               #endregion

            }
            GFBDrop(npc, Loot);
        }
        public static void GFBDrop(NPC npc, NPCLoot Loot)
        {
            //我完全重做了gfb的掉落。
            #region ModBoss
            if (npc.CheckNPCMod<DesertScourgeHead>())
                Loot.QuickGFBItemMod<AsgardianAegisold>(true);

            if (npc.CheckNPCMod<Crabulon>())
                Loot.QuickGFBItemMod<CIRampartofDeities>(true);

            if (npc.CheckNPCMod<PerforatorHive>())
                Loot.QuickGFBItemMod<RogueTypeKnivesEmpyrean>(true);

            if (npc.CheckNPCMod<HiveMind>())
                Loot.QuickGFBItemMod<EclipseSpear>(true);

            if (npc.CheckNPCMod<SlimeGodCore>())
                Loot.QuickGFBItemMod<PurifiedJam>();

            if (npc.CheckNPCMod<BrimstoneElemental>())
                Loot.QuickGFBItemMod<WaifuHeart>(true);

            if (npc.CheckNPCMod<Cryogen>())
                Loot.QuickGFBItemMod<DragonsBreathold>(true);

            if (npc.CheckNPCMod<AquaticScourgeHead>())
                Loot.QuickGFBItemMod<AncientVictideBar>();

            if (npc.CheckNPCMod<CalamitasClone>())
                Loot.QuickGFBItemMod<AcceleratorT3>();

            if (npc.CheckNPCMod<AstrumAureus>())
                Loot.QuickGFBGroupMod<AncientAstralHelm>(ItemMod<AncientAstralBreastplate>(), ItemMod<AncientAstralLeggings>());

            if ((npc.CheckNPCMod<Leviathan>() || npc.CheckNPCMod<Anahita>()) && LastAnLStanding())
                Loot.QuickGFBGroupMod<ReaperToothNecklace>(ItemMod<PolarisParrotfishLegacy>());

            if (npc.CheckNPCMod<PlaguebringerGoliath>())
                Loot.QuickGFBGroupMod<PlagueHive>(ItemMod<KnowledgeExoMechs>());

            if (npc.CheckNPCMod<RavagerBody>())
                Loot.QuickGFBItemMod<PristineFuryLegacy>(true);

            if (npc.CheckNPCMod<AstrumDeusHead>())
                Loot.QuickGFBItemMod<ArkoftheCosmosold>(true);

            if (npc.CheckNPCMod<Bumblefuck>())
                Loot.QuickGFBItemMod<DragonSpear>(true);

            if (npc.CheckNPCMod<Providence>())
                Loot.QuickGFBGroupMod<AncientTarragonHelm>(ItemMod<AncientTarragonBreastplate>(), ItemMod<AncientTarragonLeggings>(), ItemMod<AncientTarragonWings>());

            if (npc.CheckNPCMod<CeaselessVoid>())
                Loot.QuickGFBItemMod<VividClarityOld>(true);

            if (npc.CheckNPCMod<Signus>())
                Loot.QuickGFBItemMod<PhantasmalRuinold>(true);

            if (npc.CheckNPCMod<StormWeaverHead>())
                Loot.QuickGFBItemMod<SarosPossessionLegacy>(true);

            if (npc.CheckNPCMod<Polterghast>())
                Loot.QuickGFBGroupMod<AncientBloodflareMask>(ItemMod<AncientBloodflareBodyArmor>(), ItemMod<AncientBloodflareCuisses>());

            if (npc.CheckNPCMod<OldDuke>())
            {
                Loot.QuickGFBItemMod<MeleeTypeNanoblackReaper>(true);
                Loot.DropCommonMod<InsidiousImpalerLegacy>();
                Loot.DropCommonMod<LeadCore>();
            }
            if (npc.CheckNPCMod<DevourerofGodsHead>())
            {
                Loot.QuickGFBGroupMod<AncientGodSlayerHelm>(ItemMod<AncientGodSlayerChestplate>(), ItemMod<AncientGodSlayerLeggings>(), ItemMod<AncientSilvaHelm>(), ItemMod<AncientSilvaArmor>(), ItemMod<AncientSilvaLeggings>(), ItemMod<FasterGodSlayerTracers>());
                Loot.DropCommonMod<Skullmasher>(10);
            }
            if (npc.CheckNPCMod<Yharon>())
                Loot.QuickGFBGroupMod<YharimAuricTeslaHelm>(ItemMod<YharimAuricTeslaBodyArmor>(), ItemMod<YharimAuricTeslaCuisses>(), ItemMod<FasterAuricTracers>());

            if (npc.CheckNPCMod<AresBody>() || npc.CheckNPCMod<ThanatosHead>() || npc.CheckNPCMod<Apollo>())
                Loot.QuickGFBItemMod<Malice>();

            if (npc.CheckNPCMod<SupremeCalamitas>())
                Loot.QuickGFBItemMod<Armageddon>();

            #endregion
            #region 原版boss
            switch (npc.type)
            {
                case NPCID.KingSlime:
                    Loot.QuickGFBGroupMod<ElementalGauntletold>(ItemMod<ElementalQuiver>(), ItemMod<AncientEtherealTalisman>(), ItemMod<NucleogenesisLegacy>(), ItemMod<EclispeMirrorLegacy>());
                    break;
                case NPCID.EyeofCthulhu:
                    Loot.QuickGFBItemMod<DarkSunRingold>(true);
                    break;
                case NPCID.EaterofWorldsTail:
                    Loot.QuickGFBGroupMod<AncientXerocMask>(ItemMod<AncientXerocPlateMail>(), ItemMod<AncientXerocCuisses>(), ItemMod<AncientXerocWings>());
                    break;
                case NPCID.BrainofCthulhu:
                    Loot.QuickGFBItemMod<CoreOfTheBloodGod>(true);
                    break;
                case NPCID.QueenBee:
                    Loot.QuickGFBItemMod<DrewsWings>(true);
                    break;
                case NPCID.SkeletronHead:
                    Loot.QuickGFBGroupMod<MeleeTypeHammerStellarContemptLegacy>(ItemMod<RogueTypeHammerStellarContempt>());
                    break;
                case NPCID.Deerclops:
                    Loot.QuickGFBItemMod<ColdheartIcicle>(true);
                    break;
                case NPCID.WallofFlesh:
                    Loot.QuickGFBGroupMod<BadgeofBravery>(ItemMod<SamuraiBadge>(), ItemMod<YharimsInsignia>(), ItemMod<DaedalusEmblem>(), ItemMod<ManaOverloader>(),ItemMod<DoubleSonYharon>(), ItemMod<NanotechOld>());
                    break;
                case NPCID.QueenSlimeBoss:
                    Loot.QuickGFBItemMod<NeptunesBountyOld>(true);
                    break;
                case NPCID.TheDestroyer:
                case NPCID.Spazmatism:
                case NPCID.SkeletronPrime:
                    Loot.QuickGFBItemMod<AncientMiracleMatter>(true);
                    break;
                case NPCID.Plantera:
                    Loot.QuickGFBItemMod<SubsumingVortexold>(true);
                    break;
                case NPCID.Golem:
                    Loot.QuickGFBItemMod<Exobladeold>(true);
                    break;
                case NPCID.DukeFishron:
                    Loot.QuickGFBItemMod<Celestusold>(true);
                    break;
                case NPCID.HallowBoss:
                    Loot.QuickGFBItemMod<StepToolShadow>(true);
                    break;
                case NPCID.CultistBoss:
                    Loot.QuickGFBItemMod<CelestialObliterator>(true);
                    break;
                case NPCID.MoonLordCore:
                    Loot.QuickGFBItemMod<CosmicImmaterializerOld>(true);
                    break;
            }
            #endregion
        }


        public static int ItemMod<T>() where T : ModItem => ModContent.ItemType<T>();
        public static int ModNPC<T>() where T : ModNPC => ModContent.NPCType<T>();
        public void LegendaryDropHelper(int legendary, ref NPCLoot Loot)
        {
            var dropRule = ItemDropRule.ByCondition(CIDropHelper.MasterDeath, legendary);
            dropRule.OnFailedConditions(ItemDropRule.ByCondition(new Conditions.NotMasterMode(), legendary, 100));
            Loot.Add(dropRule);
        }
        #endregion

    }
    public static class ExtendedDropMethods
    {
        public static void QuickGFBItemID(this NPCLoot loot, int itemID, bool justDropOne = false, int chance = 1, int minCount = 1145, int maxCount = 1145) => QuickGFB(loot, itemID, justDropOne, chance, minCount, maxCount);
        public static void QuickGFBItemMod<T>(this NPCLoot loot, bool justDropOne = false, int chance = 1, int minCount = 1145, int maxCount = 1145) where T : ModItem => QuickGFB(loot, ModContent.ItemType<T>(), justDropOne, chance, minCount, maxCount);
        public static void QuickGFB(this NPCLoot loot, int itemID, bool justDropOne = false, int chance = 1, int minCount = 1145, int maxCount = 1145)
        {
            var GFB = loot.DefineConditionalDropSet(DropHelper.GFB);
            if (justDropOne)
                minCount = maxCount = 1;
            GFB.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), itemID, chance, minCount, maxCount), hideLootReport: true);
        }
        public static void QuickGFBGroupMod<T>(this NPCLoot loot, params int[] itemIDs) where T : ModItem => QuickGFBGroup(loot, ModContent.ItemType<T>(), itemIDs);
        public static void QuickGFBGroup(this NPCLoot loot, int dropItemIDFirst, params int[] itemIDs)
        {
            var GFB = loot.DefineConditionalDropSet(DropHelper.GFB);
            var dropGroup = ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), dropItemIDFirst, 1, 1, 1);
            foreach (var otherItem in itemIDs)
                dropGroup.OnSuccess(ItemDropRule.Common(otherItem));
            GFB.Add(dropGroup, true);
        }
    }
}

