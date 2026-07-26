using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Core.GlobalInstance.Items;
using CalamityInheritance.Core.GlobalInstance.Players;
using Terraria;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {

        public static bool HasCalamity()
        {
            return CalamityInheritance.Calamity is not null;
        }
        public static CIPlayer CI(this Player player)
        {
            return player.GetModPlayer<CIPlayer>();
        }
        public static CIGlobalItems CI(this Item item)
        {
            return item.GetGlobalItem<CIGlobalItems>();
        }
        public static CalPlayerInfo CalPlayerInfo(this Player player)
        {
            return player.GetModPlayer<CalPlayerInfo>();
        }
    }
}
