﻿using Microsoft.Xna.Framework;
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
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Projectiles.Ranged;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class CelestialObliterator : ModItem
    {
        private int shot;

        private int burst;

        public override void SetDefaults()
        {
            Item.damage = 620;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 120;
            Item.height = 38;
            Item.useTime = 4;
            Item.useAnimation = 32;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.rare = ModContent.RarityType<Violet>();
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Utils.NextFloat(Main.rand) >= 0.9f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + Main.rand.Next(-15, 16) * 0.05f;
            float SpeedY = velocity.Y + Main.rand.Next(-15, 16) * 0.05f;
            int num = 10;
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
            if (shot >= 8)
            {
                shot = 0;
                burst++;
                for (int i = 0; i < num; i++)
                {
                    Vector2 vector = Utils.RotatedByRandom(new Vector2(SpeedX, SpeedY), (double)MathHelper.ToRadians(2f));
                    vector *= 0.4f + Utils.NextFloat(Main.rand, 10f) / 25f;
                    Projectile.NewProjectile(source, position.X, position.Y, vector.X, vector.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
                }
            }
            else
            {
                shot++;
                SoundEngine.PlaySound(SoundID.Item40);
            }
            if (burst >= 5)
            {
                burst = 0;
                SoundEngine.PlaySound(SoundID.Item38);
                Vector2 vector2 = Utils.RotatedBy(Vector2.Normalize(new Vector2(SpeedX, SpeedY)), 60, default) * 9f;
                Vector2 vector3 = Utils.RotatedBy(Vector2.Normalize(new Vector2(SpeedX, SpeedY)), -60, default) * 9f;
                Projectile.NewProjectile(source, position.X, position.Y, vector2.X, vector2.Y, ModContent.ProjectileType<ExoGunBlast>(), damage, knockback, player.whoAmI, 0f, 0f);
                Projectile.NewProjectile(source, position.X, position.Y, vector3.X, vector3.Y, ModContent.ProjectileType<ExoGunBlast>(), damage, knockback, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-35f, -6f);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Onyxia>().
                AddIngredient<UniversalGenesis>().
                AddIngredient<ElementalBlaster>().
                AddIngredient<Infinity>().
                AddIngredient<MiracleMatter>().
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
