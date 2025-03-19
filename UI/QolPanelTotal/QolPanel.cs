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

            #region lore
            // 确保可以正确切换状态，具体数字对应的绘制贴图请查看方法中的注释
            // 玩家退出进入世界时的保存去ModPlayer中CIPlayerQolPanel中查看
            cIPlayer.panelloreExocount = UIpanelloreExocount;
            UIpanelloreExocount = cIPlayer.panelloreExocount;
            #endregion
        }

        public override void PageDraw(SpriteBatch spriteBatch)
        {
            Page2Draw(spriteBatch);
        }
    }
}
