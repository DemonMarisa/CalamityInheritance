using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region Buffs
        public bool BuffStatsArmorShatter = false;
        public bool Revivify = false;
        public bool BuffStatsCadence = false;
        public bool BuffStatsDraconicSurge = false;
        public bool BuffStatsPenumbra = false;
        public bool BuffStatsProfanedRage = false;
        public bool BuffStatsHolyWrath = false;
        public bool BuffStatsTitanScale = false;
        public int BuffStatsTitanScaleTrueMelee = 0;
        public bool Triumph = false;
        public bool BuffStatsYharimsStin = false;
        public bool InvincibleJam = false;
        public bool BuffStatBloodPact = false;
        public bool BloodflareCoreStat = false;//旧血炎

        public bool BuffStatsBackfire = false;   //淬火
        public int StepToolShadowChairSmallCD = 0;
        public int StepToolShadowChairSmallFireCD = 0;
        public float ManaHealMutipler = 1f; //增强魔力药水恢复量
        public bool PolarisPhase2 = false;
        public bool PolarisPhase3 = false;
        public int PolarisBoostCounter = 0;

        //丛林龙仆从
        public bool OwnSonYharon = false;
        //气功念珠
        public bool SForestBuff = false;
        //海爵剑Buff
        public bool BrinyBuff = false;
        //庇护刃buff
        public bool DefenderPower = false;

        public bool PBGPower = false;
        //火山杖buff
        public bool BetsyPower = false;
        #endregion
        #region Debuff
        public bool abyssalFlames = false;
        public bool horror = false;
        public bool vulnerabilityHexLegacy = false;
        #endregion
        public void ResetBuff()
        {
            #region Buffs
            Revivify = false;
            BuffStatsArmorShatter = false;
            BuffStatsCadence = false;
            BuffStatsDraconicSurge = false;
            BuffStatsPenumbra = false;
            BuffStatsProfanedRage = false;
            BuffStatsHolyWrath = false;
            BuffStatsYharimsStin = false;
            BuffStatsBackfire = false;
            BuffStatsTitanScale = false;
            Triumph = false;
            InvincibleJam = false;
            BuffStatBloodPact = false;
            BuffPolarisBoost = false;
            PolarisPhase2 = false;
            PolarisPhase3 = false;
            OwnSonYharon = false;
            SForestBuff = false;
            BrinyBuff = false;
            DefenderPower = false;
            PBGPower = false;
            BetsyPower = false;
            #endregion
            #region Debuff
            abyssalFlames = false;
            horror = false;
            vulnerabilityHexLegacy = false;
            #endregion
        }
        public void UpdateDeadBuff()
        {
            #region Buff
            BrinyBuff = false;
            BuffStatsArmorShatter = false;
            Revivify = false;
            BuffStatsCadence = false;
            BuffStatsDraconicSurge = false;
            BuffStatsPenumbra = false;
            BuffStatsProfanedRage = false;
            BuffStatsHolyWrath = false;
            BuffStatsTitanScale = false;
            BuffStatsTitanScaleTrueMelee = 0;
            Triumph = false;
            BuffStatsYharimsStin = false;
            InvincibleJam = false;
            BuffPolarisBoost = false;
            PolarisPhase2 = false;
            PolarisPhase3 = false;
            PolarisBoostCounter = 0;
            SForestBuff = false;
            DefenderPower = false;
            PBGPower = false;
            #endregion
            #region Debuff
            abyssalFlames = false;
            horror = false;
            vulnerabilityHexLegacy = false;
            #endregion
        }
        public void DebuffEffect()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            if (horror)
            {
                Player.statDefense -= 15;
                Player.blind = true;
                Player.moveSpeed -= 0.15f;
                Player.accRunSpeed -= 0.15f;
            }
            if (vulnerabilityHexLegacy)
            {
                Player.endurance -= 0.3f;
                Player.statDefense -= 30;
                Player.moveSpeed -= 0.10f;
                calPlayer.weakPetrification = true;
                if (Player.wingTimeMax > 0)
                    Player.wingTimeMax = (int)(Player.wingTimeMax * 0.5);
            }
        }
    }
}