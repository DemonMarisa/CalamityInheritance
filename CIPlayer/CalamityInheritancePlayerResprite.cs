using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Accessories.Magic;
using CalamityInheritance.Content.Items.Tools;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Texture;
using CalamityMod.Buffs.Pets;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Projectiles.Pets;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public static void RespriteOptions()
        {
            #region 
            if(CalamityInheritanceConfig.Instance.SetAllLegacySprite == true) //这个用于快速把一些物品转为1457版本的贴图
            {
                CalamityInheritanceConfig.Instance.RampartofDeitiesTexture = 2;
                CalamityInheritanceConfig.Instance.TriactisHammerResprite = 2;
                CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite = 2;
                CalamityInheritanceConfig.Instance.BloodOrangeResprite = 2;
                CalamityInheritanceConfig.Instance.MiracleFruitResprite= 2;
                CalamityInheritanceConfig.Instance.ElderberryResprite  = 2;
                CalamityInheritanceConfig.Instance.DragonfruitResprite = 2;
                CalamityInheritanceConfig.Instance.CometShardResprite = 2;
                CalamityInheritanceConfig.Instance.EtherealCoreResprite = 2;
                CalamityInheritanceConfig.Instance.PhantomHeartResprite = 2;
                CalamityInheritanceConfig.Instance.StellarContemptResprite  = 2;
                CalamityInheritanceConfig.Instance.FateGirlSprite = 2;
            }
            #endregion
            #region Texture
            if (TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()] != null)
            {
                if(CalamityInheritanceConfig.Instance.WulfumTexture == true)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()] = CIResprite.WulfrumAxeNew;
                }
                if (CalamityInheritanceConfig.Instance.WulfumTexture == false)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()] = CIResprite.WulfrumAxeOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.WulfumTexture == true)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()] = CIResprite.WulfrumHammerNew;
                }
                if (CalamityInheritanceConfig.Instance.WulfumTexture == false)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()] = CIResprite.WulfrumHammerOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.WulfumTexture == true)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] = CIResprite.WulfrumPickaxeNew;
                }
                if (CalamityInheritanceConfig.Instance.WulfumTexture == false)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] = CIResprite.WulfrumPickaxeOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.ArkofCosmosTexture == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] = CIResprite.ArkoftheCosmosNew;
                }
                if (CalamityInheritanceConfig.Instance.ArkofCosmosTexture == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] = CIResprite.ArkoftheCosmosOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<CIRampartofDeities>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.RampartofDeitiesTexture == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<CIRampartofDeities>()] = CIResprite.RampartofDeitiesNew;
                }
                if (CalamityInheritanceConfig.Instance.RampartofDeitiesTexture == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<CIRampartofDeities>()] = CIResprite.RampartofDeitiesOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.EtherealTalismancTexture == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] = CIResprite.EtherealTalismanNew;
                }
                if (CalamityInheritanceConfig.Instance.EtherealTalismancTexture == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] = CIResprite.EtherealTalismanOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<Skullmasher>()] != null)
            {
                if(CalamityInheritanceConfig.Instance.SkullmasherResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Skullmasher>()] = CIResprite.Skullmasher1p5;
                }
                if(CalamityInheritanceConfig.Instance.SkullmasherResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Skullmasher>()] = CIResprite.Skullmasher;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<P90Legacy>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.P90Resprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<P90Legacy>()] = CIResprite.P90;
                }
                if (CalamityInheritanceConfig.Instance.P90Resprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<P90Legacy>()] = CIResprite.P90Legacy;
                }
            }

            if(TextureAssets.Item[ModContent.ItemType<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>()]!=null)
            {
                if(CalamityInheritanceConfig.Instance.TriactisHammerResprite ==1)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>()] = CIResprite.TriactisHammerCalamity;
                }
                if(CalamityInheritanceConfig.Instance.TriactisHammerResprite ==2)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>()] = CIResprite.TriactisHammerAlter;
                    
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>()] = CIResprite.TriactisHammerCalamity;
                }
            }

            if(TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>()]!=null)
            {
                if(CalamityInheritanceConfig.Instance.TriactisHammerResprite ==1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>()] = CIResprite.TriactisHammerCalamity;
                }
                if(CalamityInheritanceConfig.Instance.TriactisHammerResprite ==2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>()] = CIResprite.TriactisHammerCalamity;
                }
            }

            if(TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerPwnageLegacy>()]!=null)
            {
                if(CalamityInheritanceConfig.Instance.PwnagehammerResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerPwnageLegacy>()] = CIResprite.HallowedHammerCalamity;
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerPwnageLegacy>()] = CIResprite.HallowedHammerCalamity;

                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerPwnageLegacyProj>()] = CIResprite.HallowedHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerPwnageLegacyProj>()] = CIResprite.HallowedHammerCalamity;
                }
                if(CalamityInheritanceConfig.Instance.PwnagehammerResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerPwnageLegacy>()] = CIResprite.HallowedHammerAlter;
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerPwnageLegacy>()] = CIResprite.HallowedHammerAlter;
                    
                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerPwnageLegacyProj>()] = CIResprite.HallowedHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerPwnageLegacyProj>()] = CIResprite.HallowedHammerCalamity;
                }
            }

            if(TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerPwnageLegacyProj>()]!=null ||
               TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerPwnageLegacyProj>()]!=null)
            {
                if(CalamityInheritanceConfig.Instance.PwnagehammerResprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerPwnageLegacyProj>()] = CIResprite.HallowedHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerPwnageLegacyProj>()] = CIResprite.HallowedHammerCalamity;
                }
                if(CalamityInheritanceConfig.Instance.PwnagehammerResprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerPwnageLegacyProj>()] = CIResprite.HallowedHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerPwnageLegacyProj>()] = CIResprite.HallowedHammerCalamity;
                }
            }


            //TODO Scarlet:射弹的贴图并未正确替换，等我之后知道啥原因了可能就会弄好了
            //改好了
            #region knives
            if (TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesEmpyrean>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.GodSlayerKnivesResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesEmpyrean>()] = CIResprite.EmpyreanKnivesCalamity;
                }
                if (CalamityInheritanceConfig.Instance.GodSlayerKnivesResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesEmpyrean>()] = CIResprite.EmpyreanKnivesAlterTypeOne;
                }
            }

            if (TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesEmpyreanProj>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.GodSlayerKnivesResprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesEmpyreanProj>()] = CIResprite.EmpyreanKnivesCalamityProj;
                }
                if (CalamityInheritanceConfig.Instance.GodSlayerKnivesResprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesEmpyreanProj>()] = CIResprite.EmpyreanKnivesAlterTypeOneProj;
                }
            }

            if (TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIResprite.ShadowspecKnivesCalamity;
                }
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIResprite.ShadowspecKnivesAlterThird;
                }
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 3)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIResprite.ShadowspecKnivesAlterSec;
                }
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 4)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIResprite.ShadowspecKnivesAlterFirst;
                }
            }

            if (TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIResprite.ShadowspecKnivesCalamityProj;
                }
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIResprite.ShadowspecKnivesAlterThirdProj;
                }
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 3)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIResprite.ShadowspecKnivesAlterSecProj;
                }
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 4)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIResprite.ShadowspecKnivesAlterThirdProj;
                }
            }
            #endregion
            if (TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.FateGirlSprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] = CIResprite.FateGirlOriginal;
                }
                if (CalamityInheritanceConfig.Instance.FateGirlSprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] = CIResprite.FateGirlLegacy;
                }
            }

            if (TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.FateGirlSprite== 1)
                {
                    TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] = CIResprite.FateGirlOriginalBuff;
                }
                if (CalamityInheritanceConfig.Instance.FateGirlSprite== 2)
                {
                    TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] = CIResprite.FateGirlLegacyBuff;
                }
            }
            #region 星体击碎者
            if (TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerStellarContemptLegacy>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerStellarContemptLegacy>()] = CIResprite.StellarContemptNew;
                }
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerStellarContemptLegacy>()] = CIResprite.StellarContemptOld;
                }
            }

            if (TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] = CIResprite.StellarContemptNew;
                }
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] = CIResprite.StellarContemptOld;
                }
            }
            //盗贼
            if (TextureAssets.Item[ModContent.ItemType<RogueTypeHammerStellarContempt>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerStellarContempt>()] = CIResprite.StellarContemptNew;
                }
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerStellarContempt>()] = CIResprite.StellarContemptOld;
                }
            }

            if (TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] = CIResprite.StellarContemptNew;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProjClone>()] = CIResprite.StellarContemptNew;
                }
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] = CIResprite.StellarContemptOld;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProjClone>()] = CIResprite.StellarContemptOld;
                }
            }
            #endregion
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
                if (CalamityInheritanceConfig.Instance.GalacticaSingularityResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<GalacticaSingularity>()] = CIResprite.GS;
                }
                if (CalamityInheritanceConfig.Instance.GalacticaSingularityResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<GalacticaSingularity>()] = CIResprite.GSAlter;
                }
            }

            if (TextureAssets.Item[ModContent.ItemType<Necroplasm>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.NecroplasmResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Necroplasm>()] = CIResprite.RedSoul;
                }
                if (CalamityInheritanceConfig.Instance.NecroplasmResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Necroplasm>()] = CIResprite.RedSoulAlter;
                }
            }
            */
            #region  增益道具
            if (TextureAssets.Item[ModContent.ItemType<BloodOrange>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.BloodOrangeResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<BloodOrange>()] = CIResprite.HealthOrange;
                }
                if (CalamityInheritanceConfig.Instance.BloodOrangeResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<BloodOrange>()] = CIResprite.HealthOrangeAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.MiracleFruitResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] = CIResprite.HealthMira;
                }
                if (CalamityInheritanceConfig.Instance.MiracleFruitResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] = CIResprite.HealthMiraAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<Elderberry>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.ElderberryResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Elderberry>()] = CIResprite.HealthBerry;
                }
                if (CalamityInheritanceConfig.Instance.ElderberryResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Elderberry>()] = CIResprite.HealthBerryAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.DragonfruitResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] = CIResprite.HealthDragon;
                }
                if (CalamityInheritanceConfig.Instance.DragonfruitResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] = CIResprite.HealthDragonAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<CometShard>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.CometShardResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<CometShard>()] = CIResprite.ManaShard;
                }
                if (CalamityInheritanceConfig.Instance.CometShardResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<CometShard>()] = CIResprite.ManaShardAlter;
                }
            }

            if (TextureAssets.Item[ModContent.ItemType<EtherealCore>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.EtherealCoreResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<EtherealCore>()] = CIResprite.ManaCore;
                }
                if (CalamityInheritanceConfig.Instance.EtherealCoreResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<EtherealCore>()] = CIResprite.ManaCoreAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<PhantomHeart>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.PhantomHeartResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<PhantomHeart>()] = CIResprite.ManaHeart;
                }
                if (CalamityInheritanceConfig.Instance.PhantomHeartResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<PhantomHeart>()] = CIResprite.ManaHeartAlter;
                }
            }

            #endregion

            #endregion
        }
    }
}