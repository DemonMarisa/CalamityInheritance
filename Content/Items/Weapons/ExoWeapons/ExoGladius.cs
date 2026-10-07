using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.ExoWeapons;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoWeapons
{
    public class ExoGladius : CIExoWeapon
    {
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
            Item.damage = 670;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 9.9f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ItemRarityID.Red;
            Item.shoot = ProjectileType<ExoGladiusProj>();
            Item.shootSpeed = 4f;
            Item.rare = RarityType<CatalystViolet>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // 只比普通模式搞高一点
            if (UseAlt)
                damage.Base *= 1.5f;
        }
        public override bool MeleePrefix() => true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (UseAlt)
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 1f, 0f);
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f, 0f);
            }
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<GalileoGladiusLegacy>()
                .AddIngredient<CosmicShivold>()
                .AddIngredient<LucreciaLegacy>()
                .AddIngredient<AuricBarold>(10)
                .AddTile<DraedonsForgeoldTile>()
                .Register();

            CreateRecipe()
                .AddIngredient<GalileoGladiusLegacy>()
                .AddIngredient<CosmicShivold>()
                .AddIngredient<LucreciaLegacy>()
                .AddIngredient<AuricBarold>(10)
                .AddTile<DraedonsForgeoldTile>()
                .Register();
        }
    }
}
