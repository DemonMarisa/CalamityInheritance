using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Magic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.CIPlayer
{
    //TODO： 将下方所有的打表全部改成数组统一管理
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        // 额外标记的用于激活的状态
        #region lore
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
        public bool PanelsLoreCryoDash = false;
        public bool PanelsLoreExo = false;//星三王传颂

        // 禁止生成物品
        public bool PanelsLoreSea = false;
        public bool PanelsLoreCorruption = false;//
        public bool PanelsLoreCrimson = false;//
        public bool PanelsLoreUnderworld = false;
        #endregion
        #region LoreCount
        // 1-4，分别标记四种状态，1为默认贴图（false），2为鼠标悬停的贴图（false），3为点击后的贴图（true）, 4为点击后悬停的贴图（true）
        public int nullType = 1;

        public int KSPanelType = 1;
        public int DSPanelType = 1;
        public int EoCPanelType = 1;
        public int CrabPanelType = 1;
        public int EoWPanelType = 1;
        public int BoCPanelType = 1;
        public int HivePanelType = 1;
        public int PerfPanelType = 1;
        public int QBPanelType = 1;
        public int SkelePanelType = 1;
        public int SGPanelType = 1;
        public int WoFPanelType = 1;
        public int CryoPanelType = 1;
        public int TwinsPanelType = 1;
        public int BrimmyPanelType = 1;
        public int DestroyerPanelType = 1;
        public int ASPanelType = 1;
        public int PrimePanelType = 1;
        public int CalClonePanelType = 1;
        public int PlantPanelType = 1;
        public int AureusPanelType = 1;
        public int LAPanelType = 1;
        public int GolemPanelType = 1;
        public int PBGPanelType = 1;
        public int RavagerPanelType = 1;
        public int DukePanelType = 1;
        public int CultistPanelType = 1;
        public int DeusPanelType = 1;
        public int MLPanelType = 1;
        public int ProviPanelType = 1;
        public int PolterPanelType = 1;
        public int ODPanelType = 1;
        public int DoGPanelType = 1;
        public int YharonPanelType = 1;
        public int ExoPanelType = 1;
        public int SCalPanelType = 1;

        public int SulphurSeaType = 1;
        public int SeaPanelType = 1;
        public int CorruptionPanelType = 1;
        public int CrimsonPanelType = 1;
        public int UnderworldPanelType = 1;
        #endregion
        public void Panels()
        {
            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer cIPlayer = player.CIMod();
            #region lore
            // 确保可以正确切换状态，具体数字对应的绘制贴图请查看方法中的注释
            // 史莱姆王
            if (KSPanelType == 3)
                PanelsLoreKingSlime = true;
            if (KSPanelType == 1)
                PanelsLoreKingSlime = false;

            // 蘑菇
            if (CrabPanelType == 3)
                PanelsLoreCrabulon = true;
            if (CrabPanelType == 1)
                PanelsLoreCrabulon = false;

            // 克眼
            if (EoCPanelType == 3)
                PanelsLoreEoC = true;
            if (EoCPanelType == 1)
                PanelsLoreEoC = false;

            // 荒灾
            if (DSPanelType == 3)
                PanelsLoreDesertScourge = true;
            if (DSPanelType == 1)
                PanelsLoreDesertScourge = false;

            // 世吞
            if (EoWPanelType == 3)
                PanelsLoreEaterofWorld = true;
            if (EoWPanelType == 1)
                PanelsLoreEaterofWorld = false;

            // 克脑
            if (BoCPanelType == 3)
                PanelsBoCLoreTeleportation = true;
            if (BoCPanelType == 1)
                PanelsBoCLoreTeleportation = false;

            // 宿主
            if (PerfPanelType == 3)
                PanelsLorePerforator = true;
            if (PerfPanelType == 1)
                PanelsLorePerforator = false;

            // 腐巢
            if (HivePanelType == 3)
                PanelsLoreHive = true;
            if (HivePanelType == 1)
                PanelsLoreHive = false;

            // 蜂王
            if (QBPanelType == 3)
                PanelsLoreQueenBee = true;
            if (QBPanelType == 1)
                PanelsLoreQueenBee = false;

            // 骷髅王
            if (SkelePanelType == 3)
                PanelsLoreSkeletron = true;
            if (SkelePanelType == 1)
                PanelsLoreSkeletron = false;

            // 史莱姆神
            if (SGPanelType == 3)
                PanelsLoreSG = true;
            if (SGPanelType == 1)
                PanelsLoreSG = false;

            // 肉山
            if (WoFPanelType == 3)
                PanelsLoreWallofFlesh = true;
            if (WoFPanelType == 1)
                PanelsLoreWallofFlesh = false;

            // 双子
            if (TwinsPanelType == 3)
                PanelsLoreTwins = true;
            if (TwinsPanelType == 1)
                PanelsLoreTwins = false;

            // 海灾
            if (ASPanelType == 3)
                PanelsLoreAquaticScourge = true;
            if (ASPanelType == 1)
                PanelsLoreAquaticScourge = false;

            // 冰灵
            if (CryoPanelType == 3)
                PanelsLoreCryoDash = true;
            if (CryoPanelType == 1)
                PanelsLoreCryoDash = false;

            // 毁灭者
            if (DestroyerPanelType == 3)
                PanelsLoreDestroyer = true;
            if (DestroyerPanelType == 1)
                PanelsLoreDestroyer = false;

            // 硫磺火
            if (BrimmyPanelType == 3)
                PanelsLoreBrimstoneElement = true;
            if (BrimmyPanelType == 1)
                PanelsLoreBrimstoneElement = false;

            // 机械骷髅王
            if (PrimePanelType == 3)
                PanelsLorePrime = true;
            if (PrimePanelType == 1)
                PanelsLorePrime = false;

            // 普灾
            if (CalClonePanelType == 3)
                PanelsLoreCalamitasClone = true;
            if (CalClonePanelType == 1)
                PanelsLoreCalamitasClone = false;

            // 花
            if (PlantPanelType == 3)
                PanelsLorePlantera = true;
            if (PlantPanelType == 1)
                PanelsLorePlantera = false;

            // 利维坦
            if (LAPanelType == 3)
                PanelsLoreLeviAnahita = true;
            if (LAPanelType == 1)
                PanelsLoreLeviAnahita = false;

            // 白金
            if (AureusPanelType == 3)
                PanelsLoreAureus = true;
            if (AureusPanelType == 1)
                PanelsLoreAureus = false;

            // 石巨人
            if (GolemPanelType == 3)
                PanelsLoreGolem = true;
            if (GolemPanelType == 1)
                PanelsLoreGolem = false;

            // 歌莉娅
            if (PBGPanelType == 3)
                PanelsLoreGoliath = true;
            if (PBGPanelType == 1)
                PanelsLoreGoliath = false;

            // 猪鲨
            if (DukePanelType == 3)
                PanelsLoreDuke = true;
            if (DukePanelType == 1)
                PanelsLoreDuke = false;

            // 魔像
            if (RavagerPanelType == 3)
                PanelsLoreRavager = true;
            if (RavagerPanelType == 1)
                PanelsLoreRavager = false;

            // 邪教徒
            if (CultistPanelType == 3)
                PanelsLoreCultist = true;
            if (CultistPanelType == 1)
                PanelsLoreCultist = false;

            // 游龙
            if (DeusPanelType == 3)
                PanelsLoreDeus = true;
            if (DeusPanelType == 1)
                PanelsLoreDeus = false;

            // 月
            if (MLPanelType == 3)
                PanelsLoreLunarBoss = true;
            if (MLPanelType == 1)
                PanelsLoreLunarBoss = false;

            // 亵渎
            if (ProviPanelType == 3)
                PanelsLoreProvidence = true;
            if (ProviPanelType == 1)
                PanelsLoreProvidence = false;

            // 幽花
            if (PolterPanelType == 3)
                PanelsLorePolter = true;
            if (PolterPanelType == 1)
                PanelsLorePolter = false;

            // 老核弹
            if (ODPanelType == 3)
                PanelsLoreDukeElder = true;
            if (ODPanelType == 1)
                PanelsLoreDukeElder = false;

            // 神长
            if (DoGPanelType == 3)
                PanelsLoreDevourer = true;
            if (DoGPanelType == 1)
                PanelsLoreDevourer = false;

            // 犽绒
            if (YharonPanelType == 3)
                PanelsLoreJungleDragon = true;
            if (YharonPanelType == 1)
                PanelsLoreJungleDragon = false;

            // 巨械
            if (ExoPanelType == 3)
                PanelsLoreExo = true;
            if (ExoPanelType == 1)
                PanelsLoreExo = false;

            // 終灾
            if (SCalPanelType == 3)
                PanelsSCalLore = true;
            if (SCalPanelType == 1)
                PanelsSCalLore = false;

            // 禁止渊海灾虫生成
            if (SulphurSeaType == 3)
                cIdisableNaturalScourgeSpawns = true;
            if (SulphurSeaType == 1)
                cIdisableNaturalScourgeSpawns = true;

            // 禁止阿娜西塔生成
            if (SeaPanelType == 3)
                cIdisableAnahitaSpawns = true;
            if (SeaPanelType == 1)
                cIdisableAnahitaSpawns = true;

            // 禁止腐化囊生成
            if (CorruptionPanelType == 3)
                cIdisableHiveCystSpawns = true;
            if (CorruptionPanelType == 1)
                cIdisableHiveCystSpawns = true;

            // 禁止血肉囊生成
            if (CrimsonPanelType == 3)
                cIdisablePerfCystSpawns = true;
            if (CrimsonPanelType == 1)
                cIdisablePerfCystSpawns = true;

            // 禁止肉山生成
            if (UnderworldPanelType == 3)
                cIdisableVoodooSpawns = true;
            if (UnderworldPanelType == 1)
                cIdisableVoodooSpawns = true;
            #endregion
        }
        public override void SaveData(TagCompound tag)
        {
            // 肉前
            tag["CIKSLoreType"] = KSPanelType;

            tag["CICrabLoreType"] = CrabPanelType;

            tag["CIEoCLoreType"] = EoCPanelType;

            tag["CIDSLoreType"] = DSPanelType;

            tag["CIEoWLoreType"] = EoWPanelType;

            tag["CIBOCLoreType"] = BoCPanelType;

            tag["CIPerfLoreType"] = PerfPanelType;

            tag["CIHiveLoreType"] = HivePanelType;

            tag["CIQBLoreType"] = QBPanelType;

            tag["CISkeleLoreType"] = SkelePanelType;

            tag["CISGLoreType"] = SGPanelType;

            tag["CIWoFLoreType"] = WoFPanelType;

            // 肉后
            tag["CIASLoreType"] = ASPanelType;

            tag["CITwinsLoreType"] = TwinsPanelType;

            tag["CICryoLoreType"] = CryoPanelType;

            tag["CIDestroyerLoreType"] = DestroyerPanelType;

            tag["CIBrimmyLoreType"] = BrimmyPanelType;

            tag["CIPrimeLoreType"] = PrimePanelType;

            tag["CICalCloneLoreType"] = CalClonePanelType;

            tag["CIPlantLoreType"] = PlantPanelType;

            // 花后
            tag["CILALoreType"] = LAPanelType;

            tag["CIAureusLoreType"] = AureusPanelType;

            tag["CIGolemLoreType"] = GolemPanelType;

            tag["CIPBGLoreType"] = PBGPanelType;

            tag["CIDuckeLoreType"] = DukePanelType;

            tag["CIRavagerLoreType"] = RavagerPanelType;

            tag["CICultistnlLoreType"] = CultistPanelType;

            tag["CIDeusLoreType"] = DeusPanelType;

            // 月后
            tag["CIMLLoreType"] = MLPanelType;

            tag["CIProvilLoreType"] = ProviPanelType;

            tag["CIPolterLoreType"] = PolterPanelType;

            tag["CIODLoreType"] = ODPanelType;

            tag["CIDoGLoreType"] = DoGPanelType;

            tag["CIYharonlLoreType"] = YharonPanelType;

            tag["CIExoLoreType"] = ExoPanelType;

            tag["CIScalLoreType"] = SCalPanelType;
            //熟练度存储
            ProficiencySaveData(ref tag); 
            //传奇物品样式保存
            LegendarySaveData(ref tag);
            //禁止生成
            tag.Add("CISulphurSeaType", SulphurSeaType);
            tag.Add("CISeaPanelType", SeaPanelType);
            tag.Add("CICorruptionPanelType", CorruptionPanelType);
            tag.Add("CICrimsonPanelType", CrimsonPanelType);
            tag.Add("CIUnderworldPanelType", UnderworldPanelType);
            

        }

        public override void LoadData(TagCompound tag)
        {
            // 肉前
            KSPanelType = tag.GetInt("CIKSLoreType");

            EoCPanelType = tag.GetInt("CIEoCLoreType");

            CrabPanelType = tag.GetInt("CICrabLoreType");

            DSPanelType = tag.GetInt("CIDSLoreType");

            EoWPanelType = tag.GetInt("CIEoWLoreType");

            BoCPanelType = tag.GetInt("CIBOCLoreType");

            PerfPanelType = tag.GetInt("CIPerfLoreType");

            HivePanelType = tag.GetInt("CIHiveLoreType");

            QBPanelType = tag.GetInt("CIQBLoreType");

            SkelePanelType = tag.GetInt("CISkeleLoreType");

            SGPanelType = tag.GetInt("CISGLoreType");

            WoFPanelType = tag.GetInt("CIWoFLoreType");

            // 肉后
            ASPanelType = tag.GetInt("CIASLoreType");

            TwinsPanelType = tag.GetInt("CITwinsLoreType");

            CryoPanelType = tag.GetInt("CICryoLoreType");

            DestroyerPanelType = tag.GetInt("CIDestroyerLoreType");

            BrimmyPanelType = tag.GetInt("CIBrimmyLoreType");

            PrimePanelType = tag.GetInt("CIPrimeLoreType");

            CalClonePanelType = tag.GetInt("CICalCloneLoreType");

            PlantPanelType = tag.GetInt("CIPlantLoreType");

            // 花后
            LAPanelType = tag.GetInt("CILALoreType");

            AureusPanelType = tag.GetInt("CIAureusLoreType");

            GolemPanelType = tag.GetInt("CIGolemLoreType");

            PBGPanelType = tag.GetInt("CIPBGLoreType");

            DukePanelType = tag.GetInt("CIDuckeLoreType");

            RavagerPanelType = tag.GetInt("CIRavagerLoreType");

            CultistPanelType = tag.GetInt("CICultistnlLoreType");

            DeusPanelType = tag.GetInt("CIDeusLoreType");

            // 月后
            MLPanelType = tag.GetInt("CIMLLoreType");

            ProviPanelType = tag.GetInt("CIProvilLoreType");

            PolterPanelType = tag.GetInt("CIPolterLoreType");

            ODPanelType = tag.GetInt("CIODLoreType");

            DoGPanelType = tag.GetInt("CIDoGLoreType");

            YharonPanelType = tag.GetInt("CIYharonlLoreType");

            ExoPanelType = tag.GetInt("CIExoLoreType");

            SCalPanelType = tag.GetInt("CIScalLoreType");

            // 禁止生成
            SulphurSeaType = tag.GetInt("CISulphurSeaType");

            SeaPanelType = tag.GetInt("CISeaPanelType");

            CorruptionPanelType = tag.GetInt("CICorruptionPanelType");

            CrimsonPanelType = tag.GetInt("CICrimsonPanelType");

            UnderworldPanelType = tag.GetInt("CIUnderworldPanelType");

            PanelsLoreExo = tag.GetBool("PanelsLoreExo");
            ProficiencyLoadData(ref tag);
            LegendaryLoadData(ref tag);    
        }
    }
}
