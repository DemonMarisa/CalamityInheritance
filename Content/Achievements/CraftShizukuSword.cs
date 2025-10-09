using System.Collections.Generic;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Achievements
{
    public class CraftShizukuSword : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Collector);
            AddItemPickupCondition(ModContent.ItemType<ShizukuSword>());
        }
        public override IEnumerable<Position> GetModdedConstraints()
        {
            yield return new After(ModContent.GetInstance<CraftDoubleArk>());
        }
    }
}