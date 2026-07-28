using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        public static LocalizedText GetText(string key)
        {
            return Language.GetOrRegister("Mods.CalamityInheritance." + key);
        }
        public static string GetTextValue(string key)
        {
            return Language.GetTextValue("Mods.CalamityInheritance." + key);
        }
    }
}
