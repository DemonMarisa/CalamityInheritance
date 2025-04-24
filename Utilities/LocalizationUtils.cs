using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;

namespace CalamityInheritance.Utilities
{
    public partial class CIFunction
    {
        /// <param name="key">The language key. This will have "Mods.CalamityMod." appended behind it.</param>
        /// <returns>
        /// A <see cref="LocalizedText"/> instance found using the provided key with "Mods.CalamityMod." appended behind it. 
        /// <para>NOTE: Modded translations are not loaded until after PostSetupContent.</para>Caching the result is suggested.
        /// </returns>
        public static LocalizedText GetText(string key)
        {
            return Language.GetOrRegister("Mods.CalamityInheritance." + key);
        }
        public static string GetTextValue(string key)
        {
            return Language.GetTextValue("Mods.CalamityInheritance." + key);
        }
        public static void SendTextOnPlayer(string key, Color color)
        {
            Player player = Main.player[Main.myPlayer];
            Rectangle location = new Rectangle((int)player.position.X, (int)player.position.Y - 16, player.width, player.height);
            CombatText.NewText(location, color, Language.GetTextValue(("Mods.CalamityInheritance." + key)));
        }
    }
}
