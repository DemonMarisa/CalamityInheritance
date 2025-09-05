using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
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
        /// <summary>
        /// 操翻所有Tooltip，并借助本地化完全重写一次
        /// </summary>
        /// <param name="tooltips"></param>
        /// <param name="replacedTextPath"></param>
        public static void FuckThisTooltipAndReplace(this List<TooltipLine> tooltips, string replacedTextPath)
        {
            tooltips.RemoveAll((line) => line.Mod == "Terraria" && line.Name != "Tooltip0" && line.Name.StartsWith("Tooltip"));
            TooltipLine getTooltip = tooltips.FirstOrDefault((x) => x.Name == "Tooltip0" && x.Mod == "Terraria");
            if (getTooltip is not null)
                getTooltip.Text = Language.GetTextValue(replacedTextPath);
        }
        /// <summary>
        /// 操翻所有Tooltip，并借助本地化完全重写一次，附带键入值
        /// </summary>
        /// <param name="tooltips"></param>
        /// <param name="replacedTextPath"></param>
        /// <param name="args"></param>
        public static void FuckThisTooltipAndReplace(this List<TooltipLine> tooltips, string replacedTextPath, params object[] args)
        {
            tooltips.RemoveAll((line) => line.Mod == "Terraria" && line.Name != "Tooltip0" && line.Name.StartsWith("Tooltip"));
            TooltipLine getTooltip = tooltips.FirstOrDefault((x) => x.Name == "Tooltip0" && x.Mod == "Terraria");
            string formateText = replacedTextPath.ToLangValue().ToFormatValue(args);
            if (getTooltip is not null)
                getTooltip.Text = formateText;
        }
        public static string ToLangValue(this string textPath) => Language.GetTextValue(textPath);

        public static string ToFormatValue(this string baseTextValue, params object[] args)
        {
            try
            {
                return string.Format(baseTextValue, args);
            }
            catch
            {
                return baseTextValue + "格式化出错";
            }
        }

    }
}
