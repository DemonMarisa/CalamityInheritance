﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class GalaxySmasherMelee : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public static int BaseDamage = 300;
        public static float Speed = 18f;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Galaxy Smasher");
            // Tooltip.SetDefault("Explodes and summons death lasers on enemy hits");
        }

        public override void SetDefaults()
        {
            Item.width = 86;
            Item.height = 72;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = BaseDamage;
            Item.knockBack = 9f;
            Item.useAnimation = 13;
            Item.useTime = 13;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;

            Item.shoot = ModContent.ProjectileType<GalaxySmasherHammerOld>();
            Item.shootSpeed = Speed;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<StellarContempt>().
                AddIngredient<CosmiliteBar>(10).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
