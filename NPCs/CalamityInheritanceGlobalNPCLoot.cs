using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Accessories.DashAccessories;
using CalamityInheritance.Content.Items.Accessories.Ranged;
using CalamityInheritance.Content.Items.Armor.AncientAuric;
using CalamityInheritance.Content.Items.Armor.Wulfum;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Placeables.Vanity;
using CalamityInheritance.Content.Items.Potions;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.Materials;
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

            LeadingConditionRule postPolter = npcLoot.DefineConditionalDropSet(DropHelper.PostPolter());
            if (npc.type == ModContent.NPCType<EidolonWyrmHead>())
                postPolter.Add(ModContent.ItemType<SoulEdge>(), 1);

            if (npc.type == ModContent.NPCType<CalamityMod.NPCs.NormalNPCs.WulfrumDrone>())
            {
                npcLoot.Add(ModContent.ItemType<MageWulfrumHoodLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<MeleeWulfrumHelmLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<RangedWulfrumHeadgearLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<SummonerWulfrumHelmetLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<ThrowerWulfrumMaskLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumArmorLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumLeggingsLegacy>(), 100);
            }

            if (npc.type == ModContent.NPCType<CalamityMod.NPCs.NormalNPCs.WulfrumGyrator>())
            {
                npcLoot.Add(ModContent.ItemType<MageWulfrumHoodLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<MeleeWulfrumHelmLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<RangedWulfrumHeadgearLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<SummonerWulfrumHelmetLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<ThrowerWulfrumMaskLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumArmorLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumLeggingsLegacy>(), 100);
            }

            if (npc.type == ModContent.NPCType<CalamityMod.NPCs.NormalNPCs.WulfrumHovercraft>())
            {
                npcLoot.Add(ModContent.ItemType<MageWulfrumHoodLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<MeleeWulfrumHelmLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<RangedWulfrumHeadgearLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<SummonerWulfrumHelmetLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<ThrowerWulfrumMaskLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumArmorLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumLeggingsLegacy>(), 100);
            }

            if (npc.type == ModContent.NPCType<CalamityMod.NPCs.NormalNPCs.WulfrumAmplifier>())
            {
                npcLoot.Add(ModContent.ItemType<MageWulfrumHoodLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<MeleeWulfrumHelmLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<RangedWulfrumHeadgearLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<SummonerWulfrumHelmetLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<ThrowerWulfrumMaskLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumArmorLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumLeggingsLegacy>(), 100);
            }

            if (npc.type == ModContent.NPCType<CalamityMod.NPCs.NormalNPCs.WulfrumRover>())
            {
                npcLoot.Add(ModContent.ItemType<MageWulfrumHoodLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<MeleeWulfrumHelmLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<RangedWulfrumHeadgearLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<SummonerWulfrumHelmetLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<ThrowerWulfrumMaskLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumArmorLegacy>(), 100);
                npcLoot.Add(ModContent.ItemType<WulfrumLeggingsLegacy>(), 100);
            }
            if (npc.type == ModContent.NPCType<CalamityMod.NPCs.NormalNPCs.Cnidrion>())
            {
                if (CIServerConfig.Instance.CalExtraDrop == true)
                {
                    npcLoot.Add(ModContent.ItemType<PearlShard> (), 1, 6, 12);
                }
            }
            #region ModBoss

            if (npc.type == ModContent.NPCType<DesertScourgeHead>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedDesertScourge, ModContent.ItemType<KnowledgeDesertScourge>(), desc: DropHelper.FirstKillText);
                npcLoot.Add(ModContent.ItemType<AeroStoneLegacy>(),1);
            }
            if (npc.type == ModContent.NPCType<Crabulon>())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCrabulon, ModContent.ItemType<KnowledgeCrabulon>(), desc: DropHelper.FirstKillText);
            if (npc.type == ModContent.NPCType<HiveMind>())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedHiveMind, ModContent.ItemType<KnowledgeHiveMind>(), desc: DropHelper.FirstKillText);
            if (npc.type == ModContent.NPCType<PerforatorHive>())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedPerforator, ModContent.ItemType<KnowledgePerforators>(), desc: DropHelper.FirstKillText);

            if (npc.type == ModContent.NPCType<SlimeGodCore>())
            {
                npcLoot.Add(ModContent.ItemType<PurifiedJam>(), 1);
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedSlimeGod, ModContent.ItemType<KnowledgeSlimeGod>(), desc: DropHelper.FirstKillText);
            }

            if (npc.type == ModContent.NPCType<Cryogen>())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCryogen, ModContent.ItemType<KnowledgeCryogen>(), desc: DropHelper.FirstKillText);
            if (npc.type == ModContent.NPCType<BrimstoneElemental> ())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedBrimstoneElemental, ModContent.ItemType<KnowledgeBrimstoneElemental>(), desc: DropHelper.FirstKillText);
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedBrimstoneElemental, ModContent.ItemType<KnowledgeBrimstoneCrag>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<AquaticScourgeHead>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedAquaticScourge, ModContent.ItemType<KnowledgeAquaticScourge>(), desc: DropHelper.FirstKillText);
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedAquaticScourge, ModContent.ItemType<KnowledgeSulphurSea>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<CalamitasClone>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCalamitasClone, ModContent.ItemType<KnowledgeCalamitasClone>(), desc: DropHelper.FirstKillText);
                if (CIServerConfig.Instance.CalExtraDrop == true)
                {
                    npcLoot.Add(ItemID.BrokenHeroSword, 3, 2, 3);
                }
            }

            if (npc.type == ModContent.NPCType<Cataclysm>() && !CIServerConfig.Instance.CustomShimmer)
                npcLoot.Add(ModContent.ItemType<HavocsBreathLegacy>(), 4);
            if (npc.type == ModContent.NPCType<Anahita>() || npc.type == ModContent.NPCType<Leviathan>())
            {
                bool shouldDropLore(DropAttemptInfo info) => !DownedBossSystem.downedLeviathan && LastAnLStanding();
                npcLoot.AddConditionalPerPlayer(shouldDropLore, ModContent.ItemType<KnowledgeLeviathanAnahita>(), desc: DropHelper.FirstKillText);
                npcLoot.AddConditionalPerPlayer(shouldDropLore, ModContent.ItemType<KnowledgeOcean>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<AstrumAureus>())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedAstrumAureus, ModContent.ItemType<KnowledgeAstrumAureus>(), desc: DropHelper.FirstKillText);
            if (npc.type == ModContent.NPCType<PlaguebringerGoliath>())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedPlaguebringer, ModContent.ItemType<KnowledgePlaguebringerGoliath>(), desc: DropHelper.FirstKillText);
            if (npc.type == ModContent.NPCType<RavagerBody>())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedRavager, ModContent.ItemType<KnowledgeRavager>(), desc: DropHelper.FirstKillText);
            if (npc.type == ModContent.NPCType<AstrumDeusHead>())
            {
                bool firstDeusKill(DropAttemptInfo info) => !DownedBossSystem.downedAstrumDeus && !ShouldNotDropThings(info.npc);
                npcLoot.AddConditionalPerPlayer( firstDeusKill, ModContent.ItemType<KnowledgeAstrumDeus>(), desc: DropHelper.FirstKillText);
                npcLoot.AddConditionalPerPlayer( firstDeusKill, ModContent.ItemType<KnowledgeAstralInfection>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<ProfanedGuardianCommander> ())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedGuardians, ModContent.ItemType<KnowledgeProfanedGuardians>(), desc: DropHelper.FirstKillText);
            if (npc.type == ModContent.NPCType<Bumblefuck>())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedDragonfolly, ModContent.ItemType<KnowledgeDragonfolly>(), desc: DropHelper.FirstKillText);
            if (npc.type == ModContent.NPCType<Providence>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedProvidence, ModContent.ItemType<KnowledgeProvidence>(), desc: DropHelper.FirstKillText);
                npcLoot.Add(ModContent.ItemType<ElysianAegisold>(), 1);
            }
            if (npc.type == ModContent.NPCType<StormWeaverHead>())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCeaselessVoid && DownedBossSystem.downedStormWeaver && DownedBossSystem.downedSignus, ModContent.ItemType<KnowledgeSentinels>());
            if (npc.type == ModContent.NPCType<CeaselessVoid>())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCeaselessVoid && DownedBossSystem.downedStormWeaver && DownedBossSystem.downedSignus, ModContent.ItemType<KnowledgeSentinels>());
            if (npc.type == ModContent.NPCType<Signus>())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCeaselessVoid && DownedBossSystem.downedStormWeaver && DownedBossSystem.downedSignus, ModContent.ItemType<KnowledgeSentinels>());
            if (npc.type == ModContent.NPCType<Polterghast>())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedPolterghast, ModContent.ItemType<KnowledgePolterghast>(), desc: DropHelper.FirstKillText);
            if (npc.type == ModContent.NPCType<OldDuke>())
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedBoomerDuke, ModContent.ItemType<KnowledgeOldDuke>(), desc: DropHelper.FirstKillText);
            if (npc.type == ModContent.NPCType<DevourerofGodsHead>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedDoG, ModContent.ItemType<KnowledgeDevourerofGods>(), desc: DropHelper.FirstKillText);
            }
            if (npc.type == ModContent.NPCType<Yharon>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedYharon, ModContent.ItemType<KnowledgeYharon>(), desc: DropHelper.FirstKillText);
                var yharimArmorLoot = ItemDropRule.Common(ModContent.ItemType<YharimAuricTeslaHelm>(), 100);
                yharimArmorLoot.OnSuccess(ItemDropRule.Common(ModContent.ItemType<YharimAuricTeslaBodyArmor>()));
                yharimArmorLoot.OnSuccess(ItemDropRule.Common(ModContent.ItemType<YharimAuricTeslaCuisses>()));
                npcLoot.Add(yharimArmorLoot);
            }
            if (npc.type == ModContent.NPCType<AresBody>() || npc.type == ModContent.NPCType<ThanatosHead>() || npc.type == ModContent.NPCType<Apollo>())
            {
                bool shouldDropLore(DropAttemptInfo info) => !DownedBossSystem.downedExoMechs && ExoCanDropLoot();
                npcLoot.AddConditionalPerPlayer(shouldDropLore, ModContent.ItemType<KnowledgeExoMechs>(), desc: DropHelper.FirstKillText);
            }

            if (npc.type == ModContent.NPCType<SupremeCalamitas>())
            {
                npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedCalamitas, ModContent.ItemType<KnowledgeCalamitas>(), desc: DropHelper.FirstKillText);
                npcLoot.Add(ModContent.ItemType<VehemencOld>(), 1);
                npcLoot.AddConditionalPerPlayer(() => DownedBossSystem.downedExoMechs, ModContent.ItemType<CalamitousEssence>());
            }
            #endregion
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
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<ConclaveCrossfire>(), 100, 50));
                    break;
                case NPCID.SeaSnail:
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<SeaShell>(), 2, 1));
                    break;
                case NPCID.DarkCaster:
                    npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<AncientShiv>(),25,15));
                    break;
                #endregion

                #region LoreItems
                // Dreadnautilus drops the Blood Moon lore
                case NPCID.BloodNautilus:
                    npcLoot.AddConditionalPerPlayer(() => !DownedBossSystem.downedDreadnautilus, ModContent.ItemType<KnowledgeBloodMoon>(), desc: DropHelper.FirstKillText);
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
                    if(!CIServerConfig.Instance.CustomShimmer)
                    npcLoot.Add(ModContent.ItemType<MeowthrowerLegacy>(), 4);
                    break;
                case NPCID.TheDestroyer:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedMechBoss1, ModContent.ItemType<KnowledgeDestroyer>(), desc: DropHelper.FirstKillText);
                    npcLoot.AddConditionalPerPlayer(ShouldDropMechLore, ModContent.ItemType<KnowledgeMechs>(), desc: DropHelper.MechBossText);
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
                    break;
                case NPCID.Golem:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedGolemBoss, ModContent.ItemType<KnowledgeGolem>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.DukeFishron:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedFishron, ModContent.ItemType<KnowledgeDukeFishron>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.CultistBoss:
                    // Lore
                    npcLoot.AddConditionalPerPlayer(() => !NPC.downedAncientCultist, ModContent.ItemType<KnowledgeLunaticCultist>(), desc: DropHelper.FirstKillText);
                    break;
                case NPCID.MoonLordCore:
                    // Lore
                    {
                        npcLoot.AddConditionalPerPlayer(() => !NPC.downedMoonlord, ModContent.ItemType<KnowledgeMoonLord>(), desc: DropHelper.FirstKillText);
                        break;
                    }
                #endregion

            }
        }
        #endregion
    }
}
