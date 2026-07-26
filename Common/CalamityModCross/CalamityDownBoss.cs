using CalamityInheritance.Core.Utils;
using CalamityMod;
using Terraria.ModLoader;

#pragma warning disable RS0030

namespace CalamityInheritance.Common.CalamityModCross
{
    public class CalamityDownBoss : ModSystem
    {
        public static bool downedYharon = true;
        public override void PostUpdateWorld()
        {
            if (CIUtils.HasCalamity())
                SetCalDownBoss();
        }
        [JITWhenModsEnabled("CalamityMod")]
        public static void SetCalDownBoss()
        {
            downedYharon = DownedBossSystem.downedYharon;
        }
    }
}
