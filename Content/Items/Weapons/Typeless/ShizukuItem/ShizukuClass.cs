using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem
{
    public class ShizukuDamageClass : DamageClass
    {
        internal static ShizukuDamageClass Instance;
        public override void Load() => Instance = this;
        public override void Unload() => Instance = null;
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Magic)
            {
                return new StatInheritanceData
                (
                    damageInheritance: 1.2f,
                    critChanceInheritance: 1.0f,
                    attackSpeedInheritance: 0.25f,
                    armorPenInheritance: 1.0f,
                    knockbackInheritance: 0.5f
                );
            }
            else if (damageClass == Melee)
            {
                return new StatInheritanceData
                (
                    damageInheritance: 1.2f,
                    critChanceInheritance: 1.0f,
                    attackSpeedInheritance: 0.5f,
                    armorPenInheritance: 1.0f,
                    knockbackInheritance: 0.5f
                );
            }
            else if (damageClass == Generic)
                return StatInheritanceData.Full;
            else return StatInheritanceData.None;
        }
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (damageClass == Melee)
                return true;
            if (damageClass == Magic)
                return true;
            return false;
        }
        public override void SetDefaultStats(Player player)
        {
            player.GetArmorPenetration<ShizukuDamageClass>() += 50;
            player.GetCritChance<ShizukuDamageClass>() += 6;
            base.SetDefaultStats(player);
        }
        public override bool UseStandardCritCalcs => true;
        public override bool ShowStatTooltipLine(Player player, string lineName)
        {
            return true;
        }
    }
}