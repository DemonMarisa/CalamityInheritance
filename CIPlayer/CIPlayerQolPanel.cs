using CalamityInheritance.Utilities;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public bool PanelsLoreKingSlime = false;//
        public bool PanelsLoreDesertScourge = false;//
        public bool PanelsLoreCrabulon = false;//
        public bool PanelsLoreEaterofWorld = false;//
        public bool PanelsBoCLoreTeleportation = false;//
        public bool PanelsLoreHive = false;//
        public bool PanelsLorePerforator = false;//
        public bool PanelsLoreQueenBee = false;//
        public bool PanelsLoreSkeletron = false;//
        public bool PanelsLoreWallofFlesh = false;//
        public bool PanelsLoreTwins = false;//
        public bool PanelsLoreDestroyer = false;//
        public bool PanelsLoreAquaticScourge = false;//
        public bool PanelsLorePrime = false;//
        public bool PanelsLoreBrimstoneElement = false;//
        public bool PanelsLoreCalamitasClone = false;//
        public bool PanelsLorePlantera = false;//
        public bool PanelsLoreLeviAnahita = false;//
        public bool PanelsLoreAureus = false;//
        public bool PanelsLoreDeus = false;//
        public bool PanelsLoreGolem = false;//
        public bool PanelsLoreGoliath = false;//
        public bool PanelsLoreDuke = false;//
        public bool PanelsLoreDukeElder = false;//
        public bool PanelsLoreRavager = false;//
        public bool PanelsLoreCultist = false;//
        public bool PanelsLoreLunarBoss = false;//
        public bool PanelsLoreProvidence = false;//
        public bool PanelsLorePolter = false;//
        public bool PanelsLoreDevourer = false;//
        public bool PanelsLoreJungleDragon = false;//
        public bool PanelsSCalLore = false;//
        public bool PanelsLoreSea = false;
        public bool PanelsLoreCorruption = false;//
        public bool PanelsLoreCrimson = false;//
        public bool PanelsLoreUnderworld = false;
        public bool PanelsLoreExo = false;//星三王传颂

        public void Panels()
        {
            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer cIPlayer = player.CIMod();

            bool DownedExo = DownedBossSystem.downedExoMechs;// 判定是否能使用星三王面板
            #region lore
            // 确保可以正确切换状态，具体数字对应的绘制贴图请查看方法中的注释
            if (cIPlayer.panelloreExocount == 3 || cIPlayer.panelloreExocount == 4)
                cIPlayer.PanelsLoreExo = true;
            if (cIPlayer.panelloreExocount == 1 || cIPlayer.panelloreExocount == 2)
                cIPlayer.PanelsLoreExo = false;
            #endregion
        }
        public override void SaveData(TagCompound tag)
        {
            tag["panelloreExocount"] = panelloreExocount;
            tag["PanelsLoreExo"] = PanelsLoreExo;
        }

        public override void LoadData(TagCompound tag)
        {
            panelloreExocount = tag.GetInt("panelloreExocount");
            PanelsLoreExo = tag.GetBool("PanelsLoreExo");
            int[] TotalPanelCounts=
            [
                KSPanelCount,
                DSPanelCount,
                EoCPanelCount,
                CrabPanelCount,
                EoWPanelCount,
                BoCPanelCount,
                HivePanelCount,
                PerfPanelCount,
                QBPanelCount,
                SkelePanelCount,
                SGPanelCount,
                WoFPanelCount,
                CryoPanelCount,  
            ];
        }
    }
}
