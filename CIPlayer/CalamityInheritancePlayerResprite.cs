using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Accessories.Magic;
using CalamityInheritance.Content.Items.Tools;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Texture;
using CalamityMod.Buffs.Pets;
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
        public static void RespriteOptions()
        {
            #region 
            if(CIConfig.Instance.SetAllLegacySprite == true) //这个用于快速把一些物品转为1457版本的贴图
            {
                CIConfig.Instance.RampartofDeitiesTexture = 2;
                CIConfig.Instance.TriactisHammerResprite = 2;
                CIConfig.Instance.ShadowspecKnivesResprite = 2;
                CIConfig.Instance.BloodOrangeResprite = 2;
                CIConfig.Instance.MiracleFruitResprite= 2;
                CIConfig.Instance.ElderberryResprite  = 2;
                CIConfig.Instance.DragonfruitResprite = 2;
                CIConfig.Instance.CometShardResprite = 2;
                CIConfig.Instance.EtherealCoreResprite = 2;
                CIConfig.Instance.PhantomHeartResprite = 2;
                CIConfig.Instance.StellarContemptResprite  = 2;
                CIConfig.Instance.FateGirlSprite = 2;
                CIConfig.Instance.HeliumFlashResprite = 2;
            }
            #endregion
            #region Texture
            if (TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()] != null)
            {
                if(CIConfig.Instance.WulfumTexture == true)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()] = CIResprite.WulfrumAxeNew;
                }
                if (CIConfig.Instance.WulfumTexture == false)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()] = CIResprite.WulfrumAxeOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()] != null)
            {
                if (CIConfig.Instance.WulfumTexture == true)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()] = CIResprite.WulfrumHammerNew;
                }
                if (CIConfig.Instance.WulfumTexture == false)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()] = CIResprite.WulfrumHammerOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] != null)
            {
                if (CIConfig.Instance.WulfumTexture == true)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] = CIResprite.WulfrumPickaxeNew;
                }
                if (CIConfig.Instance.WulfumTexture == false)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] = CIResprite.WulfrumPickaxeOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] != null)
            {
                if (CIConfig.Instance.ArkofCosmosTexture == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] = CIResprite.ArkoftheCosmosNew;
                }
                if (CIConfig.Instance.ArkofCosmosTexture == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] = CIResprite.ArkoftheCosmosOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<CIRampartofDeities>()] != null)
            {
                if (CIConfig.Instance.RampartofDeitiesTexture == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<CIRampartofDeities>()] = CIResprite.RampartofDeitiesNew;
                }
                if (CIConfig.Instance.RampartofDeitiesTexture == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<CIRampartofDeities>()] = CIResprite.RampartofDeitiesOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] != null)
            {
                if (CIConfig.Instance.EtherealTalismancTexture == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] = CIResprite.EtherealTalismanNew;
                }
                if (CIConfig.Instance.EtherealTalismancTexture == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] = CIResprite.EtherealTalismanOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<Skullmasher>()] != null)
            {
                if(CIConfig.Instance.SkullmasherResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Skullmasher>()] = CIResprite.Skullmasher1p5;
                }
                if(CIConfig.Instance.SkullmasherResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Skullmasher>()] = CIResprite.Skullmasher;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<P90Legacy>()] != null)
            {
                if (CIConfig.Instance.P90Resprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<P90Legacy>()] = CIResprite.P90;
                }
                if (CIConfig.Instance.P90Resprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<P90Legacy>()] = CIResprite.P90Legacy;
                }
            }

            if(TextureAssets.Item[ModContent.ItemType<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>()]!=null)
            {
                if(CIConfig.Instance.TriactisHammerResprite ==1)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>()] = CIResprite.TriactisHammerCalamity;
                }
                if(CIConfig.Instance.TriactisHammerResprite ==2)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>()] = CIResprite.TriactisHammerAlter;
                    
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>()] = CIResprite.TriactisHammerCalamity;
                }
            }

            if(TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>()]!=null)
            {
                if(CIConfig.Instance.TriactisHammerResprite ==1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>()] = CIResprite.TriactisHammerCalamity;
                }
                if(CIConfig.Instance.TriactisHammerResprite ==2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>()] = CIResprite.TriactisHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>()] = CIResprite.TriactisHammerCalamity;
                }
            }

            if(TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerPwnageLegacy>()]!=null)
            {
                if(CIConfig.Instance.PwnagehammerResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerPwnageLegacy>()] = CIResprite.HallowedHammerCalamity;
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerPwnageLegacy>()] = CIResprite.HallowedHammerCalamity;

                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerPwnageLegacyProj>()] = CIResprite.HallowedHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerPwnageLegacyProj>()] = CIResprite.HallowedHammerCalamity;
                }
                if(CIConfig.Instance.PwnagehammerResprite == 2)
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
                if(CIConfig.Instance.PwnagehammerResprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<MeleeTypeHammerPwnageLegacyProj>()] = CIResprite.HallowedHammerCalamity;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerPwnageLegacyProj>()] = CIResprite.HallowedHammerCalamity;
                }
                if(CIConfig.Instance.PwnagehammerResprite == 2)
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
                if (CIConfig.Instance.GodSlayerKnivesResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesEmpyrean>()] = CIResprite.EmpyreanKnivesCalamity;
                }
                if (CIConfig.Instance.GodSlayerKnivesResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesEmpyrean>()] = CIResprite.EmpyreanKnivesAlterTypeOne;
                }
            }

            if (TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesEmpyreanProj>()] != null)
            {
                if (CIConfig.Instance.GodSlayerKnivesResprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesEmpyreanProj>()] = CIResprite.EmpyreanKnivesCalamityProj;
                }
                if (CIConfig.Instance.GodSlayerKnivesResprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesEmpyreanProj>()] = CIResprite.EmpyreanKnivesAlterTypeOneProj;
                }
            }

            if (TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] != null)
            {
                if (CIConfig.Instance.ShadowspecKnivesResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIResprite.ShadowspecKnivesCalamity;
                }
                if (CIConfig.Instance.ShadowspecKnivesResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIResprite.ShadowspecKnivesAlterThird;
                }
                if (CIConfig.Instance.ShadowspecKnivesResprite == 3)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIResprite.ShadowspecKnivesAlterSec;
                }
                if (CIConfig.Instance.ShadowspecKnivesResprite == 4)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeKnivesShadowspec>()] = CIResprite.ShadowspecKnivesAlterFirst;
                }
            }

            if (TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] != null)
            {
                if (CIConfig.Instance.ShadowspecKnivesResprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIResprite.ShadowspecKnivesCalamityProj;
                }
                if (CIConfig.Instance.ShadowspecKnivesResprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIResprite.ShadowspecKnivesAlterThirdProj;
                }
                if (CIConfig.Instance.ShadowspecKnivesResprite == 3)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIResprite.ShadowspecKnivesAlterSecProj;
                }
                if (CIConfig.Instance.ShadowspecKnivesResprite == 4)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeKnivesShadowspecProj>()] = CIResprite.ShadowspecKnivesAlterThirdProj;
                }
            }
            #endregion
            if (TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] != null)
            {
                if (CIConfig.Instance.FateGirlSprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] = CIResprite.FateGirlOriginal;
                }
                if (CIConfig.Instance.FateGirlSprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] = CIResprite.FateGirlLegacy;
                }
            }

            if (TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] != null)
            {
                if (CIConfig.Instance.FateGirlSprite== 1)
                {
                    TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] = CIResprite.FateGirlOriginalBuff;
                }
                if (CIConfig.Instance.FateGirlSprite== 2)
                {
                    TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] = CIResprite.FateGirlLegacyBuff;
                }
            }
            #region 星体击碎者
            if (TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerStellarContemptLegacy>()] != null)
            {
                if (CIConfig.Instance.StellarContemptResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerStellarContemptLegacy>()] = CIResprite.StellarContemptNew;
                }
                if (CIConfig.Instance.StellarContemptResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<MeleeTypeHammerStellarContemptLegacy>()] = CIResprite.StellarContemptOld;
                }
            }

            if (TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] != null)
            {
                if (CIConfig.Instance.StellarContemptResprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] = CIResprite.StellarContemptNew;
                }
                if (CIConfig.Instance.StellarContemptResprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] = CIResprite.StellarContemptOld;
                }
            }
            //盗贼
            if (TextureAssets.Item[ModContent.ItemType<RogueTypeHammerStellarContempt>()] != null)
            {
                if (CIConfig.Instance.StellarContemptResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerStellarContempt>()] = CIResprite.StellarContemptNew;
                }
                if (CIConfig.Instance.StellarContemptResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeHammerStellarContempt>()] = CIResprite.StellarContemptOld;
                }
            }

            if (TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] != null)
            {
                if (CIConfig.Instance.StellarContemptResprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] = CIResprite.StellarContemptNew;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProjClone>()] = CIResprite.StellarContemptNew;
                }
                if (CIConfig.Instance.StellarContemptResprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProj>()] = CIResprite.StellarContemptOld;
                    TextureAssets.Projectile[ModContent.ProjectileType<RogueTypeHammerStellarContemptProjClone>()] = CIResprite.StellarContemptOld;
                }
            }
            #endregion
            #region 氦闪
            if (TextureAssets.Item[ModContent.ItemType<HeliumFlashLegacy>()] != null)
            {
                if(CIConfig.Instance.HeliumFlashResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<HeliumFlashLegacy>()] = CIResprite.HeliumFlashCalamity;
                }
                if(CIConfig.Instance.HeliumFlashResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<HeliumFlashLegacy>()] = CIResprite.HeliumFlashLegacy;
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
                if (CIConfig.Instance.GalacticaSingularityResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<GalacticaSingularity>()] = CIResprite.GS;
                }
                if (CIConfig.Instance.GalacticaSingularityResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<GalacticaSingularity>()] = CIResprite.GSAlter;
                }
            }

            if (TextureAssets.Item[ModContent.ItemType<Necroplasm>()] != null)
            {
                if (CIConfig.Instance.NecroplasmResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Necroplasm>()] = CIResprite.RedSoul;
                }
                if (CIConfig.Instance.NecroplasmResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Necroplasm>()] = CIResprite.RedSoulAlter;
                }
            }
            */
            #region  增益道具
            if (TextureAssets.Item[ModContent.ItemType<BloodOrange>()] != null)
            {
                if (CIConfig.Instance.BloodOrangeResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<BloodOrange>()] = CIResprite.HealthOrange;
                }
                if (CIConfig.Instance.BloodOrangeResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<BloodOrange>()] = CIResprite.HealthOrangeAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] != null)
            {
                if (CIConfig.Instance.MiracleFruitResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] = CIResprite.HealthMira;
                }
                if (CIConfig.Instance.MiracleFruitResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] = CIResprite.HealthMiraAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<Elderberry>()] != null)
            {
                if (CIConfig.Instance.ElderberryResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Elderberry>()] = CIResprite.HealthBerry;
                }
                if (CIConfig.Instance.ElderberryResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Elderberry>()] = CIResprite.HealthBerryAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] != null)
            {
                if (CIConfig.Instance.DragonfruitResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] = CIResprite.HealthDragon;
                }
                if (CIConfig.Instance.DragonfruitResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] = CIResprite.HealthDragonAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<CometShard>()] != null)
            {
                if (CIConfig.Instance.CometShardResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<CometShard>()] = CIResprite.ManaShard;
                }
                if (CIConfig.Instance.CometShardResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<CometShard>()] = CIResprite.ManaShardAlter;
                }
            }

            if (TextureAssets.Item[ModContent.ItemType<EtherealCore>()] != null)
            {
                if (CIConfig.Instance.EtherealCoreResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<EtherealCore>()] = CIResprite.ManaCore;
                }
                if (CIConfig.Instance.EtherealCoreResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<EtherealCore>()] = CIResprite.ManaCoreAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<PhantomHeart>()] != null)
            {
                if (CIConfig.Instance.PhantomHeartResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<PhantomHeart>()] = CIResprite.ManaHeart;
                }
                if (CIConfig.Instance.PhantomHeartResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<PhantomHeart>()] = CIResprite.ManaHeartAlter;
                }
            }

            #endregion

            #endregion
        }
    }
}