using CalamityMod.Items.Accessories;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.PointsToAnalysis;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public class CIResprite : ModPlayer
    {
        //这个用来干掉发光贴图
        public static Asset<Texture2D> RemovedGlowMask;
        #region 贴图火车
        #region 钨钢系列
        public static Asset<Texture2D> WulfrumAxeNew;

        public static Asset<Texture2D> WulfrumHammerNew;

        public static Asset<Texture2D> WulfrumPickaxeNew;

        public static Asset<Texture2D> WulfrumAxeOld;

        public static Asset<Texture2D> WulfrumHammerOld;

        public static Asset<Texture2D> WulfrumPickaxeOld;
        #endregion
        //元素手套
        public static Asset<Texture2D> ElemGloveCal;
        public static Asset<Texture2D> ElemGloveAlt;
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
        #region 盔甲
        //弑神

        #endregion
        #endregion
        #region 路径
        public static string CIExtraRoute => "CalamityInheritance/Texture"; 
        public static string CIItemsRoute => "CalamityInheritance/Content/Items";
        public static string CalItemsRoute => "CalamityMod/Items";
        #endregion
        public static void LoadTexture()
        {
            RemovedGlowMask = ModContent.Request<Texture2D>($"{CIExtraRoute}/FuckGlowMask");
            #region 钨钢家族
            WulfrumAxeNew = ModContent.Request<Texture2D>($"{CIItemsRoute}/Tools/WulfrumAxeNew");
            WulfrumHammerNew = ModContent.Request<Texture2D>($"{CIItemsRoute}/Tools/WulfrumHammerNew");
            WulfrumPickaxeNew = ModContent.Request<Texture2D>($"{CIItemsRoute}/Tools/WulfrumPickaxeNew");

            WulfrumAxeOld = ModContent.Request<Texture2D>($"{CIItemsRoute}/Tools/WulfrumAxe");
            WulfrumHammerOld = ModContent.Request<Texture2D>($"{CIItemsRoute}/Tools/WulfrumHammer");
            WulfrumPickaxeOld = ModContent.Request<Texture2D>($"{CIItemsRoute}/Tools/WulfrumPickaxe");
            #endregion
            //元素手套
            ElemGloveCal = ModContent.Request<Texture2D>($"{CIItemsRoute}/Accessories/Melee/ElementalGauntletold");
            ElemGloveAlt = ModContent.Request<Texture2D>($"{CIExtraRoute}/Accessories/ElemGloveAlt");
            #region 壁垒
            RampartofDeitiesNew = ModContent.Request<Texture2D>($"{CIItemsRoute}/Accessories/CIRampartofDeities");
            RampartofDeitiesOld = ModContent.Request<Texture2D>($"{CIExtraRoute}/Accessories/RampartofDeitiesOld");
            #endregion
            #region 空灵护符
            EtherealTalismanNew = ModContent.Request<Texture2D>($"{CIItemsRoute}/Accessories/Magic/EtherealTalisman");
            EtherealTalismanOld = ModContent.Request<Texture2D>($"{CIItemsRoute}/Accessories/Magic/AncientEtherealTalisman");
            #endregion

            #region 无记名灵基
            FateGirlOriginal = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Pets/DaawnlightSpiritOriginMinion");
            FateGirlOriginalBuff = ModContent.Request<Texture2D>("CalamityMod/Buffs/Pets/ArcherofLunamoon");
            FateGirlLegacy = ModContent.Request<Texture2D>($"{CIExtraRoute}/Summon/FateGirlReal"); 
            FateGirlLegacyBuff = ModContent.Request<Texture2D>($"{CIExtraRoute}/Summon/FateGirlBuff");
            #endregion
            
            #region 材料
            GS = ModContent.Request<Texture2D>($"{CalItemsRoute}/Materials/GalacticaSingularity");
            GSAlter = ModContent.Request<Texture2D>($"{CIExtraRoute}/Misc/GalacticaSingularityAlter");
            RedSoul = ModContent.Request<Texture2D>($"{CalItemsRoute}/Materials/Necroplasm");
            RedSoulAlter = ModContent.Request<Texture2D>($"{CIExtraRoute}/Misc/PhantoplasmAlter");
            #endregion
            #region 永久增益
            //增益
            HealthOrange = ModContent.Request<Texture2D>($"{CalItemsRoute}/PermanentBoosters/BloodOrange");
            HealthOrangeAlter= ModContent.Request<Texture2D>($"{CIExtraRoute}/Misc/BloodOrangeAlter");

            HealthMira = ModContent.Request<Texture2D>($"{CalItemsRoute}/PermanentBoosters/MiracleFruit");
            HealthMiraAlter= ModContent.Request<Texture2D>($"{CIExtraRoute}/Misc/MiracleFruitAlter");

            HealthBerry = ModContent.Request<Texture2D>($"{CalItemsRoute}/PermanentBoosters/Elderberry");
            HealthBerryAlter= ModContent.Request<Texture2D>($"{CIExtraRoute}/Misc/ElderberryAlter");

            HealthDragon = ModContent.Request<Texture2D>($"{CalItemsRoute}/PermanentBoosters/Dragonfruit");
            HealthDragonAlter= ModContent.Request<Texture2D>($"{CIExtraRoute}/Misc/DragonfruitAlter");

            ManaShard = ModContent.Request<Texture2D>($"{CalItemsRoute}/PermanentBoosters/CometShard");
            ManaShardAlter = ModContent.Request<Texture2D>($"{CIExtraRoute}/Misc/CometShardAlter");
            
            ManaCore = ModContent.Request<Texture2D>($"{CalItemsRoute}/PermanentBoosters/EtherealCore");
            ManaCoreAlter = ModContent.Request<Texture2D>($"{CIExtraRoute}/Misc/EtherealCoreAlter");

            ManaHeart = ModContent.Request<Texture2D>($"{CalItemsRoute}/PermanentBoosters/PhantomHeart");
            ManaHeartAlter = ModContent.Request<Texture2D>($"{CIExtraRoute}/Misc/PhantomHeartAlter");
            #endregion
           
            #region 天使鞋
            AngelTreadsCalamity = ModContent.Request<Texture2D>($"{CIExtraRoute}/Accessories/AngelTreadsCalamity");
            AngelTreadsAlter    = ModContent.Request<Texture2D>($"{CIExtraRoute}/Accessories/AngelTreadsLegacy");
            #endregion
            #region 夜明跑鞋
            LunarBootsCalamity  = ModContent.Request<Texture2D>($"{CIExtraRoute}/Accessories/LunarBootsCalamity");
            LunarBootsAlter     = ModContent.Request<Texture2D>($"{CIExtraRoute}/Accessories/LunarBootsLegacy");
            #endregion
            #region 气球他妈
            MOABCalamity        = ModContent.Request<Texture2D>($"{CIExtraRoute}/Accessories/MOABCalamity");
            MOABAlter           = ModContent.Request<Texture2D>($"{CIExtraRoute}/Accessories/MOABLegacy");
            #endregion
        }
        public static void UnloadTexture()
        {
            RemovedGlowMask = null;

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

            ElemGloveAlt = null;
            ElemGloveCal = null;
        }
    }
}
