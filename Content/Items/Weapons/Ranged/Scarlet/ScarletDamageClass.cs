using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Scarlet
{
    public class ScarletDamageClass : DamageClass
    {
        internal static ScarletDamageClass Instance;
        public override void Load()
        {
            Instance = this;
        }
        public override void Unload()
        {
            Instance = null;
        }
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Ranged)
                return new StatInheritanceData
                (
                    damageInheritance: 1.0f,
                    critChanceInheritance: 1.0f,
                    attackSpeedInheritance: 1.0f,
                    armorPenInheritance: 10.0f,
                    knockbackInheritance: 0.5f
                );
            else if (damageClass == Generic)
                return StatInheritanceData.Full;
            return StatInheritanceData.None;
        }
        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            if (damageClass == Ranged)
                return true;
            return false;
        }
        public override void SetDefaultStats(Player player)
        {
            player.GetArmorPenetration<ScarletDamageClass>() += 10;
            player.GetCritChance<ScarletDamageClass>() += 50;
        }
        public override bool UseStandardCritCalcs => true;
        public override bool ShowStatTooltipLine(Player player, string lineName)
        {
            return true;
        }
    }
}