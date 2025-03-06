using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items.Weapons.Melee;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class ExoFlailCI : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<ExoFlail>();
        }
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var usPlayer = player.CalamityInheritance();
            if (usPlayer.LoreExo)
            {
                damage.Base = 2000;
            }
        }
    }
}
