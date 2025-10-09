using System.Collections.Generic;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using Terraria.Achievements;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Achievements
{
    public class CraftMemeChair : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(AchievementCategory.Collector);
            AddItemPickupCondition(ModContent.ItemType<StepToolShadows>());
        }
        public override IEnumerable<Position> GetModdedConstraints()
        {
            yield return new After(ModContent.GetInstance<CraftScarletGun>());
        }
    }
}