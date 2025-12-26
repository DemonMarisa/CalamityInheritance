using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.System
{
    public class MiscFlagReset : ModSystem
    {
        public static bool PlayDogLegacyMusic = false;
        public static bool ScalSkyActive = false;
        public static bool YharonSkyActive = false;
        public static bool CalCloneSkyActive = false;
        public override void PreUpdateNPCs()
        {
            PlayDogLegacyMusic = false;
            ScalSkyActive = false;
            YharonSkyActive = false;
            CalCloneSkyActive = false;
        }
    }
}
