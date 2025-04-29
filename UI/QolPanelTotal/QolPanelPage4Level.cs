using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Weapons;
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

        public static int Page4NorRightText = 45;
        // 总的经验进度
        public static int requiredExp = 12500;
        public static string LevelImagePath => "CalamityInheritance/UI/DraedonsTexture/LevelSystem"; //一个字段
        public void Page4Draw(SpriteBatch spriteBatch)
        {
            #region 获取数据
            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer cIPlayer = player.CIMod();    
            #endregion
            #region 注册材质
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
            #region 左侧绘制内容
            string Exp = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.Exp");
            // 战士
            DrawMeleeBar(spriteBatch , cIPlayer, LevelBorder, LevelBar, Exp);
            // 射手
            DrawRangedBar(spriteBatch, cIPlayer, LevelBorder, LevelBar, Exp);
            // 法师
            DrawMagicBar(spriteBatch, cIPlayer, LevelBorder, LevelBar, Exp);
            // 召唤
            DrawSummonBar(spriteBatch, cIPlayer, LevelBorder, LevelBar, Exp);
            // 盗贼
            DrawRogueBar(spriteBatch, cIPlayer, LevelBorder, LevelBar, Exp);
            #endregion
            #region 右侧绘制
            #region 注册文字内容
            string Damage = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.Damage");
            string Crit = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.Crit");
            string ExpProgress = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.ExpProgress");
            string CurrentBoost = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.CurrentBoost");
            #endregion
            DrawMeleeBoost(spriteBatch, cIPlayer, LevelBoostBorder, LevelBar, Damage, Crit, CurrentBoost, ExpProgress);
            DrawRangedBoost(spriteBatch, cIPlayer, LevelBoostBorder, LevelBar, Damage, Crit, CurrentBoost, ExpProgress);
            DrawMagicBoost(spriteBatch, cIPlayer, LevelBoostBorder, LevelBar, Damage, Crit, CurrentBoost, ExpProgress);
            DrawSummonBoost(spriteBatch, cIPlayer, LevelBoostBorder, LevelBar, Damage, Crit, CurrentBoost, ExpProgress);
            DrawRogueBoost(spriteBatch, cIPlayer, LevelBoostBorder, LevelBar, Damage, Crit, CurrentBoost, ExpProgress);
            #endregion
        }
        #region 经验条
        #region 绘制战士经验条
        public void DrawMeleeBar(SpriteBatch spriteBatch,CalamityInheritancePlayer cIPlayer, Texture2D LevelBorder, Texture2D LevelBar, string Exp)
        {
            string Melee = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelTextMelee");
            string MeleeCurrentLevel = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelPoolText") + cIPlayer.meleeLevel;

            Melee += Exp + cIPlayer.meleePool + " / " + requiredExp;

            // 绘制进度
            float PoolProgress = (float)cIPlayer.meleePool / requiredExp;
            bool maxMeleeLevel = cIPlayer.meleeLevel >= cIPlayer.maxLevel;
            // 绘制常规的边缘
            NorDrawImage(spriteBatch, LevelBorder, PageLeftCenter, Page4FirstLine);
            // 绘制经验条
            DrawImage(spriteBatch, LevelBar, PageLeftCenter, Page4FirstLine, (int)(maxMeleeLevel ? LevelBar.Width : LevelBar.Width * PoolProgress), LevelBar.Height, 0);
            // 绘制战士标记和当前进度
            NewDrawTextLeftOrigin(spriteBatch, Melee, 1f, 1f, Page4NorLeftText, Page4NorLeftTextHeight, 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
            NewDrawTextLeftOrigin(spriteBatch, MeleeCurrentLevel, 1f, 1f, Page4NorLeftText, Page4NorLeftTextHeight + Page4NorLeftTextHeightOffset, 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
        }
        #endregion
        #region 绘制射手经验条
        public void DrawRangedBar(SpriteBatch spriteBatch, CalamityInheritancePlayer cIPlayer, Texture2D LevelBorder, Texture2D LevelBar, string Exp)
        {
            string Range = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelTextRanged");
            string RangeCurrentLevel = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelPoolText") + cIPlayer.rangeLevel;

            Range += Exp + cIPlayer.rangePool + " / " + requiredExp;

            int rangeRequiredExp = cIPlayer.CalculateRequiredExp(cIPlayer.rangeLevel);
            // 绘制进度
            float PoolProgress = (float)cIPlayer.rangePool / requiredExp;
            bool maxMeleeLevel = cIPlayer.rangeLevel >= cIPlayer.maxLevel;
            // 绘制常规的边缘
            NorDrawImage(spriteBatch, LevelBorder, PageLeftCenter, GetLinePos(2, 145));
            // 绘制经验条
            DrawImage(spriteBatch, LevelBar, PageLeftCenter, GetLinePos(2, 145), (int)(maxMeleeLevel ? LevelBar.Width : LevelBar.Width * PoolProgress), LevelBar.Height, 0);
            // 绘制标记和当前进度
            NewDrawTextLeftOrigin(spriteBatch, Range, 1f, 1f, Page4NorLeftText, GetLevelPos(2, 145), 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
            NewDrawTextLeftOrigin(spriteBatch, RangeCurrentLevel, 1f, 1f, Page4NorLeftText, GetLevelPos(2, 145) + Page4NorLeftTextHeightOffset, 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
        }
        #endregion
        #region 绘制法师经验条
        public void DrawMagicBar(SpriteBatch spriteBatch, CalamityInheritancePlayer cIPlayer, Texture2D LevelBorder, Texture2D LevelBar, string Exp)
        {
            string Magic = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelTextMagic");
            string MagicCurrentLevel = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelPoolText") + cIPlayer.magicLevel;

            Magic += Exp + cIPlayer.magicPool + " / " + requiredExp;

            int MagicRequiredExp = cIPlayer.CalculateRequiredExp(cIPlayer.magicLevel);
            // 绘制进度
            float PoolProgress = (float)cIPlayer.magicPool / requiredExp;
            bool maxMagicLevel = cIPlayer.magicLevel >= cIPlayer.maxLevel;
            // 绘制常规的边缘
            NorDrawImage(spriteBatch, LevelBorder, PageLeftCenter, GetLinePos(3, 145));
            // 绘制经验条
            DrawImage(spriteBatch, LevelBar, PageLeftCenter, GetLinePos(3, 145), (int)(maxMagicLevel ? LevelBar.Width : LevelBar.Width * PoolProgress), LevelBar.Height, 0);
            // 绘制标记和当前进度
            NewDrawTextLeftOrigin(spriteBatch, Magic, 1f, 1f, Page4NorLeftText, GetLevelPos(3, 145), 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
            NewDrawTextLeftOrigin(spriteBatch, MagicCurrentLevel, 1f, 1f, Page4NorLeftText, GetLevelPos(3, 145) + Page4NorLeftTextHeightOffset, 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
        }
        #endregion
        #region 绘制召唤经验条
        public void DrawSummonBar(SpriteBatch spriteBatch, CalamityInheritancePlayer cIPlayer, Texture2D LevelBorder, Texture2D LevelBar, string Exp)
        {
            string Summon = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelTextSummon");
            string SummonCurrentLevel = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelPoolText") + cIPlayer.summonLevel;

            Summon += Exp + cIPlayer.summonPool + " / " + requiredExp;

            int SummonRequiredExp = cIPlayer.CalculateRequiredExp(cIPlayer.summonLevel);
            // 绘制进度
            float PoolProgress = (float)cIPlayer.summonPool / requiredExp;
            bool maxSummonLevel = cIPlayer.summonLevel >= cIPlayer.maxLevel;
            // 绘制常规的边缘
            NorDrawImage(spriteBatch, LevelBorder, PageLeftCenter, GetLinePos(4, 145));
            // 绘制经验条
            DrawImage(spriteBatch, LevelBar, PageLeftCenter, GetLinePos(4, 145), (int)(maxSummonLevel ? LevelBar.Width : LevelBar.Width * PoolProgress), LevelBar.Height, 0);
            // 绘制标记和当前进度
            NewDrawTextLeftOrigin(spriteBatch, Summon, 1f, 1f, Page4NorLeftText, GetLevelPos(4, 145), 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
            NewDrawTextLeftOrigin(spriteBatch, SummonCurrentLevel, 1f, 1f, Page4NorLeftText, GetLevelPos(4, 145) + Page4NorLeftTextHeightOffset, 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
        }
        #endregion
        #region 绘制盗贼经验条
        public void DrawRogueBar(SpriteBatch spriteBatch, CalamityInheritancePlayer cIPlayer, Texture2D LevelBorder, Texture2D LevelBar, string Exp)
        {
            string Rogue = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelTextRogue");
            string RogueCurrentLevel = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelPoolText") + cIPlayer.rogueLevel;

            Rogue += Exp + cIPlayer.roguePool + " / " + requiredExp;

            int RogueRequiredExp = cIPlayer.CalculateRequiredExp(cIPlayer.rogueLevel);
            // 绘制进度
            float PoolProgress = (float)cIPlayer.roguePool / requiredExp;
            bool maxRogueLevel = cIPlayer.rogueLevel >= cIPlayer.maxLevel;
            // 绘制常规的边缘
            NorDrawImage(spriteBatch, LevelBorder, PageLeftCenter, GetLinePos(5, 145));
            // 绘制经验条
            DrawImage(spriteBatch, LevelBar, PageLeftCenter, GetLinePos(5, 145), (int)(maxRogueLevel ? LevelBar.Width : LevelBar.Width * PoolProgress), LevelBar.Height, 0);
            // 绘制标记和当前进度
            NewDrawTextLeftOrigin(spriteBatch, Rogue, 1f, 1f, Page4NorLeftText, GetLevelPos(5, 145), 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
            NewDrawTextLeftOrigin(spriteBatch, RogueCurrentLevel, 1f, 1f, Page4NorLeftText, GetLevelPos(5, 145) + Page4NorLeftTextHeightOffset, 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
        }
        #endregion
        #endregion
        #region 具体进度
        #region 战士
        public void DrawMeleeBoost(SpriteBatch spriteBatch, CalamityInheritancePlayer cIPlayer, Texture2D LevelBoostBorder, Texture2D LevelBar, string Damage, string Crit, string CurrentBoost, string ExpProgress)
        {
            string Melee = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelTextMelee");
            // 当前增幅具体显示
            // 表示为 当前增幅：伤害：15% 暴击：15%
            string MeleeCurrentBoost = CurrentBoost + Damage + cIPlayer.meleeLevel + "% " + Crit + cIPlayer.meleeLevel + "%";
            int CRrequiredExp = cIPlayer.CalculateRequiredExp(cIPlayer.meleeLevel);
            // 当前经验进度表示
            // 显示 战士经验进度： xxxx / xxxxx
            string CurrentExp = Melee + ExpProgress + cIPlayer.meleePool + " / " + CRrequiredExp;
            // 最大经验表示
            string MaxExp = Melee + ExpProgress + requiredExp + " / " + requiredExp;
            // 最大等级判定
            bool maxMeleeLevel = cIPlayer.meleeLevel >= cIPlayer.maxLevel;
            // 绘制进度
            float meleePoolProgress = (float)cIPlayer.meleePool / CRrequiredExp;
            // 绘制常规的边缘
            NorDrawImage(spriteBatch, LevelBoostBorder, PageRightCenter, Page4FirstLine);
            // 绘制经验条
            DrawImage(spriteBatch, LevelBar, PageRightCenter, Page4FirstLine, (int)(LevelBar.Width * meleePoolProgress), LevelBar.Height, 0);
            // 绘制战士标记和当前进度
            NewDrawTextLeftOrigin(spriteBatch, maxMeleeLevel ? MaxExp : CurrentExp, 1f, 1f, Page4NorRightText, Page4NorLeftTextHeight, 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
            NewDrawTextLeftOrigin(spriteBatch, MeleeCurrentBoost, 1f, 1f, Page4NorRightText, Page4NorLeftTextHeight + Page4NorLeftTextHeightOffset, 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
        }
        #endregion
        #region 射手
        public void DrawRangedBoost(SpriteBatch spriteBatch, CalamityInheritancePlayer cIPlayer, Texture2D LevelBoostBorder, Texture2D LevelBar, string Damage, string Crit, string CurrentBoost, string ExpProgress)
        {
            string Range = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelTextRanged");
            // 当前增幅具体显示
            // 表示为 当前增幅：伤害：15% 暴击：15%
            string RangeCurrentBoost = CurrentBoost + Damage + cIPlayer.rangeLevel + "% " + Crit + cIPlayer.rangeLevel + "%";
            int CRrequiredExp = cIPlayer.CalculateRequiredExp(cIPlayer.rangeLevel);
            // 当前经验进度表示
            // 显示 战士经验进度： xxxx / xxxxx
            string CurrentExp = Range + ExpProgress + cIPlayer.rangePool + " / " + CRrequiredExp;
            // 最大经验表示
            string MaxExp = Range + ExpProgress + requiredExp + " / " + requiredExp;

            // 最大等级判定
            bool maxLevel = cIPlayer.rangeLevel >= cIPlayer.maxLevel;
            // 绘制进度
            float PoolProgress = (float)cIPlayer.rangePool / CRrequiredExp;
            // 绘制常规的边缘
            NorDrawImage(spriteBatch, LevelBoostBorder, PageRightCenter, GetLinePos(2, 145));
            // 绘制经验条
            DrawImage(spriteBatch, LevelBar, PageRightCenter, GetLinePos(2, 145), (int)(LevelBar.Width * PoolProgress), LevelBar.Height, 0);
            // 绘制战士标记和当前进度
            NewDrawTextLeftOrigin(spriteBatch, maxLevel ? MaxExp : CurrentExp, 1f, 1f, Page4NorRightText, GetLevelPos(2, 145), 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
            NewDrawTextLeftOrigin(spriteBatch, RangeCurrentBoost, 1f, 1f, Page4NorRightText, GetLevelPos(2, 145) + Page4NorLeftTextHeightOffset, 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
        }
        #endregion
        #region 法师
        public void DrawMagicBoost(SpriteBatch spriteBatch, CalamityInheritancePlayer cIPlayer, Texture2D LevelBoostBorder, Texture2D LevelBar, string Damage, string Crit, string CurrentBoost, string ExpProgress)
        {
            string Range = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelTextMagic");
            // 当前增幅具体显示
            // 表示为 当前增幅：伤害：15% 暴击：15%
            string RangeCurrentBoost = CurrentBoost + Damage + cIPlayer.magicLevel + "% " + Crit + cIPlayer.magicLevel + "%";
            int CRrequiredExp = cIPlayer.CalculateRequiredExp(cIPlayer.magicLevel);
            // 当前经验进度表示
            // 显示 战士经验进度： xxxx / xxxxx
            string CurrentExp = Range + ExpProgress + cIPlayer.magicPool + " / " + CRrequiredExp;
            // 最大经验表示
            string MaxExp = Range + ExpProgress + requiredExp + " / " + requiredExp;

            // 最大等级判定
            bool maxLevel = cIPlayer.magicLevel >= cIPlayer.maxLevel;
            // 绘制进度
            float PoolProgress = (float)cIPlayer.magicPool / CRrequiredExp;
            // 绘制常规的边缘
            NorDrawImage(spriteBatch, LevelBoostBorder, PageRightCenter, GetLinePos(3, 145));
            // 绘制经验条
            DrawImage(spriteBatch, LevelBar, PageRightCenter, GetLinePos(3, 145), (int)(LevelBar.Width * PoolProgress), LevelBar.Height, 0);
            // 绘制战士标记和当前进度
            NewDrawTextLeftOrigin(spriteBatch, maxLevel ? MaxExp : CurrentExp, 1f, 1f, Page4NorRightText, GetLevelPos(3, 145), 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
            NewDrawTextLeftOrigin(spriteBatch, RangeCurrentBoost, 1f, 1f, Page4NorRightText, GetLevelPos(3, 145) + Page4NorLeftTextHeightOffset, 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
        }
        #endregion
        #region 召唤
        public void DrawSummonBoost(SpriteBatch spriteBatch, CalamityInheritancePlayer cIPlayer, Texture2D LevelBoostBorder, Texture2D LevelBar, string Damage, string Crit, string CurrentBoost, string ExpProgress)
        {
            string Summon = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelTextSummon");
            // 当前增幅具体显示
            // 表示为 当前增幅：伤害：15% 暴击：15%
            string SummonCurrentBoost = CurrentBoost + Damage + cIPlayer.summonLevel + "% " + Crit + cIPlayer.summonLevel + "%";
            int CRrequiredExp = cIPlayer.CalculateRequiredExp(cIPlayer.summonLevel);
            // 当前经验进度表示
            // 显示 战士经验进度： xxxx / xxxxx
            string CurrentExp = Summon + ExpProgress + cIPlayer.summonPool + " / " + CRrequiredExp;
            // 最大经验表示
            string MaxExp = Summon + ExpProgress + requiredExp + " / " + requiredExp;

            // 最大等级判定
            bool maxLevel = cIPlayer.summonLevel >= cIPlayer.maxLevel;
            // 绘制进度
            float PoolProgress = (float)cIPlayer.summonPool / CRrequiredExp;
            // 绘制常规的边缘
            NorDrawImage(spriteBatch, LevelBoostBorder, PageRightCenter, GetLinePos(4, 145));
            // 绘制经验条
            DrawImage(spriteBatch, LevelBar, PageRightCenter, GetLinePos(4, 145), (int)(LevelBar.Width * PoolProgress), LevelBar.Height, 0);
            // 绘制战士标记和当前进度
            NewDrawTextLeftOrigin(spriteBatch, maxLevel ? MaxExp : CurrentExp, 1f, 1f, Page4NorRightText, GetLevelPos(4, 145), 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
            NewDrawTextLeftOrigin(spriteBatch, SummonCurrentBoost, 1f, 1f, Page4NorRightText, GetLevelPos(4, 145) + Page4NorLeftTextHeightOffset, 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
        }
        #endregion
        #region 盗贼
        public void DrawRogueBoost(SpriteBatch spriteBatch, CalamityInheritancePlayer cIPlayer, Texture2D LevelBoostBorder, Texture2D LevelBar, string Damage, string Crit, string CurrentBoost, string ExpProgress)
        {
            string Range = Language.GetTextValue("Mods.CalamityInheritance.QolPanel.LevelTextRogue");
            // 当前增幅具体显示
            // 表示为 当前增幅：伤害：15% 暴击：15%
            string RangeCurrentBoost = CurrentBoost + Damage + cIPlayer.rogueLevel + "% " + Crit + cIPlayer.rogueLevel + "%";
            int CRrequiredExp = cIPlayer.CalculateRequiredExp(cIPlayer.rogueLevel);
            // 当前经验进度表示
            // 显示 战士经验进度： xxxx / xxxxx
            string CurrentExp = Range + ExpProgress + cIPlayer.roguePool + " / " + CRrequiredExp;
            // 最大经验表示
            string MaxExp = Range + ExpProgress + requiredExp + " / " + requiredExp;

            // 最大等级判定
            bool maxLevel = cIPlayer.rogueLevel >= cIPlayer.maxLevel;
            // 绘制进度
            float PoolProgress = (float)cIPlayer.roguePool / CRrequiredExp;
            // 绘制常规的边缘
            NorDrawImage(spriteBatch, LevelBoostBorder, PageRightCenter, GetLinePos(5, 145));
            // 绘制经验条
            DrawImage(spriteBatch, LevelBar, PageRightCenter, GetLinePos(5, 145), (int)(LevelBar.Width * PoolProgress), LevelBar.Height, 0);
            // 绘制战士标记和当前进度
            NewDrawTextLeftOrigin(spriteBatch, maxLevel ? MaxExp : CurrentExp, 1f, 1f, Page4NorRightText, GetLevelPos(5, 145), 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
            NewDrawTextLeftOrigin(spriteBatch, RangeCurrentBoost, 1f, 1f, Page4NorRightText, GetLevelPos(5, 145) + Page4NorLeftTextHeightOffset, 1.5f, TextColor, Color.DarkSlateGray, 999f, 0f);
        }
        #endregion
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
