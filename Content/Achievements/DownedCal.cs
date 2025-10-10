using System.Collections.Generic;
using CalamityInheritance.NPCs.Boss.CalamitasClone;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Achievements
{
    public class DownedCal : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Slayer);
            AddNPCKilledCondition(ModContent.NPCType<CalamitasCloneLegacy>());
        }
        public override IEnumerable<Position> GetModdedConstraints()
        {
            yield return new After(ModContent.GetInstance<CraftDoubleArk>());
        }

    }
}