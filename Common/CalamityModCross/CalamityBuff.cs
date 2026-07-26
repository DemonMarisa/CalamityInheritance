using CalamityInheritance.Core.Utils;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CalamityModCross
{
    /// <summary>
    /// 这就是个不经过ModSystem的方法。
    /// </summary>
    public static class CalamityBuff
    {
        public static string SulphuricPoisoningName => "SulphuricPoisoning";
        public static bool GetThis(this string name, out int type)
        {
            bool can = false;
            type = -1;
            if (!CIUtils.HasCalamity())
            {
                return can;
            }
            can = CalamityInheritance.Calamity.TryFind(name, out ModBuff buff);
            if (can)
            {
                type = buff.Type;
            }
            return can;
        }
    }
}
