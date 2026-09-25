using CalamityInheritance.Content.Items.Accessories.Combat;
using CalamityInheritance.Content.Items.Armor.ArmorBonus;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Items
{
    public partial class CIGlobalItems : GlobalItem
    {
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockBack)
        {
            if (player.CI().ExShootSpeed != 0)
                velocity *= 1f + player.CI().ExShootSpeed;
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CIPlayer cIPlayer = player.CI();
            if (cIPlayer.victideSet)
                VictideArmorBonus.VicTideArmorBonus(cIPlayer, item, player, source, damage, knockback);
            if (cIPlayer.LuxorsGiftLegacyShoot)
                LuxorsGiftLegacy.Shoot(item, player, source, position, velocity, damage);
            return true;
        }
    }
}
