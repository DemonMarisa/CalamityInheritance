using CalamityInheritance.Buffs.Summon;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Typeless;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region 饰品相关
        //cnm红月。
        public bool FUCKYOUREDMOON = false;
        public bool IfGodHand = false;
        public bool IfCalamitasSigile = false;
        public bool EHeartStats = false;
        public bool EHeartStatsBoost = false;
        public bool ElemQuiver = false;
        public bool CoreOfTheBloodGod = false;
        public bool FungalCarapace = false;
        public bool PsychoticAmulet = false;
        public bool YharimsInsignia = false;
        public bool DarkSunRings = false;
        public bool projRef = false;
        public bool AstralBulwark = false;
        public bool AstralArcanumEffect = false;
        public bool BraveBadge = false; //龙蒿套装下的勇气勋章额外加成
        public bool AuricTracersFrames = false; //天界跑鞋无敌帧
        public bool deificAmuletEffect = false;  //神圣护符的效果
        public bool RoDPaladianShieldActive = false; //神之壁垒的帕拉丁盾效果
        public bool DeadshotBroochCI = false; //独立出来的神射手徽章加成
        public int statisTimerOld = 0;//虚空饰带的计数器
        public bool nanotechold = false;//发射纳米技术的额外弹幕
        public bool TheAbsorberOld = false;//阴阳石受击回血
        public bool FuckYouBees = false;//降低蜜蜂对玩家的伤害
        public bool AmbrosialAmpouleOld = false;//百草瓶回血
        public int RaiderStacks = 0;//纳米技术击中计数器
        public int nanoTechStackDurability = 0;//纳米技术充能进度
        public bool SpeedrunNecklace = false;//速杀项链
        public bool AncientCotbg = false; //肃杀核心
        public bool AncientBloodPact = false;//血契
        public bool ElemGauntlet = false;//元素之握
        public bool FuckEHeart = false;
        public bool NucleogenesisLegacy = false;//核子之源
        public bool AnkhImmnue = false; //占位符
        public bool AsgardsValorImmnue = false; //阿斯加德英勇单独免疫的debuff
        public bool ElysianAegisImmnue = false; //亵渎盾单独免疫的debuff
        public bool AmbrosialImmnue= false; //百草瓶的免疫
        public bool AmbrosialStats = false; //百草瓶的一些数据
        public bool DraedonsHeartLegacyStats = false;
        public bool WearingStatisCurse = false;
        public bool anyShield = false; //是否有任何护盾
        //两个肥鸡
        public bool GodlySons = false;
        public bool EmpressBooster = false;
        public bool SForest = false;
        public int SForestBuffTimer = 0;
        public bool SMarble = false;
        public bool SMarbleSword = false;
        public bool SMarnite = false;
        public bool SMushroom = false;
        public bool AeroStonePower = false;
        public bool IsWearingBloodyScarf = false;
        public bool IsWearingElemQuiverCal = false;
        public bool OverloadManaPower = false;
        //日食魔镜
        public bool EMirror = false;
        #endregion
        /// <summary>
        /// 这个是在MaxLife后边的
        /// </summary>
        public void ResetAccessories()
        {
            #region Accessories
            FUCKYOUREDMOON = false;
            BuffExoApolste = false;
            IfCloneHtting = false; //克隆大锤子是否正在攻击
            AnkhImmnue = false;
            ElemQuiver = false;
            CoreOfTheBloodGod = false;
            IfCalamitasSigile = false;
            SMushroom = false;
            SMarble = false;
            SMarbleSword = false;
            SMarnite = false;
            SForest = false;
            if (!CIsponge)
                CISpongeShieldDurability = 0;

            if (!CIsponge)
                ShieldDurabilityMax = 0;
            BraveBadge = false;
            CIsponge = false;
            CIspongeShieldVisible = false;
            FungalCarapace = false;
            IfGodHand = false;
            PsychoticAmulet = false;
            YharimsInsignia = false;
            DarkSunRings = false;
            AuricTracersFrames = false; //天界跑鞋无敌帧
            deificAmuletEffect = false; //神圣护符的效果
            RoDPaladianShieldActive = false; //神之壁垒的帕拉丁盾
            projRef = false;
            AstralBulwark = false;
            AstralArcanumEffect = false;
            BraveBadge = false;
            DeadshotBroochCI = false; //独立出来的神射手徽章加成
            nanotechold = false;
            TheAbsorberOld = false;//阴阳石受击回血
            FuckYouBees = false;//降低蜜蜂对玩家的伤害
            AmbrosialAmpouleOld = false;//百草瓶回血
            SpeedrunNecklace= false;//肃杀项链
            AncientCotbg = false ;//肃杀核心
            AncientBloodPact = false;//血契
            ElemGauntlet = false;//元素之握
            BloodflareCoreStat = false;
            EHeartStats = false;
            EHeartStatsBoost = false;
            FuckEHeart = false;
            NucleogenesisLegacy = false;//核子
            WearingStatisCurse = false;
            
            AsgardsValorImmnue = false;
            ElysianAegisImmnue = false;
            AmbrosialImmnue = false;
            AmbrosialStats = false;
            DraedonsHeartLegacyStats = false;
            EmpressBooster = false;
            AeroStonePower = false; 
            IsWearingBloodyScarf = false;
            IsWearingElemQuiverCal = false;
            anyShield = false; //是否有任何护盾
            EMirror = false;
            OverloadManaPower = false;
            #endregion

        }
        public void UpdateDeadAccessories()
        {
            AnkhImmnue = false;
            FUCKYOUREDMOON = false;
            SolarShieldEndurence = false;
            SMarbleSword = false;
            ElysianAegis = false;
            ElysianGuard = false;
            statisTimerOld = 0;//虚空饰带的计数器
            SMushroom = false;
            SMarble = false;
            SMarnite = false;
            SForest = false;
            SForestBuffTimer = 0;
            TheAbsorberOld = false;//阴阳石受击回血
            FuckYouBees = false;//降低蜜蜂对玩家的伤害
            AmbrosialAmpouleOld = false;//百草瓶回血
            RaiderStacks = 0;//纳米技术击中计数器
            nanoTechStackDurability = 0;//纳米技术充能进度
            SpeedrunNecklace = false; //肃杀项链
            AncientCotbg = false; //肃杀核心
            AncientBloodPact = false;
            PerunofYharimStats = false;
            BloodflareCoreStat = false;
            EHeartStats = false;
            EHeartStatsBoost = false;
            StepToolShadowChairSmallCD = 0;
            AsgardsValorImmnue = false;
            ElysianAegisImmnue = false;
            EmpressBooster = false;
            AeroStonePower = false;
            IsWearingBloodyScarf = false;
            IsWearingElemQuiverCal = false;
            anyShield = false;
            OverloadManaPower = false;
        }
    }
}