using CalamityInheritance.Content.Items.Placeables.Relic;
using CalamityInheritance.Content.Items.SummonItems;
using CalamityInheritance.Content.Items.TreasureBags;
using CalamityInheritance.NPCs.Boss.CalamitasClone;
using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityInheritance.NPCs.Boss.Yharon;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Armor.Vanity;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Pets;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.TreasureBags;
using System;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.ModSupport
{
    public class CIWeakReferenceSupport
    {
        public static readonly Func<bool> DownedCalCloneLegacy = () => CIDownedBossSystem.DownedCalClone;
        public static readonly Func<bool> DownedScal = () => CIDownedBossSystem.DownedLegacyScal;
        public static readonly Func<bool> DownedPostEclipseYharon = () => CIDownedBossSystem.DownedLegacyYharonP1;
        // 快速获取本地化路径
        private static LocalizedText GetDisplayName(string entryName) => CIFunction.GetText($"BossChecklistSupport.{entryName}.EntryName");
        private static LocalizedText GetSpawnInfo(string entryName) => CIFunction.GetText($"BossChecklistSupport.{entryName}.SpawnInfo");
        private static LocalizedText GetDespawnMessage(string entryName) => CIFunction.GetText($"BossChecklistSupport.{entryName}.DespawnMessage");

        private static readonly Dictionary<string, float> BossChecklistProgressionValues = new()
        {
            //{ "DesertScourge", 1.6f },
            //{ "GiantClam", 1.61f },
            //{ "AcidRainT1", 2.67f },
            //{ "Crabulon", 2.7f },
            //{ "HiveMind", 3.98f },
            //{ "Perforators", 3.99f },
            //{ "SlimeGod", 6.7f }, // Thorium Granite Energy Storm is 6.4f, Buried Champion is 6.5f, and Star Scouter is 6.9f
            //{ "Cryogen", 8.5f },
            //{ "AquaticScourge", 9.5f },
            //{ "AcidRainT2", 9.51f },
            //{ "CragmawMire", 9.52f },
            //{ "BrimstoneElemental", 10.5f },
            //{ "CalamitasClone", 11.7f }, // Thorium Lich is 11.6f
            { "CalCloneLegacy", 11.8f },
            //{ "GreatSandShark", 12.09f },
            //{ "Leviathan", 12.8f },
            //{ "AstrumAureus", 12.81f },
            //{ "PlaguebringerGoliath", 14.5f },
            //{ "Ravager", 16.5f },
            //{ "AstrumDeus", 17.5f },
            //{ "ProfanedGuardians", 18.5f },
            //{ "Dragonfolly", 18.6f },
            //{ "Providence", 19f }, // Thorium Primordials (Ragnarok) is 19.5f
            //{ "CeaselessVoid", 19.6f },
            //{ "StormWeaver", 19.61f },
            //{ "Signus", 19.62f },
            //{ "Polterghast", 20f },
            //{ "AcidRainT3", 20.49f },
            //{ "Mauler", 20.491f },
            //{ "NuclearTerror", 20.492f },
            //{ "OldDuke", 20.5f },
            //{ "DevourerofGods", 21f },
            { "PostEclipseYharon", 21.5f },
            //{ "Yharon", 22f },
            //{ "ExoMechs", 22.99f },
            //{ "Calamitas", 23f },
            //{ "PrimordialWyrm", 23.5f },
            { "Scal", 25.98f },
            //{ "BossRush", 25.99f },
            //{ "Yharim", 24f },
            //{ "Noxus", 25f },
            //{ "Xeroc", 26f },
        };

        public static void Setup()
        {
            BossChecklistSupport();
        }
        public static void AddBoss(Mod bossChecklist, Mod hostMod, string name, float difficulty, Func<bool> downed, object npcTypes, Dictionary<string, object> extraInfo)
            => bossChecklist.Call("LogBoss", hostMod, name, difficulty, downed, npcTypes, extraInfo);
        private static void BossChecklistSupport()
        {
            CalamityInheritance cI = ModContent.GetInstance<CalamityInheritance>();
            Mod bossChecklist = cI.bossChecklist;
            if (bossChecklist is null)
                return;

            // Adds every single Calamity boss and miniboss to Boss Checklist's Boss Log.
            AddCIBosses(bossChecklist, cI);
        }

        #region 登记boss列表中的boss
        public static void AddCIBosses(Mod bossChecklist, Mod cI)
        {
            #region 普灾
            {
                string entryName = "CalCloneLegacy";
                BossChecklistProgressionValues.TryGetValue(entryName, out float order);
                int type = ModContent.NPCType<CalamitasCloneLegacy>();
                List<int> summons = new List<int>() {
                    ModContent.ItemType<EyeofDesolationLegacy>() };
                List<int> collection = new List<int>() { ModContent.ItemType<CalamitasCloneBag>() , ModContent.ItemType<CalCloneRelic>()};
                AddBoss(bossChecklist, cI, entryName, order, DownedCalCloneLegacy, type, new Dictionary<string, object>()
                {
                    ["displayName"] = GetDisplayName(entryName),
                    ["spawnInfo"] = GetSpawnInfo(entryName),
                    ["despawnMessage"] = GetDespawnMessage(entryName),
                    ["spawnItems"] = summons,
                    ["collectibles"] = collection,
                });
            }
            #endregion
            #region 丛林龙日蚀前
            {
                string entryName = "PostEclipseYharon";
                BossChecklistProgressionValues.TryGetValue(entryName, out float order);
                int type = ModContent.NPCType<YharonLegacy>();
                List<int> summons = new List<int>() {
                    ModContent.ItemType<YharonEggLegacy>() };
                List<int> collection = new List<int>() { ModContent.ItemType<YharonTreasureBagsLegacy>() };
                AddBoss(bossChecklist, cI, entryName, order, DownedPostEclipseYharon, type, new Dictionary<string, object>()
                {
                    ["displayName"] = GetDisplayName(entryName),
                    ["spawnInfo"] = GetSpawnInfo(entryName),
                    ["despawnMessage"] = GetDespawnMessage(entryName),
                    ["spawnItems"] = summons,
                    ["collectibles"] = collection,
                });
            }
            #endregion
            #region 至尊灾厄
            {
                string entryName = "Scal";
                BossChecklistProgressionValues.TryGetValue(entryName, out float order);
                int type = ModContent.NPCType<SupremeCalamitasLegacy>();
                List<int> summons = new List<int>() {
                    ModContent.ItemType<EyeofExtinction>() };
                List<int> collection = new List<int>() 
                {
                    ModContent.ItemType<ScalRelic>(),
                    ModContent.ItemType<SupremeCalamitasTrophy>(),
                    ModContent.ItemType<SupremeCataclysmTrophy>(),
                    ModContent.ItemType<SupremeCatastropheTrophy>(),
                    ModContent.ItemType<AshenHorns>(),
                    ModContent.ItemType<SCalMask>(),
                    ModContent.ItemType<SCalRobes>(),
                    ModContent.ItemType<SCalBoots>(),
                    ModContent.ItemType<LoreCalamitas>(),
                    ModContent.ItemType<LoreCynosure>(),
                    ModContent.ItemType<BrimstoneJewel>(),
                    ModContent.ItemType<Levi>(),
                };
                AddBoss(bossChecklist, cI, entryName, order, DownedScal, type, new Dictionary<string, object>()
                {
                    ["displayName"] = GetDisplayName(entryName),
                    ["spawnInfo"] = GetSpawnInfo(entryName),
                    ["despawnMessage"] = GetDespawnMessage(entryName),
                    ["spawnItems"] = summons,
                    ["collectibles"] = collection,
                    ["overrideHeadTextures"] = "CalamityInheritance/NPCs/Boss/SCAL/SupremeCalamitasLegacy_Head_Boss"
                });
            }
            #endregion
        }
        #endregion
    }
}
