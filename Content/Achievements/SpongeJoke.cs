using System;
using System.Collections.Generic;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Achievements
{
    public class SpongeJoke : ModAchievement
    {
        public CustomFlagCondition ActivedSpongeBarrier{ get; private set; }
        public CustomFlagCondition EquippedCalamitySponge{ get; private set; }
        public CustomFlagCondition LastHitToYharon{ get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            ActivedSpongeBarrier = AddCondition("ActivedSpongeBarrier");
            EquippedCalamitySponge = AddCondition("EquippedCalamitySponge");
            LastHitToYharon = AddCondition("LastHitToYharon");
            Achievement.UseTracker
            (
                new CustomTrack
                (
                    new Dictionary<AchievementCondition, float>()
                    {
                        [ActivedSpongeBarrier] = 1f,
                        [EquippedCalamitySponge] = 1f,
                        [LastHitToYharon] = 1f
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