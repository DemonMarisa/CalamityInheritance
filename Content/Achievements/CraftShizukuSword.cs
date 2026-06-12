using CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Achievements
{
    public class CraftShizukuSword : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Collector);
            AddItemPickupCondition(ItemType<ShizukuSword>());
        }
        public override IEnumerable<Position> GetModdedConstraints()
        {
            yield return new After(GetInstance<CraftDoubleArk>());
        }
    }
}