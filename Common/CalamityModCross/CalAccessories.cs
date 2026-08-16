using CalamityInheritance.Core.Utils;
using CalamityMod.Items.Accessories;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CalamityModCross
{
    public class CalAccessories : ModSystem
    {
        public static int IlmerisSpark;
        public static int CrawCarapace;
        public static int GiantTortoiseShell;
        public static int CleansingJelly;
        public static int LifeJelly;
        public static int VitalJelly;
        public override void OnModLoad()
        {
            if (CIUtils.HasCalamity())
            {
                GetCalamityAccessID();
            }
        }
        [JITWhenModsEnabled("CalamityMod")]
        public static void GetCalamityAccessID()
        {
            IlmerisSpark = ItemType<IlmerisSpark>();
            CrawCarapace = ItemType<CrawCarapace>();
            GiantTortoiseShell = ItemType<GiantTortoiseShell>();
            CleansingJelly = ItemType<CleansingJelly>();
            LifeJelly = ItemType<LifeJelly>();
            VitalJelly = ItemType<VitalJelly>();
        }
    }
}
