using CalamityInheritance.CIPlayer;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using log4net.Core;
using Microsoft.CodeAnalysis.Text;
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
        public static int PageLeftCenter = -300;
        public static int Page4FirstLine = -270;

        public static int Page4NorLeftText = -560;
        public static int Page4NorLeftTextHeight = -308;
        public static int Page4NorLeftTextHeightOffset = 95;

        public static string LevelImagePath => "CalamityInheritance/UI/DraedonsTexture/LevelSystem"; //一个字段
        public void Page4Draw(SpriteBatch spriteBatch)
        {
            #region 获取数据
            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer cIPlayer = player.CIMod();    
            #endregion
            #region 注册材质
            // 按钮关闭时
            Texture2D EffectOff = ModContent.Request<Texture2D>($"{LevelImagePath}/EffectOff").Value;
            // 按钮开启时
            Texture2D EffectOn = ModContent.Request<Texture2D>($"{LevelImagePath}/EffectOn").Value;
            // 经验条
            Texture2D LevelBar = ModContent.Request<Texture2D>($"{LevelImagePath}/LevelBar").Value;
            // 右侧加成用的边缘
            Texture2D LevelBoostBorder = ModContent.Request<Texture2D>($"{LevelImagePath}/LevelBoostBorder").Value;
            // 左侧进度边缘
            Texture2D LevelBorder = ModContent.Request<Texture2D>($"{LevelImagePath}/LevelBorder").Value;
            // 左侧最上方文字下划线
            Texture2D LevelHead = ModContent.Request<Texture2D>($"{LevelImagePath}/LevelHead").Value;
            #endregion

            #region 绘制最上面的文字
            string HeadTextLevel = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.HeadText2");
            // -388 - 364都是我手动确定的初始偏移
            CIFunction.DrawTextNoLine(spriteBatch, HeadTextLevel, 1f, 1f, PageLeftCenter + 16, -378, 1.4f, TextColor, Color.DarkSlateGray, 400f, 0f);
            NorDrawImage(spriteBatch, LevelHead, PageLeftCenter, -354);
            #endregion

            DrawMeleeBar(spriteBatch , cIPlayer, LevelBorder, LevelBar);
            DrawRangeBar(spriteBatch, cIPlayer, LevelBorder, LevelBar);
        }
        #region 绘制战士经验条
        public void DrawMeleeBar(SpriteBatch spriteBatch,CalamityInheritancePlayer cIPlayer, Texture2D LevelBorder, Texture2D LevelBar)
        {
            string Melee = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelTextMelee");
            string MeleeCurrentLevel = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelPoolText") + cIPlayer.meleeLevel;

            int requiredExp = cIPlayer.CalculateRequiredExp(cIPlayer.meleeLevel);
            // 绘制进度
            float meleePoolProgress = (float)cIPlayer.meleePool / requiredExp;
            bool maxMeleeLevel = cIPlayer.meleeLevel >= cIPlayer.maxLevel;
            // 绘制常规的边缘
            NorDrawImage(spriteBatch, LevelBorder, PageLeftCenter, Page4FirstLine);
            // 绘制经验条
            DrawImage(spriteBatch, LevelBar, PageLeftCenter, Page4FirstLine, (int)(maxMeleeLevel ? LevelBar.Width : LevelBar.Width * meleePoolProgress), LevelBar.Height, 0);
            // 绘制战士标记和当前进度
            NewDrawTextLeftOrigin(spriteBatch, Melee, 1f, 1f, Page4NorLeftText, Page4NorLeftTextHeight, 1.5f, TextColor, Color.DarkSlateGray, 400f, 0f);
            NewDrawTextLeftOrigin(spriteBatch, MeleeCurrentLevel, 1f, 1f, Page4NorLeftText, Page4NorLeftTextHeight + Page4NorLeftTextHeightOffset, 1.5f, TextColor, Color.DarkSlateGray, 400f, 0f);
        }
        #endregion
        #region 绘制射手经验条
        public void DrawRangeBar(SpriteBatch spriteBatch, CalamityInheritancePlayer cIPlayer, Texture2D LevelBorder, Texture2D LevelBar)
        {
            string Range = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelTextRanged");
            string RangeCurrentLevel = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelPoolText") + cIPlayer.rangeLevel;

            int rangeRequiredExp = cIPlayer.CalculateRequiredExp(cIPlayer.rangeLevel);
            // 绘制进度
            float PoolProgress = (float)cIPlayer.rangePool / rangeRequiredExp;
            bool maxMeleeLevel = cIPlayer.rangeLevel >= cIPlayer.maxLevel;
            // 绘制常规的边缘
            NorDrawImage(spriteBatch, LevelBorder, PageLeftCenter, GetLinePos(2, CIConfig.Instance.UIY));
            // 绘制经验条
            DrawImage(spriteBatch, LevelBar, PageLeftCenter, GetLinePos(2, CIConfig.Instance.UIY), (int)(maxMeleeLevel ? LevelBar.Width : LevelBar.Width * PoolProgress), LevelBar.Height, 0);
            // 绘制标记和当前进度
            NewDrawTextLeftOrigin(spriteBatch, Range, 1f, 1f, Page4NorLeftText, GetLevelPos(2, CIConfig.Instance.UIY), 1.5f, TextColor, Color.DarkSlateGray, 400f, 0f);
            NewDrawTextLeftOrigin(spriteBatch, RangeCurrentLevel, 1f, 1f, Page4NorLeftText, GetLevelPos(2, CIConfig.Instance.UIY) + Page4NorLeftTextHeightOffset, 1.5f, TextColor, Color.DarkSlateGray, 400f, 0f);
        }
        #endregion

        public static int GetLevelPos(int Line, int LineSpace)
        {
            return (Page4NorLeftTextHeight + (Line - 1) * LineSpace);
        }
        public static int GetLinePos(int Line, int LineSpace)
        {
            return (Page4FirstLine + (Line - 1) * LineSpace);
        }
    }
}
