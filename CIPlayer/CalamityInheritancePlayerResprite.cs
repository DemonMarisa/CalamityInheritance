
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
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Pets;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public static void RespriteOptions()
        {
            #region Texture
            if (TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()] != null)
            {
                if(CalamityInheritanceConfig.Instance.WulfumTexture == true)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()] = CalamityInheritanceTexture.WulfrumAxeNew;
                }
                if (CalamityInheritanceConfig.Instance.WulfumTexture == false)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumAxe>()] = CalamityInheritanceTexture.WulfrumAxeOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.WulfumTexture == true)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()] = CalamityInheritanceTexture.WulfrumHammerNew;
                }
                if (CalamityInheritanceConfig.Instance.WulfumTexture == false)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumHammer>()] = CalamityInheritanceTexture.WulfrumHammerOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.WulfumTexture == true)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] = CalamityInheritanceTexture.WulfrumPickaxeNew;
                }
                if (CalamityInheritanceConfig.Instance.WulfumTexture == false)
                {
                    TextureAssets.Item[ModContent.ItemType<WulfrumPickaxe>()] = CalamityInheritanceTexture.WulfrumPickaxeOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.ArkofCosmosTexture == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] = CalamityInheritanceTexture.ArkoftheCosmosNew;
                }
                if (CalamityInheritanceConfig.Instance.ArkofCosmosTexture == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<ArkoftheCosmosold>()] = CalamityInheritanceTexture.ArkoftheCosmosOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<CIRampartofDeities>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.RampartofDeitiesTexture == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<CIRampartofDeities>()] = CalamityInheritanceTexture.RampartofDeitiesNew;
                }
                if (CalamityInheritanceConfig.Instance.RampartofDeitiesTexture == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<CIRampartofDeities>()] = CalamityInheritanceTexture.RampartofDeitiesOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.EtherealTalismancTexture == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] = CalamityInheritanceTexture.EtherealTalismanNew;
                }
                if (CalamityInheritanceConfig.Instance.EtherealTalismancTexture == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<AncientEtherealTalisman>()] = CalamityInheritanceTexture.EtherealTalismanOld;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<Skullmasher>()] != null)
            {
                if(CalamityInheritanceConfig.Instance.SkullmasherResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Skullmasher>()] = CalamityInheritanceTexture.Skullmasher1p5;
                }
                if(CalamityInheritanceConfig.Instance.SkullmasherResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Skullmasher>()] = CalamityInheritanceTexture.Skullmasher;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<P90Legacy>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.P90Resprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<P90Legacy>()] = CalamityInheritanceTexture.P90;
                }
                if (CalamityInheritanceConfig.Instance.P90Resprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<P90Legacy>()] = CalamityInheritanceTexture.P90Legacy;
                }
            }

            //TODO Scarlet:射弹的贴图并未正确替换，等我之后知道啥原因了可能就会弄好了
            //改好了
            #region knives
            if (TextureAssets.Item[ModContent.ItemType<EmpyreanKnivesLegacyRogue>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.GodSlayerKnivesResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<EmpyreanKnivesLegacyRogue>()] = CalamityInheritanceTexture.GodSlayerKnivesLegacyType;
                }
                if (CalamityInheritanceConfig.Instance.GodSlayerKnivesResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<EmpyreanKnivesLegacyRogue>()] = CalamityInheritanceTexture.GodSlayerKnivesAlterType;
                }
            }

            if (TextureAssets.Projectile[ModContent.ProjectileType<EmpyreanKnivesProjectileLegacyRogue>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.GodSlayerKnivesResprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<EmpyreanKnivesProjectileLegacyRogue>()] = CalamityInheritanceTexture.GodSlayerKnivesLegacyTypeProj;
                }
                if (CalamityInheritanceConfig.Instance.GodSlayerKnivesResprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<EmpyreanKnivesProjectileLegacyRogue>()] = CalamityInheritanceTexture.GodSlayerKnivesAlterTypeProj;
                }
            }

            if (TextureAssets.Item[ModContent.ItemType<ShadowspecKnivesLegacyRogue>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<ShadowspecKnivesLegacyRogue>()] = CalamityInheritanceTexture.ShadowspecKnivesAlterTypeOne;
                }
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<ShadowspecKnivesLegacyRogue>()] = CalamityInheritanceTexture.ShadowspecKnivesLegacyType;
                }
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 3)
                {
                    TextureAssets.Item[ModContent.ItemType<ShadowspecKnivesLegacyRogue>()] = CalamityInheritanceTexture.ShadowspecKnivesAlterTypeOne;
                }
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 4)
                {
                    TextureAssets.Item[ModContent.ItemType<ShadowspecKnivesLegacyRogue>()] = CalamityInheritanceTexture.ShadowspecKnivesAlterTypeSecond;
                }
            }

            if (TextureAssets.Projectile[ModContent.ProjectileType<ShadowspecKnivesProjectileLegacyRogue>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<ShadowspecKnivesProjectileLegacyRogue>()] = CalamityInheritanceTexture.ShadowspecKnivesAlterTypeProjOne;
                }
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<ShadowspecKnivesProjectileLegacyRogue>()] = CalamityInheritanceTexture.ShadowspecKnivesLegacyTypeProj;
                }
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 3)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<ShadowspecKnivesProjectileLegacyRogue>()] = CalamityInheritanceTexture.ShadowspecKnivesAlterTypeProjOne;
                }
                if (CalamityInheritanceConfig.Instance.ShadowspecKnivesResprite == 4)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<ShadowspecKnivesProjectileLegacyRogue>()] = CalamityInheritanceTexture.ShadowspecKnivesAlterTypeProjSecond;
                }
            }
            #endregion
            if (TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.FateGirlSprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] = CalamityInheritanceTexture.FateGirlOriginal;
                }
                if (CalamityInheritanceConfig.Instance.FateGirlSprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<DaawnlightSpiritOriginMinion>()] = CalamityInheritanceTexture.FateGirlLegacy;
                }
            }

            if (TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.FateGirlSprite== 1)
                {
                    TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] = CalamityInheritanceTexture.FateGirlOriginalBuff;
                }
                if (CalamityInheritanceConfig.Instance.FateGirlSprite== 2)
                {
                    TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] = CalamityInheritanceTexture.FateGirlLegacyBuff;
                }
            }
            #region 星体击碎者
            if (TextureAssets.Item[ModContent.ItemType<StellarContemptOld>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<StellarContemptOld>()] = CalamityInheritanceTexture.StellarContemptNew;
                }
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<StellarContemptOld>()] = CalamityInheritanceTexture.StellarContemptOld;
                }
            }

            if (TextureAssets.Projectile[ModContent.ProjectileType<StellarContemptHammerOld>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<StellarContemptHammerOld>()] = CalamityInheritanceTexture.StellarContemptNew;
                }
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<StellarContemptHammerOld>()] = CalamityInheritanceTexture.StellarContemptOld;
                }
            }
            //盗贼
            if (TextureAssets.Item[ModContent.ItemType<RogueTypeStellarContempt>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeStellarContempt>()] = CalamityInheritanceTexture.StellarContemptNew;
                }
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<RogueTypeStellarContempt>()] = CalamityInheritanceTexture.StellarContemptOld;
                }
            }

            if (TextureAssets.Projectile[ModContent.ProjectileType<StellarContemptHammerRogue>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 1)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<StellarContemptHammerRogue>()] = CalamityInheritanceTexture.StellarContemptNew;
                }
                if (CalamityInheritanceConfig.Instance.StellarContemptResprite == 2)
                {
                    TextureAssets.Projectile[ModContent.ProjectileType<StellarContemptHammerRogue>()] = CalamityInheritanceTexture.StellarContemptOld;
                }
            }
            #endregion
            //核子与星系是动图（现灾厄）不能直接用
            // if (TextureAssets.Item[ModContent.ItemType<Nucleogenesis>()] != null)
            // {
            //     if (CalamityInheritanceConfig.Instance.NucleogenesisResprite == 1)
            //     {
            //         TextureAssets.Item[ModContent.ItemType<Nucleogenesis>()] = CalamityInheritanceTexture.SummonNuke;
            //     }
            //     if (CalamityInheritanceConfig.Instance.NucleogenesisResprite == 2)
            //     {
            //         TextureAssets.Item[ModContent.ItemType<Nucleogenesis>()] = CalamityInheritanceTexture.SummonNukeAlter;
            //     }
            // }
            // if (TextureAssets.Item[ModContent.ItemType<GalacticaSingularity>()] != null)
            // {
            //     if (CalamityInheritanceConfig.Instance.GalacticaSingularityResprite== 1)
            //     {
            //         TextureAssets.Item[ModContent.ItemType<GalacticaSingularity>()] = CalamityInheritanceTexture.GS;
            //     }
            //     if (CalamityInheritanceConfig.Instance.GalacticaSingularityResprite== 2)
            //     {
            //         TextureAssets.Item[ModContent.ItemType<GalacticaSingularity>()] = CalamityInheritanceTexture.GSAlter;
            //     }
            // }
            // if (TextureAssets.Item[ModContent.ItemType<Necroplasm>()] != null)
            // {
            //     if (CalamityInheritanceConfig.Instance.NecroplasmResprite== 1)
            //     {
            //         TextureAssets.Item[ModContent.ItemType<Necroplasm>()] = CalamityInheritanceTexture.RedSoul;
            //     }
            //     if (CalamityInheritanceConfig.Instance.NecroplasmResprite== 2)
            //     {
            //         TextureAssets.Item[ModContent.ItemType<Necroplasm>()] = CalamityInheritanceTexture.RedSoulAlter;
            //     }
            // }
            #region  增益道具
            if (TextureAssets.Item[ModContent.ItemType<BloodOrange>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.BloodOrangeResprite == 1)
                {
                    TextureAssets.Item[ModContent.ItemType<BloodOrange>()] = CalamityInheritanceTexture.HealthOrange;
                }
                if (CalamityInheritanceConfig.Instance.BloodOrangeResprite == 2)
                {
                    TextureAssets.Item[ModContent.ItemType<BloodOrange>()] = CalamityInheritanceTexture.HealthOrangeAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.MiracleFruitResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] = CalamityInheritanceTexture.HealthMira;
                }
                if (CalamityInheritanceConfig.Instance.MiracleFruitResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<MiracleFruit>()] = CalamityInheritanceTexture.HealthMiraAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<Elderberry>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.ElderberryResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Elderberry>()] = CalamityInheritanceTexture.HealthBerry;
                }
                if (CalamityInheritanceConfig.Instance.ElderberryResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Elderberry>()] = CalamityInheritanceTexture.HealthBerryAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.DragonfruitResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] = CalamityInheritanceTexture.HealthDragon;
                }
                if (CalamityInheritanceConfig.Instance.DragonfruitResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<Dragonfruit>()] = CalamityInheritanceTexture.HealthDragonAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<CometShard>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.CometShardResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<CometShard>()] = CalamityInheritanceTexture.ManaShard;
                }
                if (CalamityInheritanceConfig.Instance.CometShardResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<CometShard>()] = CalamityInheritanceTexture.ManaShardAlter;
                }
            }

            if (TextureAssets.Item[ModContent.ItemType<EtherealCore>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.EtherealCoreResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<EtherealCore>()] = CalamityInheritanceTexture.ManaCore;
                }
                if (CalamityInheritanceConfig.Instance.EtherealCoreResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<EtherealCore>()] = CalamityInheritanceTexture.ManaCoreAlter;
                }
            }
            if (TextureAssets.Item[ModContent.ItemType<PhantomHeart>()] != null)
            {
                if (CalamityInheritanceConfig.Instance.PhantomHeartResprite== 1)
                {
                    TextureAssets.Item[ModContent.ItemType<PhantomHeart>()] = CalamityInheritanceTexture.ManaHeart;
                }
                if (CalamityInheritanceConfig.Instance.PhantomHeartResprite== 2)
                {
                    TextureAssets.Item[ModContent.ItemType<PhantomHeart>()] = CalamityInheritanceTexture.ManaHeartAlter;
                }
            }

            #endregion

            #endregion
        }
    }
}