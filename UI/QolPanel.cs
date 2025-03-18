using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.UI
{
    public struct LorePosData
    {
        public int LorePosX;
        public int LorePosY;
        public int LoreBtnX;
        public int LoreBtnY;
    }
    public struct DrawUIData
    {
        public SpriteBatch spriteBatch;
        public Texture2D buttonTextureTrue;
        public Texture2D buttonTextureTrueHover;
        public Texture2D buttonTextureFalse;
        public Texture2D buttonTextureFalseHover;
        public Texture2D buttonTextureUnAvailable;
        public Rectangle mouseRectangle;
        public float xResolutionScale;
        public float yResolutionScale;
        public float scale;
        public bool flipHorizontally;
    }
    public struct DrawLoreData
    {
        public SpriteBatch spriteBatch;
        public Texture2D loreTextureUnAvailable;
        public float scale;
        public float xResolutionScale;
        public float yResolutionScale;
        public bool flipHorizontally;
    }
    public class QolPanel : DraedonsPanelUI
    {
        public static string LoreImagePath => "CalamityInheritance/UI/DraedonsTexture/Lore"; //一个字段a
        // 准备了10个页面
        public override int TotalPages => 10;
        
        #region 状态数组
        public int UIpanelloreExocount = 1;//用于qol面板的星三王传颂计数
        public int exoPanelID = 1;// 星三王面板功能的ID
        //注：这里的排序用的是boss checklist给的boss顺序
        #region 状态计数
        public int KSPanelType = 1;
        public int DSPanelType = 1;
        public int EoCPanelType = 1;
        public int CrabPanelType = 1;
        public int EoWPanelType = 1;
        public int BoCPanelType = 1;
        public int HivePanelType = 1;
        public int PerfPanelType = 1;
        public int QBPanelType = 1;
        public int SkelePanelType = 1;
        public int SGPanelType = 1;
        public int WoFPanelType = 1;
        public int CryoPanelType = 1;
        public int TwinsPanelType = 1;
        public int BrimmyPanelType = 1;
        public int DestroyerPanelType = 1;
        public int ASPanelType = 1;
        public int PrimePanelType = 1;
        public int CalClonePanelType = 1;
        public int PlantPanelType = 1;
        public int AureusPanelType = 1;
        public int LAPanelType = 1;
        public int GolemPanelType = 1;
        public int PBGPanelType = 1; 
        public int RavagerPanelType = 1; 
        public int DukePanelType = 1; 
        public int CultistPanelType = 1; 
        public int DeusPanelType = 1; 
        public int MLPanelType = 1; 
        public int ProviPanelType = 1; 
        public int PolterPanelType = 1; 
        public int ODPanelType = 1; 
        public int DoGPanelType = 1; 
        public int YharonPanelType = 1; 
        public int ExoPanelType = 1; 
        public int SCalPanelType = 1;
        #endregion
        #region 文本ID
        public int KSBtnID = 1;
        public int DSBtnID = 2;
        public int EoCBtnID = 3;
        public int CrabBtnID = 4;
        public int EoWBtnID = 5;
        public int BoCBtnID = 6;
        public int HiveBtnID = 7;
        public int PerfBtnID = 8;
        public int QBBtnID = 9;
        public int SkeleBtnID = 10;
        public int SGBtnID = 11;
        public int WoFBtnID = 12;
        public int CryoBtnID = 13;
        public int TwinsBtnID = 14;
        public int BrimmyBtnID = 15;
        public int DestroyerBtnID = 16;
        public int ASBtnID = 17;
        public int PrimeBtnID = 18;
        public int CalCloneBtnID = 19;
        public int PlantBtnID = 20;
        public int AureusBtnID = 21;
        public int LABtnID = 22;
        public int GolemBtnID = 23;
        public int PBGBtnID = 24; 
        public int RavagerBtnID = 25; 
        public int DukeBtnID = 26; 
        public int CultistBtnID = 27; 
        public int DeusBtnID = 28; 
        public int MLBtnID = 29; 
        public int ProviBtnID = 30; 
        public int PolterBtnID = 31; 
        public int ODBtnID = 32; 
        public int DoGBtnID = 33; 
        public int YharonBtnID = 34; 
        public int ExoBtnID = 35; 
        public int SCalBtnID = 36; 
        #endregion
        #endregion
        /*
        *Scarlet: 制表-绘制格式，下方出现的值都是相对于0差值绝对值
        *对于传颂, 每次绘制都应当在最上方开始
        *水平距离下允许用绝对值计算差值，垂直则严格按照由上到下的方式绘制
        *其中：
        *x1: 500, y1: 300 ->将其绘制在页面最边角
        *x1: 100, y1: 300 ->将其绘制在页面翻页的边角
        *每一个传颂之间的X坐标差值应当预留150的垂直空间
        *对于按钮：
        *x坐标应当与传颂相同，y坐标之间的差值应当为|y1-y2|=100, 便于将按钮绘制在正下方。
        */
        #region 制表格式

        // 将按钮绘制在传颂正下方的差值
        public int LoreButtonDist = 60;
        // 传颂的列距因子
        public int LoreGapX = 85;
        // 传颂的行距因子（考虑按钮）
        public int LoreGapY = 120;

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
            //打表！
            bool DownedAS = DownedBossSystem.downedAquaticScourge;
            bool DownedAureus = DownedBossSystem.downedAstrumAureus;
            bool DownedDeus = DownedBossSystem.downedAstrumDeus;
            bool DownedBoC = Condition.DownedBrainOfCthulhu.IsMet();
            bool DownedBrimmy = DownedBossSystem.downedBrimstoneElemental;
            bool DownedCalClone = DownedBossSystem.downedCalamitasClone;
            bool DownedSCal = DownedBossSystem.downedCalamitas;
            bool DownedCrab = DownedBossSystem.downedCrabulon;
            bool DownedCryo = DownedBossSystem.downedCryogen;
            bool DownedDS = DownedBossSystem.downedDesertScourge;
            bool DownedDestroyer = Condition.DownedDestroyer.IsMet();
            bool DownedDoG = DownedBossSystem.downedDoG;
            bool DownedDuke = Condition.DownedDukeFishron.IsMet();
            bool DownedEoC = Condition.DownedEyeOfCthulhu.IsMet();
            bool DownedEoW = Condition.DownedEaterOfWorlds.IsMet();
            bool DownedExo = DownedBossSystem.downedExoMechs;
            bool DownedGolem = Condition.DownedGolem.IsMet();
            bool DownedHive = DownedBossSystem.downedHiveMind;
            bool DownedKS = Condition.DownedKingSlime.IsMet();
            bool DownedLA = DownedBossSystem.downedLeviathan;
            bool DownedCultist = Condition.DownedCultist.IsMet();
            bool DownedML = Condition.DownedMoonLord.IsMet();
            bool DownedOD = DownedBossSystem.downedBoomerDuke;
            bool DownedPerf = DownedBossSystem.downedPerforator;
            bool DownedPBG = DownedBossSystem.downedPlaguebringer;
            bool DownedPlant = Condition.DownedPlantera.IsMet();
            bool DownedPolter = DownedBossSystem.downedPolterghast;
            bool DownedProvi = DownedBossSystem.downedProvidence;
            bool DownedQB = Condition.DownedQueenBee.IsMet();
            bool DownedRavager = DownedBossSystem.downedRavager;
            bool DownedSkele = Condition.DownedSkeletron.IsMet();
            bool DownedPrime = Condition.DownedSkeletronPrime.IsMet();
            bool DownedSG = DownedBossSystem.downedSlimeGod;
            bool DownedTwins = Condition.DownedTwins.IsMet();
            bool DownedWoF = Condition.Hardmode.IsMet();
            bool DownedYharon = DownedBossSystem.downedYharon;
            #endregion
            #region 什么嘛……不就是打表吗
            Texture2D LoreAS = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreAquaticScourge").Value;
            Texture2D LoreAureus = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreAstrumAureus").Value;
            Texture2D LoreDeus = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreAstrumDeus").Value;
            Texture2D LoreBoC = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreBOC").Value;
            Texture2D LoreBrimmy = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreBrimstoneElemental").Value;
            Texture2D LoreCalClone = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreCalamitasClone").Value;
            Texture2D LoreSCal = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreCalamity").Value;
            Texture2D LoreCrab = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreCrabulon").Value;
            Texture2D LoreCryo = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreCryogen").Value;
            Texture2D LoreDS = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreDesertScourge").Value;
            Texture2D LoreDestroyer = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreDestroyer").Value;
            Texture2D LoreDoG = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreDOG").Value;
            Texture2D LoreDuke = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreDukeFish").Value;
            Texture2D LoreEoC = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreEOC").Value;
            Texture2D LoreEoW = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreEOW").Value;
            Texture2D LoreExo = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreExoMech").Value;
            Texture2D LoreGolem = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreGolem").Value;
            Texture2D LoreHive = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreHiveMind").Value;
            Texture2D LoreKS = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreKingSlime").Value;
            Texture2D LoreLA = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreLAndA").Value;
            Texture2D LoreCultist = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreLunaticCultist").Value;
            Texture2D LoreML = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreMoonLord").Value;
            Texture2D LoreOD = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreOldNuke").Value;
            Texture2D LorePerf = ModContent.Request<Texture2D>($"{LoreImagePath}/LorePerforators").Value;
            Texture2D LorePBG = ModContent.Request<Texture2D>($"{LoreImagePath}/LorePlaguebringerGoliath").Value;
            Texture2D LorePlant = ModContent.Request<Texture2D>($"{LoreImagePath}/LorePlentera").Value;
            Texture2D LorePolter = ModContent.Request<Texture2D>($"{LoreImagePath}/LorePolterghast").Value;
            Texture2D LoreProvi = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreProvidence").Value;
            Texture2D LoreQB = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreQueenBee").Value;
            Texture2D LoreRavager = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreRavager").Value;
            Texture2D LoreSkele = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreSkeletron").Value;
            Texture2D LorePrime = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreSkeletronPrime").Value;
            Texture2D LoreSG = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreSlimeGod").Value;
            Texture2D LoreTwins = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreTwins").Value;
            Texture2D LoreWoF = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreWOF").Value;
            Texture2D LoreYharon = ModContent.Request<Texture2D>($"{LoreImagePath}/LoreYharon").Value;
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
            DrawUIData genericBtonData = GetDrawBtnData(xResolutionScale, yResolutionScale, 0.8f, spriteBatch, buttonTextureTrue, buttonTextureTrueHover, buttonTextureFalse, buttonTextureFalseHover, buttonTextureUnAvailable, mouseRectangle, false);
            DrawLoreData genericLoreData = GetDrawLoreData(spriteBatch, loreTextureUnAvailable, 0.8f, xResolutionScale, yResolutionScale, false);
            // 要绘制在第几页
            if (Page == 1)
            {
                /*
                *Scarlet:
                *尽管打表是按照字母顺序来的
                *但是我们绘制的顺序按照Boss流程来，也就是BossChecklist提供的顺序。
                *这样会比较符合逻辑，而且，我们也不做相关的排序方法
                *总之，开始绘制吧😡
                *其中这里使用的方法的注释：
                *LorePosData = GetLorePos(line, colume, rightsplitePage? = false),
                *行数line应当低于7，列数Colume应当低于7。不包括。
                *如，你需要把史莱姆王的图标绘制在左分页的左上角，你应当输入1，1，然后如下方调用就行
                *Line>= 7 / Colume >=7时Line与Colume会被强制置零。
                *默认情况下优先绘制左分页，如果需要绘制右分页，则rigtSplitPage set为True即可
                *附2:我有想过用数组尝试遍历，但我发现最后还是不如打表。完蛋了。
                */
                #region 遍历-Lore贴图
                CIFunction.DrawLore(genericLoreData, GetLorePos(1,1).LorePosX, GetLorePos(1,1).LorePosY, LoreKS, ref DownedKS);
                CIFunction.DrawLore(genericLoreData, GetLorePos(1,2).LorePosX, GetLorePos(1,2).LorePosY, LoreDS, ref DownedDS);
                CIFunction.DrawLore(genericLoreData, GetLorePos(1,3).LorePosX, GetLorePos(1,3).LorePosY, LoreEoC, ref DownedEoC);
                CIFunction.DrawLore(genericLoreData, GetLorePos(1,4).LorePosX, GetLorePos(1,4).LorePosY, LoreCrab, ref DownedCrab);
                CIFunction.DrawLore(genericLoreData, GetLorePos(1,5).LorePosX, GetLorePos(1,5).LorePosY, LoreEoW, ref DownedEoW);
                CIFunction.DrawLore(genericLoreData, GetLorePos(1,6).LorePosX, GetLorePos(1,6).LorePosY, LoreBoC, ref DownedBoC);
                CIFunction.DrawLore(genericLoreData, GetLorePos(2,1).LorePosX, GetLorePos(2,1).LorePosY, LoreHive, ref DownedHive);
                CIFunction.DrawLore(genericLoreData, GetLorePos(2,2).LorePosX, GetLorePos(2,2).LorePosY, LorePerf, ref DownedPerf);
                CIFunction.DrawLore(genericLoreData, GetLorePos(2,3).LorePosX, GetLorePos(2,3).LorePosY, LoreQB, ref DownedQB);
                CIFunction.DrawLore(genericLoreData, GetLorePos(2,4).LorePosX, GetLorePos(2,4).LorePosY, LoreSkele, ref DownedSkele);
                CIFunction.DrawLore(genericLoreData, GetLorePos(2,5).LorePosX, GetLorePos(2,5).LorePosY, LoreSG, ref DownedSG);
                CIFunction.DrawLore(genericLoreData, GetLorePos(2,6).LorePosX, GetLorePos(2,6).LorePosY, LoreWoF, ref DownedWoF);
                CIFunction.DrawLore(genericLoreData, GetLorePos(3,1).LorePosX, GetLorePos(3,1).LorePosY, LoreCryo, ref DownedCryo);
                CIFunction.DrawLore(genericLoreData, GetLorePos(3,2).LorePosX, GetLorePos(3,2).LorePosY, LoreTwins, ref DownedTwins);
                CIFunction.DrawLore(genericLoreData, GetLorePos(3,3).LorePosX, GetLorePos(3,3).LorePosY, LoreBrimmy, ref DownedBrimmy);
                CIFunction.DrawLore(genericLoreData, GetLorePos(3,4).LorePosX, GetLorePos(3,4).LorePosY, LoreDestroyer, ref DownedDestroyer);
                CIFunction.DrawLore(genericLoreData, GetLorePos(3,5).LorePosX, GetLorePos(3,5).LorePosY, LoreAS, ref DownedAS);
                CIFunction.DrawLore(genericLoreData, GetLorePos(3,6).LorePosX, GetLorePos(3,6).LorePosY, LorePrime, ref DownedPrime);
                CIFunction.DrawLore(genericLoreData, GetLorePos(4,1).LorePosX, GetLorePos(4,1).LorePosY, LoreCalClone, ref DownedCalClone);
                CIFunction.DrawLore(genericLoreData, GetLorePos(4,2).LorePosX, GetLorePos(4,2).LorePosY, LorePlant, ref DownedPlant);
                CIFunction.DrawLore(genericLoreData, GetLorePos(4,3).LorePosX, GetLorePos(4,3).LorePosY, LoreAureus, ref DownedAureus);
                CIFunction.DrawLore(genericLoreData, GetLorePos(4,4).LorePosX, GetLorePos(4,4).LorePosY, LoreLA, ref DownedLA);
                CIFunction.DrawLore(genericLoreData, GetLorePos(4,5).LorePosX, GetLorePos(4,5).LorePosY, LoreGolem, ref DownedGolem);
                CIFunction.DrawLore(genericLoreData, GetLorePos(4,6).LorePosX, GetLorePos(4,6).LorePosY, LorePBG, ref DownedPBG);
                CIFunction.DrawLore(genericLoreData, GetLorePos(5,1).LorePosX, GetLorePos(5,1).LorePosY, LoreDuke, ref DownedDuke);
                CIFunction.DrawLore(genericLoreData, GetLorePos(5,2).LorePosX, GetLorePos(5,2).LorePosY, LoreRavager, ref DownedRavager);
                CIFunction.DrawLore(genericLoreData, GetLorePos(5,3).LorePosX, GetLorePos(5,3).LorePosY, LoreCultist, ref DownedCultist);
                CIFunction.DrawLore(genericLoreData, GetLorePos(5,4).LorePosX, GetLorePos(5,4).LorePosY, LoreDeus, ref DownedDeus);
                CIFunction.DrawLore(genericLoreData, GetLorePos(5,5).LorePosX, GetLorePos(5,5).LorePosY, LoreML, ref DownedML);
                CIFunction.DrawLore(genericLoreData, GetLorePos(5,6).LorePosX, GetLorePos(5,6).LorePosY, LoreProvi, ref DownedProvi);
                CIFunction.DrawLore(genericLoreData, GetLorePos(5,1).LorePosX, GetLorePos(6,1).LorePosY, LorePolter, ref DownedPolter);
                CIFunction.DrawLore(genericLoreData, GetLorePos(6,2).LorePosX, GetLorePos(6,2).LorePosY, LoreOD, ref DownedOD);
                CIFunction.DrawLore(genericLoreData, GetLorePos(6,3).LorePosX, GetLorePos(6,3).LorePosY, LoreDoG, ref DownedDoG);
                CIFunction.DrawLore(genericLoreData, GetLorePos(6,4).LorePosX, GetLorePos(6,4).LorePosY, LoreYharon, ref DownedYharon);
                CIFunction.DrawLore(genericLoreData, GetLorePos(6,5).LorePosX, GetLorePos(6,5).LorePosY, LoreExo, ref DownedExo);
                CIFunction.DrawLore(genericLoreData, GetLorePos(6,6).LorePosX, GetLorePos(6,6).LorePosY, LoreSCal, ref DownedSCal);
                #endregion
                #region 按钮绘制
                CIFunction.DrawBton(genericBtonData, GetLorePos(1,1).LoreBtnX, GetLorePos(1,1).LoreBtnY, ref DownedKS, ref KSPanelType, ref KSBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(1,2).LoreBtnX, GetLorePos(1,2).LoreBtnY, ref DownedDS, ref DSPanelType, ref DSBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(1,3).LoreBtnX, GetLorePos(1,3).LoreBtnY, ref DownedEoC, ref EoCPanelType, ref EoCBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(1,4).LoreBtnX, GetLorePos(1,4).LoreBtnY, ref DownedCrab, ref CrabPanelType, ref CrabBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(1,5).LoreBtnX, GetLorePos(1,5).LoreBtnY, ref DownedEoW, ref EoWPanelType, ref EoWBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(1,6).LoreBtnX, GetLorePos(1,6).LoreBtnY, ref DownedBoC, ref BoCPanelType, ref BoCBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(2,1).LoreBtnX, GetLorePos(2,1).LoreBtnY, ref DownedHive, ref HivePanelType, ref HiveBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(2,2).LoreBtnX, GetLorePos(2,2).LoreBtnY, ref DownedPerf, ref PerfPanelType, ref PerfBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(2,3).LoreBtnX, GetLorePos(2,3).LoreBtnY, ref DownedQB, ref QBPanelType, ref QBBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(2,4).LoreBtnX, GetLorePos(2,4).LoreBtnY, ref DownedSkele, ref SkelePanelType, ref SkeleBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(2,5).LoreBtnX, GetLorePos(2,5).LoreBtnY, ref DownedSG, ref SGPanelType, ref SGBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(2,6).LoreBtnX, GetLorePos(2,6).LoreBtnY, ref DownedWoF, ref WoFPanelType, ref WoFBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(3,1).LoreBtnX, GetLorePos(3,1).LoreBtnY, ref DownedCryo, ref CryoPanelType, ref CryoBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(3,2).LoreBtnX, GetLorePos(3,2).LoreBtnY, ref DownedTwins, ref TwinsPanelType, ref TwinsBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(3,3).LoreBtnX, GetLorePos(3,3).LoreBtnY, ref DownedBrimmy, ref BrimmyPanelType, ref BrimmyBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(3,4).LoreBtnX, GetLorePos(3,4).LoreBtnY, ref DownedDestroyer, ref DestroyerPanelType, ref DestroyerBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(3,5).LoreBtnX, GetLorePos(3,5).LoreBtnY, ref DownedAS, ref ASPanelType, ref ASBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(3,6).LoreBtnX, GetLorePos(3,6).LoreBtnY, ref DownedPrime, ref PrimePanelType, ref PrimeBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(4,1).LoreBtnX, GetLorePos(4,1).LoreBtnY, ref DownedCalClone, ref CalClonePanelType, ref CalCloneBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(4,2).LoreBtnX, GetLorePos(4,2).LoreBtnY, ref DownedPlant, ref PlantPanelType, ref PlantBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(4,3).LoreBtnX, GetLorePos(4,3).LoreBtnY, ref DownedAureus, ref AureusPanelType, ref AureusBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(4,4).LoreBtnX, GetLorePos(4,4).LoreBtnY, ref DownedLA, ref LAPanelType, ref LABtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(4,5).LoreBtnX, GetLorePos(4,5).LoreBtnY, ref DownedGolem, ref GolemPanelType, ref GolemBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(4,6).LoreBtnX, GetLorePos(4,6).LoreBtnY, ref DownedPBG, ref PBGPanelType, ref PBGBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(5,1).LoreBtnX, GetLorePos(5,1).LoreBtnY, ref DownedDuke, ref DukePanelType, ref DukeBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(5,2).LoreBtnX, GetLorePos(5,2).LoreBtnY, ref DownedRavager, ref RavagerPanelType, ref RavagerBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(5,3).LoreBtnX, GetLorePos(5,3).LoreBtnY, ref DownedCultist, ref CultistPanelType, ref CultistBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(5,4).LoreBtnX, GetLorePos(5,4).LoreBtnY, ref DownedDeus, ref DeusPanelType, ref DeusBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(5,5).LoreBtnX, GetLorePos(5,5).LoreBtnY, ref DownedML, ref MLPanelType, ref MLBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(5,6).LoreBtnX, GetLorePos(5,6).LoreBtnY, ref DownedProvi, ref ProviPanelType, ref ProviBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(6,1).LoreBtnX, GetLorePos(6,1).LoreBtnY, ref DownedPolter, ref PolterPanelType, ref PolterBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(6,2).LoreBtnX, GetLorePos(6,2).LoreBtnY, ref DownedOD, ref ODPanelType, ref ODBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(6,3).LoreBtnX, GetLorePos(6,3).LoreBtnY, ref DownedDoG, ref DoGPanelType, ref DoGBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(6,4).LoreBtnX, GetLorePos(6,4).LoreBtnY, ref DownedYharon, ref YharonPanelType, ref YharonBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(6,5).LoreBtnX, GetLorePos(6,5).LoreBtnY, ref DownedEoC, ref exoPanelID, ref ExoBtnID);
                CIFunction.DrawBton(genericBtonData, GetLorePos(6,6).LoreBtnX, GetLorePos(6,6).LoreBtnY, ref DownedSCal, ref SCalPanelType, ref SCalBtnID);
                #endregion
            }
        }
        /// <summary>
        /// 这要是不封装我只会疯掉的.用于存放绘制按钮的数据
        /// </summary>
        /// <param name="xResolutionScale">X缩放尺寸</param>
        /// <param name="yResolutionScale">Y缩放尺寸</param>
        /// <param name="scale">尺寸</param>
        /// <param name="spriteBatch">绘制</param>
        /// <param name="buttonTextureTrue">按钮可使用用贴图</param>
        /// <param name="buttonTextureTrueHover">按钮可用悬浮贴图</param>
        /// <param name="buttonTextureFalse">按钮不可使用用贴图</param>
        /// <param name="buttonTextureFlaseHover">按钮不可用悬浮贴图</param>
        /// <param name="buttonTextureUnAvailable">按钮不可用状态贴图</param>
        /// <param name="mouseRectangle">鼠标判定</param>
        /// <returns>一个具有上述所有数据的巨大结构体</returns>
        public DrawUIData GetDrawBtnData(float xResolutionScale, float yResolutionScale, float scale ,SpriteBatch spriteBatch, Texture2D buttonTextureTrue, Texture2D buttonTextureTrueHover,Texture2D buttonTextureFalse, Texture2D buttonTextureFlaseHover, Texture2D buttonTextureUnAvailable, Rectangle mouseRectangle, bool canFlip)
        {
            DrawUIData newDraw;
            newDraw.spriteBatch = spriteBatch;
            newDraw.buttonTextureTrue = buttonTextureTrue;
            newDraw.buttonTextureTrueHover = buttonTextureTrueHover;
            newDraw.buttonTextureFalse = buttonTextureFalse;
            newDraw.buttonTextureFalseHover = buttonTextureFlaseHover;
            newDraw.buttonTextureUnAvailable = buttonTextureUnAvailable;
            newDraw.mouseRectangle = mouseRectangle;
            newDraw.xResolutionScale = xResolutionScale;
            newDraw.yResolutionScale = yResolutionScale;
            newDraw.flipHorizontally = canFlip;
            newDraw.scale =scale;
            return newDraw;
        }
        /// <summary>
        /// 存放绘制传颂贴图的数据
        /// </summary>
        /// <param name="spriteBatch">贴图</param>
        /// <param name="loreTextureUnAvailable">传颂不可用</param>
        /// <param name="scale">尺寸</param>
        /// <param name="xResolutionScale">x缩放</param>
        /// <param name="yResolutionScale">y缩放</param>
        /// <param name="canFlip">是否镜像</param>
        /// <returns></returns>
        public DrawLoreData GetDrawLoreData(SpriteBatch spriteBatch, Texture2D loreTextureUnAvailable, float scale, float xResolutionScale, float yResolutionScale, bool canFlip)
        {
            DrawLoreData newData;
            newData.spriteBatch = spriteBatch;
            newData.loreTextureUnAvailable = loreTextureUnAvailable;
            newData.scale = scale;
            newData.xResolutionScale = xResolutionScale;
            newData.yResolutionScale = yResolutionScale;
            newData.flipHorizontally = canFlip;
            return newData;
        }
        /// <summary>
        /// 用于获取lore的坐标信息。或者用自然语言来说，就是希望这个lore出现在第几行第几列
        /// </summary>
        /// <param name="line">你需要的行数，不超过6行</param>
        /// <param name="colume">你需要的列数，不超过6列</param>
        /// <param name="needOffset">增加lore的行距，除非你要绘制第一行，否则默认为true，主要是要预留空间绘制按钮。</param>
        /// <param name="rightSplitPage">是否需要绘制在右半分页，我们默认取左半分页</param>
        /// <returns></returns>
        public LorePosData GetLorePos(int line, int colume, bool? rightSplitPage = false)
        {
            /*
            我们需要借助制表格式去间接绘制一个lore的位置，其次就是按钮
            但，幸运的是我们的按钮绘制并没有那么困难。
            rightSplitPage=false时，将其绘制在左半页面
            我们一次只考虑一个分页，因此不会让列数大于5，不然我们就能看到传颂挎着页面了
            */
            if (!rightSplitPage.Value)
                //如果colume被错误的尝试获得大于7的值的话这里会被置零也只能置零，主要是防止如果强行退出容易崩游戏
                colume = colume < 7 ? 7 - colume : 0;

                //如果需要在右半分页进行绘制的话就会执行这个程式
            if (rightSplitPage.Value)
                colume = colume < 7 ?  -colume : 0;
            /*
            行数，由于页面的大小可能只能用上6行, 并且因为绘制的坐标问题，这里有一点弯绕
            因为锚点位于0的位置，而页面最上方相当于负坐标，因此如果要呈现（6）行的效果，这里的每一行都得有一个对应
            具体来说，如果你输入的行数不大于3，则所有的输入的行数都会被减去4，如你输入第一行则最终得到-3的值
            然后，他就会被绘制在页面最上方的位置，呈现出第一行的效果。
            如果你输入的行数大于了3，即以输入了4为例子。此时会直接在正中间作画。呈现出第四行的效果
            然后如此递推
            */
            line = line > 3 ? line - 4 : line - 4;

            //同样，如果输入的行数大于6行，强行置零
            if (line > 6) line = 0;
            //赋值给结构体
            LorePosData newData = new()
            {
                LorePosX = -LoreGapX * colume,
                //行距因子已经默认给按钮绘制预留了空间。
                LorePosY = LoreGapY * line + 50,
            };
            //按钮的水平坐标应当与传颂之物的水平坐标一致， 垂直坐标则默认加上这个传颂与按钮的差值
            newData.LoreBtnX = newData.LorePosX;
            newData.LoreBtnY = newData.LorePosY + LoreButtonDist;
            return newData;
        }
    }
}
