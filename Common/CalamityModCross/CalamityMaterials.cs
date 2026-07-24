using CalamityInheritance.Core.Utils;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Abyss;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CalamityModCross
{
    public class CalamityMaterials : ModSystem
    {
        public static int EssenceofSunlight;// 日光精华
        public static int EssenceofEleum;
        public static int ShadowspecBar;
        public static int CoreofCalamity;
        public static int Lumenyl;
        public static int DepthCells;
        public static int Voidstone;
        public static int CosmiliteBar;
        public static int DarksunFragment;
        public static int NightmareFuel;
        public static int EndothermicEnergy;
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
            CosmiliteBar = ItemType<CosmiliteBar>();
            DarksunFragment = ItemType<DarksunFragment>();
            NightmareFuel = ItemType<NightmareFuel>();
            EndothermicEnergy = ItemType<EndothermicEnergy>();
        }
    }
}
