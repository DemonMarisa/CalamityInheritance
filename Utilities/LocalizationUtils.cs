﻿using Microsoft.CodeAnalysis;
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
    }
}
