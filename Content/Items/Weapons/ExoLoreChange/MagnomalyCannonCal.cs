using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Ranged;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityMod.Projectiles.Ranged;

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
                damage.Base = 330;
            }
        }
        public override bool CanUseItem(Item item, Player player)
        {

            var CIPlayer = player.CalamityInheritance();

            if (CIPlayer.exoMechLore)
            {
                item.shoot = ModContent.ProjectileType<MagnomalyRocket>();
                item.useAnimation = item.useTime = 67;
            }
            else
            {
                item.useTime = 15;
                item.useAnimation = 15;
                item.shoot = ModContent.ProjectileType<MagnomalyRocket>();
            }
            return base.CanUseItem(item , player);
        }
    }
}
