using CalamityInheritance.Common.CalamityModCross;
using Terraria;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        public static bool HasCalamity()
        {
            return CalamityInheritance.Calamity is not null;
        }
        public static CalPlayerInfo CalPlayerInfo(this Player player)
        {
            return player.GetModPlayer<CalPlayerInfo>();
        }
    }
}
