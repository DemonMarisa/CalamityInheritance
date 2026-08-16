using CalamityInheritance.Common.CalamityModCross;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public int CurAOTCCharge;
        public bool CanUseOldLordDash;
        public bool BlockDefenseDamage;
        public float ContactDamageReduction;
        public float HurtHeal;
        public float FinalDefenseMult = 1f;
        public void MainResetEffects()
        {
            if (BlockDefenseDamage)
                Player.SetImmunityDefenseDamage();
            BlockDefenseDamage = false;
            ContactDamageReduction = 1f;
            HurtHeal = 0f;
            FinalDefenseMult = 1f;
        }
        public void MainOnHurt(Player.HurtInfo info)
        {
            if (HurtHeal != 0)
                Player.NCHeal((int)(info.Damage * HurtHeal));
        }
        public void MultDefense_PostUpdate()
        {
            if (FinalDefenseMult != 1f)
                Player.statDefense *= FinalDefenseMult;
        }
        public override void PostUpdate()
        {
            MultDefense_PostUpdate();
            UpdateShield_PostUpdate();
        }
    }
}
