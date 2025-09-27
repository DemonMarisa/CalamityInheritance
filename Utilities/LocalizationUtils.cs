using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
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
        /// <summary>
        /// 从最后一行Tooltip后插入值
        /// </summary>
        /// <param name="tooltips"></param>
        /// <param name="textPath"></param>
        public static void InsertNewLineToFinalLine(this List<TooltipLine> tooltips, Mod mod,string textPath)
        {
            string text = textPath.ToLangValue();
            var newLine = new TooltipLine(mod, "ModName", text)
            {
                OverrideColor = tooltips.Count > 0 ? tooltips[^1].OverrideColor : Color.White
            };
            if (tooltips.Count is 0)
                tooltips.Add(newLine);
            else
                tooltips.Insert(tooltips.Count, newLine);
        }
        /// <summary>
        /// 从最后一行Tooltip后插入值，重载传参方法
        /// </summary>
        /// <param name="tooltips"></param>
        /// <param name="textPath"></param>
        public static void InsertNewLineToFinalLine(this List<TooltipLine> tooltips, Mod mod, string textPath, params object[] args)
        {
            string text = textPath.ToLangValue().ToFormatValue(args);
            var newLine = new TooltipLine(mod, "ModName", text)
            {
                OverrideColor = tooltips.Count > 0 ? tooltips[^1].OverrideColor : Color.White
            };
            if (tooltips.Count is 0)
                tooltips.Add(newLine);
            else
                tooltips.Insert(tooltips.Count, newLine);
        }
        /// <summary>
        /// 从最后一行Tooltip后插入值，重载颜色代码
        /// </summary>
        /// <param name="tooltips"></param>
        /// <param name="textPath"></param>
        public static void InsertNewLineToFinalLine(this List<TooltipLine> tooltips, Mod mod,string textPath, Color color)
        {
            string text = textPath.ToLangValue();
            var newLine = new TooltipLine(mod, "ModName", text)
            {
                OverrideColor = color
            };
            if (tooltips.Count is 0)
                tooltips.Add(newLine);
            else
                tooltips.Insert(tooltips.Count, newLine);
        }
        /// <summary>
        /// 从最后一行Tooltip后插入值，重载传参方法，颜色代码
        /// </summary>
        /// <param name="tooltips"></param>
        /// <param name="textPath"></param>
        public static void InsertNewLineToFinalLine(this List<TooltipLine> tooltips, Mod mod,string textPath, Color color, params object[] args)
        {
            string text = textPath.ToLangValue().ToFormatValue(args);
            var newLine = new TooltipLine(mod, "ModName", text)
            {
                OverrideColor = color
            };
            if (tooltips.Count is 0)
                tooltips.Add(newLine);
            else
                tooltips.Insert(tooltips.Count, newLine);
        }
        /// <summary>
        /// 从最后一行Tooltip后插入值
        /// </summary>
        /// <param name="tooltips"></param>
        /// <param name="textPath"></param>
        public static void InsertNewLineToFinalLineTest(this List<TooltipLine> tooltips, Mod mod,string textPath)
        {
            string text = textPath.ToLangValue();
            var newLine = new TooltipLine(mod, "ModName", text)
            {
                OverrideColor = tooltips.Count > 0 ? tooltips[^1].OverrideColor : Color.White
            };
            List<TooltipLine> customLine = [newLine]; 
            if (tooltips.Count is 0)
                tooltips.Add(newLine);
            else
            {
                //搜寻可能存在的重铸文本
                //似乎重铸文名字不是"Modifier"
                int modifyIndex = -1;
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips[i].Name == "Modifier")
                    {
                        modifyIndex = i;
                        break;
                    }
                }
                if (modifyIndex != -1)
                    tooltips.InsertRange(modifyIndex - 1, customLine);
                else
                    tooltips.Insert(tooltips.Count, newLine);
            }
        }
        public static string ToHexColor(this Color color) => $"{color.R:X2}{color.G:X2}{color.B:X2}";

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
