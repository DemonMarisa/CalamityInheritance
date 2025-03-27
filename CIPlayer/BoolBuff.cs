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
            #endregion
        }
        public void UpdateDeadBuff()
        {
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
        }
    }
}