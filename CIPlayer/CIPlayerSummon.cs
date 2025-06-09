using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region Summon
        public bool MagicHatOld = false;
        public bool MidnnightSunBuff = false;
        public bool cosmicEnergy = false;
        public bool IsAncientClasper = false;
        public bool bloodClot = false;
        public bool CosmicEnergyExtra = false;
        public bool wulfrumDroidOld = false;
        public bool siriusLegacy = false;
        public bool sarosPossessionLegacy = false;
        #endregion
        public void ReSetSummon()
        {
            #region Summon
            MagicHatOld = false;
            MidnnightSunBuff = false;
            ReaverSummonerOrb = false;
            cosmicEnergy = false;
            IsAncientClasper = false;
            bloodClot = false;
            CosmicEnergyExtra = false;
            wulfrumDroidOld = false;
            siriusLegacy = false;
            sarosPossessionLegacy = false;
            #endregion
        }
    } 
}
