﻿using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class VividClarityOld : CIMagic, ILocalizedModType
    {
        public static readonly SoundStyle UseSound = new("CalamityMod/Sounds/Item/VividClarityShoot") { Volume = 0.9f };
        public static readonly SoundStyle BeamSound = new("CalamityMod/Sounds/Item/VividClarityBeamAppear");
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 90;
            Item.height = 112;
            Item.damage = 300;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 40;
            Item.useTime = 6;
            Item.useAnimation = 54;
            Item.reuseDelay = 25;
            Item.useLimitPerAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.UseSound = UseSound;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CIVividBeam>();
            Item.shootSpeed = 12f;
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }
        public override bool CanUseItem(Player player)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                Item.useTime = 6;
                Item.useAnimation = 54;
                Item.reuseDelay = 25;
                Item.useLimitPerAnimation = 9;
            }
            else
            {
                Item.useTime = 4;
                Item.useAnimation = 20;
                Item.reuseDelay = Item.useAnimation;
                Item.useLimitPerAnimation = 5;
            }
            return base.CanUseItem(player);
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // 只比普通模式搞高一点
            if (Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo)
                damage.Base *= 1.2f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo projSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();

            Vector2 playerPos = player.RotatedRelativePoint(player.MountedCenter, true);
            float speed = Item.shootSpeed;
            float xPos = Main.mouseX + Main.screenPosition.X - playerPos.X;
            float yPos = Main.mouseY + Main.screenPosition.Y - playerPos.Y;
            float f = Main.rand.NextFloat() * MathHelper.TwoPi;
            float spreadX = 20f;
            float spreadY = 60f;
            float sourceVariationLow = 120f;
            float sourceVariationHigh = 180f;
            Vector2 source = playerPos + f.ToRotationVector2() * MathHelper.Lerp(sourceVariationLow, sourceVariationHigh, Main.rand.NextFloat());
            Vector2 sourceExoLore = playerPos + f.ToRotationVector2() * MathHelper.Lerp(spreadX, spreadY, Main.rand.NextFloat());
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                for (int i = 0; i < 50; i++)
                {
                    source = playerPos + f.ToRotationVector2() * MathHelper.Lerp(sourceVariationLow, sourceVariationHigh, Main.rand.NextFloat());
                    if (Collision.CanHit(playerPos, 0, 0, source + (source - playerPos).SafeNormalize(Vector2.UnitX) * 8f, 0, 0))
                    {
                        break;
                    }
                    f = Main.rand.NextFloat() * MathHelper.TwoPi;
                }
                Vector2 velocityReal = Main.MouseWorld - source;
                Vector2 velocityVariation = new Vector2(xPos, yPos).SafeNormalize(Vector2.UnitY) * speed;
                velocityReal = velocityReal.SafeNormalize(velocityVariation) * speed;
                velocityReal = Vector2.Lerp(velocityReal, velocityVariation, 0.25f);

                Projectile.NewProjectile(projSource, source, velocityReal, ModContent.ProjectileType<CIVividBeamExoLore>(), damage, knockback, player.whoAmI, 0f, Main.rand.Next(3));

                Vector2 targetPosition = Main.MouseWorld;
                player.itemRotation = CIFunction.CalculateItemRotation(player, targetPosition, 7);
            }
            else
            {
                for (int i = 0; i < 50; i++)
                {
                    sourceExoLore = playerPos + f.ToRotationVector2() * MathHelper.Lerp(spreadX, spreadY, Main.rand.NextFloat());
                    if (Collision.CanHit(playerPos, 0, 0, sourceExoLore + (sourceExoLore - playerPos).SafeNormalize(Vector2.UnitX) * 8f, 0, 0))
                    {
                        break;
                    }
                    f = Main.rand.NextFloat() * MathHelper.TwoPi;
                }
                Vector2 velocityReal = Main.MouseWorld - sourceExoLore;
                Vector2 velocityVariation = new Vector2(xPos, yPos).SafeNormalize(Vector2.UnitY) * speed;
                velocityReal = velocityReal.SafeNormalize(velocityVariation) * speed;
                velocityReal = Vector2.Lerp(velocityReal, velocityVariation, 0.25f);

                Projectile.NewProjectile(projSource, sourceExoLore, velocityReal, type, damage, knockback, player.whoAmI, 0f, Main.rand.Next(3));

                Vector2 targetPosition = Main.MouseWorld;
                player.itemRotation = CIFunction.CalculateItemRotation(player, targetPosition, 7);
            }

            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>($"{Generic.WeaponRoute}/Magic/VividClarityOldGlow").Value);

        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (usPlayer.LoreExo == true || usPlayer.PanelsLoreExo)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Magic.VividClarityOld.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient<ElementalRayold>().
            AddIngredient<PhantasmalFuryOld>().
            AddIngredient<ArchAmaryllis>().
            AddIngredient<AsteroidStaff>().
            AddIngredient<ShadowboltStaff>().
            AddIngredient<UltraLiquidator>().
            AddIngredient<HeliumFlashLegacy>().
            DisableDecraft().
            AddIngredient<AuricBarold>(10).
            AddTile(ModContent.TileType<DraedonsForgeold>()).
            Register();

            CreateRecipe().
            AddRecipeGroup("CalamityInheritance:AnyElementalRay").
            AddRecipeGroup("CalamityInheritance:AnyPhantasmalFury").
            AddIngredient<ArchAmaryllis>().
            AddIngredient<AsteroidStaff>().
            AddIngredient<ShadowboltStaff>().
            AddIngredient<UltraLiquidator>().
            AddDecraftCondition(CalamityConditions.DownedExoMechs).
            AddRecipeGroup("CalamityInheritance:AnyHeliumFlash").
            AddIngredient<MiracleMatter>().
            AddTile(ModContent.TileType<DraedonsForge>()).
            Register();

            CreateRecipe().
            AddRecipeGroup("CalamityInheritance:AnyElementalRay").
            AddRecipeGroup("CalamityInheritance:AnyPhantasmalFury").
            AddIngredient<ArchAmaryllis>().
            AddIngredient<AsteroidStaff>().
            AddIngredient<ShadowboltStaff>().
            AddIngredient<UltraLiquidator>().
            AddRecipeGroup("CalamityInheritance:AnyHeliumFlash").
            AddIngredient<AncientMiracleMatter>().
            AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
            DisableDecraft().
            AddTile(ModContent.TileType<DraedonsForge>()).
            Register();
        }
    }
}

