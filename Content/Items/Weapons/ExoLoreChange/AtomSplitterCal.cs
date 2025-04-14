using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class AtomSplitterCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ModContent.ItemType<TheAtomSplitter>();
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (player.CIMod().LoreExo || player.CIMod().PanelsLoreExo)
                damage.Base = 1000;
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.CIMod().LoreExo || player.CIMod().PanelsLoreExo)
                velocity *= 2.4f;
            base.ModifyShootStats(item, player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
    }
}