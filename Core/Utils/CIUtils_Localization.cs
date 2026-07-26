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
        /// <summary>
        /// 快速创建一个tooltip行，通过传入<paramref name="textPath"/>本地化文件路径
        /// <br>需注意的是<paramref name="textPath"/>使用了<see cref="GetTextValue(string)"/>方法，你可以跳过<see langword="Mods.CalamityInheritance."/>的前缀</br>
        /// <br><paramref name="line"/>为指定你插入的tooltip行，需要自己从外部获取，其默认值为<see langword="-1"/>，则自动插入到最后一行</br>
        /// </summary>
        public static void CreateTooltip(this List<TooltipLine> tooltips, string textPath, Color? color = null, int line = -1, string LineName = "CalamityInheritanceLineName")
        {
            string text = GetTextValue(textPath);
            Color overrideColor = color ?? Color.White;
            var newLine = new TooltipLine(CalamityInheritance.Instance, LineName, text)
            {
                OverrideColor = overrideColor
            };
            if (line == -1)
            {
                if (tooltips.Count == 0)
                    tooltips.Add(newLine);
                else
                    tooltips.Insert(tooltips.Count, newLine);
            }
            else
                tooltips.Insert(line, newLine);
        }
        /// <summary>
        /// 快速创建一个tooltip行，通过传入<paramref name="textPath"/>本地化文件路径，重载了<paramref name="args"/>用于插入传参
        /// <br>需注意的是<paramref name="textPath"/>使用了<see cref="GetTextValue(string)"/>方法，你可以跳过<see langword="Mods.CalamityInheritance."/>的前缀</br>
        /// <br><paramref name="line"/>为指定你插入的tooltip行，需要自己从外部获取，其默认值为<see langword="-1"/>，则自动插入到最后一行</br>
        /// </summary>

        public static void CreateTooltip(this List<TooltipLine> tooltips, string textPath, Color? color = null, int line = -1, string LineName = "CalamityInheritanceLineName", params object[] args)
        {
            string text = GetText(textPath).Format(args);
            Color overrideColor = color ?? Color.White;
            var newLine = new TooltipLine(CalamityInheritance.Instance, LineName, text)
            {
                OverrideColor = overrideColor
            };
            if (line == -1)
            {
                if (tooltips.Count == 0)
                    tooltips.Add(newLine);
                else
                    tooltips.Insert(tooltips.Count, newLine);
            }
            else
                tooltips.Insert(line, newLine);
        }
    }
}
