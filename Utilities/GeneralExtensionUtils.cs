using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items;
using Terraria;

namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
    {
        public static CalamityInheritancePlayer CalamityInheritance(this Player player)
        {
            return player.GetModPlayer<CalamityInheritancePlayer>();
        }
        public static CalamityInheritanceGlobalItem CalamityInheritance(this Item item) => item.GetGlobalItem<CalamityInheritanceGlobalItem>();
    }
}
