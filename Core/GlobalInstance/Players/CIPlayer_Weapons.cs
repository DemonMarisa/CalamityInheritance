using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public int HellbornBoost;
        public int PolarisPhase;
        public int PolarisBoostCounter;
        public void ResetWeapons()
        {
            // 地狱降临
            if (HellbornBoost > 0)
                HellbornBoost--;
            // 北辰鱼
            if (PolarisBoostCounter > 0 && Player.miscCounter % 4 == 0 && Player.itemTime == 0)
                PolarisBoostCounter--;
            if (PolarisBoostCounter < 5)
                PolarisPhase = 0;
            else if (PolarisBoostCounter >= 5 && PolarisBoostCounter < 10)
                PolarisPhase = 1;
            else if (PolarisBoostCounter >= 15 && PolarisBoostCounter < 30)
                PolarisPhase = 2;
            else if (PolarisBoostCounter > 30)
                PolarisBoostCounter = 20;
        }
    }
}
