using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public class CIWeaponsResprite: ModPlayer
    {
        #region 武器切换
        #region 战士
        //泰拉矛
        public static Asset<Texture2D> TerraLanceCal;
        public static Asset<Texture2D> TerraLanceCalGlow;
        public static Asset<Texture2D> TerraLanceCalProj;
        public static Asset<Texture2D> TerraLanceAlt;
        public static Asset<Texture2D> TerraLanceAltProj;
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
        public static Asset<Texture2D> RareArkCal;
        public static Asset<Texture2D> RareArkAlt;
        public static Asset<Texture2D> ElemLanceCal;
        public static Asset<Texture2D> ElemLanceAlt;
        public static Asset<Texture2D> ElemLanceProjCal;
        public static Asset<Texture2D> ElemLanceProjAlt;
        #endregion
        #region 短剑系列
        public static Asset<Texture2D> FlameShivCal;
        public static Asset<Texture2D> FlameShivAlt;

        public static Asset<Texture2D> SeaShivCal;
        public static Asset<Texture2D> SeaShivAlt;
        public static Asset<Texture2D> DungeonShivCal;
        public static Asset<Texture2D> DungeonShivAlt;
        public static Asset<Texture2D> HiveMindShivCal;
        public static Asset<Texture2D> HiveMindShivAlt;
    
        //圣短剑
        public static Asset<Texture2D> HallowedShivCal;
        public static Asset<Texture2D> HallowedShivAlt;
        //真圣
        public static Asset<Texture2D> TrueHallowedShivCal;
        public static Asset<Texture2D> TrueHallowedShivAlt;
        //元素
        public static Asset<Texture2D> ElemShivCal;
        public static Asset<Texture2D> ElemShivAlt;
        //伽利略
        public static Asset<Texture2D> GalileoCal;
        public static Asset<Texture2D> GalileoAlt;
        //永夜
        public static Asset<Texture2D> NightShivCal;
        public static Asset<Texture2D> NightShivAlt;
        //真永夜
        public static Asset<Texture2D> TrueNightShivCal;
        public static Asset<Texture2D> TrueNightShivAlt;
        #endregion
        #region 月明链刃
        public static Asset<Texture2D> CerscentMoonProjCal;
        public static Asset<Texture2D> CerscentMoonProjAlt;
        #endregion

        #endregion
        #region 远程

        //泰拉弓
        public static Asset<Texture2D> TerraBowCal;
        public static Asset<Texture2D> TerraBowCalGlow;
        public static Asset<Texture2D> TerraBowAlt;
        //元素BYD
        public static Asset<Texture2D> ElemBYDCal;
        public static Asset<Texture2D> ElemBYDAlt;
        //元素喷火器
        public static Asset<Texture2D> ElemFlamethrowerCal;
        public static Asset<Texture2D> ElemFlamethrowerAlt;
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
        public static Asset<Texture2D> DrataBowLegacyAlt;
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
        public static Asset<Texture2D> TerraSummonCal;
        public static Asset<Texture2D> TerraSummonCalGlow;
        public static Asset<Texture2D> TerraSummonAlt;
        #endregion
        #region 盗贼
        #region 裂阳路线
        public static Asset<Texture2D> PrismllCal;
        public static Asset<Texture2D> PrismllAlt;
        public static Asset<Texture2D> RadiantCal;
        public static Asset<Texture2D> RadiantAlt;
        public static Asset<Texture2D> ShatteredCal;
        public static Asset<Texture2D> ShatteredAlt;
        #endregion
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
        //路径
        //职业武器路径
        public static string CIGenericWeaponRoute => "CalamityInheritance/Content/Items/Weapons";
        public static string CIMeleeWeaponRoute=> $"{CIGenericWeaponRoute}/Melee";
        public static string CIRangedWeaponRoute=> $"{CIGenericWeaponRoute}/Ranged";
        public static string CIMagicWeaponRoute=> $"{CIGenericWeaponRoute}/Magic";
        public static string CIRogueWeaponRoute=> $"{CIGenericWeaponRoute}/Rogue";
        //射弹路径
        public static string CIProjRoute => "CalamityInheritance/Content/Projectiles";
        //额外贴图路径
        public static string CIExtraRoute => "CalamityInheritance/Texture";
        public static string CIMeleeExtraRoute => $"{CIExtraRoute}/Melee";
        public static string CIRangedExtraRoute => $"{CIExtraRoute}/Ranged";
        public static string CIRogueExtraRoute => $"{CIExtraRoute}/Rogue";
        //部分灾厄的路径
        //武器路径
        public static string CalWeaponRoute => "CalamityMod/Items/Weapons";
        public static string CalMeleeWeaponRoute => $"{CalWeaponRoute}/Melee";
        public static string CalRangedWeaponRoute => $"{CalWeaponRoute}/Ranged";
        public static string CalMagicWeaponRoute => $"{CalWeaponRoute}/Magic";
        public static string CalRogueWeaponRoute => $"{CalWeaponRoute}/Rogue";
        //射弹路径
        public static string CalProjRoute => "CalamityMod/Projectiles";
        public static void LoadTexture()
        {
            #region 战士
            //泰拉矛
            TerraLanceCal = ModContent.Request<Texture2D>       ($"{CalMeleeWeaponRoute}/BotanicPiercer");
            TerraLanceCalGlow = ModContent.Request<Texture2D>   ($"{CalMeleeWeaponRoute}/BotanicPiercerGlow");
            TerraLanceCalProj = ModContent.Request<Texture2D>   ($"{CalProjRoute}/Melee/Spears/BotanicPiercerProjectile");
            TerraLanceAlt = ModContent.Request<Texture2D>       ($"{CIMeleeExtraRoute}/TerraLance");
            TerraLanceAltProj = ModContent.Request<Texture2D>   ($"{CIMeleeExtraRoute}/TerraLanceProj");
            //方舟
            AotCCal = ModContent.Request<Texture2D>             ($"{CIMeleeExtraRoute}/ArkoftheCosmosNew");
            AotCAlt = ModContent.Request<Texture2D>             ($"{CIMeleeWeaponRoute}/ArkoftheCosmosold");
            #endregion
            //元素系列
            RareArkCal = ModContent.Request<Texture2D>          ($"{CalMeleeWeaponRoute}/Swordsplosion");
            RareArkAlt = ModContent.Request<Texture2D>          ($"{CIMeleeExtraRoute}/RareArkAlt");
            ElemDiskCal = ModContent.Request<Texture2D>         ($"{CIMeleeWeaponRoute}/MeleeTypeElementalDisk");
            ElemDiskAlt = ModContent.Request<Texture2D>         ($"{CIMeleeExtraRoute}/ElemDiskAlt");
            ElemSwordCal = ModContent.Request<Texture2D>        ($"{CIMeleeWeaponRoute}/ArkoftheElementsold");
            ElemSwordAlt = ModContent.Request<Texture2D>        ($"{CIMeleeExtraRoute}/ElemSword");


            ElemLanceCal = ModContent.Request<Texture2D>        ($"{CalMeleeWeaponRoute}/ElementalLance");
            ElemLanceProjCal = ModContent.Request<Texture2D>    ($"{CalProjRoute}/Melee/Spears/ElementalLanceProjectile");
            ElemLanceAlt = ModContent.Request<Texture2D>        ($"{CIMeleeExtraRoute}/ElemLanceAlt");
            ElemLanceProjAlt = ModContent.Request<Texture2D>    ($"{CIMeleeExtraRoute}/ElemLanceProj");
        
            #region 短剑系列
            //火短剑
            FlameShivCal= ModContent.Request<Texture2D>         ($"{CIMeleeWeaponRoute}/Shortsword/FlameburstShortsword");
            FlameShivAlt = ModContent.Request<Texture2D>        ($"{CIMeleeExtraRoute}/Shivs/FlameShiv");
            //地牢短剑
            DungeonShivCal = ModContent.Request<Texture2D>      ($"{CIMeleeWeaponRoute}/Shortsword/AncientShiv");
            DungeonShivAlt = ModContent.Request<Texture2D>      ($"{CIMeleeExtraRoute}/Shivs/DungeonShiv");
            //水短剑
            SeaShivCal = ModContent.Request<Texture2D>          ($"{CIMeleeWeaponRoute}/Shortsword/EutrophicShank");
            SeaShivAlt = ModContent.Request<Texture2D>          ($"{CIMeleeExtraRoute}/Shivs/SeaShiv");
            //腐巢短剑
            HiveMindShivCal = ModContent.Request<Texture2D>     ($"{CIMeleeWeaponRoute}/Shortsword/LeechingDagger");
            HiveMindShivAlt = ModContent.Request<Texture2D>     ($"{CIMeleeExtraRoute}/Shivs/HiveMindShiv");
            //圣短剑
            HallowedShivCal = ModContent.Request<Texture2D>     ($"{CIMeleeWeaponRoute}/Shortsword/ExcaliburShortsword");
            HallowedShivAlt = ModContent.Request<Texture2D>     ($"{CIMeleeExtraRoute}/Shivs/HallowedShiv");
            TrueHallowedShivCal = ModContent.Request<Texture2D> ($"{CIMeleeWeaponRoute}/Shortsword/TrueExcaliburShortsword");
            TrueHallowedShivAlt = ModContent.Request<Texture2D> ($"{CIMeleeExtraRoute}/Shivs/TrueHallowedShiv");
            //永夜
            NightShivCal = ModContent.Request<Texture2D>        ($"{CIMeleeWeaponRoute}/Shortsword/NightsStabber");
            NightShivAlt = ModContent.Request<Texture2D>        ($"{CIMeleeExtraRoute}/Shivs/NightShiv");
            //真永夜
            TrueNightShivCal = ModContent.Request<Texture2D>    ($"{CIMeleeWeaponRoute}/Shortsword/TrueNightsStabber");
            TrueNightShivAlt = ModContent.Request<Texture2D>    ($"{CIMeleeExtraRoute}/Shivs/TrueNightShiv");
            //伽利略
            GalileoCal = ModContent.Request<Texture2D>          ($"{CalMeleeWeaponRoute}/GalileoGladius");
            GalileoAlt = ModContent.Request<Texture2D>          ($"{CIMeleeExtraRoute}/Shivs/GalileoShiv");
            //元素
            ElemShivCal = ModContent.Request<Texture2D>         ($"{CIMeleeWeaponRoute}/Shortsword/ElementalShivold");
            ElemShivAlt = ModContent.Request<Texture2D>         ($"{CIMeleeExtraRoute}/Shivs/ElementalShiv");
            #endregion

            //庇护
            AegisCal = ModContent.Request<Texture2D>            ($"{CalMeleeWeaponRoute}/AegisBlade");
            AegisAlt = ModContent.Request<Texture2D>            ($"{CIGenericWeaponRoute}/Legendary/DefenseBlade");


            //月明链刃
            CerscentMoonProjCal = ModContent.Request<Texture2D> ($"{CalProjRoute}/Melee/CrescentMoonFlail");
            CerscentMoonProjAlt = ModContent.Request<Texture2D> ($"{CIMeleeExtraRoute}/CerscentFlailLegay");

            #region 远程
            //元素BYD
            ElemBYDCal = ModContent.Request<Texture2D>          ($"{CalRangedWeaponRoute}/ElementalBlaster");
            ElemBYDAlt = ModContent.Request<Texture2D>          ($"{CIRangedExtraRoute}/ElemBYD");
            //元素喷火器
            ElemFlamethrowerCal = ModContent.Request<Texture2D> ($"{CIRangedWeaponRoute}/ElementalEruptionLegacy");
            ElemFlamethrowerAlt = ModContent.Request<Texture2D> ($"{CIRangedExtraRoute}/ElemFlamethrower");

            //碎颅
            SkullmasherCal= ModContent.Request<Texture2D>       ($"{CIRangedExtraRoute}/SkullmasherAlt");
            Skullmasher   = ModContent.Request<Texture2D>       ($"{CIRangedWeaponRoute}/Skullmasher");

            //龙弓
            DrataBowLegacyAlt = ModContent.Request<Texture2D>   ($"{CalRangedWeaponRoute}/Drataliornus");
            DrataliornusLegacy = ModContent.Request<Texture2D>  ($"{CIRangedWeaponRoute}/DrataliornusLegacy");

            //P90
            P90 = ModContent.Request<Texture2D>                 ($"{CIRangedExtraRoute}/P90Cal");
            P90Legacy = ModContent.Request<Texture2D>           ($"{CIRangedWeaponRoute}/P90Legacy");

            
            //哈雷
            HalleyCal = ModContent.Request<Texture2D>           ($"{CIRangedWeaponRoute}/HalleysInfernoLegacy");
            HalleyAlt = ModContent.Request<Texture2D>           ($"{CIRangedExtraRoute}/HalleysLegacy");
            #endregion

            #region 法师
            //元素
            ElemRayCal = ModContent.Request<Texture2D>          ($"{CIMagicWeaponRoute}/Ray/ElementalRayold");
            ElemRayAlt = ModContent.Request<Texture2D>          ($"{CIExtraRoute}/Magic/ElemRayAlt");
            //氦闪
            HeliumCal = ModContent.Request<Texture2D>           ($"{CIExtraRoute}/Magic/HeliumCal");
            HeliumFlashLegacy = ModContent.Request<Texture2D>   ($"{CIMagicWeaponRoute}/HeliumFlashLegacy");

            #endregion

            #region 召唤
            TerraSummonCal = ModContent.Request<Texture2D>      ($"{CIExtraRoute}/Summon/TerraSummonAlt");
            #endregion

            #region 盗贼
            //烈阳路线
            PrismllCal = ModContent.Request<Texture2D>          ($"{CalRogueWeaponRoute}/Prismalline");
            PrismllAlt = ModContent.Request<Texture2D>          ($"{CIRogueExtraRoute}/PrismallineAlt");
            RadiantCal = ModContent.Request<Texture2D>          ($"{CalRogueWeaponRoute}/RadiantStar");
            RadiantAlt = ModContent.Request<Texture2D>          ($"{CIRogueExtraRoute}/RadiantStarAlt");
            ShatteredCal = ModContent.Request<Texture2D>        ($"{CalRangedWeaponRoute}/ShatteredSun");
            ShatteredAlt = ModContent.Request<Texture2D>        ($"{CIRogueExtraRoute}/ShatteredSunAlt");

            //圣泰阿克提斯之锤
            GiantHammerCal = ModContent.Request<Texture2D>      ($"{CIRogueExtraRoute}/GiantHammerCal");
            GiantHammerCalProj = ModContent.Request<Texture2D>  ($"{CIRogueExtraRoute}/GiantHammerCal");
            GiantHammerAlt = ModContent.Request<Texture2D>      ($"{CIRogueWeaponRoute}/RogueTypeHammerTriactisTruePaladinianMageHammerofMight");
            GiantHammerAltProj = ModContent.Request<Texture2D>  ($"{CIRogueWeaponRoute}/RogueTypeHammerTriactisTruePaladinianMageHammerofMight");

            #region 飞刀
            //苍穹飞刀(现)
            EmpyreanKnivesCal = ModContent.Request<Texture2D>   ($"{CIRogueWeaponRoute}/RogueTypeKnivesEmpyrean");
            EmpyreanKnivesCalProj= ModContent.Request<Texture2D>($"{CIProjRoute}/Rogue/RogueTypeKnivesEmpyreanProj");
            //苍穹飞刀(初)
            EmpyreanKnivesAlt = ModContent.Request<Texture2D>   ($"{CIRogueExtraRoute}/EmpyreanKnivesAlterFirst");
            EmpyreanKnivesAltProj= ModContent.Request<Texture2D>($"{CIRogueExtraRoute}/EmpyreanKnivesAlterFirstProj");
            //圣光飞刀(现)
            ShadowKnivesCal= ModContent.Request<Texture2D>      ($"{CIRogueExtraRoute}/ShadowKnivesCal");
            ShadowKnivesCalProj= ModContent.Request<Texture2D>  ($"{CIRogueExtraRoute}/ShadowKnivesCalProj");
            //圣光飞刀(三)
            ShadowKnivsAlt3= ModContent.Request<Texture2D>      ($"{CIRogueWeaponRoute}/RogueTypeKnivesShadowspec");
            ShadowKnivsAlt3Proj= ModContent.Request<Texture2D>  ($"{CIProjRoute}/Rogue/RogueTypeKnivesShadowspecProj");
            //圣光飞刀(二)
            ShadowKnivsAlt2= ModContent.Request<Texture2D>      ($"{CIRogueExtraRoute}/ShadowKnivesAlt2");
            ShadowKnivsAlt2Proj= ModContent.Request<Texture2D>  ($"{CIRogueExtraRoute}/ShadowKnivesAlt2Proj");
            //圣光飞刀(初)
            ShadowKnivsAlt1= ModContent.Request<Texture2D>      ($"{CIRogueExtraRoute}/ShadowKnivesAlt1");
            ShadowKnivsAlt1Proj= ModContent.Request<Texture2D>  ($"{CIRogueExtraRoute}/EmpyreanKnivesAlterFirstProj");
            #endregion

            #endregion
            
            #region 战/盗混合
            //圣时之锤
            PwnageHammerCal = ModContent.Request<Texture2D>     ($"{CIMeleeWeaponRoute}/MeleeTypeHammerPwnageLegacy");
            PwnageHammerCalProj = ModContent.Request<Texture2D> ($"{CIMeleeWeaponRoute}/MeleeTypeHammerPwnageLegacy");
            PwnageHammerAlt = ModContent.Request<Texture2D>     ($"{CIMeleeExtraRoute}/PwnagehammerAlter");
            PwnageHammerAltProj = ModContent.Request<Texture2D> ($"{CIMeleeExtraRoute}/PwnagehammerAlter");
      

            //星体击碎者
            StellarContemptNew= ModContent.Request<Texture2D>   ($"{CIMeleeWeaponRoute}/MeleeTypeHammerStellarContemptLegacy");
            StellarContemptOld = ModContent.Request<Texture2D>  ($"{CIRogueWeaponRoute}/RogueTypeHammerStellarContempt");

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

            DrataBowLegacyAlt = null;
            DrataliornusLegacy = null;

            HalleyCal = null;
            HalleyAlt = null;

            AegisCal = null;
            AegisAlt = null;
            
            GalileoAlt = null;
            GalileoCal  = null;
            HallowedShivCal = null;
            HallowedShivAlt = null;
            TrueHallowedShivCal = null;
            TrueHallowedShivAlt = null;
            NightShivCal = null;
            NightShivAlt = null;
            TrueNightShivAlt = null;
            TrueNightShivCal = null;
            FlameShivCal = null;
            FlameShivAlt = null;
            SeaShivAlt = null;
            SeaShivCal = null;
            DungeonShivAlt = null;
            DungeonShivCal = null;

            CerscentMoonProjCal = null;
            CerscentMoonProjAlt = null;

            RareArkAlt = null;
            RareArkCal = null;
            ElemShivCal = null;
            ElemShivAlt = null;
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
            ElemFlamethrowerAlt = null;
            ElemFlamethrowerCal = null;

            
            TerraSummonCal = null;
            TerraSummonAlt = null;

            ShatteredAlt = null;
            ShatteredCal = null;
            RadiantAlt = null;
            RadiantCal = null;
            PrismllAlt = null;
            PrismllCal = null;
        }
    }
}