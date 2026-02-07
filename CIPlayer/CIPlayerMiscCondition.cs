using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        // 这两个东西都是在对应地方手动重置
        public bool wasMouseDown = false;//用于qol面板的鼠标状态跟踪
        public bool canFreeScope = false;
        
        public static bool inSpace;
        
        public void ReSet()
        {            //生命上限（们）
            ResetLifeMax();
        }
        public static void PreUp()
        {
            float spacef = 16f;
            float spaceh = (float)Main.maxTilesX / 4200f;
            spaceh *= spaceh;
            inSpace = (float)((double)((Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / spacef - (65f + 10f * spaceh)) / (Main.worldSurface / 5.0)) < 1f;
        }
    }
}
