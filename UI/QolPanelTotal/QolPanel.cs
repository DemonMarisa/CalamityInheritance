using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.System.Configs;
using CalamityInheritance.System.DownedBoss;

namespace CalamityInheritance.UI.QolPanelTotal
{
    public partial class QolPanel : DraedonsPanelUI
    {
        public static void PageDraw(SpriteBatch spriteBatch)
        {
            // 头图
            if (Page == 0)
                Page1Draw(spriteBatch);
            // 有效果的Lore
            if (Page == 1)
                Page2Draw(spriteBatch);
            // 其余Lore
            if (Page == 2)
                Page3Draw(spriteBatch);
            // 等级
            if (Page == 3)
                Page4Draw(spriteBatch);
        }
    }
}