using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria;
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
        public int yharimArmorinvincibility = 0;//魔君套装无敌时间

        // 1-4，分别标记四种状态，1为默认贴图（false），2为鼠标悬停的贴图（false），3为点击后的贴图（true）, 4为点击后悬停的贴图（true）
        public int panelloreExocount = 1;//用于qol面板的星三王传颂计数
        #region 面板计数
        public int KSPanelCount= 1;
        public int DSPanelCount= 1;
        public int EoCPanelCount= 1;
        public int CrabPanelCount= 1;
        public int EoWPanelCount= 1;
        public int BoCPanelCount= 1;
        public int HivePanelCount= 1;
        public int PerfPanelCount= 1;
        public int QBPanelCount= 1;
        public int SkelePanelCount= 1;
        public int SGPanelCount= 1;
        public int WoFPanelCount= 1;
        public int CryoPanelCount= 1;
        public int TwinsPanelCount= 1;
        public int BrimmyPanelCount= 1;
        public int DestroyerPanelCount= 1;
        public int ASPanelCount= 1;
        public int PrimePanelCount= 1;
        public int CalClonePanelCount= 1;
        public int PlantPanelCount= 1;
        public int AureusPanelCount= 1;
        public int LAPanelCount= 1;
        public int GolemPanelCount= 1;
        public int PBGPanelCount= 1;
        public int DukePanelCount= 1;
        public int RavagerPanelCount= 1;
        public int CultistPanelCount= 1;
        public int DeusPanelCount= 1;
        public int MLPanelCount= 1;
        public int ProviPanelCount= 1;
        public int PolterPanelCount= 1;
        public int ODPanelCount= 1;
        public int DoGPanelCount= 1;
        public int YharonPanelCount= 1;
        public int ExoPanelCount= 1;
        public int SCalPanelCount= 1;
        #endregion
        public bool wasMouseDown = false;//用于qol面板的鼠标状态跟踪

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
            return;
        }
    }
}