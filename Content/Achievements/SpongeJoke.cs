using System.Collections.Generic;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Achievements
{
    public class SpongeJoke : ModAchievement
    {
        public CustomFlagCondition LastHitToWithCalSpongeYharon { get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            LastHitToWithCalSpongeYharon = AddCondition("LastHitToYharon");
            Achievement.UseTracker
            (
                new CustomTrack
                (
                    new Dictionary<AchievementCondition, float>()
                    {
                        [LastHitToWithCalSpongeYharon] = 1f
                    }
                )
            );
        }
        public override IEnumerable<Position> GetModdedConstraints()
        {
            yield return new After(ModContent.GetInstance<GetMalaLegendary>());
        }
    }
}