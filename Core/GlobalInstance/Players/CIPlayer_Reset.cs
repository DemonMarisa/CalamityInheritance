using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public override void ResetEffects()
        {
            MainResetEffects();
            UpdateTimer();
            ResetWeapons();
            ResetArmor();
            ResetAccessories();
            PreKillReset();
            ResetDodge();
            ResetShield();
        }
        public override void UpdateDead()
        {
            ResetTimerDeath();
        }
    }
}
