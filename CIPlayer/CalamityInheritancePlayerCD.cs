using Terraria.ModLoader; 

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer: ModPlayer
    {
        public int HammerCounts = 0; //用于锤子打击次数的计时
        public float HammerSpin = 0f; //用于大锤子Echo的旋转, 我也不知道有啥好的解决方法       

        public static void ResetCD()
        {
            return;
        }
    }
}