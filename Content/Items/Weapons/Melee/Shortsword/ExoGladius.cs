﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityInheritance.Rarity;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.Tiles.Furniture.CraftingStations;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class ExoGladius : CIMelee, ILocalizedModType 
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Melee.Shortsword";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 12;
            Item.width = 56;
            Item.height = 56;
            Item.damage = 2000;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 9.9f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ItemRarityID.Red;
            Item.shoot = ModContent.ProjectileType<ExoGladiusProj>();
            Item.shootSpeed = 4f;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>($"{Generic.WeaponRoute}/Melee/Shortsword/ExoGladiusGlow").Value);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (usPlayer.LoreExo == true || usPlayer.PanelsLoreExo)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Melee.Shortsword.ExoGladius.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GalileoGladius>()
                .AddRecipeGroup("CalamityInheritance:AnyCosmicShiv")
                .AddIngredient<Lucrecia>()
                .AddIngredient<AuricBarold>(15)
                .DisableDecraft()
                .AddTile<DraedonsForgeold>()
                .Register();
            CreateRecipe()
                .AddIngredient<GalileoGladius>()
                .AddRecipeGroup("CalamityInheritance:AnyCosmicShiv")
                .AddIngredient<Lucrecia>()
                .AddIngredient<MiracleMatter>()
                .AddDecraftCondition(CalamityConditions.DownedExoMechs)
                .AddTile<DraedonsForge>()
                .Register();

            CreateRecipe().
                AddIngredient<GalileoGladius>().
                AddRecipeGroup("CalamityInheritance:AnyCosmicShiv").
                AddIngredient<Lucrecia>().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                DisableDecraft().
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
