using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Weapons.Ranged;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class PhotovisceratorCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<Photoviscerator>();
        }
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                damage.Base = 810;
            }
        }
    }
}
