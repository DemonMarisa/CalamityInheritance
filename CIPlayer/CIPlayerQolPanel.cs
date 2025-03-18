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
        public bool PanelsLoreEoC = false;//
        public bool PanelsLoreEaterofWorld = false;//
        public bool PanelsBoCLoreTeleportation = false;//
        public bool PanelsLoreHive = false;//
        public bool PanelsLorePerforator = false;//
        public bool PanelsLoreQueenBee = false;//
        public bool PanelsLoreSkeletron = false;//
        public bool PanelsLoreSG = false;//
        public bool PanelsLoreWallofFlesh = false;//
        public bool PanelsCryo = false;//
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

        //试着写成了数组的遍历形式。
        public void Panels()
        {
            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer cIPlayer = player.CIMod();
            #region lore
            for (int i = 0; i < PanelBoolInit().Length; i++)
            {
                switch (PanelCountInit()[i])
                {
                    case 1:
                    case 2:
                         PanelBoolInit()[i]= false;
                        break;
                    case 3:
                    case 4:
                        PanelBoolInit()[i] = true;
                        break;
                }
            }

            // 确保可以正确切换状态，具体数字对应的绘制贴图请查看方法中的注释
            if (cIPlayer.panelloreExocount == 3 || cIPlayer.panelloreExocount == 4)
                cIPlayer.PanelsLoreExo = true;
            if (cIPlayer.panelloreExocount == 1 || cIPlayer.panelloreExocount == 2)
                cIPlayer.PanelsLoreExo = false;
            #endregion
        }
        public int[] PanelCountInit()
        {
            int[] TotalPanelCounts =
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
                TwinsPanelCount,
                BrimmyPanelCount,
                DestroyerPanelCount,
                ASPanelCount,
                PrimePanelCount,
                CalClonePanelCount,
                PlantPanelCount,
                AureusPanelCount,
                LAPanelCount,
                GolemPanelCount,
                PBGPanelCount,
                DukePanelCount,
                RavagerPanelCount,
                CultistPanelCount,
                DeusPanelCount,
                MLPanelCount,
                ProviPanelCount,
                PolterPanelCount,
                ODPanelCount,
                DoGPanelCount,
                YharonPanelCount,
                ExoPanelCount,
                SCalPanelCount,
            ];
            return TotalPanelCounts;
        }
        public bool[] PanelBoolInit()
        {
            bool[] TotalPanelsBool =
            [
                PanelsLoreKingSlime,
                PanelsLoreDesertScourge,
                PanelsLoreEoC,
                PanelsLoreEaterofWorld,
                BoCLoreTeleportation,
                PanelsLoreHive,
                PanelsLorePerforator,
                PanelsLoreQueenBee,
                PanelsLoreSkeletron,
                PanelsLoreSG,
                PanelsLoreWallofFlesh,
                PanelsCryo,
                PanelsLoreTwins,
                PanelsLoreBrimstoneElement,
                PanelsLoreDestroyer,
                PanelsLoreAquaticScourge,
                PanelsLorePrime,
                PanelsLoreCalamitasClone,
                PanelsLorePlantera,
                PanelsLoreAureus,
                PanelsLoreLeviAnahita,
                PanelsLoreGolem,
                PanelsLoreGoliath,
                PanelsLoreDuke,
                PanelsLoreCultist,
                PanelsLoreRavager,
                PanelsLoreCultist,
                PanelsLoreLunarBoss,
                PanelsLoreProvidence,
                PanelsLorePolter,
                PanelsLoreDukeElder,
                PanelsLoreDevourer,
                PanelsLoreJungleDragon,
                PanelsLoreExo,
                PanelsSCalLore,
                PanelsLoreSea,
                PanelsLoreCorruption,
                PanelsLoreCrimson,
                PanelsLoreUnderworld,
            ];
            return TotalPanelsBool;
        }
        public override void SaveData(TagCompound tag)
        {

            for (int i = 0; i < PanelCountInit().Length; i ++)
            {
                string newString = PanelCountInit()[i].ToString();
                if (tag.ContainsKey(newString))
                    tag[newString] = PanelCountInit()[i];
            }
            for (int j = 0; j < PanelBoolInit().Length; j++)
            {
                string newString = PanelBoolInit()[j].ToString();
                if (tag.ContainsKey(newString))
                    tag[newString] = PanelBoolInit()[j];
            }
            // tag["panelloreExocount"] = panelloreExocount;
            // tag["PanelsLoreExo"] = PanelsLoreExo;
        }

        public override void LoadData(TagCompound tag)
        {
            panelloreExocount = tag.GetInt("panelloreExocount");
            PanelsLoreExo = tag.GetBool("PanelsLoreExo");
            for (int i = 0; i < PanelCountInit().Length; i++)
            {
                string newString = PanelCountInit()[i].ToString();
                //加一个，虽然一般情况下应该不会，但加一个安全写法在这一般也不会怎么样
                if (tag.ContainsKey(newString))
                    PanelCountInit()[i] = tag.GetInt(newString);
            }
            for (int j = 0; j < PanelBoolInit().Length; j++)
            {
                string newString = PanelBoolInit().ToString();
                if (tag.ContainsKey(newString))
                    PanelBoolInit()[j] = tag.GetBool(newString);
            }
        }
    }
}
