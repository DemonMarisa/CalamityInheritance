using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Projectiles.Melee;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class ExoFlail : ModItem
    {
        public static int BaseDamage = 3000;

        public static float Speed = 34f;

        public static float MouseHomingAcceleration = 0.75f;

        public static float MaxRange = 780f;

        public static float ReturnSpeed = 40f;

        private int hitCount = 0;
        public override void SetDefaults()
        {
            Item.width = 76;
            Item.height = 82;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = 3000;
            Item.knockBack = 9f;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item101;
            Item.channel = true;
            Item.rare = ModContent.RarityType<Violet>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.shoot = ModContent.ProjectileType<ExoFlailProj>();
            Item.shootSpeed = 24f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            hitCount++;
            float ai3 = (Main.rand.NextFloat() - 0.75f) * 0.7853982f;
            if (hitCount >= 5)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ExoFlailProj2>(), damage, knockback, player.whoAmI, 0f);
                hitCount = 0;
            }
            else
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, ai3);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DragonPow>());
            recipe.AddIngredient(ModContent.ItemType<CrescentMoon>());
            recipe.AddIngredient(ModContent.ItemType<Mourningstar>());
            recipe.AddIngredient(ModContent.ItemType<ClamCrusher>());
            recipe.AddIngredient(ModContent.ItemType<BallOFugu>());
            recipe.AddIngredient(ModContent.ItemType<MiracleMatter>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
