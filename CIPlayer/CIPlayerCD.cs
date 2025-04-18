using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Projectiles.Melee;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    /*
    *大部分的重复类的东西，如Buff， 套装效果， 饰品属性等的判定现在全部单独包装起来了
    *这样主要是方便后续进行维护，而且不用每次来这里都得找一大堆史
    *同时我将玩家类里面的几乎所有成员，除了部分实在是不能乱来的全部更名了
    *现在应该能更容易地看出来这个成员是干嘛的。
    */
    public partial class CalamityInheritancePlayer: ModPlayer
    {
        public int HammerCounts = 0; //用于锤子打击次数的计时
        public float HammerSpin = 0f; //用于大锤子Echo的旋转, 我也不知道有啥好的解决方法       
        public bool IfCloneHtting = false; //用于记录克隆的大锤子是否正在击中敌人
        public bool BuffExoApolste = false; //加强星流投矛
        public bool ForceHammerStealth = false;
        public int GodSlayerDMGprotectMax = 80;//金源伤害保护的衰减
        public int GlobalLegendaryT3CD = 0; //T3传奇武器特殊效果的全局CD（对，共享）
        public int yharimArmorinvincibility = 0;//魔君套装无敌时间
        public int AeroFlightPower = 0;
        #region 传奇武器的一些计数器
        public int DukeDefenseCounter = 0;
        public int DukeDefenseTimer = 0;
        #endregion
        public int SparkTimer = 0;
        public bool wasMouseDown = false;//用于qol面板的鼠标状态跟踪
        // 通用开火冷却
        public int fireCD = 0;
        public int GlobalSoundDelay = 0;
        //通用计时器
        public int GlobalFireDelay = 0;
        //通用……任意计数器？
        public int BrimstoneDartsCD = 0;
        public int GlobalMiscCounter = 1;
        public void ResetCD()
        {
            if (GodSlayerDMGprotect)
            {
                if (GodSlayerDMGprotectMax < 80)
                    GodSlayerDMGprotectMax++;
            }
            if (YharimAuricSet)
            {
                if (yharimArmorinvincibility > 0)
                    yharimArmorinvincibility--;
            }
            
            if (GlobalLegendaryT3CD > 0)
                GlobalLegendaryT3CD--;

            if (DukeDefenseTimer > 0)
                DukeDefenseTimer--;

            if (DukeDefenseCounter > 0 && DukeDefenseTimer == 0)
                DukeDefenseCounter--;

            if (SparkTimer > 0)
                SparkTimer --;

            if (fireCD > 0)
                fireCD--;
            if (AeroFlightPower > 0)
                AeroFlightPower--;
            if (GlobalSoundDelay > 0)
                GlobalSoundDelay --;
            if (GlobalFireDelay > 0)
                GlobalFireDelay--;
            if (BrimstoneDartsCD > 0)
                BrimstoneDartsCD--;
            return;
        }
    }
}