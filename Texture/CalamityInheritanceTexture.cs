using CalamityMod.Items.Accessories;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.PointsToAnalysis;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public class CIResprite : ModPlayer
    {
        #region 贴图火车
        #region 钨钢系列
        public static Asset<Texture2D> WulfrumAxeNew;

        public static Asset<Texture2D> WulfrumHammerNew;

        public static Asset<Texture2D> WulfrumPickaxeNew;

        public static Asset<Texture2D> WulfrumAxeOld;

        public static Asset<Texture2D> WulfrumHammerOld;

        public static Asset<Texture2D> WulfrumPickaxeOld;
        #endregion
        #region 神之壁垒
        public static Asset<Texture2D> RampartofDeitiesNew;
        public static Asset<Texture2D> RampartofDeitiesOld;
        #endregion
        #region 空灵护符
        public static Asset<Texture2D> EtherealTalismanNew;
        public static Asset<Texture2D> EtherealTalismanOld;
        #endregion
        #region 无记名灵基
        public static Asset<Texture2D> FateGirlLegacy;
        public static Asset<Texture2D> FateGirlLegacyBuff;
        public static Asset<Texture2D> FateGirlOriginal;
        public static Asset<Texture2D> FateGirlOriginalBuff;
        #endregion
        #region 材料
        //星系异石
        public static Asset<Texture2D> GS;
        public static Asset<Texture2D> GSAlter;
        //红色的那个鬼魂
        public static Asset<Texture2D> RedSoul;
        public static Asset<Texture2D> RedSoulAlter;
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
        #region 天使鞋
        public static Asset<Texture2D> AngelTreadsCalamity;
        public static Asset<Texture2D> AngelTreadsAlter;
        #endregion
        #region 夜明跑鞋
        public static Asset<Texture2D> LunarBootsCalamity;
        public static Asset<Texture2D> LunarBootsAlter;
        #endregion
        #region 气球他妈
        public static Asset<Texture2D> MOABCalamity;
        public static Asset<Texture2D> MOABAlter;
        #endregion
        #endregion
        public static void LoadTexture()
        {
            #region 钨钢家族
            WulfrumAxeNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumAxeNew");
            WulfrumHammerNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumHammerNew");
            WulfrumPickaxeNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumPickaxeNew");

            WulfrumAxeOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumAxe");
            WulfrumHammerOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumHammer");
            WulfrumPickaxeOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Tools/WulfrumPickaxe");
            #endregion
            
            #region 壁垒
            RampartofDeitiesNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Accessories/CIRampartofDeities");
            RampartofDeitiesOld = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Accessories/RampartofDeitiesOld");
            #endregion
            #region 空灵护符
            EtherealTalismanNew = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Accessories/Magic/EtherealTalisman");
            EtherealTalismanOld = ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Accessories/Magic/AncientEtherealTalisman");
            #endregion

            #region 无记名灵基
            FateGirlOriginal = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/DaawnlightSpiritOriginMinion");
            FateGirlOriginalBuff = ModContent.Request<Texture2D>("CalamityMod/Buffs/Pets/ArcherofLunamoon");
            FateGirlLegacy = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Summon/FateGirlReal"); 
            FateGirlLegacyBuff = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Summon/FateGirlBuff");
            #endregion
            
            #region 材料
            GS = ModContent.Request<Texture2D>("CalamityMod/Items/Materials/GalacticaSingularity");
            GSAlter = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Misc/GalacticaSingularityAlter");
            RedSoul = ModContent.Request<Texture2D>("CalamityMod/Items/Materials/Necroplasm");
            RedSoulAlter = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Misc/PhantoplasmAlter");
            #endregion
            #region 永久增益
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
            #endregion
           
            #region 天使鞋
            AngelTreadsCalamity = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Accessories/AngelTreadsCalamity");
            AngelTreadsAlter    = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Accessories/AngelTreadsLegacy");
            #endregion
            #region 夜明跑鞋
            LunarBootsCalamity  = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Accessories/LunarBootsCalamity");
            LunarBootsAlter     = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Accessories/LunarBootsLegacy");
            #endregion
            #region 气球他妈
            MOABCalamity        = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Accessories/MOABCalamity");
            MOABAlter           = ModContent.Request<Texture2D>("CalamityInheritance/Texture/Accessories/MOABLegacy");
            #endregion
        }
        public static void UnloadTexture()
        {
            WulfrumAxeNew = null;
            WulfrumHammerNew = null;
            WulfrumPickaxeNew = null;

            WulfrumAxeNew = null;
            WulfrumHammerNew = null;
            WulfrumPickaxeNew = null;

            

            RampartofDeitiesNew = null;
            RampartofDeitiesOld = null;

            EtherealTalismanNew = null;
            EtherealTalismanOld = null;

                        //飞刀，排版同上
            

            FateGirlLegacy = null;
            FateGirlLegacyBuff = null;
            FateGirlOriginal = null;
            FateGirlOriginalBuff = null;
            
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

            AngelTreadsCalamity = null;
            AngelTreadsAlter = null;

            LunarBootsCalamity = null;
            LunarBootsAlter = null;
            
            MOABCalamity = null;
            MOABAlter = null;
        }
    }
}
