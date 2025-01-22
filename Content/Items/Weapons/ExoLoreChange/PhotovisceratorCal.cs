using CalamityMod.Items.Weapons.Melee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var usPlayer = player.CalamityInheritance();
            if (usPlayer.exoMechLore)
            {
                damage.Base = 810;
            }
        }
    }
}
