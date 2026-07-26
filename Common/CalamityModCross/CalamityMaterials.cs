using CalamityInheritance.Core.Utils;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Abyss;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Placeables.SunkenSea;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CalamityModCross
{
    public class CalamityMaterials : ModSystem
    {
        public static int EssenceofSunlight;// 日光精华
        public static int EssenceofEleum;
        public static int EssenceofHavoc;

        public static int ShadowspecBar;
        public static int CoreofCalamity;
        public static int Lumenyl;
        public static int DepthCells;
        public static int Voidstone;
        public static int CosmiliteBar;
        public static int DarksunFragment;
        public static int NightmareFuel;
        public static int EndothermicEnergy;

        public static int ReaperTooth;
        public static int RuinousSoul;

        public static int UelibloomBar;
        public static int CryonicBar;

        public static int BloodstoneCore;
        public static int UnholyEssence;
        public static int DivineGeode;
        
        public static int ExodiumCluster;
        public static int GrandScale;

        public static int PearlShard;
        public static int SeaPrism;
        public static int Navystone;
        public override void OnModLoad()
        {
            if (CIUtils.HasCalamity())
            {
                GetCalamityMaterialsID();
            }
        }

        [JITWhenModsEnabled("CalamityMod")]
        public static void GetCalamityMaterialsID()
        {
            EssenceofSunlight = ItemType<EssenceofSunlight>();
            ShadowspecBar = ItemType<ShadowspecBar>();
            CoreofCalamity = ItemType<CoreofCalamity>();
            Lumenyl = ItemType<Lumenyl>();
            DepthCells = ItemType<DepthCells>();
            Voidstone = ItemType<Voidstone>();
            EssenceofEleum = ItemType<EssenceofEleum>();
            EssenceofHavoc = ItemType<EssenceofHavoc>();
            CosmiliteBar = ItemType<CosmiliteBar>();
            DarksunFragment = ItemType<DarksunFragment>();
            NightmareFuel = ItemType<NightmareFuel>();
            EndothermicEnergy = ItemType<EndothermicEnergy>();
            ReaperTooth = ItemType<EndothermicEnergy>();
            RuinousSoul = ItemType<RuinousSoul>();
            UelibloomBar = ItemType<UelibloomBar>();
            CryonicBar = ItemType<CryonicBar>();
            BloodstoneCore = ItemType<BloodstoneCore>();
            UnholyEssence = ItemType<UnholyEssence>();
            DivineGeode = ItemType<DivineGeode>();
            ExodiumCluster = ItemType<ExodiumCluster>();
            GrandScale = ItemType<GrandScale>();
            PearlShard = ItemType<PearlShard>();
            SeaPrism = ItemType<SeaPrism>();
            Navystone = ItemType<Navystone>();
        }
    }
}
