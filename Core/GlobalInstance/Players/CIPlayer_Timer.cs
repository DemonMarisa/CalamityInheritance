using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public int soundDelay = 0;
        public int GlobalHealProjCD;
        public void UpdateTimer()
        {
            if (soundDelay > 0)
                soundDelay--;
            if (GlobalHealProjCD > 0)
                GlobalHealProjCD--;
        }
        public void ResetTimerDeath()
        {
            soundDelay = 0;
            GlobalHealProjCD = 0;
        }
    }
}
