using System.Collections.Generic;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Melee;
using Terraria.Achievements;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Achievements
{
    public class CraftDoubleArk : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            var oldOne = AddItemPickupCondition(ModContent.ItemType<ArkoftheCosmos>());
            var newOne = AddItemPickupCondition(ModContent.ItemType<ArkoftheCosmosold>());
            Achievement.SetCategory(AchievementCategory.Collector);
            Achievement.UseTracker
            (
                new CustomTrack
                (
                    new Dictionary<AchievementCondition, float>()
                    {
                        [oldOne] = 1f,
                        [newOne] = 1f
                    }
                )
            );
        }
        public override IEnumerable<Position> GetModdedConstraints()
        {
            yield return new After(ModContent.GetInstance<CraftMemeChair>());
        }
    }
}