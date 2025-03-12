using log4net.Core;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public class CIWeaponsResprite: ModPlayer
    {
        #region 武器切换
        #region 战士

        #region 方舟
        public static Asset<Texture2D> AotCCal;
        public static Asset<Texture2D> AotCAlt;
        #endregion

        #region 庇护之刃
        public static Asset<Texture2D>  AegisCal;
        public static Asset<Texture2D> AegisAlt;
        #endregion

        #region 元素系列
        public static Asset<Texture2D> ElemDiskCal;
        public static Asset<Texture2D> ElemDiskAlt;
        public static Asset<Texture2D> ElemSwordCal;
        public static Asset<Texture2D> ElemSwordAlt;
        public static Asset<Texture2D> ElemLanceCal;
        public static Asset<Texture2D> ElemLanceAlt;
        public static Asset<Texture2D> ElemLanceProjCal;
        public static Asset<Texture2D> ElemLanceProjAlt;
        #endregion
        #region 短剑系列
        //圣短剑
        public static Asset<Texture2D> CaliburCal;
        public static Asset<Texture2D> CaliburAlt;
        //真圣
        public static Asset<Texture2D> TrueCaliburCal;
        public static Asset<Texture2D> TrueCaliburAlt;
        //元素
        public static Asset<Texture2D> ElemShivCal;
        public static Asset<Texture2D> ElemShivAlt;
        //伽利略
        public static Asset<Texture2D> GalileoCal;
        public static Asset<Texture2D> GalileoAlt;
        //永夜
        public static Asset<Texture2D> NightCal;
        public static Asset<Texture2D> NightAlt;
        //真永夜
        public static Asset<Texture2D> TrueNightCal;
        public static Asset<Texture2D> TrueNightAlt;
        #endregion

        #region 月明链刃
        public static Asset<Texture2D> CerscentMoonProjCal;
        public static Asset<Texture2D> CerscentMoonProjAlt;
        #endregion

        #endregion
        #region 远程
        //泰拉弓
        public static Asset<Texture2D> TerraBowCal;
        public static Asset<Texture2D> TerraBowAlt;
        public static Asset<Texture2D> ElemBYDCal;
        public static Asset<Texture2D> ElemBYDAlt;
        #region 碎颅者
        public static Asset<Texture2D> SkullmasherCal;
        public static Asset<Texture2D> Skullmasher;
        #endregion
        #region P90
        public static Asset<Texture2D> P90;
        public static Asset<Texture2D> P90Legacy;
        #endregion
        #region 龙弓
        public static Asset<Texture2D> DrataliornusLegacy;
        public static Asset<Texture2D> DrataliornusLegacyAlter;
        #endregion
        public static Asset<Texture2D> HalleyCal;
        public static Asset<Texture2D> HalleyAlt;
        #endregion
        #region 法师
        #region 氦闪 
        public static Asset<Texture2D> HeliumCal;
        public static Asset<Texture2D> HeliumFlashLegacy;
        public static Asset<Texture2D> ElemRayCal;
        public static Asset<Texture2D> ElemRayAlt;
        #endregion
        #endregion
        #region 召唤
        //小花法杖
        public static Asset<Texture2D> PlantareCal;
        public static Asset<Texture2D> PlantareAlt;
        #endregion
        #region 盗贼
        #region 苍穹,圣光飞刀
        public static Asset<Texture2D> EmpyreanKnivesCal; //苍穹飞刀的第二版本贴图(现在的版本)
        public static Asset<Texture2D> EmpyreanKnivesCalProj; //苍穹飞刀的第二版射弹(现在的版本)
        //下面的编排与上方的一样。
        public static Asset<Texture2D> EmpyreanKnivesAlt;  //苍穹飞刀的初版贴图
        public static Asset<Texture2D> EmpyreanKnivesAltProj;
        public static Asset<Texture2D> ShadowKnivesCal; //圣光飞刀灾厄贴图
        public static Asset<Texture2D> ShadowKnivesCalProj;
        public static Asset<Texture2D> ShadowKnivsAlt3;  //圣光飞刀的第三版贴图
        public static Asset<Texture2D> ShadowKnivsAlt3Proj;
        public static Asset<Texture2D> ShadowKnivsAlt2; //圣光飞刀的二版贴图
        public static Asset<Texture2D> ShadowKnivsAlt2Proj;
        public static Asset<Texture2D> ShadowKnivsAlt1; //圣光飞刀的初版贴图
        public static Asset<Texture2D> ShadowKnivsAlt1Proj;
        #endregion
        #endregion
        #region 战/盗混合

        #region 圣时之锤
        public static Asset<Texture2D> PwnageHammerCal;
        public static Asset<Texture2D> PwnageHammerCalProj;
        public static Asset<Texture2D> PwnageHammerAlt;
        public static Asset<Texture2D> PwnageHammerAltProj;
        #endregion
        #region 星体击碎者
        public static Asset<Texture2D> StellarContemptNew; //星体击碎者灾厄贴图
        public static Asset<Texture2D> StellarContemptOld;
        #endregion
        #region 圣泰阿克提斯之锤
        public static Asset<Texture2D> GiantHammerAlt;
        public static Asset<Texture2D> GiantHammerAltProj;
        public static Asset<Texture2D> GiantHammerCal;
        public static Asset<Texture2D> GiantHammerCalProj;
        #endregion
        #endregion
        #region 其他
        #endregion
        #endregion

        public static void LoadTexture()
        {
            #region 战士
            //方舟
            AotCCal = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/ArkoftheCosmosNew");
            AotCAlt = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/ArkoftheCosmosold");
            #endregion
            //元素系列
            ElemDiskCal = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/MeleeTypeElementalDisk");
            ElemDiskAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/ElemDiskAlt");
            ElemSwordCal = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/ArkoftheElementsold");
            ElemSwordAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/ElemSword");


            ElemLanceCal = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Melee/ElementalLance");
            ElemLanceProjCal = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Melee/Spears/ElementalLanceProjectile");
            ElemLanceAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/ElemLanceAlt");
            ElemLanceProjAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/ElemLanceProj");
        
            #region 短剑系列
            //圣短剑
            CaliburCal = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/Shortsword/ExcaliburShortsword");
            CaliburAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/CaliburLegacy");
            TrueCaliburCal = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/Shortsword/TrueExcaliburShortsword");
            TrueCaliburAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/TrueCaliburAlt");
            //永夜
            NightCal = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/Shortsword/NightsStabber");
            NightAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/NightLegacy");
            //真永夜
            TrueNightCal = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/Shortsword/TrueNightsStabber");
            TrueNightAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/TrueNightLegacy");
            //伽利略
            GalileoCal = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Melee/GalileoGladius");
            GalileoAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/GalileoLegacy");
            //元素
            ElemShivCal = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/Shortsword/ElementalShivold");
            ElemShivAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/ElementalShivLegacy");
            #endregion

            //庇护
             AegisCal = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Melee/AegisBlade");
            AegisAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/AegisBladeLegacy");


            //月明链刃
            CerscentMoonProjCal = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Melee/CrescentMoonFlail");
            CerscentMoonProjAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/CerscentFlailLegay");

            #region 远程
            //元素BYD
            ElemBYDCal = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Ranged/ElementalBlaster");
            ElemBYDAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Ranged/ElemBYD");

            //碎颅
            SkullmasherCal= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Ranged/SkullmasherAlt");
            Skullmasher   = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/Skullmasher");

            //龙弓
            DrataliornusLegacyAlter = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Ranged/Drataliornus");
            DrataliornusLegacy = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/DrataliornusLegacy");

            //P90
            P90 = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Ranged/P90Cal");
            P90Legacy = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/P90Legacy");

            
            //哈雷
            HalleyCal = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/HalleysInfernoLegacy");
            HalleyAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Ranged/HalleysLegacy");
            #endregion

            #region 法师
            //元素
            ElemRayCal = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Magic/Ray/ElementalRayold");
            ElemRayAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Magic/ElemRayAlt");
            //氦闪
            HeliumCal = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Magic/HeliumCal");
            HeliumFlashLegacy = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Magic/HeliumFlashLegacy");

            #endregion

            #region 召唤
            PlantareCal = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Summon/PlantareAlt");
            #endregion

            #region 盗贼

            //圣泰阿克提斯之锤
            GiantHammerCal = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/GiantHammerCal");
            GiantHammerCalProj = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/GiantHammerCal");
            GiantHammerAlt = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerTriactisTruePaladinianMageHammerofMight");
            GiantHammerAltProj = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerTriactisTruePaladinianMageHammerofMight");

            #region 飞刀
            //苍穹飞刀(现)
            EmpyreanKnivesCal = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeKnivesEmpyrean");
            EmpyreanKnivesCalProj= ModContent.Request<Texture2D>("CalamityInheritance/Content/Projectiles/Rogue/RogueTypeKnivesEmpyreanProj");
            //苍穹飞刀(初)
            EmpyreanKnivesAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/EmpyreanKnivesAlterFirst");
            EmpyreanKnivesAltProj= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/EmpyreanKnivesAlterFirstProj");
            //圣光飞刀(现)
            ShadowKnivesCal= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/ShadowKnivesCal");
            ShadowKnivesCalProj= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/ShadowKnivesCalProj");
            //圣光飞刀(三)
            ShadowKnivsAlt3= ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeKnivesShadowspec");
            ShadowKnivsAlt3Proj= ModContent.Request<Texture2D>("CalamityInheritance/Content/Projectiles/Rogue/RogueTypeKnivesShadowspecProj");
            //圣光飞刀(二)
            ShadowKnivsAlt2= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/ShadowKnivesAlt2");
            ShadowKnivsAlt2Proj= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/ShadowKnivesAlt2Proj");
            //圣光飞刀(初)
            ShadowKnivsAlt1= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/ShadowKnivesAlt1");
            ShadowKnivsAlt1Proj= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/EmpyreanKnivesAlterFirstProj");
            #endregion

            #endregion
            
            #region 战/盗混合
            //圣时之锤
            PwnageHammerCal = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/MeleeTypeHammerPwnageLegacy");
            PwnageHammerCalProj = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/MeleeTypeHammerPwnageLegacy");
            PwnageHammerAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/PwnagehammerAlter");
            PwnageHammerAltProj = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/PwnagehammerAlter");
      

            //星体击碎者
            StellarContemptNew= ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/MeleeTypeHammerStellarContemptLegacy");
            StellarContemptOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerStellarContempt");

            #endregion
            
            #region 其他
            #endregion
        }
        public static void UnloadTexture()
        {
            HeliumCal = null;
            HeliumFlashLegacy = null;

            AotCCal = null;
            AotCAlt = null;
            EmpyreanKnivesCal = null;
            EmpyreanKnivesCalProj = null;
            EmpyreanKnivesAlt = null;
            EmpyreanKnivesAltProj = null;
            ShadowKnivesCal = null;
            ShadowKnivesCalProj = null;
            ShadowKnivsAlt3= null;
            ShadowKnivsAlt3Proj= null;
            ShadowKnivsAlt2= null;
            ShadowKnivsAlt2Proj= null;
            ShadowKnivsAlt3= null;
            ShadowKnivsAlt3Proj= null;
            
            SkullmasherCal = null;
            Skullmasher    = null;

            P90 = null;
            P90Legacy = null;

            GiantHammerCal = null;
            GiantHammerAlt = null;

            //星体击碎者
            StellarContemptNew = null;
            StellarContemptOld = null;

            DrataliornusLegacyAlter = null;
            DrataliornusLegacy = null;

            HalleyCal = null;
            HalleyAlt = null;

             AegisCal = null;
            AegisAlt = null;

            ElemShivAlt = null;
            ElemShivCal = null;

            GalileoAlt = null;
            GalileoCal  = null;

            CaliburCal = null;
            CaliburAlt = null;

            NightCal = null;
            NightAlt = null;

            TrueNightAlt = null;
            TrueNightCal = null;

            CerscentMoonProjCal = null;
            CerscentMoonProjAlt = null;

            ElemBYDAlt = null;
            ElemBYDCal = null;
            ElemDiskAlt = null;
            ElemDiskCal = null;
            ElemLanceAlt = null;
            ElemLanceProjAlt = null;
            ElemLanceCal = null;
            ElemLanceProjCal = null;
            ElemSwordAlt = null;
            ElemSwordCal = null;
            ElemRayAlt = null;
            ElemRayCal = null;
            PlantareCal = null;
            PlantareAlt = null;
        }
    }
}