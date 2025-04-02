using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items
{
    public partial class CalamityInheritanceGlobalItem : GlobalItem
    {
        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            if (item.ModItem != null)
            if (item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>() && item.useStyle != ItemUseStyleID.Thrust)
                CIFunction.BetterSwing(player);
        }
    }
}
