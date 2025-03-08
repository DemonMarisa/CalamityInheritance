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
        public static Asset<Texture2D> ArkoftheCosmosNew;
        public static Asset<Texture2D> ArkoftheCosmosOld;
        #endregion

        #region 庇护之刃
        public static Asset<Texture2D> CalAegis;
        public static Asset<Texture2D> AltAegis;
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
        #region 碎颅者
        public static Asset<Texture2D> Skullmasher1p5;
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
        public static Asset<Texture2D> HeliumFlashCalamity;
        public static Asset<Texture2D> HeliumFlashLegacy;
        #endregion
        #endregion
        #region 召唤
        #endregion
        #region 盗贼
        #region 苍穹,圣光飞刀
        public static Asset<Texture2D> EmpyreanKnivesCalamity; //苍穹飞刀的第二版本贴图(现在的版本)
        public static Asset<Texture2D> EmpyreanKnivesCalamityProj; //苍穹飞刀的第二版射弹(现在的版本)
        //下面的编排与上方的一样。
        public static Asset<Texture2D> EmpyreanKnivesAlterTypeOne;  //苍穹飞刀的初版贴图
        public static Asset<Texture2D> EmpyreanKnivesAlterTypeOneProj;
        public static Asset<Texture2D> ShadowspecKnivesCalamity; //圣光飞刀灾厄贴图
        public static Asset<Texture2D> ShadowspecKnivesCalamityProj;
        public static Asset<Texture2D> ShadowspecKnivesAlterThird;  //圣光飞刀的第三版贴图
        public static Asset<Texture2D> ShadowspecKnivesAlterThirdProj;
        public static Asset<Texture2D> ShadowspecKnivesAlterSec; //圣光飞刀的二版贴图
        public static Asset<Texture2D> ShadowspecKnivesAlterSecProj;
        public static Asset<Texture2D> ShadowspecKnivesAlterFirst; //圣光飞刀的初版贴图
        public static Asset<Texture2D> ShadowspecKnivesAlterFirstProj;
        #endregion
        #endregion
        #region 战/盗混合
        #region 圣时之锤
        public static Asset<Texture2D> HallowedHammerCalamity;
        public static Asset<Texture2D> HallowedHammerCalamityProj;
        public static Asset<Texture2D> HallowedHammerAlter;
        public static Asset<Texture2D> HallowedHammerAlterProj;
        #endregion
        #region 星体击碎者
        public static Asset<Texture2D> StellarContemptNew; //星体击碎者灾厄贴图
        public static Asset<Texture2D> StellarContemptOld;
        #endregion
        #region 圣泰阿克提斯之锤
        public static Asset<Texture2D> TriactisHammerAlter;
        public static Asset<Texture2D> TriactisHammerAlterProj;
        public static Asset<Texture2D> TriactisHammerCalamity;
        public static Asset<Texture2D> TriactisHammerCalamityProj;
        #endregion
        #endregion
        #region 其他
        #endregion
        #endregion

        public static void LoadTexture()
        {
            #region 战士
            #region 方舟
            ArkoftheCosmosNew = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/ArkoftheCosmosNew");
            ArkoftheCosmosOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/ArkoftheCosmosold");
            #endregion

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

            #region 庇护
            CalAegis = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Melee/AegisBlade");
            AltAegis = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/AegisBladeLegacy");
            #endregion

            #region 月明链刃
            CerscentMoonProjCal = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Melee/CrescentMoonFlail");
            CerscentMoonProjAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/CerscentFlailLegay");

            #endregion
            #endregion

            #region 远程
            #region 碎颅
            Skullmasher1p5= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Ranged/Skullmasher1p5");
            Skullmasher   = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/Skullmasher");
            #endregion

            #region 龙弓
            DrataliornusLegacyAlter = ModContent.Request<Texture2D>("CalamityMod/Items/Weapons/Ranged/Drataliornus");
            DrataliornusLegacy = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/DrataliornusLegacy");
            #endregion

            #region P90
            P90 = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Ranged/P90");
            P90Legacy = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/P90Legacy");
            #endregion
            
            #region 哈雷
            HalleyCal = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/HalleysInfernoLegacy");
            HalleyAlt = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Ranged/HalleysLegacy");
            #endregion
            #endregion

            #region 法师

            #region 氦闪
            HeliumFlashCalamity = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Magic/HeliumFlashCalamity");
            HeliumFlashLegacy = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Magic/HeliumFlashLegacy");
            #endregion

            #endregion

            #region 召唤
            #endregion

            #region 盗贼

            #region 圣泰阿克提斯之锤
            TriactisHammerCalamity = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/TriactisHammerCalamity");
            TriactisHammerCalamityProj = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/TriactisHammerCalamity");
            TriactisHammerAlter = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerTriactisTruePaladinianMageHammerofMight");
            TriactisHammerAlterProj = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerTriactisTruePaladinianMageHammerofMight");
            #endregion 

            #region 飞刀
            //苍穹飞刀(现)
            EmpyreanKnivesCalamity = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeKnivesEmpyrean");
            EmpyreanKnivesCalamityProj= ModContent.Request<Texture2D>("CalamityInheritance/Content/Projectiles/Rogue/RogueTypeKnivesEmpyreanProj");
            //苍穹飞刀(初)
            EmpyreanKnivesAlterTypeOne = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/EmpyreanKnivesAlterFirst");
            EmpyreanKnivesAlterTypeOneProj= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/EmpyreanKnivesAlterFirstProj");
            //圣光飞刀(现)
            ShadowspecKnivesCalamity= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/ShadowspecKnivesCalamity");
            ShadowspecKnivesCalamityProj= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/ShadowspecKnivesCalamityProj");
            //圣光飞刀(三)
            ShadowspecKnivesAlterThird= ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeKnivesShadowspec");
            ShadowspecKnivesAlterThirdProj= ModContent.Request<Texture2D>("CalamityInheritance/Content/Projectiles/Rogue/RogueTypeKnivesShadowspecProj");
            //圣光飞刀(二)
            ShadowspecKnivesAlterSec= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/ShadowspecKnivesAlterSec");
            ShadowspecKnivesAlterSecProj= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/ShadowspecKnivesAlterSecProj");
            //圣光飞刀(初)
            ShadowspecKnivesAlterFirst= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/ShadowspecKnivesAlterFirst");
            ShadowspecKnivesAlterFirstProj= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/EmpyreanKnivesAlterFirstProj");
            #endregion

            #endregion
            
            #region 战/盗混合
            #region 圣时之锤
            HallowedHammerCalamity = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/MeleeTypeHammerPwnageLegacy");
            HallowedHammerCalamityProj = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/MeleeTypeHammerPwnageLegacy");
            HallowedHammerAlter = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/PwnagehammerAlter");
            HallowedHammerAlterProj = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/PwnagehammerAlter");
            #endregion

            #region 星体击碎者
            StellarContemptNew= ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/MeleeTypeHammerStellarContemptLegacy");
            StellarContemptOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerStellarContempt");
            #endregion

            #endregion
            
            #region 其他
            #endregion
        }
        public static void UnloadTexture()
        {
            HeliumFlashCalamity = null;
            HeliumFlashLegacy = null;

            ArkoftheCosmosNew = null;
            ArkoftheCosmosOld = null;
            EmpyreanKnivesCalamity = null;
            EmpyreanKnivesCalamityProj = null;
            EmpyreanKnivesAlterTypeOne = null;
            EmpyreanKnivesAlterTypeOneProj = null;
            ShadowspecKnivesCalamity = null;
            ShadowspecKnivesCalamityProj = null;
            ShadowspecKnivesAlterThird= null;
            ShadowspecKnivesAlterThirdProj= null;
            ShadowspecKnivesAlterSec= null;
            ShadowspecKnivesAlterSecProj= null;
            ShadowspecKnivesAlterThird= null;
            ShadowspecKnivesAlterThirdProj= null;
            
            Skullmasher1p5 = null;
            Skullmasher    = null;

            P90 = null;
            P90Legacy = null;

            TriactisHammerCalamity = null;
            TriactisHammerAlter = null;

            //星体击碎者
            StellarContemptNew = null;
            StellarContemptOld = null;

            DrataliornusLegacyAlter = null;
            DrataliornusLegacy = null;

            HalleyCal = null;
            HalleyAlt = null;

            CalAegis = null;
            AltAegis = null;

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
        }
    }
}