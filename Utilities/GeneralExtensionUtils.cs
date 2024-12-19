using CalamityInheritance.CIPlayer;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.NPCs;
using CalamityMod.Projectiles;
using Terraria;

namespace CalamityInheritance.Utilities
{
    public static partial class CalamityInheritanceUtils
    {
        public static CalamityInheritancePlayer CalamityInheritance(this Player player)
        {
            return player.GetModPlayer<CalamityInheritancePlayer>();
        }
    }
}
