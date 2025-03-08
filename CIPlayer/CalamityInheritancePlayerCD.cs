using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ModLoader; 

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer: ModPlayer
    {
        public int HammerCounts = 0; //用于锤子打击次数的计时
        public float HammerSpin = 0f; //用于大锤子Echo的旋转, 我也不知道有啥好的解决方法       
        public bool IfCloneHtting = false; //用于记录克隆的大锤子是否正在击中敌人
        public bool BuffExoApolste = false; //加强星流投矛
        public bool ForceHammerStealth = false;
        public int GodSlayerDMGprotectMax = 80;//金源伤害保护的衰减
        public int yharimArmorinvincibility = 0;//魔君套装无敌时间
        public void ResetCD()
        {
            if (GodSlayerDMGprotect)
            {
                if (GodSlayerDMGprotectMax < 80)
                    GodSlayerDMGprotectMax++;
            }
            if (yharimAuricArmor)
            {
                if (yharimArmorinvincibility > 0)
                    yharimArmorinvincibility--;
            }
            return;
        }
    }
}