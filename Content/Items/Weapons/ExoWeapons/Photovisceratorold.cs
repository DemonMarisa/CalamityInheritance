using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Ranged.Flamethrower;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.ExoWeapons;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoWeapons
{
    public class Photovisceratorold : CIExoWeapon
    {

        public const float AmmoNotConsumeChance = 0.9f;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 230;
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

            Item.rare = RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;

            Item.shoot = ProjectileType<PhotovisceratorLegacyHeldProj>();
            Item.shootSpeed = 18f;
            Item.noUseGraphic = true;
            Item.channel = true;
        }
        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            Item.LAP().UseCustomStatInflationMult = false;
            float mult = 1300 / (float)Item.damage;
            Item.LAP().StatInflationMult = mult;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundStyle leftClick = UseAlt ? CISounds.ExoFlameLeft : CISoundID.SoundFlamethrower;
            SoundStyle rightClick = UseAlt ? CISounds.ExoFlameRight : CISoundID.SoundFlamethrower;
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
            if (player.ownedProjectileCounts[ProjectileType<PhotovisceratorLegacyHeldProj>()] < 1)
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai0);
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // 低一点伤害
            if (UseAlt)
                damage.Base *= 0.7f;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() > AmmoNotConsumeChance;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ElementalEruptionLegacy>().
                AddIngredient<CleansingBlazeLegacy>().
                AddIngredient<HalleysInfernoLegacy>().
                AddIngredient<BloodBoilerLegacy>().
                AddIngredient<AuricBarold>(10).
                AddTile<DraedonsForgeoldTile>().
                Register();

            CreateRecipe().
                AddIngredient<ElementalEruptionLegacy>().
                AddIngredient<CleansingBlazeLegacy>().
                AddIngredient<HalleysInfernoLegacy>().
                AddIngredient<BloodBoilerLegacy>().
                AddIngredient<AncientMiracleMatter>().
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
