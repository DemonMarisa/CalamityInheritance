using System;
using System.Collections.Generic;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Achievements
{
    public class GetMalaLegendary : ModAchievement
    {
        public CustomFlagCondition LegendaryComplete1{ get; private set; }
        public CustomFlagCondition LegendaryComplete2{ get; private set; }
        public CustomFlagCondition LegendaryComplete3{ get; private set; }
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Challenger);
            LegendaryComplete1 = AddCondition("Condition1");
            LegendaryComplete2 = AddCondition("Condition2");
            LegendaryComplete3 = AddCondition("Condition3");
            var pickUpCondition = AddItemPickupCondition(ModContent.ItemType<PBGLegendary>());
            Achievement.UseTracker
            (
                new CustomTrack
                (
                    new Dictionary<AchievementCondition, float>()
                    {
                        [LegendaryComplete1] = 1f,
                        [LegendaryComplete2] = 1f,
                        [LegendaryComplete3] = 1f,
                        [pickUpCondition] = 1f,
                    }
                )
            );
        }
        public override IEnumerable<Position> GetModdedConstraints()
        {
            yield return new After(ModContent.GetInstance<DownedYharonP2>());
        }
    }
    
    public class CustomTrack : ConditionFloatTracker
    {
        private Dictionary<AchievementCondition, float> weightedCondi;
        public CustomTrack(Dictionary<AchievementCondition, float> weightedCondi)
        {
            this.weightedCondi = weightedCondi;
            foreach ((AchievementCondition condition, float weight) in weightedCondi)
            {
                _maxValue += weight;
                condition.OnComplete += OnConditionCompleted;
            }
        }
        private void OnConditionCompleted(AchievementCondition condi)
        {
            SetValue(Math.Min(_value + weightedCondi[condi], _maxValue));
        }
        protected override void Load()
        {
            foreach ((AchievementCondition condition, float weight) in weightedCondi)
            {
                if (condition.IsCompleted)
                    _value += weight;
            }
        }
    }
}