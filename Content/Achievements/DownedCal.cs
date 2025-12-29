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
            AddNPCKilledCondition(NPCType<CalamitasCloneLegacy>());
        }
        public override IEnumerable<Position> GetModdedConstraints()
        {
            yield return new After(GetInstance<CraftDoubleArk>());
        }

    }
}