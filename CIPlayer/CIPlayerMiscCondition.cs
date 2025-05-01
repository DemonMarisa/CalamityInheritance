using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.System.DownedBoss;
using CalamityMod.CalPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        // 这两个东西都是在对应地方手动重置
        public bool wasMouseDown = false;//用于qol面板的鼠标状态跟踪
        public bool canFreeScope = false;
        public static bool inSpace;
        public void ReSet()
        {

        }
        public void PreUp()
        {
            var remnantsMod = ModLoader.TryGetMod("Remnants", out Mod remnants);

            float spacef = remnantsMod ? 17f : 16f;
            float spaceh = (float)Main.maxTilesX / 4200f;
            spaceh *= spaceh;

            inSpace = (float)((double)((Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / spacef - (65f + 10f * spaceh)) / (Main.worldSurface / 5.0)) < 1f;
        }
    }
}
