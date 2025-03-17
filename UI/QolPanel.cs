using CalamityMod.Items.DraedonMisc;
using CalamityMod.UI.DraedonLogs;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.UI
{
    public class QolPanel : DraedonsPanelUI
    {
        // 准备了10个页面
        public override int TotalPages => 10;

        #region 状态数组
        public int UIpanelloreExocount = 1;//用于qol面板的星三王传颂计数
        public int exoPanelID = 1;// 星三王面板功能的ID
        #endregion

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
            #region 功能开启
            bool DownedExoMechs = DownedBossSystem.downedExoMechs;// 判定是否能使用星三王面板
            #endregion
            // 这里是与设置相关，便于调试的设置
            // 结果：
            // 左上角为 Lore偏移：X - 515 Y - 330 按钮偏移：X - 515 Y - 260
            // 第二行步进为 y - 140 第二列步进为 X + 80

            int LoreX = CIConfig.Instance.UIX;
            int LoreY = CIConfig.Instance.UIY;

            int LoreX2 = CIConfig.Instance.UIX2;
            int LoreY2 = CIConfig.Instance.UIY2;
            
            float xResolutionScale = Main.screenWidth / 2560f;
            float yResolutionScale = Main.screenHeight / 1440f;
            Rectangle mouseRectangle = new((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 2, 2);

            // 案例绘制
            // 按钮开始时的材质
            Texture2D buttonTextureTrue = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/DraedonsLogButtonTrue").Value;
            // 按钮开启时，鼠标悬停悬停的材质，用于过度
            Texture2D buttonTextureTrueHover = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/DraedonsLogButtonTrueHover").Value;
            // 按钮关闭时的材质
            Texture2D buttonTextureFalse = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/DraedonsLogButtonFalse").Value;
            // 按钮关闭时，鼠标悬停悬停的材质，用于过度
            Texture2D buttonTextureFalseHover = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/DraedonsLogButtonFalseHover").Value;
            // 按钮不可用时的材质
            Texture2D buttonTextureUnAvailable = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/DraedonsLogButtonUnAvailable").Value;
            // lore不可用时的材质
            Texture2D loreTextureUnAvailable = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/DraedonsLogLoreUnAvailable").Value;

            // 要绘制在第几页
            if (Page == 1)
            {
                // 前五个选项均为材质，对应材质见上
                // 2f 为缩放，材质大小 x 2
                // x/y ResolutionScale yysy我也不是很懂这两个干啥的，不过不管就没问题
                // 0 ，0 为绘制坐标，相对于屏幕中心
                // Y -xxx 即向上xxx像素
                // X -xxx 即向左xxx像素
                // UIpanelloreExocount为当前按钮状态，用于传递给对应int来实现开启与关闭功能
                // exoPanelID 为对应按钮ID，用于后续制作本地化时的文本标记
                // DownedExoMechs为与downBossSystem的关联
                // false 为是否镜像绘制，true为镜像
                // mouseRectangle为鼠标点击判定，上面自带
                CIFunction.DrawButtom(spriteBatch, buttonTextureFalse, buttonTextureFalseHover, buttonTextureTrue, buttonTextureTrueHover, buttonTextureUnAvailable, 0.8f, xResolutionScale, yResolutionScale, LoreX, LoreY, ref UIpanelloreExocount, ref exoPanelID, ref DownedExoMechs, false, mouseRectangle);

                Texture2D LoreExo = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/Lore/LoreExoMech").Value;

                // 见上，同理，只不过drawimage是drawbuttom的简化版
                CIFunction.DrawImage(spriteBatch, LoreExo , loreTextureUnAvailable, 0.8f, xResolutionScale, yResolutionScale, LoreX2, LoreY2, ref DownedExoMechs,false);
            }
        }
    }
}
