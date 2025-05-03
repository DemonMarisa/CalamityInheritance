using CalamityMod;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region Set Bonuses
        //蘑菇喷火头
        public bool ShroomiteFlameBooster = false;
        #region AncientAeroArmor
        public bool AncientAeroSet = false;
        public bool AncientAeroWingsPower = false;
        #endregion
        #region  AncientBloodflare
        public bool AncientBloodflareSet = false; //远古血炎套
        public bool AncientBloodflareStat = false; //血炎数值
        public int AncientBloodflareHeartDropCD = 0; //产红心CD
        #endregion
        #region  AncientGodSlayer
        public bool AncientGodSlayerSet = false;
        public bool AncientGodSlayerStat = false; //弑神数值
        #endregion
        #region AncientSilva
        public bool AncientSilvaForceRegen = false;
        public bool AncientSilvaStat = false; //林海数值
        public int AncientSilvaRegenCD = 0;
        public int AncientSilvaRegenTimer = 120; //一秒
        #endregion
        #region AncientTarragon
        public bool AncientTarragonSet = false;
        #endregion
        #region GodSlayer
        public bool GodSlayerMelee = false;
        public bool GodSlayerReborn = false;
        public bool GodSlayerDMGprotect = false;
        public bool GodSlayerReflect = false;
        public bool GodSlayerMagicSet = false;
        public bool GodSlayerRangedSet = false;
        public bool GodSlayerSummonSet = false;
        public bool GodSlayerRogueSet = false;
        #endregion
        #region Silva
        public bool SilvaMagicSetLegacy = false;
        public int SilvaMagicSetLegacyCooldown = 0;
        public bool SilvaMeleeSetLegacy = false;
        public int SilvaStunDebuffCooldown = 0;
        public bool SilvaRangedSetLegacy = false;
        public bool SilvaSummonSetLegacy = false;
        public bool SilvaRougeSetLegacy = false;
        public bool SilvaFakeDeath = false;
        #endregion
        #region Auric
        public bool AuricDebuffImmune = false;
        public bool AuricbloodflareRangedSoul = false;
        public bool auricBoostold = false;
        public bool AuricSilvaFakeDeath = false;
        public bool AncientAuricSet = false; //暴君套
        public int AncientAuricHealCooldown = 0; //暴君套回血CD
        public int PerunofYharimCooldown = 0; //暴君套打击cd
        public bool PerunofYharimStats= false;
        public bool IsUsedSilvaReborn = false;
        public const int SilvaRebornDura = 900;
        public int DoSilvaCountDown = SilvaRebornDura;
        public const int AuricSilvaRebornDura = 600;
        public int DoAuricSilvaCountdown = AuricSilvaRebornDura;
        #endregion
        #region Reaver
        //永恒套
        public bool ReaverRogueExProj = false;
        //盗贼永恒套的套装奖励
        public bool ReaverMeleeBlast = false;
        //战士永恒套的套装奖励
        public int ReaverBlastCooldown = 0;
        //战士永恒套爆炸CD
        //战士永恒套怒气
        public bool ReaverSummonerOrb = false;
        //召唤永恒套的套装奖励
        public bool ReaverSummoner = false;
        //法师永恒套的套装奖励
        public bool ReaverMageBurst = false;
        // 法师永恒套内置的弹幕CD
        public int ReaverBurstCooldown = 0;
        //射手永恒套的套装奖励
        public bool ReaverRangedRocket = false;
        public bool ReaverRocketFires = false;
        //石巨人，我杀你妈妈 
        public bool FuckYouGolem =false;
        #endregion
        public bool Test = false;
        #region AncientXeroc
        public bool AncientXerocSet     = false;
        //克希洛克翅膀的远古狂怒效果
        public bool AncientXerocWrath   = false;
        
        //xeroc套装 
        #endregion
        #region 远古星辉套(Revamped)
        public bool AncientAstralSet = false; //是否为远古星辉
        public int AncientAstralCritsCount = 0; //星辉的暴击次
        const int RequireCrits = 20;//星辉套触发暴击效果需要的攻击次数
        public int AncientAstralCritsCD = 0;//星辉每次暴击的间隔
        public int AncientAstralStealthCD = 0; //星辉每次潜伏的间隔
        public int AncientAstralStealth = 0; //星辉潜伏次数
        public int AncientAstralStealthGap = 0; //星辉套生命恢复效果消失的需求CD 
        #endregion
        #region
        public bool YharimAuricSet = false;
        #endregion
        #endregion
        public void ResetArmorSet()
        {
            ShroomiteFlameBooster = false;
            #region Set Bonuses
            #region AncientBloodflare
            AncientBloodflareSet = false;
            AncientBloodflareStat = false;
            #endregion
            #region AncientGodSlayer
            AncientGodSlayerSet = false;
            AncientGodSlayerStat = false;
            #endregion
            #region AncientSilva
            AncientSilvaForceRegen = false;
            AncientSilvaStat = false;
            #endregion
            #region AncientTarragon
            AncientTarragonSet = false;
            #endregion
            #region GodSlayer
            GodSlayerMelee = false;
            GodSlayerReborn = false;
            GodSlayerDMGprotect = false;
            GodSlayerReflect = false;
            GodSlayerMagicSet = false;
            GodSlayerRangedSet = false;
            GodSlayerSummonSet = false;
            GodSlayerRogueSet = false;
            #endregion
            #region Sliva
            SilvaMagicSetLegacy = false;
            SilvaMeleeSetLegacy = false;
            SilvaRangedSetLegacy = false;
            SilvaSummonSetLegacy = false;
            SilvaRougeSetLegacy = false;
            SilvaFakeDeath = false;
            #endregion
            #region Auric
            AuricDebuffImmune = false;
            AuricbloodflareRangedSoul = false;
            auricBoostold = false;
            AuricSilvaFakeDeath = false;
            AncientAuricSet = false;
            PerunofYharimStats = false;
            #endregion
            #region Reaver
            ReaverMeleeBlast = false;
            ReaverRangedRocket = false;
            ReaverMageBurst = false;
            ReaverSummoner = false;
            FuckYouGolem = false;
            #endregion
            #region Xeroc
            AncientXerocSet     = false;
            AncientXerocWrath   = false;
            #endregion

            AncientAstralSet = false;
            AncientAeroSet = false;
            AncientAeroWingsPower = false;
            Test = false;
            #endregion
           
        }
        public void UpdateDeadArmorSet()
        {
            ShroomiteFlameBooster = false;
            #region Set Bonuses
            #region AncientBloodflare
            AncientBloodflareSet = false;
            AncientBloodflareHeartDropCD = 0;
            AncientBloodflareStat = false;
            #endregion
            #region AncientGodSlayer
            AncientGodSlayerSet = false;
            AncientGodSlayerStat = false;
            #endregion
            #region AncientSilva
            AncientSilvaForceRegen = false;
            AncientSilvaStat = false;
            AncientSilvaRegenCD = 0;
            AncientSilvaRegenTimer = 0;
            #endregion
            #region AncientTarragon
            AncientTarragonSet = false;
            #endregion
            #region GodSlayer
            GodSlayerDMGprotect = false;
            GodSlayerMelee = false;
            GodSlayerReflect = false;
            GodSlayerMagicSet = false;
            GodSlayerRangedSet = false;
            GodSlayerSummonSet = false;
            GodSlayerRogueSet = false;
            #endregion
            #region Sliva
            SilvaMagicSetLegacy = false;
            SilvaMeleeSetLegacy = false;
            SilvaRangedSetLegacy = false;
            SilvaSummonSetLegacy = false;
            SilvaRougeSetLegacy = false;
            SilvaFakeDeath = false;
            #endregion
            #region Auric
            AuricDebuffImmune = false;
            AuricbloodflareRangedSoul = false;
            IsUsedSilvaReborn = false;
            DoAuricSilvaCountdown = AuricSilvaRebornDura;
            DoSilvaCountDown = SilvaRebornDura;
            auricBoostold = false;
            AncientAuricSet = false;
            AuricSilvaFakeDeath = false;
            AncientAuricHealCooldown = 0;
            PerunofYharimCooldown = 0;
            PerunofYharimStats = false;
            #endregion
            #region Reaver
            ReaverMeleeBlast = false;
            ReaverBlastCooldown = 0;
            ReaverMageBurst = false;
            ReaverBurstCooldown = 0;
            ReaverRangedRocket = false;
            ReaverSummoner = false;
            #endregion
            #region Xeroc
            AncientXerocSet     = false;
            AncientXerocWrath   = false;
            #endregion
            AncientAstralSet = false;
            AncientAstralCritsCount = 0;
            AncientAstralCritsCD = 0;
            AncientAstralStealth = 0;
            AncientAstralStealthCD = 0;
            #endregion
            AncientAeroSet = false;
            AncientAeroWingsPower = false;
        }
    }
}