using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Accessories.Magic;
using CalamityInheritance.Content.Items.Accessories.Melee;
using CalamityInheritance.Content.Items.Accessories.Wings;
using CalamityInheritance.Content.Items.MiscItem;
using CalamityInheritance.Content.Items.Tools;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Texture;
using CalamityMod.Buffs.Pets;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Melee.Shortswords;
using CalamityMod.Projectiles.Melee.Spears;
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
                CIRespriteConfig.Instance.RampartofDeitiesTexture =     false;
                CIRespriteConfig.Instance.TriactisHammerResprite =      false;
                CIRespriteConfig.Instance.BloodOrangeResprite =         false;
                CIRespriteConfig.Instance.MiracleFruitResprite=         false;
                CIRespriteConfig.Instance.ElderberryResprite  =         false;
                CIRespriteConfig.Instance.DragonfruitResprite =         false;
                CIRespriteConfig.Instance.CometShardResprite =          false;
                CIRespriteConfig.Instance.EtherealCoreResprite =        false;
                CIRespriteConfig.Instance.PhantomHeartResprite =        false;
                CIRespriteConfig.Instance.StellarContemptResprite  =    false;
                CIRespriteConfig.Instance.FateGirlSprite =              false;
                CIRespriteConfig.Instance.HeliumFlashResprite =         false;
                CIRespriteConfig.Instance.DrataliornusResprite =        false;
                CIRespriteConfig.Instance.AngelTreadsResprite =         false;
                CIRespriteConfig.Instance.MOABResprite =                false;

                CIRespriteConfig.Instance.ShadowspecKnivesResprite = 2;
            }
            #endregion
            #region Texture
            #region 钨钢Family
            if (TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()]     != null ||
                TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()]  != null ||
                TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] != null)
            {
                if(!CIRespriteConfig.Instance.WulfumTexture)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()] = CIResprite.WulfrumAxeNew;
                    TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()] = CIResprite.WulfrumHammerNew;
                    TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] = CIResprite.WulfrumPickaxeNew;
                }
                if (CIRespriteConfig.Instance.WulfumTexture)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()] = CIResprite.WulfrumAxeOld;
                    TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()] = CIResprite.WulfrumHammerOld;
                    TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] = CIResprite.WulfrumPickaxeOld;
                }
            }
            #endregion
            #region 全部的元素武器
            if (TextureAssets.Item[ModContent.ItemType<ElementalShivold>()] != null ||
                TextureAssets.Item[ModContent.ItemType<ElementalRayold>()]  != null ||
                TextureAssets.Item[ModContent.ItemType<MeleeTypeElementalDisk>()] != null ||
                TextureAssets.Item[ModContent.ItemType<ElementalLance>()] != null ||
                TextureAssets.Item[ModContent.ItemType<ElementalGauntletold>()] != null ||
                TextureAssets.Item[ModContent.ItemType<ElementalBlaster>()] != null ||
                TextureAssets.Item[ModContent.ItemType<ArkoftheElementsold>()] != null ||
                TextureAssets.Item[ModContent.ItemType<Swordsplosion>()] != null) 
            {
                if (!CIRespriteConfig.Instance.AllElemental)
                {
                    //方舟
                    TextureAssets.Item[ModContent.ItemType<ArkoftheElementsold>()] =CIWeaponsResprite.ElemSwordCal;   
                    //短剑
                    TextureAssets.Item[ModContent.ItemType<ElementalShivold>()] = CIWeaponsResprite.ElemShivCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<ElementalShivoldProj>()] = CIWeaponsResprite.ElemShivCal;
                    //长矛
                    TextureAssets.Item[ModContent.ItemType<ElementalLance>()] = CIWeaponsResprite.ElemLanceCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<ElementalLanceProjectile>()] = CIWeaponsResprite.ElemLanceProjCal;
                    //飞盘(近战)
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeElementalDisk>()] = CIWeaponsResprite.ElemDiskCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeElementalDiskProj>()] = CIWeaponsResprite.ElemDiskCal;
                    //射线
                    TextureAssets.Item[ModContent.ItemType<ElementalRayold>()] = CIWeaponsResprite.ElemRayCal;
                    //手套
                    TextureAssets.Item[ModContent.ItemType<ElementalGauntletold>()] = CIResprite.ElemGloveCal;
                    //元素BYD
                    TextureAssets.Item[ModContent.ItemType<ElementalBlaster>()] = CIWeaponsResprite.ElemBYDCal;
                    //爆破
                    TextureAssets.Item[ModContent.ItemType<Swordsplosion>()] = CIWeaponsResprite.RareArkCal;
                    TextureAssets.Item[ModContent.ItemType<ElementalEruptionLegacy>()] = CIWeaponsResprite.ElemFlamethrowerCal;

                }
                if (CIRespriteConfig.Instance.AllElemental)
                {
                    //方舟
                    TextureAssets.Item[ModContent.ItemType<ArkoftheElementsold>()] =CIWeaponsResprite.ElemSwordAlt;   
                    //短剑
                    TextureAssets.Item[ModContent.ItemType<ElementalShivold>()] = CIWeaponsResprite.ElemShivAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<ElementalShivoldProj>()] = CIWeaponsResprite.ElemShivAlt;
                    //长矛
                    TextureAssets.Item[ModContent.ItemType<ElementalLance>()] = CIWeaponsResprite.ElemLanceAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<ElementalLanceProjectile>()] = CIWeaponsResprite.ElemLanceProjAlt;
                    //飞盘(近战)
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeElementalDisk>()] = CIWeaponsResprite.ElemDiskAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeElementalDiskProj>()] = CIWeaponsResprite.ElemDiskAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeElementalDiskProjSplit>()] = CIWeaponsResprite.ElemDiskAlt;
                    //射线
                    TextureAssets.Item[ModContent.ItemType<ElementalRayold>()] = CIWeaponsResprite.ElemRayAlt;
                    //手套
                    TextureAssets.Item[ModContent.ItemType<ElementalGauntletold>()] = CIResprite.ElemGloveAlt;
                    //byd
                    TextureAssets.Item[ModContent.ItemType<ElementalBlaster>()] = CIWeaponsResprite.ElemBYDAlt;
                    //爆破
                    TextureAssets.Item[ModContent.ItemType<Swordsplosion>()] = CIWeaponsResprite.RareArkAlt;
                    //元素喷火器
                    TextureAssets.Item[ModContent.ItemType<ElementalEruptionLegacy>()] = CIWeaponsResprite.ElemFlamethrowerAlt;
                }
            }
            #endregion
            #region 泰拉系列 目前是占位符没有作用
            
            // if (TextureAssets.Item[ModContent.ItemType<BotanicPiercer>()] != null)
            // {
            //     if (!CIRespriteConfig.Instance.AllTerra)
            //     {
            //         TextureAssets.Item[ModContent.ItemType<BotanicPiercer>()] = CIWeaponsResprite.TerraLanceCal;
            //         TextureAssets.Projectile[ModContent.ProjectileType<BotanicPiercerProjectile>()] = CIWeaponsResprite.TerraLanceAlt;
            //     }
            //     if (CIRespriteConfig.Instance.AllTerra)
            //     {
            //         TextureAssets.Item[ModContent.ItemType<BotanicPiercer>()] = CIWeaponsResprite.TerraLanceAlt;
            //         TextureAssets.Projectile[ModContent.ProjectileType<BotanicPiercerProjectile>()] = CIWeaponsResprite.TerraLanceAltProj;
            //     }
            // }
            #endregion
            #region 近战物品
            if (TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] != null)
            {
                if (!CIRespriteConfig.Instance.ArkofCosmosTexture)
                {
                    TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] = CIWeaponsResprite.AotCCal;
                }
                if (CIRespriteConfig.Instance.ArkofCosmosTexture)
                {
                    TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] = CIWeaponsResprite.AotCAlt;
                }
            }
            #region 圣短剑
            if (TextureAssets.Item[ModContent.ItemType<ExcaliburShortsword>()] != null ||
                TextureAssets.Item[ModContent.ItemType<TrueExcaliburShortsword>()] != null ||
                TextureAssets.Item[ModContent.ItemType<NightsStabber>()] != null ||
                TextureAssets.Item[ModContent.ItemType<TrueNightsStabber>()] != null ||
                TextureAssets.Item[ModContent.ItemType<EutrophicShank>()] != null ||
                TextureAssets.Item[ModContent.ItemType<GalileoGladius>()] != null ||
                TextureAssets.Item[ModContent.ItemType<FlameburstShortsword>()] != null ||
                TextureAssets.Item[ModContent.ItemType<AncientShiv>()] != null ||
                TextureAssets.Item[ModContent.ItemType<LeechingDagger>()] != null
                )
            {
                if (!CIRespriteConfig.Instance.AllShivs)
                {
                    //圣短剑
                    TextureAssets.Item[ModContent.ItemType<ExcaliburShortsword>()] = CIWeaponsResprite.HallowedShivCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<ExcaliburShortswordProj>()] = CIWeaponsResprite.HallowedShivCal;
                    //真圣短剑
                    TextureAssets.Item[ModContent.ItemType<TrueExcaliburShortsword>()] = CIWeaponsResprite.TrueHallowedShivCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<TrueExcaliburShortswordProj>()] = CIWeaponsResprite.TrueHallowedShivCal;
                    //永夜短剑
                    TextureAssets.Item[ModContent.ItemType<NightsStabber>()] = CIWeaponsResprite.NightShivCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<NightsStabberProj>()] = CIWeaponsResprite.NightShivCal;
                    //真永夜短剑
                    TextureAssets.Item[ModContent.ItemType<TrueNightsStabber>()] = CIWeaponsResprite.TrueNightShivCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<TrueNightsStabberProj>()] = CIWeaponsResprite.TrueNightShivCal;
                    //伽利略
                    TextureAssets.Item[ModContent.ItemType<GalileoGladius>()] = CIWeaponsResprite.GalileoCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<GalileoGladiusProj>()] = CIWeaponsResprite.GalileoCal;
                    //水华
                    TextureAssets.Item[ModContent.ItemType<EutrophicScimitar>()] = CIWeaponsResprite.SeaShivCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<EutrophicScimitarProj>()] = CIWeaponsResprite.SeaShivCal;
                    //狱炎
                    TextureAssets.Item[ModContent.ItemType<FlameburstShortsword>()] = CIWeaponsResprite.FlameShivCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<FlameburstShortswordProj>()] = CIWeaponsResprite.FlameShivCal;
                    //远古
                    TextureAssets.Item[ModContent.ItemType<AncientShiv>()] = CIWeaponsResprite.DungeonShivCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<AncientShivProj>()] = CIWeaponsResprite.DungeonShivCal;
                    //腐巢
                    TextureAssets.Item[ModContent.ItemType<LeechingDagger>()] = CIWeaponsResprite.HiveMindShivCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<LeechingDaggerProj>()] = CIWeaponsResprite.HiveMindShivCal;


                }
                if (CIRespriteConfig.Instance.AllShivs)
                {
                    //圣短剑
                    TextureAssets.Item[ModContent.ItemType<ExcaliburShortsword>()] = CIWeaponsResprite.HallowedShivAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<ExcaliburShortswordProj>()] = CIWeaponsResprite.HallowedShivAlt;
                    //真圣短剑
                    TextureAssets.Item[ModContent.ItemType<TrueExcaliburShortsword>()] = CIWeaponsResprite.TrueHallowedShivAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<TrueExcaliburShortswordProj>()] = CIWeaponsResprite.TrueHallowedShivAlt;
                    //永夜短剑
                    TextureAssets.Item[ModContent.ItemType<NightsStabber>()] = CIWeaponsResprite.NightShivAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<NightsStabberProj>()] = CIWeaponsResprite.NightShivAlt;
                    //真永夜短剑
                    TextureAssets.Item[ModContent.ItemType<TrueNightsStabber>()] = CIWeaponsResprite.TrueNightShivAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<TrueNightsStabberProj>()] = CIWeaponsResprite.TrueNightShivAlt;
                    //伽利略
                    TextureAssets.Item[ModContent.ItemType<GalileoGladius>()] = CIWeaponsResprite.GalileoAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<GalileoGladiusProj>()] = CIWeaponsResprite.GalileoAlt;
                    //水华
                    TextureAssets.Item[ModContent.ItemType<EutrophicScimitar>()] = CIWeaponsResprite.SeaShivAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<EutrophicScimitarProj>()] = CIWeaponsResprite.SeaShivAlt;
                    //狱炎
                    TextureAssets.Item[ModContent.ItemType<FlameburstShortsword>()] = CIWeaponsResprite.FlameShivAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<FlameburstShortswordProj>()] = CIWeaponsResprite.FlameShivAlt;
                    //远古
                    TextureAssets.Item[ModContent.ItemType<AncientShiv>()] = CIWeaponsResprite.DungeonShivAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<AncientShivProj>()] = CIWeaponsResprite.DungeonShivAlt;
                    //腐巢
                    TextureAssets.Item[ModContent.ItemType<LeechingDagger>()] = CIWeaponsResprite.HiveMindShivAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<LeechingDaggerProj>()] = CIWeaponsResprite.HiveMindShivAlt;
                }
            }
            #endregion
            #region 庇护
            if (TextureAssets.Item[ModContent.ItemType<AegisBlade>()] != null)
            {
                if (!CIRespriteConfig.Instance.AegisResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<AegisBlade>()] = CIWeaponsResprite. AegisCal;
                }
                if (CIRespriteConfig.Instance.AegisResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<AegisBlade>()] = CIWeaponsResprite.AegisAlt;
                }
            }
            #endregion
            #region 明月链刃
            if (TextureAssets.Item[ModContent.ItemType<CrescentMoon>()] != null)
            {
                if (!CIRespriteConfig.Instance.CrescentMoonResprite)
                {
                    // TextureAssets.Item[ModContent.ItemType<CrescentMoon>()] = CIWeaponsResprite.CerscentMoonProjCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<CrescentMoonProj>()] = CIWeaponsResprite.CerscentMoonProjCal;
                }
                if (CIRespriteConfig.Instance.CrescentMoonResprite)
                {
                    // TextureAssets.Item[ModContent.ItemType<GalileoGladius>()] = CIWeaponsResprite.CerscentMoonProjAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<CrescentMoonProj>()] = CIWeaponsResprite.CerscentMoonProjAlt;
                }
            }
            #endregion
            #endregion

            #region 射手物品
            #region 碎颅
            if (TextureAssets.Item[ModContent.ItemType<Skullmasher>()] != null)
            {
                if(!CIRespriteConfig.Instance.SkullmasherResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<Skullmasher>()] = CIWeaponsResprite.SkullmasherCal;
                }
                if(CIRespriteConfig.Instance.SkullmasherResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<Skullmasher>()] = CIWeaponsResprite.Skullmasher;
                }
            }
            #endregion
            #region P90
            if (TextureAssets.Item[ModContent.ItemType<P90Legacy>()] != null)
            {
                if (!CIRespriteConfig.Instance.P90Resprite)
                {
                    TextureAssets.Item[ModContent.ItemType<P90Legacy>()] = CIWeaponsResprite.P90;
                }
                if (CIRespriteConfig.Instance.P90Resprite)
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
                if (!CIRespriteConfig.Instance.DrataliornusResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<DrataliornusLegacy>()] = CIWeaponsResprite.DrataBowLegacyAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<DragonBow>()] = CIWeaponsResprite.DrataBowLegacyAlt;
                }
                if (CIRespriteConfig.Instance.DrataliornusResprite)
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
                if(!CIRespriteConfig.Instance.TriactisHammerResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>()] = CIWeaponsResprite.GiantHammerCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>()] = CIWeaponsResprite.GiantHammerCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>()] = CIWeaponsResprite.GiantHammerCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>()] = CIWeaponsResprite.GiantHammerCal;
                }
                if(CIRespriteConfig.Instance.TriactisHammerResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>()] = CIWeaponsResprite.GiantHammerAlt;
                    
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>()] = CIWeaponsResprite.GiantHammerCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>()] = CIWeaponsResprite.GiantHammerCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>()] = CIWeaponsResprite.GiantHammerCal;
                }
            }
            #endregion
            #region 圣时之锤
            //TODO:同上
            if(TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerPwnageLegacy>()]!=null)
            {
                if(!CIRespriteConfig.Instance.PwnagehammerResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerPwnageLegacy>()] = CIWeaponsResprite.PwnageHammerCal;
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerPwnageLegacy>()] = CIWeaponsResprite.PwnageHammerCal;

                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerPwnageLegacyProj>()] = CIWeaponsResprite.PwnageHammerCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerPwnageLegacyProj>()] = CIWeaponsResprite.PwnageHammerCal;
                }
                if(CIRespriteConfig.Instance.PwnagehammerResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerPwnageLegacy>()] = CIWeaponsResprite.PwnageHammerAlt;
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerPwnageLegacy>()] = CIWeaponsResprite.PwnageHammerAlt;
                    
                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerPwnageLegacyProj>()] = CIWeaponsResprite.PwnageHammerCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerPwnageLegacyProj>()] = CIWeaponsResprite.PwnageHammerCal;
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
                if (!CIRespriteConfig.Instance.StellarContemptResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerStellarContemptLegacy>()] = CIWeaponsResprite.StellarContemptNew;
                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerStellarContemptLegacyProj>()] = CIWeaponsResprite.StellarContemptNew;
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerStellarContempt>()] = CIWeaponsResprite.StellarContemptNew;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] = CIWeaponsResprite.StellarContemptNew;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProjClone>()] = CIWeaponsResprite.StellarContemptNew;
                }
                if (CIRespriteConfig.Instance.StellarContemptResprite)
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
                if (!CIRespriteConfig.Instance.GodSlayerKnivesResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesEmpyrean>()] = CIWeaponsResprite.EmpyreanKnivesCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesEmpyreanProj>()] = CIWeaponsResprite.EmpyreanKnivesCalProj;
                }
                if (CIRespriteConfig.Instance.GodSlayerKnivesResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesEmpyrean>()] = CIWeaponsResprite.EmpyreanKnivesAlt;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesEmpyreanProj>()] = CIWeaponsResprite.EmpyreanKnivesAltProj;
                }
            }
            #endregion
            #region 圣光飞刀
            if (TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] != null &&
                TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] != null)
            {
                if (CIRespriteConfig.Instance.ShadowspecKnivesResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIWeaponsResprite.ShadowKnivesCal;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIWeaponsResprite.ShadowKnivesCalProj;
                }
                if (CIRespriteConfig.Instance.ShadowspecKnivesResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIWeaponsResprite.ShadowKnivsAlt3;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIWeaponsResprite.ShadowKnivsAlt3Proj;
                }
                if (CIRespriteConfig.Instance.ShadowspecKnivesResprite == 3)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIWeaponsResprite.ShadowKnivsAlt2;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIWeaponsResprite.ShadowKnivsAlt2Proj;
                }
                if (CIRespriteConfig.Instance.ShadowspecKnivesResprite == 4)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIWeaponsResprite.ShadowKnivsAlt1;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIWeaponsResprite.ShadowKnivsAlt3Proj;
                }
            }
            #endregion
            #endregion
            #region 法师武器
            #region 氦闪
            if (TextureAssets.Item[ModContent.ItemType<HeliumFlashLegacy>()] != null)
            {
                if(!CIRespriteConfig.Instance.HeliumFlashResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<HeliumFlashLegacy>()] = CIWeaponsResprite.HeliumCal;
                }
                if(CIRespriteConfig.Instance.HeliumFlashResprite)
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
                if (!CIRespriteConfig.Instance.RampartofDeitiesTexture)
                {
                    TextureAssets.Item[ModContent.ItemType<CIRampartofDeities>()] = CIResprite.RampartofDeitiesNew;
                }
                if (CIRespriteConfig.Instance.RampartofDeitiesTexture)
                {
                    TextureAssets.Item[ModContent.ItemType<CIRampartofDeities>()] = CIResprite.RampartofDeitiesOld;
                }
            }
            #endregion
            #region 空灵护符
            if (TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] != null)
            {
                if (!CIRespriteConfig.Instance.EtherealTalismancTexture)
                {
                    TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] = CIResprite.EtherealTalismanNew;
                }
                if (CIRespriteConfig.Instance.EtherealTalismancTexture)
                {
                    TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] = CIResprite.EtherealTalismanOld;
                }
            }
            
            #endregion
            #region 无记名灵基
            if (TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] != null &&
                TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] != null)
            {
                if (!CIRespriteConfig.Instance.FateGirlSprite)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] = CIResprite.FateGirlOriginal;
                    TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] = CIResprite.FateGirlOriginalBuff;
                }
                if (CIRespriteConfig.Instance.FateGirlSprite)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] = CIResprite.FateGirlLegacy;
                    TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] = CIResprite.FateGirlLegacyBuff;
                }
            }
            #endregion
            #region 天使鞋
            if (TextureAssets.Item[ModContent.ItemType<AngelTreads>()] != null)
            {
                if (!CIRespriteConfig.Instance.AngelTreadsResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<AngelTreads>()] = CIResprite.AngelTreadsCalamity; 
                }
                if (CIRespriteConfig.Instance.AngelTreadsResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<AngelTreads>()] = CIResprite.AngelTreadsAlter; 
                }
            }
            #endregion
            #region 夜明跑鞋
            if (TextureAssets.Item[ModContent.ItemType<FasterLunarTracers>()] != null)
            {
                if (!CIRespriteConfig.Instance.LunarBootsResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<FasterLunarTracers>()] = CIResprite.LunarBootsCalamity;
                }
                if (CIRespriteConfig.Instance.LunarBootsResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<FasterLunarTracers>()] = CIResprite.LunarBootsAlter;
                }
            }
            #endregion
            #region 气球他妈
            if (TextureAssets.Item[ModContent.ItemType<MOAB>()] != null)
            {
                //TODO:贴图切换失败 
                if (!CIRespriteConfig.Instance.MOABResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<MOAB>()] = CIResprite.MOABCalamity;
                }
                if (CIRespriteConfig.Instance.MOABResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<MOAB>()] = CIResprite.MOABAlter;
                }
            }
            #endregion

            #endregion
            #region 增益道具
            if (TextureAssets.Item[ModContent.ItemType<BloodOrange>()] != null)
            {
                if (!CIRespriteConfig.Instance.BloodOrangeResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<BloodOrange>()] = CIResprite.HealthOrange;
                }
                if (CIRespriteConfig.Instance.BloodOrangeResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<BloodOrange>()] = CIResprite.HealthOrangeAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] != null)
            {
                if (!CIRespriteConfig.Instance.MiracleFruitResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] = CIResprite.HealthMira;
                }
                if (CIRespriteConfig.Instance.MiracleFruitResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] = CIResprite.HealthMiraAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<Elderberry>()] != null)
            {
                if (!CIRespriteConfig.Instance.ElderberryResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<Elderberry>()] = CIResprite.HealthBerry;
                }
                if (CIRespriteConfig.Instance.ElderberryResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<Elderberry>()] = CIResprite.HealthBerryAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] != null)
            {
                if (!CIRespriteConfig.Instance.DragonfruitResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] = CIResprite.HealthDragon;
                }
                if (CIRespriteConfig.Instance.DragonfruitResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] = CIResprite.HealthDragonAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<CometShard>()] != null)
            {
                if (!CIRespriteConfig.Instance.CometShardResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<CometShard>()] = CIResprite.ManaShard;
                }
                if (CIRespriteConfig.Instance.CometShardResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<CometShard>()] = CIResprite.ManaShardAlter;
                }
            }

            if (TextureAssets.Item[ModContent.ItemType<EtherealCore>()] != null)
            {
                if (!CIRespriteConfig.Instance.EtherealCoreResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<EtherealCore>()] = CIResprite.ManaCore;
                }
                if (CIRespriteConfig.Instance.EtherealCoreResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<EtherealCore>()] = CIResprite.ManaCoreAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<PhantomHeart>()] != null)
            {
                if (!CIRespriteConfig.Instance.PhantomHeartResprite)
                {
                    TextureAssets.Item[ModContent.ItemType<PhantomHeart>()] = CIResprite.ManaHeart;
                }
                if (CIRespriteConfig.Instance.PhantomHeartResprite)
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
                if (CIRespriteConfig.Instance.GalacticaSingularityResprite)
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
                if (CIRespriteConfig.Instance.NecroplasmResprite)
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