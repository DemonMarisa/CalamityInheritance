global using static Terraria.ModLoader.ModContent;
using System.Reflection;
using Terraria.ModLoader;

namespace CalamityInheritance
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class CalamityInheritance : Mod
    {
        public static CalamityInheritance Instance;
        public static readonly BindingFlags UniversalBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        public static Mod UCA = null;
        public static Mod Calamity = null;
        public override void Load()
        {
            Instance = this;
            UCA = null;
            Calamity = null;
            ModLoader.TryGetMod("CalamityMod", out Calamity);
        }
        public override void Unload()
        {
            UCA = null;
            Calamity = null;
        }
    }
}
