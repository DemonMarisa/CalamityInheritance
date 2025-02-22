using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Ranged;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class MagnomalyCannonCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<MagnomalyCannon>();
        }
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var usPlayer = player.CalamityInheritance();
            if (usPlayer.exoMechLore)
            {
                damage.Base = 800;
            }
        }
    }
}
