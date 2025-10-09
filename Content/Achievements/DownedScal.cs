using System.Collections.Generic;
using CalamityInheritance.NPCs.Boss.SCAL;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Achievements
{
    public class DownedScal : ModAchievement
    {
        public CustomFlagCondition ForceKilledCondition { get; private set; }
        public override void SetStaticDefaults()
        {
            ForceKilledCondition = AddCondition();
            var killed = AddNPCKilledCondition(ModContent.NPCType<SupremeCalamitasLegacy>());
            Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Slayer);
            Achievement.UseTracker
            (
                new CustomTrack
                (
                    new Dictionary<Terraria.Achievements.AchievementCondition, float>()
                    {
                        [ForceKilledCondition] = 1f,
                        [killed] = 1f
                    }
                )
            );
        }
        public override IEnumerable<Position> GetModdedConstraints()
        {
            yield return new After(ModContent.GetInstance<DownedCal>());
        }

    }
}