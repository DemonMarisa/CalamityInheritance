using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Accessories.Magic;
using CalamityInheritance.Content.Items.Accessories.Wings;
using CalamityInheritance.Content.Items.Tools;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Texture;
using CalamityMod.Buffs.Pets;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Projectiles.Pets;
using ReLogic.Text;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        //3.6: 将重绘的排序更加严格地分类
        public static void RespriteOptions()
        {
            #region 切换1457贴图
            if(CIRespriteConfig.Instance.SetAllLegacySprite == true) //这个用于快速把一些物品转为1457版本的贴图
            {
                CIRespriteConfig.Instance.RampartofDeitiesTexture = 2;
                CIRespriteConfig.Instance.TriactisHammerResprite = 2;
                CIRespriteConfig.Instance.ShadowspecKnivesResprite = 2;
                CIRespriteConfig.Instance.BloodOrangeResprite = 2;
                CIRespriteConfig.Instance.MiracleFruitResprite= 2;
                CIRespriteConfig.Instance.ElderberryResprite  = 2;
                CIRespriteConfig.Instance.DragonfruitResprite = 2;
                CIRespriteConfig.Instance.CometShardResprite = 2;
                CIRespriteConfig.Instance.EtherealCoreResprite = 2;
                CIRespriteConfig.Instance.PhantomHeartResprite = 2;
                CIRespriteConfig.Instance.StellarContemptResprite  = 2;
                CIRespriteConfig.Instance.FateGirlSprite = 2;
                CIRespriteConfig.Instance.HeliumFlashResprite = 2;
                CIRespriteConfig.Instance.DrataliornusResprite = 2;
                CIRespriteConfig.Instance.AngelTreadsResprite = 2;
                CIRespriteConfig.Instance.MOABResprite = 2;
            }
            #endregion
            #region Texture
            #region 钨钢Family
            if (TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()]     != null ||
                TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()]  != null ||
                TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] != null)
            {
                if(CIRespriteConfig.Instance.WulfumTexture == true)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()] = CIResprite.WulfrumAxeNew;
                    TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()] = CIResprite.WulfrumHammerNew;
                    TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] = CIResprite.WulfrumPickaxeNew;
                }
                if (CIRespriteConfig.Instance.WulfumTexture == false)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()] = CIResprite.WulfrumAxeOld;
                    TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()] = CIResprite.WulfrumHammerOld;
                    TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] = CIResprite.WulfrumPickaxeOld;
                }
            }
            #endregion
            #region 近战物品
            if (TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] != null)
            {
                if (CIRespriteConfig.Instance.ArkofCosmosTexture == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] = CIWeaponsResprite.ArkoftheCosmosNew;
                }
                if (CIRespriteConfig.Instance.ArkofCosmosTexture == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] = CIWeaponsResprite.ArkoftheCosmosOld;
                }
            }
            #endregion
            #region 射手物品
            #region 碎颅
            if (TextureAssets.Item[ModContent.ItemType<Skullmasher>()] != null)
            {
                if(CIRespriteConfig.Instance.SkullmasherResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Skullmasher>()] = CIWeaponsResprite.Skullmasher1p5;
                }
                if(CIRespriteConfig.Instance.SkullmasherResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Skullmasher>()] = CIWeaponsResprite.Skullmasher;
                }
            }
            #endregion
            #region P90
            if (TextureAssets.Item[ModContent.ItemType<P90Legacy>()] != null)
            {
                if (CIRespriteConfig.Instance.P90Resprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<P90Legacy>()] = CIWeaponsResprite.P90;
                }
                if (CIRespriteConfig.Instance.P90Resprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<P90Legacy>()] = CIWeaponsResprite.P90Legacy;
                }
            }
            #endregion P90
            #region 龙弓
            //byd你用星体击碎者的开关切换龙弓贴图是吧 
            if (TextureAssets.Item[ModContent.ItemType<DrataliornusLegacy>()] != null &&
                TextureAssets.Projectile[ModContent.ProjectileType<DragonBow>()] != null)
            {
                if (CIRespriteConfig.Instance.DrataliornusResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<DrataliornusLegacy>()] = CIWeaponsResprite.DrataliornusLegacyAlter;
                    TextureAssets.Projectile[ModContent.ProjectileType<DragonBow>()] = CIWeaponsResprite.DrataliornusLegacyAlter;
                }
                if (CIRespriteConfig.Instance.DrataliornusResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<DrataliornusLegacy>()] = CIWeaponsResprite.DrataliornusLegacy;
                    TextureAssets.Projectile[ModContent.ProjectileType<DragonBow>()] = CIWeaponsResprite.DrataliornusLegacy;
                }
            }
            #endregion
            #endregion
            #region 战/盗混合武器
            #region 泰阿克提斯之锤
            //TODO:大锤子射弹贴图没能成功切换
            if(TextureAssets.Item[ModContent.ItemType<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>()]!=null)
            {
                if(CIRespriteConfig.Instance.TriactisHammerResprite ==1)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>()] = CIWeaponsResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>()] = CIWeaponsResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>()] = CIWeaponsResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>()] = CIWeaponsResprite.TriactisHammerCalamity;
                }
                if(CIRespriteConfig.Instance.TriactisHammerResprite ==2)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>()] = CIWeaponsResprite.TriactisHammerAlter;
                    
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>()] = CIWeaponsResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>()] = CIWeaponsResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>()] = CIWeaponsResprite.TriactisHammerCalamity;
                }
            }
            #endregion
            #region 圣时之锤
            //TODO:同上
            if(TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerPwnageLegacy>()]!=null)
            {
                if(CIRespriteConfig.Instance.PwnagehammerResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerPwnageLegacy>()] = CIWeaponsResprite.HallowedHammerCalamity;
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerPwnageLegacy>()] = CIWeaponsResprite.HallowedHammerCalamity;

                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerPwnageLegacyProj>()] = CIWeaponsResprite.HallowedHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerPwnageLegacyProj>()] = CIWeaponsResprite.HallowedHammerCalamity;
                }
                if(CIRespriteConfig.Instance.PwnagehammerResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerPwnageLegacy>()] = CIWeaponsResprite.HallowedHammerAlter;
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerPwnageLegacy>()] = CIWeaponsResprite.HallowedHammerAlter;
                    
                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerPwnageLegacyProj>()] = CIWeaponsResprite.HallowedHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerPwnageLegacyProj>()] = CIWeaponsResprite.HallowedHammerCalamity;
                }
            }
            #endregion
            #region 星体击碎者(们)
            //mod内有两把星体击碎者，两把都用同一句话的判定
            if (TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerStellarContemptLegacy>()] != null &&
                TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerStellarContemptLegacyProj>()] != null &&
                TextureAssets.Item[ModContent.ItemType<RogueTypeHammerStellarContempt>()] != null &&
                TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] != null)
            {
                if (CIRespriteConfig.Instance.StellarContemptResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerStellarContemptLegacy>()] = CIWeaponsResprite.StellarContemptNew;
                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerStellarContemptLegacyProj>()] = CIWeaponsResprite.StellarContemptNew;
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerStellarContempt>()] = CIWeaponsResprite.StellarContemptNew;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] = CIWeaponsResprite.StellarContemptNew;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProjClone>()] = CIWeaponsResprite.StellarContemptNew;
                }
                if (CIRespriteConfig.Instance.StellarContemptResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerStellarContemptLegacy>()] = CIWeaponsResprite.StellarContemptOld;
                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerStellarContemptLegacyProj>()] = CIWeaponsResprite.StellarContemptOld;
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerStellarContempt>()] = CIWeaponsResprite.StellarContemptOld;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] = CIWeaponsResprite.StellarContemptOld;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProjClone>()] = CIWeaponsResprite.StellarContemptOld;
                }
            }
            #endregion
            #region 苍穹飞刀
            //飞刀类，因为原灾也有，所以也归入战/盗混合武器
            //Todo: 可能给原灾的飞刀上贴图切换?
            if (TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesEmpyrean>()] != null &&
                TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesEmpyreanProj>()] != null)
            {
                if (CIRespriteConfig.Instance.GodSlayerKnivesResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesEmpyrean>()] = CIWeaponsResprite.EmpyreanKnivesCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesEmpyreanProj>()] = CIWeaponsResprite.EmpyreanKnivesCalamityProj;
                }
                if (CIRespriteConfig.Instance.GodSlayerKnivesResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesEmpyrean>()] = CIWeaponsResprite.EmpyreanKnivesAlterTypeOne;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesEmpyreanProj>()] = CIWeaponsResprite.EmpyreanKnivesAlterTypeOneProj;
                }
            }
            #endregion
            #region 圣光飞刀
            if (TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] != null &&
                TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] != null)
            {
                if (CIRespriteConfig.Instance.ShadowspecKnivesResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIWeaponsResprite.ShadowspecKnivesCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIWeaponsResprite.ShadowspecKnivesCalamityProj;
                }
                if (CIRespriteConfig.Instance.ShadowspecKnivesResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIWeaponsResprite.ShadowspecKnivesAlterThird;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIWeaponsResprite.ShadowspecKnivesAlterThirdProj;
                }
                if (CIRespriteConfig.Instance.ShadowspecKnivesResprite == 3)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIWeaponsResprite.ShadowspecKnivesAlterSec;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIWeaponsResprite.ShadowspecKnivesAlterSecProj;
                }
                if (CIRespriteConfig.Instance.ShadowspecKnivesResprite == 4)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIWeaponsResprite.ShadowspecKnivesAlterFirst;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIWeaponsResprite.ShadowspecKnivesAlterThirdProj;
                }
            }
            #endregion
            #endregion
            #region 法师武器
            #region 氦闪
            if (TextureAssets.Item[ModContent.ItemType<HeliumFlashLegacy>()] != null)
            {
                if(CIRespriteConfig.Instance.HeliumFlashResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<HeliumFlashLegacy>()] = CIWeaponsResprite.HeliumFlashCalamity;
                }
                if(CIRespriteConfig.Instance.HeliumFlashResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<HeliumFlashLegacy>()] = CIWeaponsResprite.HeliumFlashLegacy;
                }
            }
            #endregion
            #endregion
            #region 饰品
            #region 壁垒
            if (TextureAssets.Item[ModContent.ItemType<CIRampartofDeities>()] != null)
            {
                if (CIRespriteConfig.Instance.RampartofDeitiesTexture == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<CIRampartofDeities>()] = CIResprite.RampartofDeitiesNew;
                }
                if (CIRespriteConfig.Instance.RampartofDeitiesTexture == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<CIRampartofDeities>()] = CIResprite.RampartofDeitiesOld;
                }
            }
            #endregion
            #region 空灵护符
            if (TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] != null)
            {
                if (CIRespriteConfig.Instance.EtherealTalismancTexture == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] = CIResprite.EtherealTalismanNew;
                }
                if (CIRespriteConfig.Instance.EtherealTalismancTexture == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] = CIResprite.EtherealTalismanOld;
                }
            }
            
            #endregion
            #region 无记名灵基
            if (TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] != null &&
                TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] != null)
            {
                if (CIRespriteConfig.Instance.FateGirlSprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] = CIResprite.FateGirlOriginal;
                    TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] = CIResprite.FateGirlOriginalBuff;
                }
                if (CIRespriteConfig.Instance.FateGirlSprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] = CIResprite.FateGirlLegacy;
                    TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] = CIResprite.FateGirlLegacyBuff;
                }
            }
            #endregion
            #region 天使鞋
            if (TextureAssets.Item[ModContent.ItemType<AngelTreads>()] != null)
            {
                if (CIRespriteConfig.Instance.AngelTreadsResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<AngelTreads>()] = CIResprite.AngelTreadsCalamity; 
                }
                if (CIRespriteConfig.Instance.AngelTreadsResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<AngelTreads>()] = CIResprite.AngelTreadsAlter; 
                }
            }
            #endregion
            #region 夜明跑鞋
            if (TextureAssets.Item[ModContent.ItemType<FasterLunarTracers>()] != null)
            {
                if (CIRespriteConfig.Instance.LunarBootsResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<FasterLunarTracers>()] = CIResprite.LunarBootsCalamity;
                }
                if (CIRespriteConfig.Instance.LunarBootsResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<FasterLunarTracers>()] = CIResprite.LunarBootsAlter;
                }
            }
            #endregion
            #region 气球他妈
            if (TextureAssets.Item[ModContent.ItemType<MOAB>()] != null)
            {
                //TODO:贴图切换失败 
                if (CIRespriteConfig.Instance.MOABResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<MOAB>()] = CIResprite.MOABCalamity;
                }
                if (CIRespriteConfig.Instance.MOABResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<MOAB>()] = CIResprite.MOABAlter;
                }
            }
            #endregion

            #endregion
            #region 增益道具
            if (TextureAssets.Item[ModContent.ItemType<BloodOrange>()] != null)
            {
                if (CIRespriteConfig.Instance.BloodOrangeResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<BloodOrange>()] = CIResprite.HealthOrange;
                }
                if (CIRespriteConfig.Instance.BloodOrangeResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<BloodOrange>()] = CIResprite.HealthOrangeAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] != null)
            {
                if (CIRespriteConfig.Instance.MiracleFruitResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] = CIResprite.HealthMira;
                }
                if (CIRespriteConfig.Instance.MiracleFruitResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] = CIResprite.HealthMiraAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<Elderberry>()] != null)
            {
                if (CIRespriteConfig.Instance.ElderberryResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Elderberry>()] = CIResprite.HealthBerry;
                }
                if (CIRespriteConfig.Instance.ElderberryResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Elderberry>()] = CIResprite.HealthBerryAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] != null)
            {
                if (CIRespriteConfig.Instance.DragonfruitResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] = CIResprite.HealthDragon;
                }
                if (CIRespriteConfig.Instance.DragonfruitResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] = CIResprite.HealthDragonAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<CometShard>()] != null)
            {
                if (CIRespriteConfig.Instance.CometShardResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<CometShard>()] = CIResprite.ManaShard;
                }
                if (CIRespriteConfig.Instance.CometShardResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<CometShard>()] = CIResprite.ManaShardAlter;
                }
            }

            if (TextureAssets.Item[ModContent.ItemType<EtherealCore>()] != null)
            {
                if (CIRespriteConfig.Instance.EtherealCoreResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<EtherealCore>()] = CIResprite.ManaCore;
                }
                if (CIRespriteConfig.Instance.EtherealCoreResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<EtherealCore>()] = CIResprite.ManaCoreAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<PhantomHeart>()] != null)
            {
                if (CIRespriteConfig.Instance.PhantomHeartResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<PhantomHeart>()] = CIResprite.ManaHeart;
                }
                if (CIRespriteConfig.Instance.PhantomHeartResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<PhantomHeart>()] = CIResprite.ManaHeartAlter;
                }
            }
            
            //核子与星系是动图（现灾厄）不能直接用
            /*DemonMarisa: 开修
             *单独去注册hook替换帧图算法了
             *应该是修完了
             *把核子的删了，改成了单独注册了
             *星系异石和灵质不会改，保留了注册，删除了贴图读取
             */
            /*
            if (TextureAssets.Item[ModContent.ItemType<GalacticaSingularity>()] != null)
            {
                if (CIRespriteConfig.Instance.GalacticaSingularityResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<GalacticaSingularity>()] = CIResprite.GS;
                }
                if (CIRespriteConfig.Instance.GalacticaSingularityResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<GalacticaSingularity>()] = CIResprite.GSAlter;
                }
            }

            if (TextureAssets.Item[ModContent.ItemType<Necroplasm>()] != null)
            {
                if (CIRespriteConfig.Instance.NecroplasmResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Necroplasm>()] = CIResprite.RedSoul;
                }
                if (CIRespriteConfig.Instance.NecroplasmResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Necroplasm>()] = CIResprite.RedSoulAlter;
                }
            }
            */
            

            #endregion
            #endregion
        }
    }
}