﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Magic.Ray;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityMod.Projectiles.Magic;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Ray
{
    public class TerraRay : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Terra Ray");
            // Tooltip.SetDefault("Casts an energy ray that splits if enemies are near it");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 54;
            Item.height = 54;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TerraBeamMain>();
            Item.shootSpeed = 6f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 shootVelocity = velocity;
            Vector2 shootPosition = position + shootVelocity * 12f;
            Projectile.NewProjectile(source, shootPosition, shootVelocity, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(((ModItem)this).Item, 0, (string)null), position, velocity, ModContent.ProjectileType<TerraBeam>(), damage, knockback, ((Entity)player).whoAmI, 0f, 0f, 0f);
            return false;
        }

        /*
        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }
        */
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<NightsRay>());
            recipe.AddIngredient(ModContent.ItemType<ValkyrieRay>());
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
            recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CarnageRay>());
            recipe.AddIngredient(ModContent.ItemType<ValkyrieRay>());
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
