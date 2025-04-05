using CalamityMod;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region Set Bonuses
        #region AncientAeroArmor
        public bool AncientAeroSet = false;
        public bool DisableAeroWings = false;
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
        public bool AncientSilvaSet = false;
        public bool AncientSilvaStat = false; //林海数值
        public int AncientSilvaRegenCD = 0;
        public int AncientSilvaRegenTimer = 120; //一秒
        public bool AncientSilvaRegenFlag = false;
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
        public bool SilvaStunDebuff = false;
        public int SilvaStunDebuffCooldown = 0;
        public bool SilvaRangedSetLegacy = false;
        public bool SilvaSummonSetLegacy = false;
        public bool SilvaRougeSetLegacy = false;
        public bool SilvaRebornMark = false;
        #endregion
        #region Auric
        public bool AuricDebuffImmune = false;
        public bool AuricbloodflareRangedSoul = false;
        public bool auricBoostold = false;
        public bool AuricSilvaSet = false;
        public bool AncientAuricSet = false; //暴君套
        public int AncientAuricHealCooldown = 0; //暴君套回血CD
        public bool auricYharimAntiSummonerDMGReduction = false; //暴君套直接数值对撞抗召唤减伤
        public int PerunofYharimCooldown = 0; //暴君套打击cd
        public bool PerunofYharimStats= false;
        public bool AuricGetSilvaEffect = false;
        public static int CIsilvaReviveDuration = 900;
        public int CIsilvaCountdown = CIsilvaReviveDuration;
        public static int AuricSilvaInvincibleTime = 600;
        public int auricsilvaCountdown = AuricSilvaInvincibleTime;
        #endregion
        #region Reaver
        //永恒套
        public bool ReaverRogueExProj = false;
        //盗贼永恒套的套装奖励
        public bool ReaverMeleeBlast = false;
        //战士永恒套的套装奖励
        public int ReaverBlastCooldown = 0;
        //战士永恒套爆炸CD
        public bool ReaverMeleeRage = false;
        //战士永恒套怒气
        public bool ReaverSummonerOrb = false;
        public bool ReaverSummoner = false;
        //召唤永恒套的套装奖励
        public bool ReaverMageBurst = false;
        //法师永恒套的套装奖励
        public int ReaverBurstCooldown = 0;
        // 法师永恒套内置的弹幕CD
        public bool ReaverMagePower = false;
        //法师永恒套追加的一个击发式buff
        public bool ReaverRangedRocket = false;
        //射手永恒套的套装奖励
        public bool ReaverRocketFires = false;
        //石巨人，我杀你妈妈 
        public bool FuckYouGolem =false;
        #endregion
        public bool test = false;
        #region AncientXeroc
        public bool AncientXerocSet     = false;
        public bool AncientXerocWrath   = false;
        //克希洛克翅膀的远古狂怒效果
        public bool AncientXerocMad = false;
        //克希洛克套装的远古暴乱效果
        public bool AncientXerocShame = false;
        //克希洛克残念
        
        //xeroc套装 
        #endregion
        #region 远古星辉套(Revamped)
        public bool AncientAstralSet = false; //是否为远古星辉
        public bool AncientAstralStatBuff = false; //是否正在启用星之铸造
        public int AncientAstralCritsCount = 0; //星辉的暴击次
        public int RequireCrits = 20;//星辉套触发暴击效果需要的攻击次数
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
            AncientSilvaSet = false;
            AncientSilvaRegenFlag = false;
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
            SilvaRebornMark = false;
            #endregion
            #region Auric
            AuricDebuffImmune = false;
            AuricbloodflareRangedSoul = false;
            auricBoostold = false;
            AuricSilvaSet = false;
            AncientAuricSet = false;
            PerunofYharimStats = false;
            #endregion
            #region Reaver
            ReaverMeleeBlast = false;
            ReaverRangedRocket = false;
            ReaverMageBurst = false;
            ReaverMeleeRage = false;
            ReaverMagePower = false;
            ReaverSummoner = false;
            FuckYouGolem = false;
            #endregion
            #region Xeroc
            AncientXerocSet     = false;
            AncientXerocWrath   = false;
            AncientXerocMad = false;
            AncientXerocShame   = false;
            #endregion

            AncientAstralSet = false;
            AncientAstralStatBuff = false;
            test = false;
            #endregion
           
        }
        public void UpdateDeadArmorSet()
        {
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
            AncientSilvaSet = false;
            AncientSilvaStat = false;
            AncientSilvaRegenCD = 0;
            AncientSilvaRegenTimer = 0;
            AncientSilvaRegenFlag = false;
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
            #endregion
            #region Auric
            AuricDebuffImmune = false;
            AuricbloodflareRangedSoul = false;
            AuricGetSilvaEffect = false;
            auricsilvaCountdown = AuricSilvaInvincibleTime;
            CIsilvaCountdown = CIsilvaReviveDuration;
            auricBoostold = false;
            AncientAuricSet = false;
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
            AncientAstralStatBuff = false; //buff
            AncientAstralCritsCount = 0;
            AncientAstralCritsCD = 0;
            AncientAstralStealth = 0;
            AncientAstralStealthCD = 0;
            #endregion
        }
    }
}