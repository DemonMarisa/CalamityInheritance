using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public int soundDelay = 0;
        public void UpdateTimer()
        {
            if (soundDelay > 0)
                soundDelay--;
        }
        public void ResetTimerDeath()
        {
            soundDelay = 0;
        }
    }
}
