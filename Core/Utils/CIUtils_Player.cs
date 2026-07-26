using Terraria;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        public static bool CantUseHoldout(this Player player, bool needsToHold = true)
        {
            if (player != null && player.active && !player.dead && !(!player.channel && needsToHold) && !player.CCed)
            {
                return player.noItems;
            }
            return true;
        }
    }
}
