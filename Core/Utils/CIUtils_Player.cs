using Terraria;
using Terraria.ModLoader;

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
        public static float CalcDamage<T>(this Player player, float baseDamage) where T : DamageClass => player.GetTotalDamage<T>().ApplyTo(baseDamage);
        public static int CalcIntDamage<T>(this Player player, float baseDamage) where T : DamageClass => (int)player.CalcDamage<T>(baseDamage);
    }
}
