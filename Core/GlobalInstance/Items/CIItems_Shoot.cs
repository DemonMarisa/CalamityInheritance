using CalamityInheritance.Content.Items.Armor.ArmorBonus;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Items
{
    public partial class CIGlobalItems : GlobalItem
    {
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CIPlayer cIPlayer = player.CI();
            VictideArmorBonus.VicTideArmorBonus(cIPlayer, item, player, source, damage, knockback);
            return true;
        }
    }
}
