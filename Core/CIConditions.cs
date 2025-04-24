using CalamityInheritance.System.DownedBoss;
using CalamityMod;
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

    }
}
