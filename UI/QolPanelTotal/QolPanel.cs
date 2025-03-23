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
        // 准备了10个页面
        public override int TotalPages => 10;

        public override void StateSaving()
        {
            // 获取玩家
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer cIPlayer = player.CIMod();
        }

        public override void PageDraw(SpriteBatch spriteBatch)
        {
            Page2Draw(spriteBatch);
        }
    }
}
