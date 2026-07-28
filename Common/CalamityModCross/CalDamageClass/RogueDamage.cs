using LAP.Common.Utilities;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CalamityModCross.CalDamageClass
{
    public class RogueDamage : DamageClass
    {
        public static RogueDamage Instance;
        public override void Load() => Instance = this;
        public override void Unload() => Instance = null;

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (ModCrossUtils.HasCalamityMod())
            {
                if (damageClass == Throwing || damageClass == Generic || damageClass.CheckCalRogue())
                    return StatInheritanceData.Full;
            }
            if (damageClass == Throwing || damageClass == Generic)
                return StatInheritanceData.Full;

            return StatInheritanceData.None;
        }

        public override bool GetEffectInheritance(DamageClass damageClass) => damageClass == Throwing;
    }
}
