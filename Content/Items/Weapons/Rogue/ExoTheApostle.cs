﻿using CalamityMod.Items.Weapons.Rogue;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.Audio;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using Microsoft.Xna.Framework.Graphics;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Projectiles.ExoLore;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Tiles.Furniture.CraftingStations;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class ExoTheApostle : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public static readonly SoundStyle ThrowSound1 = new("CalamityMod/Sounds/Item/RealityRupture") { Volume = 1.2f, PitchVariance = 0.3f };
        public static readonly SoundStyle ThrowSound2 = new("CalamityInheritance/Sounds/Custom/ExoApostleStealth") { Volume = 1.2f, PitchVariance = 0.3f };
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 5555;
            Item.width = 92;
            Item.height = 100;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = CIConfig.Instance.SpecialRarityColor?ModContent.RarityType<SeraphPurple>():ModContent.RarityType<CatalystViolet>();
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ExoSpearProj>();
            Item.shootSpeed = 16f;
        }
        public override float StealthDamageMultiplier => 0.5f;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            if (usPlayer.PanelsLoreExo || usPlayer.LoreExo)
            {
                if (!player.Calamity().StealthStrikeAvailable())
                {
                    Projectile.NewProjectileDirect(source, position, velocity * 1.5f, ModContent.ProjectileType<ExoSpearProj>(), damage, knockback, player.whoAmI);
                    Projectile.NewProjectileDirect(source, position, -velocity * 1.5f, ModContent.ProjectileType<ExoSpearBack>(), damage, knockback, player.whoAmI);
                }
                SoundEngine.PlaySound(ThrowSound1, player.Center);
                if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
                {
                    Projectile.NewProjectileDirect(source, position, -velocity * 3.5f, ModContent.ProjectileType<ExoSpearBack>(), damage, knockback, player.whoAmI);
                    int stealth = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ExoSpearStealthProj>(), damage, knockback, player.whoAmI);
                    SoundEngine.PlaySound(ThrowSound2, player.Center);
                    if (stealth.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[stealth].Calamity().stealthStrike = true;
                        Main.projectile[stealth].usesLocalNPCImmunity = true;
                    }
                }
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity * 1.5f, ModContent.ProjectileType<ExoSpearProjNor>(), damage, knockback, player.whoAmI);
                if (player.Calamity().StealthStrikeAvailable())
                {
                    Projectile.NewProjectile(source, position, velocity * 1.5f, ModContent.ProjectileType<ExoSpearProjNor>(), damage, knockback, player.whoAmI);
                }
            }
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>($"{Generic.WeaponPath}/Rogue/ExoTheApostleGlow").Value);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (usPlayer.LoreExo == true || usPlayer.PanelsLoreExo)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Rogue.ExoTheApostle.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DragonSpear>().
                AddIngredient<StormfrontRazor>().
                AddIngredient<ShardofAntumbra>(500).
                AddIngredient<PhantasmalRuinold>().
                AddIngredient<EclipseSpear>().
                AddIngredient<TarragonThrowingDart>(500).
                DisableDecraft().
                AddIngredient<AuricBarold>(10).
                AddTile<DraedonsForgeold>().
                Register();

            CreateRecipe().
                AddRecipeGroup(CIRecipeGroup.DragonSpear).
                AddIngredient<StormfrontRazor>().
                AddIngredient<ShardofAntumbra>(500).
                AddRecipeGroup("CalamityInheritance:AnyPhantasmalRuin").
                AddRecipeGroup(CIRecipeGroup.EclipsesFall).
                AddIngredient<TarragonThrowingDart>(500).
                AddIngredient<MiracleMatter>().
                AddDecraftCondition(CalamityConditions.DownedExoMechs).
                AddTile<DraedonsForge>().
                Register();
            
            CreateRecipe().
                AddRecipeGroup(CIRecipeGroup.DragonSpear).
                AddIngredient<StormfrontRazor>().
                AddIngredient<ShardofAntumbra>(500).
                AddRecipeGroup("CalamityInheritance:AnyPhantasmalRuin").
                AddRecipeGroup(CIRecipeGroup.EclipsesFall).
                AddIngredient<TarragonThrowingDart>(500).
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                DisableDecraft().
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
