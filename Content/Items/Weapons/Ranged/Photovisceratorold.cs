using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Projectiles.ExoLore;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using Terraria.Audio;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Content.Projectiles.HeldProj.Magic;
using CalamityInheritance.Content.Projectiles.HeldProj.Ranged;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class Photovisceratorold : FlamethrowerSpecial, ILocalizedModType
    {
        public int OwnerIndex;
        public Player Owner => Main.player[OwnerIndex];
        
        public const float AmmoNotConsumeChance = 0.9f;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 750;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 84;
            Item.height = 30;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.UseSound = CISoundID.SoundFlamethrower;
            Item.autoReuse = true;
            Item.useAmmo = AmmoID.Gel;

            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;

            Item.shoot = ModContent.ProjectileType<PhotovisceratorLegacyHeldProj>();
            Item.shootSpeed = 18f;
            Item.noUseGraphic = true;
            Item.channel = true;
        }
        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool isLoreExo = player.CIMod().LoreExo || player.CIMod().PanelsLoreExo;
            SoundStyle leftClick = isLoreExo ? CISoundMenu.ExoFlameLeft : CISoundID.SoundFlamethrower;
            SoundStyle rightClick = isLoreExo ? CISoundMenu.ExoFlameRight : CISoundID.SoundFlamethrower;
            float ai0 = 0f;
            if (player.altFunctionUse == 2)
            {
                ai0 = 1f;
                Item.UseSound = rightClick;
            }
            else
            {
                Item.UseSound = leftClick;
            }
            if(player.ownedProjectileCounts[ModContent.ProjectileType<PhotovisceratorLegacyHeldProj>()] < 1)
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai0);
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (usPlayer.LoreExo == true || usPlayer.PanelsLoreExo)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Ranged.Photovisceratorold.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
        }
        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() > AmmoNotConsumeChance;
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>($"{Generic.WeaponPath}/Ranged/PhotovisceratoroldGlow").Value);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ElementalEruptionLegacy>().
                AddIngredient<CleansingBlazeLegacy>().
                AddIngredient<HalleysInfernoLegacy>().
                AddIngredient<BloodBoilerLegacy>().
                DisableDecraft().
                AddIngredient<AuricBarold>(15).
                AddTile<DraedonsForgeold>().
                Register();

            CreateRecipe().
                AddRecipeGroup(CIRecipeGroup.ElementalEruption).
                AddRecipeGroup(CIRecipeGroup.CleansingBlaze).
                AddRecipeGroup(CIRecipeGroup.HalleysInferno).
                AddRecipeGroup(CIRecipeGroup.BloodBoiler).
                AddDecraftCondition(CalamityConditions.DownedExoMechs).
                AddIngredient<MiracleMatter>().
                AddTile<DraedonsForge>().
                Register();

            CreateRecipe().
                AddRecipeGroup(CIRecipeGroup.ElementalEruption).
                AddRecipeGroup(CIRecipeGroup.CleansingBlaze).
                AddRecipeGroup(CIRecipeGroup.HalleysInferno).
                AddRecipeGroup(CIRecipeGroup.BloodBoiler).
                AddIngredient<AncientMiracleMatter>().
                DisableDecraft().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
