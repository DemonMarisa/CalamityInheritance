using Terraria.Localization;

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
