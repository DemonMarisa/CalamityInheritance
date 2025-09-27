using CalamityInheritance.CICooldowns;
using CalamityInheritance.Content.Items.Armor.AncientAstral;
using CalamityMod;
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
        // 通用开火冷却
        public int fireCD = 0;
        public int GlobalSoundDelay = 0;
        //通用计时器
        public int GlobalFireDelay = 0;
        //通用……任意计数器？
        public int BrimstoneDartsCD = 0;
        public int GlobalMiscCounter = 1;
        public int CotbgCounter = 0;
        public int GlobalHealProjCD = 0;
        // 弑神套的CD差分
        public int GlobalGodSlayerHealProjCD = 0;
        //归元漩涡
        public int BuffSubsumingVortexFireRate = 0;
        // 环境的计时器
        public int maliceModeBlizzardTime = 0;
        public int maliceModeUnderworldTime = 0;
        //远古弑神闪避计时
        public int AncinetGodSlayerDodgeCount = 3;
        
        public int AncientAuricDashCache = 0;
        public int AncientGodSlayerBuffCounter = 0;
        public int AncientGodSlayerBuffCD = 0;
        #region 林海复活
        // 用于计时
        public int SilvaRebornTimer = 0;
        public int R99Shooting = 0;
        public int R99TargetWhoAmI = -1;
        #endregion
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
            if (AncientAuricDashCache > 0)
                AncientAuricDashCache--;

            if (AncientGodSlayerBuffCounter > 0)
                AncientGodSlayerBuffCounter--;

            if (AncientGodSlayerBuffCD > 0)
                AncientGodSlayerBuffCD--;

            if (summonProjCooldown > 0f)
                summonProjCooldown -= 1;

            if (SilvaMagicSetLegacyCooldown > 0)
                SilvaMagicSetLegacyCooldown--;

            if (SilvaStunDebuffCooldown > 0)
                SilvaStunDebuffCooldown--;

            if (ReaverBlastCooldown > 0)
                ReaverBlastCooldown--; //战士永恒套cd

            if (ReaverBurstCooldown > 0)
                ReaverBurstCooldown--; //法师永恒套CD

            if (StepToolShadowChairSmallCD > 0)
                StepToolShadowChairSmallCD--;

            if (AncientAuricHealCooldown > 0)
                AncientAuricHealCooldown--;

            if (PerunofYharimCooldown > 0)
                PerunofYharimCooldown--;

            if (AncientBloodflareHeartDropCD > 0)
                AncientBloodflareHeartDropCD--;

            if (AncientSilvaRegenCD > 0)
                AncientSilvaRegenCD--;

            //生命恢复消失前的CD
            if (AncientAstralStealthGap > 0)
                AncientAstralStealthGap--;
            //暴击CD 
            if (AncientAstralCritsCD > 0) //暴击内置CD
                AncientAstralCritsCD--;

            //星辉套重置暴击达到指定次数时重置 
            if (AncientAstralCritsCount > AncientAstralHelm.RogueCritsTimes)
                AncientAstralCritsCount = 0;
            //星辉套每次潜伏攻击的CD
            if (AncientAstralStealthCD > 0) //每次潜伏攻击之间的CD
                AncientAstralStealthCD--;

            if (statisTimerOld > 0 && CIDashDelay >= 0)
                statisTimerOld = 0;//斯塔提斯CD

            if (Player.miscCounter % 150 == 0)
            {
                ReaverRocketFires = true;
            }
            if (GlobalLegendaryT3CD > 0)
                GlobalLegendaryT3CD--;

            if (DukeDefenseTimer > 0)
                DukeDefenseTimer--;

            if (DukeDefenseCounter > 0 && DukeDefenseTimer == 0)
                DukeDefenseCounter--;

            if (SparkTimer > 0)
                SparkTimer--;

            if (fireCD > 0)
                fireCD--;

            if (AeroFlightPower > 0)
                AeroFlightPower--;

            if (GlobalSoundDelay > 0)
                GlobalSoundDelay--;

            if (GlobalFireDelay > 0)
                GlobalFireDelay--;

            if (BrimstoneDartsCD > 0)
                BrimstoneDartsCD--;

            if (CotbgCounter > 0)
                CotbgCounter--;

            if (DNAImmnue > 0)
                DNAImmnue--;

            if (DNAImmnueActive > 0)
                DNAImmnueActive--;

            if (GlobalHealProjCD > 0)
                GlobalHealProjCD--;

            if (InitNanotechSound > 0)
                InitNanotechSound--;

            if (GlobalGodSlayerHealProjCD > 0)
                GlobalGodSlayerHealProjCD--;

            if (BuffSubsumingVortexFireRate > 0)
                BuffSubsumingVortexFireRate--;

            if (!Player.HasCooldown(GodSlayerCooldown.ID) && AncinetGodSlayerDodgeCount <= 0)
                AncinetGodSlayerDodgeCount = 3;
            return;
        }
    }
}