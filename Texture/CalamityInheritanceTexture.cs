using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public class CalamityInheritanceTexture : ModPlayer
    {
        #region 贴图火车
        public static Asset<Texture2D> WulfrumAxeNew;

        public static Asset<Texture2D> WulfrumHammerNew;

        public static Asset<Texture2D> WulfrumPickaxeNew;

        public static Asset<Texture2D> WulfrumAxeOld;

        public static Asset<Texture2D> WulfrumHammerOld;

        public static Asset<Texture2D> WulfrumPickaxeOld;

        public static Asset<Texture2D> ArkoftheCosmosNew;

        public static Asset<Texture2D> ArkoftheCosmosOld;

        public static Asset<Texture2D> RampartofDeitiesNew;

        public static Asset<Texture2D> RampartofDeitiesOld;

        public static Asset<Texture2D> EtherealTalismanNew;

        public static Asset<Texture2D> EtherealTalismanOld;

        public static Asset<Texture2D> Skullmasher1p5;

        public static Asset<Texture2D> Skullmasher;

        public static Asset<Texture2D> P90;

        public static Asset<Texture2D> P90Legacy;

        public static Asset<Texture2D> TriactisHammerAlter;
        public static Asset<Texture2D> TriactisHammerAlterProj;
        public static Asset<Texture2D> TriactisHammerCalamity;
        public static Asset<Texture2D> TriactisHammerCalamityProj;

        public static Asset<Texture2D> HallowedHammerCalamity;
        public static Asset<Texture2D> HallowedHammerCalamityProj;
        public static Asset<Texture2D> HallowedHammerAlter;
        public static Asset<Texture2D> HallowedHammerAlterProj;
        //下：无记名灵基
        public static Asset<Texture2D> FateGirlLegacy;
        public static Asset<Texture2D> FateGirlLegacyBuff;
        public static Asset<Texture2D> FateGirlOriginal;
        public static Asset<Texture2D> FateGirlOriginalBuff;

        #region 各种飞刀的贴图
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
        //星体击碎者
        public static Asset<Texture2D> StellarContemptNew; //圣光飞刀的初版贴图
        public static Asset<Texture2D> StellarContemptOld;
        #endregion
        #region 材料
        //星系异石
        public static Asset<Texture2D> GS;
        public static Asset<Texture2D> GSAlter;
        //红色的那个鬼魂
        public static Asset<Texture2D> RedSoul;
        public static Asset<Texture2D> RedSoulAlter;
        #endregion
        #endregion
        #region 永久增益
        /*加血的。血橙、奇迹果、蓝莓和龙果*/
        public static Asset<Texture2D> HealthOrange;
        public static Asset<Texture2D> HealthOrangeAlter;
        //血橙
        public static Asset<Texture2D> HealthMira;
        public static Asset<Texture2D> HealthMiraAlter;
        //奇迹果
        public static Asset<Texture2D> HealthBerry;
        public static Asset<Texture2D> HealthBerryAlter;
        //蓝莓
        public static Asset<Texture2D> HealthDragon;
        public static Asset<Texture2D> HealthDragonAlter;
        //龙果
        /*加魔力的，一个肉后初期的，一个星神游龙后的和魂花后的 */
        public static Asset<Texture2D> ManaShard;
        public static Asset<Texture2D> ManaShardAlter;
        //肉后初期的
        public static Asset<Texture2D> ManaCore;
        public static Asset<Texture2D> ManaCoreAlter;
        //游龙后的
        public static Asset<Texture2D> ManaHeart;
        public static Asset<Texture2D> ManaHeartAlter;
        //魂花后的
        #endregion
        public static void LoadTexture()
        {
            WulfrumAxeNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumAxeNew");
            WulfrumHammerNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumHammerNew");
            WulfrumPickaxeNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumPickaxeNew");

            WulfrumAxeOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumAxe");
            WulfrumHammerOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumHammer");
            WulfrumPickaxeOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumPickaxe");

            ArkoftheCosmosNew = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/ArkoftheCosmosNew");
            ArkoftheCosmosOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/ArkoftheCosmosold");

            RampartofDeitiesNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Accessories/CIRampartofDeities");
            RampartofDeitiesOld = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Accessories/RampartofDeitiesOld");

            EtherealTalismanNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Accessories/Magic/EtherealTalisman");
            EtherealTalismanOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Accessories/Magic/AncientEtherealTalisman");

            Skullmasher1p5= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Ranged/Skullmasher1p5");
            Skullmasher   = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/Skullmasher");

            P90 = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Ranged/P90");
            P90Legacy = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/P90Legacy");

            TriactisHammerCalamity = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/TriactisHammerCalamity");
            TriactisHammerCalamityProj = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Rogue/TriactisHammerCalamity");
            TriactisHammerAlter = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerTriactisTruePaladinianMageHammerofMight");
            TriactisHammerAlterProj = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerTriactisTruePaladinianMageHammerofMight");
            

            HallowedHammerCalamity = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/MeleeTypeHammerPwnageLegacy");
            HallowedHammerCalamityProj = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/MeleeTypeHammerPwnageLegacy");
            HallowedHammerAlter = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/PwnagehammerAlter");
            HallowedHammerAlterProj = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Melee/PwnagehammerAlter");

            /*下方为飞刀的各种贴图，排版与上述定义时相同*/
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
            //无记名灵基
            FateGirlOriginal = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/DaawnlightSpiritOriginMinion");
            FateGirlOriginalBuff = ModContent.Request<Texture2D>("CalamityMod/Buffs/Pets/ArcherofLunamoon");
            FateGirlLegacy = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Summon/FateGirlReal"); 
            FateGirlLegacyBuff = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Summon/FateGirlBuff");
            //星体击碎者
            StellarContemptNew= ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/MeleeTypeHammerStellarContemptLegacy");
            StellarContemptOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerStellarContempt");
            /*各类材料*/
            GS = ModContent.Request<Texture2D>("CalamityMod/Items/Materials/GalacticaSingularity");
            GSAlter = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Misc/GalacticaSingularityAlter");
            //灵质
            RedSoul = ModContent.Request<Texture2D>("CalamityMod/Items/Materials/Necroplasm");
            RedSoulAlter = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Misc/PhantoplasmAlter");
            //增益
            HealthOrange = ModContent.Request<Texture2D>("CalamityMod/Items/PermanentBoosters/BloodOrange");
            HealthOrangeAlter= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Misc/BloodOrangeAlter");

            HealthMira = ModContent.Request<Texture2D>("CalamityMod/Items/PermanentBoosters/MiracleFruit");
            HealthMiraAlter= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Misc/MiracleFruitAlter");

            HealthBerry = ModContent.Request<Texture2D>("CalamityMod/Items/PermanentBoosters/Elderberry");
            HealthBerryAlter= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Misc/ElderberryAlter");

            HealthDragon = ModContent.Request<Texture2D>("CalamityMod/Items/PermanentBoosters/Dragonfruit");
            HealthDragonAlter= ModContent.Request<Texture2D>("CalamityInheritance/Texture/Misc/DragonfruitAlter");

            ManaShard = ModContent.Request<Texture2D>("CalamityMod/Items/PermanentBoosters/CometShard");
            ManaShardAlter = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Misc/CometShardAlter");
            
            ManaCore = ModContent.Request<Texture2D>("CalamityMod/Items/PermanentBoosters/EtherealCore");
            ManaCoreAlter = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Misc/EtherealCoreAlter");

            ManaHeart = ModContent.Request<Texture2D>("CalamityMod/Items/PermanentBoosters/PhantomHeart");
            ManaHeartAlter = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Misc/PhantomHeartAlter");

        }
        public static void UnloadTexture()
        {
            WulfrumAxeNew = null;
            WulfrumHammerNew = null;
            WulfrumPickaxeNew = null;

            WulfrumAxeNew = null;
            WulfrumHammerNew = null;
            WulfrumPickaxeNew = null;

            ArkoftheCosmosNew = null;
            ArkoftheCosmosOld = null;

            RampartofDeitiesNew = null;
            RampartofDeitiesOld = null;

            EtherealTalismanNew = null;
            EtherealTalismanOld = null;

            Skullmasher1p5 = null;
            Skullmasher    = null;

            P90 = null;
            P90Legacy = null;

            TriactisHammerCalamity = null;
            TriactisHammerAlter = null;

            //飞刀，排版同上
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

            FateGirlLegacy = null;
            FateGirlLegacyBuff = null;
            FateGirlOriginal = null;
            FateGirlOriginalBuff = null;
            //星体击碎者
            StellarContemptNew = null;
            StellarContemptOld = null;
            //材料
            GS = null;
            GSAlter = null;

            RedSoul = null;
            RedSoulAlter = null;
            //增益
            HealthOrange = null;
            HealthOrangeAlter = null;

            HealthMira = null;
            HealthMiraAlter = null;

            HealthBerry = null;
            HealthBerryAlter = null;
            
            HealthDragon = null;
            HealthDragonAlter =null;

            ManaShard =null;
            ManaShardAlter = null;

            ManaCore = null;
            ManaCoreAlter = null;
            
            ManaHeart = null;
            ManaHeartAlter = null;
        }
    }
}
