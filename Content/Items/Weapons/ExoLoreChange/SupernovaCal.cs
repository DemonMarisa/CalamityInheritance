using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Weapons.Rogue;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class SupernovaCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<Supernova>();
        }
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                damage.Base = 9999;
            }
        }
    }
}
