﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using Microsoft.Xna.Framework.Graphics;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Tiles.Furniture.CraftingStations;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class HeavenlyGaleold : CIRanged, ILocalizedModType
    {
        
        public const float NormalArrowDamageMult = 1.25f;
        private static int[] ExoArrows;
        private static int[] ExoArrowsExoLore;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ExoArrows =
            [
                ModContent.ProjectileType<ExoArrowTeal>(),
                ModContent.ProjectileType<OrangeExoArrow>(),
                ModContent.ProjectileType<ExoArrowGreen>(),
                ModContent.ProjectileType<ExoArrowBlue>()
            ];
            ExoArrowsExoLore =
            [
                ModContent.ProjectileType<ExoArrowTealExoLore>(),
                ModContent.ProjectileType<ExoArrowOrangeExoLore>(),
                ModContent.ProjectileType<ExoArrowOrangeExoLore>(),
                ModContent.ProjectileType<ExoArrowGreenExoLore>(),
                ModContent.ProjectileType<ExoArrowBlueExoLore>()
            ];
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }


        public override void SetDefaults()
        {
            Item.damage = 208;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 98;
            Item.useTime = 9;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.Calamity().canFirePointBlankShots = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo spawnSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();

            Vector2 source = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 baseOffset = velocity;
            baseOffset.Normalize();
            baseOffset *= 40f;

            float piOver10 = MathHelper.Pi / 10f;
            bool againstWall = !Collision.CanHit(source, 0, 0, source + baseOffset, 0, 0);

            int numArrows = (usPlayer.LoreExo || usPlayer.PanelsLoreExo) ? 7 : 5;

            for (int i = 0; i < numArrows; i++)
            {
                float offsetAmt = i - (numArrows - 1f) / 2f;
                Vector2 offset = baseOffset.RotatedBy(piOver10 * offsetAmt);

                if (againstWall)
                    offset -= baseOffset;

                int thisArrowType = type;
            if (CIConfig.Instance.AmmoConversion == false)
            {
                if (type == ProjectileID.WoodenArrowFriendly)
                {
                    if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                    {
                        thisArrowType = Main.rand.Next(ExoArrowsExoLore);
                    }
                    else
                    {
                        thisArrowType = Main.rand.Next(ExoArrows);
                    }
                }
            }
            if (CIConfig.Instance.AmmoConversion == true)
            {
                    if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                    {
                        thisArrowType = Main.rand.Next(ExoArrowsExoLore);
                    }
                    else
                    {
                        thisArrowType = Main.rand.Next(ExoArrows);
                    }
                }

                int proj = Projectile.NewProjectile(spawnSource, source + offset, velocity, thisArrowType, damage, knockback, player.whoAmI);

                if (type != ProjectileID.WoodenArrowFriendly)
                {
                    Main.projectile[proj].noDropItem = true;
                }
            }

            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        => Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>($"{Generic.WeaponPath}/Ranged/HeavenlyGaleoldGlow").Value);

        public override bool CanConsumeAmmo(Item ammo, Player player)
            {
                if (Main.rand.Next(0, 100) < 66)
                    return false;
                return true;
            }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (usPlayer.LoreExo == true || usPlayer.PanelsLoreExo)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Ranged.HeavenlyGaleold.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
            if (CIConfig.Instance.AmmoConversion == true)
            {
                string AmmoConversionOn = Language.GetTextValue("Mods.CalamityInheritance.ConfigsMessage.AmmoConversionCIWeapon");

                tooltips.Add(new TooltipLine(Mod, "AmmoConversionCIWeapon", AmmoConversionOn));
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlanetaryAnnihilation>().
                AddIngredient<TelluricGlare>().
                AddIngredient<ClockBowLegacy>().
                AddIngredient<Alluvion>().
                AddIngredient<AstrealDefeat>().
                AddIngredient<FlarewingBow>().
                AddIngredient<PhangasmOS>().
                AddIngredient<TheBallista>().
                AddIngredient<AuricBarold>(10).
                DisableDecraft().
                AddTile<DraedonsForgeold>().
                Register();

            CreateRecipe().
                AddIngredient<PlanetaryAnnihilation>().
                AddIngredient<TelluricGlare>().
                AddRecipeGroup(CIRecipeGroup.ClockworkBow).
                AddIngredient<Alluvion>().
                AddIngredient<AstrealDefeat>().
                AddIngredient<FlarewingBow>().
                AddRecipeGroup(CIRecipeGroup.Phangasm).
                AddIngredient<TheBallista>().
                AddDecraftCondition(CalamityConditions.DownedExoMechs).
                AddIngredient<MiracleMatter>().
                AddTile<CalamityMod.Tiles.Furniture.CraftingStations.DraedonsForge>().
                Register();

            CreateRecipe().
                AddIngredient<PlanetaryAnnihilation>().
                AddIngredient<TelluricGlare>().
                AddRecipeGroup(CIRecipeGroup.ClockworkBow).
                AddIngredient<Alluvion>().
                AddIngredient<AstrealDefeat>().
                AddIngredient<FlarewingBow>().
                AddRecipeGroup(CIRecipeGroup.Phangasm).
                AddIngredient<TheBallista>().
                AddIngredient<AncientMiracleMatter>().
                DisableDecraft().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile<CalamityMod.Tiles.Furniture.CraftingStations.DraedonsForge>().
                Register();
        }
    }
}
