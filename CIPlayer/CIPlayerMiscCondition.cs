using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.System.DownedBoss;
using CalamityMod.CalPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public bool passBuffSolar = true;
        public bool currentSolarEclipse = false;
        public void CIMiscCondition()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            CalamityInheritancePlayer clPlayer = Player.CIMod();
            if (passBuffSolar)
            {
                if (Main.eclipse && CIDownedBossSystem.DownedLegacyYharonP1)
                    currentSolarEclipse = true;

                if (!Main.eclipse && currentSolarEclipse && passBuffSolar)
                    CIDownedBossSystem.DownedBuffedSolarEclipse = true;
            }
        }
    }
}
