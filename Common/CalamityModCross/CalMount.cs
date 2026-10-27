using CalamityInheritance.Core.Utils;
using CalamityMod.Items.Mounts;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CalamityModCross
{
    public class CalMount : ModSystem
    {
        public static int DraedonGamerChairMount;
        public override void OnModLoad()
        {
            if (CIUtils.HasCalamity())
            {
                GetCalamityMountID();
            }
        }
        [JITWhenModsEnabled("CalamityMod")]
        public static void GetCalamityMountID()
        {
            DraedonGamerChairMount = MountType<DraedonGamerChairMount>();
        }
    }
}
