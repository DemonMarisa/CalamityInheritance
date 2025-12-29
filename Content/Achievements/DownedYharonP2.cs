using System.Collections.Generic;
using CalamityInheritance.NPCs.Boss.Yharon;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Achievements
{
    public class DownedYharonP2 : ModAchievement
    {
        public CustomFlagCondition ForceKilledCondition { get; private set; } 
        public override void SetStaticDefaults()
        {
            ForceKilledCondition = AddCondition();
            Achievement.SetCategory(AchievementCategory.Slayer);
            var killed = AddNPCKilledCondition(NPCType<YharonLegacy>());
            Achievement.UseTracker
            (
                new CustomTrack
                (
                    new Dictionary<AchievementCondition, float>()
                    {
                        [ForceKilledCondition] = 1f,
                        [killed] = 1f,
                    }
                )
            );

        }
        public override IEnumerable<Position> GetModdedConstraints()
        {
            yield return new After(GetInstance<DownedScal>());
        }

    }
}