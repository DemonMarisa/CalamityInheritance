using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public override void ResetEffects()
        {
            base.ResetEffects();
        }
        public override void UpdateDead()
        {
            ResetTimerDeath();
        }

    }
}
