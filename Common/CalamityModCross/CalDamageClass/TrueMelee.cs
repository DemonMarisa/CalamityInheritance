using LAP.Common.Utilities;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CalamityModCross.CalDamageClass
{
    public class TrueMelee : DamageClass
    {
        public static TrueMelee Instance { get; private set; }
        public override void Load() => Instance = this;
        public override void Unload() => Instance = null;

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (ModCrossUtils.HasCalamityMod())
            {
                if (damageClass == Melee || damageClass == Generic || damageClass.CheckCalTrueMelee())
                    return StatInheritanceData.Full;
            }
            if (damageClass == Melee || damageClass == Generic)
                return StatInheritanceData.Full;

            return StatInheritanceData.None;
        }
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (ModCrossUtils.HasCalamityMod())
            {
                return damageClass == Melee || damageClass.CheckCalTrueMelee();
            }
            else
            {
                return damageClass == Melee;
            }
        }
    }
    public class TrueMeleeNoSpeed : DamageClass
    {
        internal static TrueMeleeNoSpeed Instance;
        public override void Load() => Instance = this;
        public override void Unload() => Instance = null;
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (ModCrossUtils.HasCalamityMod())
            {
                if (damageClass == Generic || damageClass == Melee || damageClass.CheckCalTrueMeleeNoSpeed() || damageClass == TrueMelee.Instance)
                    return StatInheritanceData.Full with { attackSpeedInheritance = 0 };
            }

            if (damageClass == Generic || damageClass == Melee || damageClass == TrueMelee.Instance)
                return StatInheritanceData.Full with { attackSpeedInheritance = 0 };

            return StatInheritanceData.None;
        }
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (ModCrossUtils.HasCalamityMod())
            {
                return damageClass == Melee || damageClass == MeleeNoSpeed || damageClass == TrueMelee.Instance || damageClass.CheckCalTrueMeleeNoSpeed();
            }
            else
            {
                return damageClass == Melee || damageClass == MeleeNoSpeed || damageClass == TrueMelee.Instance;
            }
        }
    }
}
