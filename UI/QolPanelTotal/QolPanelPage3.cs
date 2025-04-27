using CalamityInheritance.CIPlayer;
using CalamityInheritance.System.Configs;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.LoreItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.UI.QolPanelTotal
{
    public partial class QolPanel
    {
        public static string MiscLoreImagePath => "CalamityInheritance/UI/DraedonsTexture/MiscLore"; //一个字段

        #region Lore面板
        #region Page 3
        public int LoreBloodMoonID = 37;
        public int LoreCorruptionID = 38;
        public int LoreCrimsonID = 39;
        public int LoreUnderworldID = 40;
        public int LoreSulphurSeaID = 41;
        public int LoreBrimstoneCragID = 42;
        public int LoreMechID = 43;
        public int LoreOceanID = 44;
        public int LoreAstralInfectionID = 45;
        public int LoreDragonfollyID = 46;
        public int LoreProfanedGuardiansID = 47;
        public int LoreSentinelsID = 48;
        #endregion
        public void Page3Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer cIPlayer = player.CIMod();

            #region 功能开启
            bool DownedBloodMoon = CIDownedBossSystem.DownedBloodMoon;
            bool DownedAnyMech = Condition.DownedMechBossAny.IsMet();
            bool DownedProfanedGuardians = DownedBossSystem.downedGuardians;
            bool DownedDragonfolly = DownedBossSystem.downedDragonfolly;
            bool DownedAnySentinels = DownedBossSystem.downedSignus && DownedBossSystem.downedCeaselessVoid && DownedBossSystem.downedStormWeaver;
            #endregion

            bool Any = true;
            bool unAny = false;

            bool DownedAS = DownedBossSystem.downedAquaticScourge;
            bool DownedHive = DownedBossSystem.downedHiveMind;
            bool DownedLA = DownedBossSystem.downedLeviathan;
            bool DownedWoF = Condition.Hardmode.IsMet();
            bool DownedDeus = DownedBossSystem.downedAstrumDeus;
            bool DownedBrimmy = DownedBossSystem.downedBrimstoneElemental;
            bool DownedPerf = DownedBossSystem.downedPerforator;
            #region 材质
            Texture2D LoreBloodMoon = ModContent.Request<Texture2D>($"{MiscLoreImagePath}/LoreBloodMoon").Value;
            Texture2D LoreAstralInfection = ModContent.Request<Texture2D>($"{MiscLoreImagePath}/LoreAstralInfection").Value;
            Texture2D LoreBrimstoneCrag = ModContent.Request<Texture2D>($"{MiscLoreImagePath}/LoreBrimstoneCrag").Value;
            Texture2D LoreCorruption = ModContent.Request<Texture2D>($"{MiscLoreImagePath}/LoreCorruption").Value;
            Texture2D LoreCrimson = ModContent.Request<Texture2D>($"{MiscLoreImagePath}/LoreCrimson").Value;
            Texture2D LoreDragonfolly = ModContent.Request<Texture2D>($"{MiscLoreImagePath}/LoreDragonfolly").Value;
            Texture2D LoreMech = ModContent.Request<Texture2D>($"{MiscLoreImagePath}/LoreMech").Value;
            Texture2D LoreOcean = ModContent.Request<Texture2D>($"{MiscLoreImagePath}/LoreOcean").Value;
            Texture2D LoreProfanedGuardians = ModContent.Request<Texture2D>($"{MiscLoreImagePath}/LoreProfanedGuardians").Value;
            Texture2D LoreSentinels = ModContent.Request<Texture2D>($"{MiscLoreImagePath}/LoreSentinels").Value;
            Texture2D LoreSulphurSea = ModContent.Request<Texture2D>($"{MiscLoreImagePath}/LoreSulphurSea").Value;
            Texture2D LoreUnderWorld = ModContent.Request<Texture2D>($"{MiscLoreImagePath}/LoreUnderWorld").Value;
            #endregion
            #region 按钮材质
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
            // lore悬停时的材质
            Texture2D loreTextureOutLine = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/Lore/LoreOutLine").Value;
            // lore不可用时的悬停材质
            Texture2D loreTextureOutLineUnAvailable = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/Lore/LoreOutLineUnAvailable").Value;
            // 如果不想或者懒得新建存储，可以直接用这个透明材质
            Texture2D InvisibleUI = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/InvisibleUI").Value;
            #endregion
            float xResolutionScale = 0.8f;
            float yResolutionScale = 0.8f;
            Rectangle mouseRectangle = new((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 2, 2);

            // 储存按钮数据
            DrawUIData genericBtonData = GetDrawBtnData(xResolutionScale, yResolutionScale, 0.65f, spriteBatch, buttonTextureTrue, buttonTextureTrueHover, buttonTextureFalse, buttonTextureFalseHover, buttonTextureUnAvailable, mouseRectangle, false);
            // 这个1是默认的Lore按钮状态
            DrawLoreData genericLoreData = GetDrawLoreData(spriteBatch, loreTextureUnAvailable, loreTextureOutLine, loreTextureOutLineUnAvailable, mouseRectangle, 0.75f, xResolutionScale, yResolutionScale, false, 1);
            DrawLoreData genericLoreDataNotOutLine = GetDrawLoreData(spriteBatch, loreTextureUnAvailable, InvisibleUI, InvisibleUI, mouseRectangle, 0.75f, xResolutionScale, yResolutionScale, false, 1);

            if (Page == 2)
            {
                int PanelLore1 = 1;
                int PanelLore2 = 2;
                int PanelLore3 = 3;
                int PanelLore4 = 4;
                #region 绘制Lore
                // 绘制Lore
                CIFunction.DrawLore(genericLoreData, GetLorePos(1, 1).LorePosX, GetLorePos(1, 1).LorePosY, LoreBloodMoon, ref TextDisplayID, ref LoreBloodMoonID, ref DownedBloodMoon, ref draedonsLoreChoice, ref PanelLore2);
                CIFunction.DrawLore(genericLoreData, GetLorePos(1, 2).LorePosX, GetLorePos(1, 2).LorePosY, LoreCorruption, ref TextDisplayID, ref LoreCorruptionID, ref DownedHive, ref draedonsLoreChoice, ref PanelLore2);
                CIFunction.DrawLore(genericLoreData, GetLorePos(1, 3).LorePosX, GetLorePos(1, 3).LorePosY, LoreCrimson, ref TextDisplayID, ref LoreCrimsonID, ref DownedPerf, ref draedonsLoreChoice, ref PanelLore3);
                CIFunction.DrawLore(genericLoreData, GetLorePos(1, 4).LorePosX, GetLorePos(1, 4).LorePosY, LoreUnderWorld, ref TextDisplayID, ref LoreUnderworldID, ref DownedWoF, ref draedonsLoreChoice, ref PanelLore4);
                CIFunction.DrawLore(genericLoreData, GetLorePos(1, 5).LorePosX, GetLorePos(1, 5).LorePosY, LoreSulphurSea, ref TextDisplayID, ref LoreSulphurSeaID, ref DownedAS, ref draedonsLoreChoice, ref PanelLore3);
                CIFunction.DrawLore(genericLoreData, GetLorePos(1, 6).LorePosX, GetLorePos(1, 6).LorePosY, LoreBrimstoneCrag, ref TextDisplayID, ref LoreBrimstoneCragID, ref DownedBrimmy, ref draedonsLoreChoice, ref PanelLore1);
                CIFunction.DrawLore(genericLoreData, GetLorePos(2, 1).LorePosX, GetLorePos(2, 1).LorePosY, LoreMech, ref TextDisplayID, ref LoreMechID, ref DownedAnyMech, ref draedonsLoreChoice, ref PanelLore3);
                CIFunction.DrawLore(genericLoreData, GetLorePos(2, 2).LorePosX, GetLorePos(2, 2).LorePosY, LoreOcean, ref TextDisplayID, ref LoreOceanID, ref DownedLA, ref draedonsLoreChoice, ref PanelLore1);
                CIFunction.DrawLore(genericLoreData, GetLorePos(2, 3).LorePosX, GetLorePos(2, 3).LorePosY, LoreAstralInfection, ref TextDisplayID, ref LoreAstralInfectionID, ref DownedDeus, ref draedonsLoreChoice, ref PanelLore2);
                CIFunction.DrawLore(genericLoreData, GetLorePos(2, 4).LorePosX, GetLorePos(2, 4).LorePosY, LoreProfanedGuardians, ref TextDisplayID, ref LoreProfanedGuardiansID, ref DownedProfanedGuardians, ref draedonsLoreChoice, ref PanelLore4);
                CIFunction.DrawLore(genericLoreData, GetLorePos(2, 5).LorePosX, GetLorePos(2, 5).LorePosY, LoreDragonfolly, ref TextDisplayID, ref LoreDragonfollyID, ref DownedDragonfolly, ref draedonsLoreChoice, ref PanelLore4);
                CIFunction.DrawLore(genericLoreData, GetLorePos(2, 6).LorePosX, GetLorePos(2, 6).LorePosY, LoreSentinels, ref TextDisplayID, ref LoreSentinelsID, ref DownedAnySentinels, ref draedonsLoreChoice, ref PanelLore2);
                #endregion

                #region 绘制按钮
                CIFunction.DrawBton(genericBtonData, GetLorePos(1, 1).LoreBtnX, GetLorePos(1, 1).LoreBtnY, ref unAny, ref cIPlayer.nullType, ref LoreBloodMoonID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(1, 2).LoreBtnX, GetLorePos(1, 2).LoreBtnY, ref DownedHive, ref cIPlayer.CorruptionPanelType, ref LoreCorruptionID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(1, 3).LoreBtnX, GetLorePos(1, 3).LoreBtnY, ref DownedPerf, ref cIPlayer.CrimsonPanelType, ref LoreCrimsonID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(1, 4).LoreBtnX, GetLorePos(1, 4).LoreBtnY, ref DownedWoF, ref cIPlayer.CrabPanelType, ref LoreUnderworldID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(1, 5).LoreBtnX, GetLorePos(1, 5).LoreBtnY, ref DownedAS, ref cIPlayer.SulphurSeaType, ref LoreSulphurSeaID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(1, 6).LoreBtnX, GetLorePos(1, 6).LoreBtnY, ref unAny, ref cIPlayer.nullType, ref LoreBrimstoneCragID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(2, 1).LoreBtnX, GetLorePos(2, 1).LoreBtnY, ref unAny, ref cIPlayer.nullType, ref LoreMechID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(2, 2).LoreBtnX, GetLorePos(2, 2).LoreBtnY, ref DownedLA, ref cIPlayer.SeaPanelType, ref LoreOceanID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(2, 3).LoreBtnX, GetLorePos(2, 3).LoreBtnY, ref unAny, ref cIPlayer.nullType, ref LoreAstralInfectionID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(2, 4).LoreBtnX, GetLorePos(2, 4).LoreBtnY, ref unAny, ref cIPlayer.nullType, ref LoreProfanedGuardiansID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(2, 5).LoreBtnX, GetLorePos(2, 5).LoreBtnY, ref unAny, ref cIPlayer.nullType, ref LoreDragonfollyID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(2, 6).LoreBtnX, GetLorePos(2, 6).LoreBtnY, ref unAny, ref cIPlayer.nullType, ref LoreSentinelsID);
                #endregion

                #region 绘制高清lore
                // 我没有新建结构体而是继续调用后并修改部分数值
                // 右侧界面的中心位置
                // 305

                Texture2D loreTexturePanelVer = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/PanelLore" + draedonsLoreChoice).Value;

                CIFunction.DrawLore(genericLoreDataNotOutLine, 305, GetLorePos(3, 5, true).LoreBtnY - 210, loreTexturePanelVer, ref TextDisplayID, ref DefaultType, ref Any, ref draedonsLoreChoice, ref PanelLore1, 0.98f);

                // 下划线贴图
                Texture2D loreTextLineTexture = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/TextLine").Value;

                // 下划线贴图
                Texture2D loreTextLineShortTexture = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/TextLineShort").Value;

                CIFunction.DrawImage(spriteBatch, loreTextLineShortTexture, null, 1f, 1.12f, 1f, 305, GetLorePos(3, 4, true).LoreBtnY - 12, false, ref Any);
                #endregion
                #region 绘制文字
                // 背景贴图
                // 用于文字背景，暂时无用
                Texture2D bgTexture = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/TextHoverTip").Value;

                // 获取文字
                string TileText = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LoreT" + TextDisplayID);
                string LoreText = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.Lore" + TextDisplayID);

                CIFunction.DrawText(spriteBatch, TileText, 0.9f, 0.9f, 336, -60, 1f, TextColor, Color.DarkSlateGray, InvisibleUI, 15, 400f, 1.2f);

                CIFunction.DrawText(spriteBatch, LoreText, 0.9f, 0.9f, 340, -27, 1f, TextColor, Color.DarkSlateGray, loreTextLineTexture, 23, 400f, 1.4f);
                #endregion
                Texture2D EndOfDataSet = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/MiscText/EndOfDataSet").Value;

                // 绘制数据结束提示
                float drawPositionX = Main.screenWidth * 0.5f;
                Vector2 drawPosition = new Vector2(drawPositionX, Main.screenHeight * 0.5f);
                // 左侧的
                Vector2 pageOriginLeft = new(EndOfDataSet.Width, EndOfDataSet.Height / 2);

                Vector2 scale = new(1f, 1f);

                // UI缩放
                scale.X *= scaleX;
                scale.Y *= scaleY;

                spriteBatch.Draw(EndOfDataSet, drawPosition, null, Color.White, 0f, pageOriginLeft, scale, SpriteEffects.None, 0f);
            }
        }
        #endregion
    }
}
