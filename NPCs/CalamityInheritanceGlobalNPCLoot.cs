using System;
using System.Data;
using System.Data.SqlTypes;
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
using CalamityInheritance.Content.Items.TreasureBags;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityInheritance.Content.Items.Weapons.Typeless;
using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityInheritance.System.Configs;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.SummonItems;
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
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            LeadingConditionRule postDoG = npcLoot.DefineConditionalDropSet(DropHelper.PostDoG());

            if (npc.type == ModContent.NPCType<IrradiatedSlime>())
                npcLoot.Add(ModContent.ItemType<LeadCore>(), 3 );

            if (npc.type == ModContent.NPCType<GammaSlime>())
                npcLoot.Add(ModContent.ItemType<LeadCore>(), 3);

            if (npc.type == ModContent.NPCType<CragmawMire>())
                npcLoot.Add(ModContent.ItemType<LeadCore>(), 2);

            if (npc.type == ModContent.NPCType<Mauler>())
                npcLoot.Add(ModContent.ItemType<LeadCore>(), 2);

            if (npc.type == ModContent.NPCType<NuclearTerror>())
                npcLoot.Add(ModContent.ItemType<LeadCore>(), 1);

            if (npc.type == ModContent.NPCType<EutrophicRay>())
                npcLoot.Add(ModContent.ItemType<EutrophicShank>(), 3);
            if (npc.type == ModContent.NPCType<IceClasper>())
                npcLoot.Add(ModContent.ItemType<AncientAncientIceChunk>(), 3);

            LeadingConditionRule postPolter = npcLoot.DefineConditionalDropSet(DropHelper.PostPolter());

            if (npc.type == ModContent.NPCType<EidolonWyrmHead>())
                postPolter.Add(ModContent.ItemType<SoulEdge>(), 1);

            if (npc.type == ModContent.NPCType<WulfrumDrone>())
            {
                npcLoot.Add(ModContent.ItemType<MageWulfrumHoodLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<MeleeWulfrumHelmLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<RangedWulfrumHeadgearLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<SummonerWulfrumHelmetLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<ThrowerWulfrumMaskLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumArmorLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumLeggingsLegacy>(), 100);
            }

            if (npc.type == ModContent.NPCType<WulfrumGyrator>())
            {
                npcLoot.Add(ModContent.ItemType<MageWulfrumHoodLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<MeleeWulfrumHelmLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<RangedWulfrumHeadgearLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<SummonerWulfrumHelmetLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<ThrowerWulfrumMaskLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumArmorLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumLeggingsLegacy>(), 100);
            }

            if (npc.type == ModContent.NPCType<WulfrumHovercraft>())
            {
                npcLoot.Add(ModContent.ItemType<MageWulfrumHoodLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<MeleeWulfrumHelmLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<RangedWulfrumHeadgearLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<SummonerWulfrumHelmetLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<ThrowerWulfrumMaskLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumArmorLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumLeggingsLegacy>(), 100);
            }

            if (npc.type == ModContent.NPCType<WulfrumAmplifier>())
            {
                npcLoot.Add(ModContent.ItemType<MageWulfrumHoodLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<MeleeWulfrumHelmLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<RangedWulfrumHeadgearLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<SummonerWulfrumHelmetLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<ThrowerWulfrumMaskLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumArmorLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumLeggingsLegacy>(), 100);
            }

            if (npc.type == ModContent.NPCType<WulfrumRover>())
            {
                npcLoot.Add(ModContent.ItemType<MageWulfrumHoodLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<MeleeWulfrumHelmLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<RangedWulfrumHeadgearLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<SummonerWulfrumHelmetLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<ThrowerWulfrumMaskLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumArmorLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumLeggingsLegacy>(), 100);
            }
            if (npc.type == ModContent.NPCType<Cnidrion>())
            {
                if (CIServerConfig.Instance.CalExtraDrop == true)
                {
                    npcLoot.Add(ModContent.ItemType<PearlShard> (), 1, 6, 12);
                }
            }
            #region 深渊掉落
            var postCalClone = npcLoot.DefineConditionalDropSet(CIDropHelper.CIPostCalClone());
            // 魔鬼鱼
            if (npc.type == ModContent.NPCType<DevilFish>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ModContent.ItemType<Lumenyl>(), 2);
            }
            // 蝰蛇鱼
            if (npc.type == ModContent.NPCType<Viperfish>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ModContent.ItemType<Lumenyl>(), 2);
            }
            // 流光石首鱼
            if (npc.type == ModContent.NPCType<LuminousCorvina>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ModContent.ItemType<Lumenyl>(), 2);
            }
            // 剧毒米诺鱼
            if (npc.type == ModContent.NPCType<ToxicMinnow>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ModContent.ItemType<Lumenyl>(), 2);
            }
            // 巨大乌贼
            if (npc.type == ModContent.NPCType<GiantSquid>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ModContent.ItemType<Lumenyl>(), 2);
            }
            // 桨鱼
            if (npc.type == ModContent.NPCType<OarfishHead>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ModContent.ItemType<Lumenyl>(), 2);
            }
            // 幻海水母
            if (npc.type == ModContent.NPCType<MirageJelly>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ModContent.ItemType<Lumenyl>(), 2);
            }
            // 肿胀翻车鱼
            if (npc.type == ModContent.NPCType<Bloatfish>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ModContent.ItemType<Lumenyl>(), 2);
            }
            // 大嘴鳗
            if (npc.type == ModContent.NPCType<GulperEelHead>())
            {
                postCalClone.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<DepthCells>(), 2, 1, 2, 2, 3));
                postCalClone.Add(ModContent.ItemType<Lumenyl>(), 2);
            }
            #endregion
            #region ModBoss

            if (npc.type == ModContent.NPCType<DesertScourgeHead>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedDesertScourge, ModContent.ItemType<KnowledgeDesertScourge>(), desc: DropHelper.FirstKillText);
                npcLoot.Add(ModContent.ItemType<AeroStoneLegacy>(),1);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<DesertScourgeBag>());
            }
            if (npc.type == ModContent.NPCType<Crabulon>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCrabulon, ModContent.ItemType<KnowledgeCrabulon>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<CrabulonBag>());
            }
            if (npc.type == ModContent.NPCType<HiveMind>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedHiveMind, ModContent.ItemType<KnowledgeHiveMind>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<HiveMindBag>());
            }
            if (npc.type == ModContent.NPCType<PerforatorHive>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedPerforator, ModContent.ItemType<KnowledgePerforators>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<PerforatorBag>());
            }
            if (npc.type == ModContent.NPCType<SlimeGodCore>())
            {
                npcLoot.Add(ModContent.ItemType<PurifiedJam>(), 1);
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedSlimeGod, ModContent.ItemType<KnowledgeSlimeGod>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<SlimeGodBag>());
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<PurifiedJam>());
            }

            if (npc.type == ModContent.NPCType<Cryogen>())
            {
                LegendaryDropHelper(ModContent.ItemType<CyrogenLegendary>(), ref npcLoot);
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCryogen, ModContent.ItemType<KnowledgeCryogen>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<CryogenBag>());
            }
            if (npc.type == ModContent.NPCType<BrimstoneElemental> ())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedBrimstoneElemental, ModContent.ItemType<KnowledgeBrimstoneElemental>(), desc: DropHelper.FirstKillText);
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedBrimstoneElemental, ModContent.ItemType<KnowledgeBrimstoneCrag>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<BrimstoneWaifuBag>());
            }
            if (npc.type == ModContent.NPCType<AquaticScourgeHead>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedAquaticScourge, ModContent.ItemType<KnowledgeAquaticScourge>(), desc: DropHelper.FirstKillText);
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedAquaticScourge, ModContent.ItemType<KnowledgeSulphurSea>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<AquaticScourgeBag>());
            }
            if (npc.type == ModContent.NPCType<CalamitasClone>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCalamitasClone, ModContent.ItemType<KnowledgeCalamitasClone>(), desc: DropHelper.FirstKillText);
                if (CIServerConfig.Instance.CalExtraDrop == true)
                {
                    npcLoot.Add(ItemID.BrokenHeroSword, 3, 2, 3);
                }
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<CalamitasCloneBag>());
            }
            if (npc.type == ModContent.NPCType<Cataclysm>() )
                npcLoot.Add(ModContent.ItemType<HavocsBreathLegacy>(), 4);
            if (npc.type == ModContent.NPCType<Catastrophe>())
                npcLoot.Add(ModContent.ItemType<BrimstoneFlameblaster>(), 4);
            if (npc.type == ModContent.NPCType<Anahita>() || npc.type == ModContent.NPCType<Leviathan>())
            {
                bool shouldDropLore(DropAttemptInfo info) => (!DownedBossSystem.downedLeviathan || !DownedBossSystem.downedCalamitasClone) && LastAnLStanding();
                npcLoot.AddConditionalPerPlayer(shouldDropLore, ModContent.ItemType<KnowledgeLeviathanAnahita>(), desc: DropHelper.FirstKillText);
                npcLoot.AddConditionalPerPlayer(shouldDropLore, ModContent.ItemType<KnowledgeOcean>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<LeviathanBag>());
            }
            if (npc.type == ModContent.NPCType<AstrumAureus>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedAstrumAureus, ModContent.ItemType<KnowledgeAstrumAureus>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<AstrumAureusBag>());
            }
            if (npc.type == ModContent.NPCType<PlaguebringerGoliath>())
            {
                LegendaryDropHelper(ModContent.ItemType<PBGLegendary>(), ref npcLoot);
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedPlaguebringer, ModContent.ItemType<KnowledgePlaguebringerGoliath>(), desc: DropHelper.FirstKillText);
                var onlyMasterDeath = ItemDropRule.ByCondition(CIDropHelper.MasterDeath, ModContent.ItemType<PBGLegendary>(), 1);
                onlyMasterDeath.OnFailedConditions(ItemDropRule.ByCondition(new Conditions.NotMasterMode(), ModContent.ItemType<PBGLegendary>(), 100), true);
                npcLoot.Add(onlyMasterDeath);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<PlaguebringerGoliathBag>());
            }
            if (npc.type == ModContent.NPCType<RavagerBody>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedRavager, ModContent.ItemType<KnowledgeRavager>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<RavagerBag>());
            }
            if (npc.type == ModContent.NPCType<AstrumDeusHead>())
            {
                bool firstDeusKill(DropAttemptInfo info) => !DownedBossSystem.downedAstrumDeus && !ShouldNotDropThings(info.npc);
                npcLoot.AddConditionalPerPlayer( firstDeusKill, ModContent.ItemType<KnowledgeAstrumDeus>(), desc: DropHelper.FirstKillText);
                npcLoot.AddConditionalPerPlayer( firstDeusKill, ModContent.ItemType<KnowledgeAstralInfection>(), desc: DropHelper.FirstKillText);
                npcLoot.Add(ModContent.ItemType<ConclaveCrossfire>(), 10);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<AstrumDeusBag>());
            }
            if (npc.type == ModContent.NPCType<ProfanedGuardianCommander> ())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedGuardians, ModContent.ItemType<KnowledgeProfanedGuardians>(), desc: DropHelper.FirstKillText);
            if (npc.type == ModContent.NPCType<Bumblefuck>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedDragonfolly, ModContent.ItemType<KnowledgeDragonfolly>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<DragonfollyBag>());
            }
            if (npc.type == ModContent.NPCType<Providence>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedProvidence, ModContent.ItemType<KnowledgeProvidence>(), desc: DropHelper.FirstKillText);
                npcLoot.Add(ModContent.ItemType<PristineFuryLegacy>(), 10);
                npcLoot.AddConditionalPerPlayer(() => Condition.InUnderworld.IsMet(), ModContent.ItemType<ElysianAegisold>());
               
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<ProvidenceBag>());
            }
            if (npc.type == ModContent.NPCType<StormWeaverHead>())
            {
                npcLoot.AddConditionalPerPlayer(() => DownedBossSystem.downedCeaselessVoid && !DownedBossSystem.downedStormWeaver && DownedBossSystem.downedSignus, ModContent.ItemType<KnowledgeSentinels>());
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<StormWeaverBag>());
            }
            if (npc.type == ModContent.NPCType<CeaselessVoid>())
            {
                npcLoot.AddConditionalPerPlayer(() => DownedBossSystem.downedCeaselessVoid && !DownedBossSystem.downedStormWeaver && DownedBossSystem.downedSignus, ModContent.ItemType<KnowledgeSentinels>());
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<CeaselessVoidBag>());
            }
            if (npc.type == ModContent.NPCType<Signus>())
            {
                npcLoot.AddConditionalPerPlayer(() => DownedBossSystem.downedCeaselessVoid && !DownedBossSystem.downedStormWeaver && DownedBossSystem.downedSignus, ModContent.ItemType<KnowledgeSentinels>());
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<SignusBag>());
            }
            if (npc.type == ModContent.NPCType<Polterghast>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedPolterghast, ModContent.ItemType<KnowledgePolterghast>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<PolterghastBag>());
            }
            if (npc.type == ModContent.NPCType<OldDuke>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedBoomerDuke, ModContent.ItemType<KnowledgeOldDuke>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<OldDukeBag>());
            }
            if (npc.type == ModContent.NPCType<DevourerofGodsHead>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedDoG, ModContent.ItemType<KnowledgeDevourerofGods>(), desc: DropHelper.FirstKillText);
                npcLoot.Add(ItemDropRule.ByCondition(DropHelper.RevAndMaster, ModContent.ItemType<AncientMurasama>(), 1));
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<DevourerofGodsBag>());
            }
            if (npc.type == ModContent.NPCType<Yharon>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedYharon, ModContent.ItemType<KnowledgeYharon>(), desc: DropHelper.FirstKillText);
                LegendaryDropHelper(ModContent.ItemType<YharimsCrystalLegendary>(), ref npcLoot);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<YharonBag>());
            }
            if (npc.type == ModContent.NPCType<AresBody>() || npc.type == ModContent.NPCType<ThanatosHead>() || npc.type == ModContent.NPCType<Apollo>())
            {
                bool shouldDropLore(DropAttemptInfo info) => !DownedBossSystem.downedExoMechs && ExoCanDropLoot();
                npcLoot.AddConditionalPerPlayer(shouldDropLore, ModContent.ItemType<KnowledgeExoMechs>(), desc: DropHelper.FirstKillText);
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<DraedonBag>());
            }
            if (npc.type == ModContent.NPCType<SupremeCalamitas>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCalamitas, ModContent.ItemType<KnowledgeCalamitas>(), desc: DropHelper.FirstKillText);
                npcLoot.Add(ModContent.ItemType<VehemencOld>(), 1);
                npcLoot.AddConditionalPerPlayer(() => DownedBossSystem.downedExoMechs, ModContent.ItemType<CalamitousEssence>());
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(),ModContent.ItemType<Armageddon>(), 1, 3000, 9999));
                CIFunction.ArmageddonBagDrop(npcLoot, ModContent.ItemType<CalamitasCoffer>());
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
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<FungalCarapace>(), 7, 4));
                    break;
                case NPCID.PossessedArmor:
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<PsychoticAmulet>(), 40, 20));
                    break;
                case NPCID.PirateCrossbower:
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<RaidersGlory>(), 25, 15));
                    break;
                case NPCID.PirateDeadeye:
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<ProporsePistol>(), 25, 15));
                    break;
                case NPCID.IchorSticker:
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<IchorSpearLegacy>(), 100, 50));
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<SpearofDestinyLegacy>(), 200, 100));
                    break;
                case NPCID.VortexRifleman:
                    //我能问个问题吗，为啥交叉集火是星璇小怪掉的？
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<ConclaveCrossfire>(), 100, 50));
                    break;
                case NPCID.SeaSnail:
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<SeaShell>(), 2, 1));
                    break;
                case NPCID.DarkCaster:
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<AncientShiv>(),25,15));
                    break;
                //礼物宝箱怪掉节日矛
                case NPCID.PresentMimic:
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<HolidayHalberd>(), 7, 5));
                    break;
                case NPCID.MartianWalker:
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<WingmanLegacy>(), 7, 5));
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<ACTWingman>(), 7, 5));
                    break;
                //蚁狮掉弓和爪子
                case NPCID.Antlion:
                case NPCID.FlyingAntlion:
                case NPCID.WalkingAntlion:
                case NPCID.GiantWalkingAntlion:
                case NPCID.GiantFlyingAntlion:
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<AntlionBow>(), 50, 33));
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<MandibleClaws>(), 50, 33));
                    break;
                case NPCID.GoblinWarrior:
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Warblade>(), 25, 20));
                    break;
                //骷髅掉战斧
                case NPCID.Skeleton:
                case NPCID.ArmoredSkeleton:
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Waraxe>(), 20, 15));
                    break;
                //符文法师掉马格努斯眼
                case NPCID.RuneWizard:
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MagnusEye>(), 10));
                    break;
                #endregion
                #region LoreItems
                // Dreadnautilus drops the Blood Moon lore
                case NPCID.BloodNautilus:
                    npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedDreadnautilus, ModContent.ItemType<KnowledgeBloodMoon>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.DD2Betsy:
                    LegendaryDropHelper(ModContent.ItemType<RavagerLegendary>(), ref npcLoot);
                    break;
                case NPCID.KingSlime:
                    // More gel is not dropped on Expert because he has more minions, which increases the amount of gel provided.
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedSlimeKing, ModContent.ItemType<KnowledgeKingSlime>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.EyeofCthulhu:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedBoss1, ModContent.ItemType<KnowledgeEyeofCthulhu>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                    // Corruption World OR Drunk World: Corruption Lore
                    LeadingConditionRule eowLoreCorruption = new(DropHelper.If((info) => info.npc.boss && (!WorldGen.crimson || WorldGen.drunkWorldGen) && !NPC.downedBoss2, desc: DropHelper.FirstKillText));
                    eowLoreCorruption.Add(ModContent.ItemType<KnowledgeCorruption>(), hideLootReport: WorldGen.crimson && !WorldGen.drunkWorldGen);
                    eowLoreCorruption.Add(ModContent.ItemType<KnowledgeEaterofWorlds>(), hideLootReport: WorldGen.crimson && !WorldGen.drunkWorldGen);
                    npcLoot.Add(eowLoreCorruption);

                    // Crimson World OR Drunk World: Crimson Lore
                    LeadingConditionRule eowLoreCrimson = new(DropHelper.If((info) => info.npc.boss && (WorldGen.crimson || WorldGen.drunkWorldGen) && !NPC.downedBoss2, desc: DropHelper.FirstKillText));
                    eowLoreCrimson.Add(ModContent.ItemType<KnowledgeCrimson>(), hideLootReport: !WorldGen.crimson && !WorldGen.drunkWorldGen);
                    eowLoreCrimson.Add(ModContent.ItemType<KnowledgeBrainofCthulhu>(), hideLootReport: !WorldGen.crimson && !WorldGen.drunkWorldGen);
                    npcLoot.Add(eowLoreCrimson);
                    break;
                case NPCID.BrainofCthulhu:
                    // Corruption World OR Drunk World: Corruption Lore
                    LeadingConditionRule bocLoreCorruption = new(DropHelper.If(() => (!WorldGen.crimson || WorldGen.drunkWorldGen) && !NPC.downedBoss2, desc: DropHelper.FirstKillText));
                    bocLoreCorruption.Add(ModContent.ItemType<KnowledgeCorruption>(), hideLootReport: WorldGen.crimson && !WorldGen.drunkWorldGen);
                    bocLoreCorruption.Add(ModContent.ItemType<KnowledgeEaterofWorlds>(), hideLootReport: WorldGen.crimson && !WorldGen.drunkWorldGen);
                    npcLoot.Add(bocLoreCorruption);

                    // Crimson World OR Drunk World: Crimson Lore
                    LeadingConditionRule bocLoreCrimson = new(DropHelper.If(() => (WorldGen.crimson || WorldGen.drunkWorldGen) && !NPC.downedBoss2, desc: DropHelper.FirstKillText));
                    bocLoreCrimson.Add(ModContent.ItemType<KnowledgeCrimson>(), hideLootReport: !WorldGen.crimson && !WorldGen.drunkWorldGen);
                    bocLoreCrimson.Add(ModContent.ItemType<KnowledgeBrainofCthulhu>(), hideLootReport: !WorldGen.crimson && !WorldGen.drunkWorldGen);
                    npcLoot.Add(bocLoreCrimson);
                    break;
                case NPCID.QueenBee:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedQueenBee, ModContent.ItemType<KnowledgeQueenBee>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.SkeletronHead:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedBoss3, ModContent.ItemType<KnowledgeSkeletron>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.WallofFlesh:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !Main.hardMode, ModContent.ItemType<KnowledgeUnderworld>(), desc: DropHelper.FirstKillText);
                    npcLoot.AddConditionalPerPlayer(() => !Main.hardMode, ModContent.ItemType<KnowledgeWallofFlesh>(), desc: DropHelper.FirstKillText);
                    npcLoot.AddConditionalPerPlayer(() => !Main.hardMode, ModContent.ItemType<MLGRune>(), desc: DropHelper.FirstKillText);
                    npcLoot.Add(ModContent.ItemType<MeowthrowerLegacy>(), 4);
                    break;
                case NPCID.TheDestroyer:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedMechBoss1, ModContent.ItemType<KnowledgeDestroyer>(), desc: DropHelper.FirstKillText);
                    npcLoot.AddConditionalPerPlayer(ShouldDropMechLore, ModContent.ItemType<KnowledgeMechs>(), desc: DropHelper.MechBossText);
                    //三个机械boss都会掉，有意为之的
                    LegendaryDropHelper(ModContent.ItemType<DestroyerLegendary>(), ref npcLoot);
                    break;
                case NPCID.Retinazer:
                case NPCID.Spazmatism:
                    // Lore
                    npcLoot.AddConditionalPerPlayer((info) => !NPC.downedMechBoss2 && IsLastTwinStanding(info), ModContent.ItemType<KnowledgeTwins>(), desc: DropHelper.FirstKillText);
                    npcLoot.AddConditionalPerPlayer(ShouldDropMechLore, ModContent.ItemType<KnowledgeMechs>(), desc: DropHelper.MechBossText);
                    break;
                case NPCID.SkeletronPrime:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedMechBoss3, ModContent.ItemType<KnowledgeSkeletronPrime>(), desc: DropHelper.FirstKillText);
                    npcLoot.AddConditionalPerPlayer(ShouldDropMechLore, ModContent.ItemType<KnowledgeMechs>(), desc: DropHelper.MechBossText);
                    npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<SpearofDestinyLegacy>()));
                    break;
                case NPCID.Plantera:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedPlantBoss, ModContent.ItemType<KnowledgePlantera>(), desc: DropHelper.FirstKillText);
                    LegendaryDropHelper(ModContent.ItemType<PlanteraLegendary>(), ref npcLoot);
                    break;
                case NPCID.Golem:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedGolemBoss, ModContent.ItemType<KnowledgeGolem>(), desc: DropHelper.FirstKillText);
                    LegendaryDropHelper(ModContent.ItemType<DefenseBlade>(), ref npcLoot);
                    break;
                case NPCID.DukeFishron:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedFishron, ModContent.ItemType<KnowledgeDukeFishron>(), desc: DropHelper.FirstKillText);
                    LegendaryDropHelper(ModContent.ItemType<DukeLegendary>(), ref npcLoot);
                    break;
                case NPCID.CultistBoss:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedAncientCultist, ModContent.ItemType<KnowledgeLunaticCultist>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.MoonLordCore:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedMoonlord, ModContent.ItemType<KnowledgeMoonLord>(), desc: DropHelper.FirstKillText);
                    break;

               #endregion

            }
            GFBDrop(npc, npcLoot);
        }
        public void GFBDrop(NPC npc, NPCLoot npcLoot)
        {
            // GFB掉落
            var GFBOnly = npcLoot.DefineConditionalDropSet(DropHelper.GFB);
            #region ModBoss
            if (npc.type == ModContent.NPCType<DesertScourgeHead>())
                GFBOnly.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<AsgardianAegisold>(), 1), hideLootReport: true);
            if (npc.type == ModContent.NPCType<AquaticScourgeHead>())
                GFBOnly.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<AncientVictideBar>(), 1, 3000, 9999), hideLootReport: true);
            if (npc.type == ModContent.NPCType<AquaticScourgeHead>())
                GFBOnly.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<AncientVictideBar>(), 1, 3000, 9999), hideLootReport: true);
            if (npc.type == ModContent.NPCType<CalamitasClone>())
                GFBOnly.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<AcceleratorT3>(), 1, 100, 999), hideLootReport: true);
            if (npc.type == ModContent.NPCType<AstrumAureus>())
            {
                var astralArmorLoot = ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<AncientAstralHelm>(), 1);
                astralArmorLoot.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientAstralBreastplate>()));
                astralArmorLoot.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientAstralLeggings>()));
                GFBOnly.Add(astralArmorLoot, hideLootReport: true);
            }
            if (npc.type == ModContent.NPCType<AstrumDeusHead>())
                GFBOnly.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<FourSeasonsGalaxiaold>(), 1, 1), hideLootReport: true);
            if (npc.type == ModContent.NPCType<Bumblefuck>())
                GFBOnly.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<YharimsGiftLegacy>(), 1), hideLootReport: true);
            if (npc.type == ModContent.NPCType<Providence>())
            {
                var tarragon = ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<AncientTarragonHelm>(), 1);
                tarragon.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientTarragonBreastplate>()));
                tarragon.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientTarragonLeggings>()));
                GFBOnly.Add(tarragon, hideLootReport: true);
            }
            if (npc.type == ModContent.NPCType<Polterghast>())
            {
                var tarragon = ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<AncientBloodflareMask>(), 1);
                tarragon.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientBloodflareBodyArmor>()));
                tarragon.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientBloodflareCuisses>()));
                GFBOnly.Add(tarragon, hideLootReport: true);
            }
            if (npc.type == ModContent.NPCType<OldDuke>())
            {
                GFBOnly.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<TriumphPotion>(), 1, 3000, 9999), hideLootReport: true);
            }
            if (npc.type == ModContent.NPCType<DevourerofGodsHead>())
            {
                var tarragon = ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<AncientGodSlayerChestplate>(), 1);
                tarragon.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientGodSlayerHelm>()));
                tarragon.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientGodSlayerLeggings>()));
                tarragon.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientSilvaHelm>()));
                tarragon.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientSilvaLeggings>()));
                tarragon.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientSilvaArmor>()));
                GFBOnly.Add(tarragon, hideLootReport: true);
            }
            if (npc.type == ModContent.NPCType<Yharon>())
            {
                var yharimArmorLoot = ItemDropRule.ByCondition(new Conditions.ZenithSeedIsNotUp(), ModContent.ItemType<YharimAuricTeslaHelm>(), 100000);
                yharimArmorLoot.OnSuccess(ItemDropRule.Common(ModContent.ItemType<YharimAuricTeslaBodyArmor>()));
                yharimArmorLoot.OnSuccess(ItemDropRule.Common(ModContent.ItemType<YharimAuricTeslaCuisses>()));
                GFBOnly.Add(yharimArmorLoot, hideLootReport: true);

                var yharimArmorLoot2 = ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<YharimAuricTeslaHelm>(), 1);
                yharimArmorLoot2.OnSuccess(ItemDropRule.Common(ModContent.ItemType<YharimAuricTeslaBodyArmor>()));
                yharimArmorLoot2.OnSuccess(ItemDropRule.Common(ModContent.ItemType<YharimAuricTeslaCuisses>()));
                GFBOnly.Add(yharimArmorLoot2, hideLootReport: true);
            }
            if (npc.type == ModContent.NPCType<AresBody>() || npc.type == ModContent.NPCType<ThanatosHead>() || npc.type == ModContent.NPCType<Apollo>())
                GFBOnly.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<Malice>(), 1, 1000, 2000), hideLootReport: true);
            if (npc.type == ModContent.NPCType<SupremeCalamitas>())
                GFBOnly.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<Armageddon>(), 1, 3000, 9999), hideLootReport: true);
            #endregion
            #region 原版boss
            switch (npc.type)
            {
                case NPCID.KingSlime:
                    GFBOnly.Add(ModContent.ItemType<AstralArcanum>(), hideLootReport: true);
                    break;
                case NPCID.EyeofCthulhu:
                    GFBOnly.Add(ModContent.ItemType<DarkSunRingold>(), hideLootReport: true);
                    break;
                case NPCID.EaterofWorldsTail:
                    GFBOnly.Add(ModContent.ItemType<CoreOfTheBloodGod>(), hideLootReport: true);
                    break;
                case NPCID.QueenBee:
                    GFBOnly.Add(ModContent.ItemType<AncientTarragonWings>(), hideLootReport: true);
                    break;
                case NPCID.WallofFlesh:
                    var allClass = ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<ElementalGauntletold>());
                    allClass.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Content.Items.Accessories.Ranged.ElementalQuiver>()));
                    allClass.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientEtherealTalisman>()));
                    allClass.OnSuccess(ItemDropRule.Common(ModContent.ItemType<NucleogenesisLegacy>()));
                    allClass.OnSuccess(ItemDropRule.Common(ModContent.ItemType<NanotechOld>()));
                    GFBOnly.Add(allClass, hideLootReport: true);
                    break;
                case NPCID.TheDestroyer:
                    GFBOnly.Add(ModContent.ItemType<AncientMiracleMatter>(), hideLootReport: true);
                    break;
                case NPCID.Spazmatism:
                    GFBOnly.Add(ModContent.ItemType<AncientMiracleMatter>(), hideLootReport: true);
                    break;
                case NPCID.SkeletronPrime:
                    GFBOnly.Add(ModContent.ItemType<AncientMiracleMatter>(), hideLootReport: true);
                    break;
                case NPCID.Plantera:
                    GFBOnly.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<VoidVortexLegacy>(), 1), hideLootReport: true);
                    break;
                case NPCID.Golem:
                    GFBOnly.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<AcceleratorT3>(), 1), hideLootReport: true);
                    break;
                case NPCID.CultistBoss:
                    GFBOnly.Add(ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<NebulaBar>(), 1, 3000, 9999), hideLootReport: true);
                    break;
                case NPCID.MoonLordCore:
                    var mlArmorLoot = ItemDropRule.ByCondition(new Conditions.ZenithSeedIsUp(), ModContent.ItemType<AncientXerocPlateMail>(), 1);
                    mlArmorLoot.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientXerocMask>()));
                    mlArmorLoot.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientXerocCuisses>()));
                    GFBOnly.Add(mlArmorLoot);
                    break;
            }
            #endregion
        }
        public void LegendaryDropHelper(int legendary, ref NPCLoot npcLoot)
        {
            var dropRule = ItemDropRule.ByCondition(CIDropHelper.MasterDeath, legendary);
            dropRule.OnFailedConditions(ItemDropRule.ByCondition(new Conditions.NotMasterMode(), legendary, 100));
            npcLoot.Add(dropRule);
        }
        #endregion

    }
}
