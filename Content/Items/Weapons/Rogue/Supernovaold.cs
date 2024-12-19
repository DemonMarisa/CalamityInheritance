﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Rarities;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Projectiles.Rogue;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class Supernovaold : RogueWeapon
    {
        public static readonly SoundStyle ExplosionSound = new("CalamityMod/Sounds/Item/SupernovaBoom") { Volume = 0.8f };
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 36;
            Item.damage = 675;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 24;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.shoot = ModContent.ProjectileType<SupernovaBombold>();
            Item.shootSpeed = 16f;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.rare = ModContent.RarityType<Violet>();
        }

        public override float StealthDamageMultiplier => 1.08f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                if (stealth.WithinBounds(Main.maxProjectiles))
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SealedSingularity>().
                AddIngredient<StarofDestruction>().
                AddIngredient<TotalityBreakers>().
                AddIngredient<BallisticPoisonBomb>().
                AddIngredient<MiracleMatter>().
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
