using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.System
{
    public class MiscFlagReset : ModSystem
    {
        public static bool PlayDogLegacyMusic = false;
        public override void PreUpdateNPCs()
        {
            PlayDogLegacyMusic = false;
        }
    }
}
