﻿using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Summon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.DataStructures;
using CalamityInheritance.Content.Projectiles.Summon;

namespace CalamityInheritance.Content.Items.Weapons.Summon
{
    public class MidnightSunBeaconold : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.damage = 240;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item90;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MidnightSunBeaconProjold>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;

            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (Main.projectile.IndexInRange(p))
                Main.projectile[p].originalDamage = Item.damage;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.XenoStaff).
                AddIngredient(ItemID.MoonlordTurretStaff).
                AddIngredient<AuricBar>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}