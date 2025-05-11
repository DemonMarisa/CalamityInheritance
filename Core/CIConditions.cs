using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.World;
using CalamityMod;
using CalamityMod.World;
using System;
using Terraria;
using Terraria.Localization;

namespace CalamityInheritance.Core
{
    public static class CIConditions
    {

        private static Condition Create(string key, Func<bool> predicate)
        {
            return new Condition(
                Language.GetText($"Mods.CalamityInheritance.Condition.{key}"),
                predicate
            );
        }

        public static readonly Condition DownedBOC = Create("DownedBOC", () => CIDownedBossSystem.DownedBOC);
        public static readonly Condition DownedEOW = Create("DownedEOW", () => CIDownedBossSystem.DownedEOW);
        public static readonly Condition DownedBloodMoon = Create("DownedBloodMoon", () => CIDownedBossSystem.DownedBloodMoon);
        public static readonly Condition DownedLegacyScal = Create("DownedLegacyScal", () => CIDownedBossSystem.DownedLegacyScal);
        public static readonly Condition DownedLegacyYharonP1 = Create("DownedLegacyYharonP1", () => CIDownedBossSystem.DownedLegacyYharonP1);
        public static readonly Condition DownedBuffedSolarEclipse = Create("DownedBuffedSolarEclipse", () => CIDownedBossSystem.DownedBuffedSolarEclipse);
        public static readonly Condition ArmageddonNoNor = Create("ArmageddonDrop", () => CIWorld.Armageddon && (Main.expertMode || Main.masterMode));
        public static readonly Condition Malice = Create("Malice", () => CIWorld.Malice);
        public static readonly Condition Defiled = Create("Defiled", () => CIWorld.Defiled);
        public static readonly Condition MasterDeath = Create("MD", () => Main.masterMode && CalamityWorld.death);
        public static readonly Condition MAD = Create("MAD", () => CIWorld.Armageddon && CIWorld.Malice && CIWorld.Defiled && Main.masterMode);
        public static readonly Condition DownedCalClone = Create("DownedCalClone", () => CIDownedBossSystem.DownedCalClone);
    }
}
