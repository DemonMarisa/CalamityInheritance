global using Microsoft.Xna.Framework;
global using static Terraria.ModLoader.ModContent;
global using static CalamityInheritance.Core.Utils.CIUtils;
using LAP.Core.LAPUI.FocusBar;
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
            FocusBarManger.UseFocus = true;

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
