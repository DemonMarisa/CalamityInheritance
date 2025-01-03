﻿using CalamityMod.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityMod.Items.LoreItems;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class LeadWizard : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 58;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 66;
            Item.height = 34;
            Item.useTime = 3;
            Item.reuseDelay = 12;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item31;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.BulletHighVelocity;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 30;

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float rotation = MathHelper.ToRadians(6);
            for (int i = 0; i < 2; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i == 1 ? 0 : 2));
                Projectile.NewProjectile(source, position, perturbedSpeed, ProjectileID.BulletHighVelocity, damage, knockback, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 33)
                return false;
            return true;
        }
        public override void AddRecipes()
        {
            if (CalamityInheritanceConfig.Instance.LegendaryitemsRecipes == true)
            {
                Recipe recipe1 = CreateRecipe();
                recipe1.AddIngredient<LoreGolem>();
                recipe1.AddTile(TileID.AncientMythrilBrick);
                recipe1.Register();

                Recipe recipe2 = CreateRecipe();
                recipe2.AddIngredient<KnowledgeGolem>();
                recipe2.AddTile(TileID.AncientMythrilBrick);
                recipe2.Register();
            }
        }
    }
}
