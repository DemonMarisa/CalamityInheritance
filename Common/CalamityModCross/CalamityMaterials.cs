using CalamityInheritance.Core.Utils;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Abyss;
using CalamityMod.Items.Placeables.FurnitureAcidwood;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Placeables.SunkenSea;
using CalamityMod.Items.Weapons.Ranged;
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

        public static int AstralBar;
        public static int Acidwood;
        public static int SulphuricScale;

        public static int StarblightSoot;

        public static int MeldBlob;
        public static int PerennialBar;
        public static int InfectedArmorPlating;
<<<<<<< HEAD
        /// <summary>
        /// 这个有可能会删除
        /// </summary>
        public static int LifeAlloy;
=======

        public static int AstralBar;
        public static int Acidwood;
        public static int SulphuricScale;

        public static int StarblightSoot;

        public static int MeldBlob;
        public static int PerennialBar;
        public static int InfectedArmorPlating;
>>>>>>> 6a5a5aaee095507203de8ddd707e66b4043192dd
        public override void OnModLoad()
        {
            if (CIUtils.HasCalamity())
            {
                GetCalamityMaterialsID();
                GetCalamityWeaponID();
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
            AstralBar = ItemType<AstralBar>();
            Acidwood = ItemType<Acidwood>();
            SulphuricScale = ItemType<SulphuricScale>();
            StarblightSoot = ItemType<StarblightSoot>();
            MeldBlob = ItemType<StarblightSoot>();
            PerennialBar = ItemType<PerennialBar>();
            InfectedArmorPlating = ItemType<InfectedArmorPlating>();
<<<<<<< HEAD
            LifeAlloy = ItemType<LifeAlloy>();
        }
        /// <summary>
        /// 这么搞下去要没完没了了你自己寻思一下怎么去处理这个武器材料
        /// <br>反正我先这么写放在这里了</br>
        /// </summary>
        public static int PestilentDefiler;
        [JITWhenModsEnabled("CalamityMod")]
        public static void GetCalamityWeaponID()
        {
            PestilentDefiler = ItemType<PestilentDefiler>();
=======
            AstralBar = ItemType<AstralBar>();
            Acidwood = ItemType<Acidwood>();
            SulphuricScale = ItemType<SulphuricScale>();
            StarblightSoot = ItemType<StarblightSoot>();
            MeldBlob = ItemType<StarblightSoot>();
            PerennialBar = ItemType<PerennialBar>();
            InfectedArmorPlating = ItemType<InfectedArmorPlating>();
>>>>>>> 6a5a5aaee095507203de8ddd707e66b4043192dd
        }
    }
}
