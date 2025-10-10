using CalamityInheritance.Content.Items.Weapons.Ranged.Scarlet;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Achievements
{
    public class CraftScarletGun : ModAchievement
    {
        public override void SetStaticDefaults()
        {
            Achievement.SetCategory(Terraria.Achievements.AchievementCategory.Collector);
            AddItemPickupCondition(ModContent.ItemType<R99>());
        }
        public override Position GetDefaultPosition()
        {
            return new After("TO_INFINITY_AND_BEYOND");
        }
    }
}