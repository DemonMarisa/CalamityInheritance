using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public override void ResetEffects()
        {
            UpdateTimer();
            ResetWeapons();
            ResetArmor();
        }
        public override void UpdateDead()
        {
            ResetTimerDeath();
        }
    }
}
