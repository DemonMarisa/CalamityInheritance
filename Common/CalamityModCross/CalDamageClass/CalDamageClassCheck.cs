using CalamityMod;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CalamityModCross.CalDamageClass
{
    public static class CalDamageClassCheck
    {
        [JITWhenModsEnabled("CalamityMod")]
        public static bool CheckCalTrueMelee(this DamageClass damageClass)
        {
            return damageClass == GetInstance<TrueMeleeDamageClass>();
        }
        [JITWhenModsEnabled("CalamityMod")]
        public static bool CheckCalTrueMeleeNoSpeed(this DamageClass damageClass)
        {
            return damageClass == GetInstance<TrueMeleeNoSpeedDamageClass>();
        }
    }
}
