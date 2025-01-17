﻿using CalamityMod.Items.Materials;
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

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class Photovisceratorold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        public const float AmmoNotConsumeChance = 0.9f;
        private const float AltFireShootSpeed = 17f;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 230;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 84;
            Item.height = 30;
            Item.useTime = 2;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item34;
            Item.autoReuse = true;
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Gel;

            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shoot = ModContent.ProjectileType<ExoFlareClusterold>();
                Item.useTime = Item.useAnimation = 27;
            }
            else
            {
                Item.useTime = 2;
                Item.useAnimation = 10;
                Item.shoot = ModContent.ProjectileType<ExoFireold>();
            }
            return base.CanUseItem(player);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Alt fire: Shoot an Exo Flare Cluster.
            if (player.altFunctionUse == 2)
            {
                int projID = ModContent.ProjectileType<ExoFlareClusterold>();

                position += velocity.ToRotation().ToRotationVector2() * 80f;
                Projectile.NewProjectile(source, position, velocity.SafeNormalize(Vector2.Zero) * AltFireShootSpeed, projID, damage, knockback, player.whoAmI);
                return false;
            }

            // Left click: Exo Fire, with a chance of Exo Light Bombs.
            for (int i = 0; i < 2; i++)
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            }
            if (player.itemAnimation == 2)
            {
                for (int i = 0; i < 2; i++)
                {

                    position += velocity.ToRotation().ToRotationVector2() * 64f;
                    int yDirection = (i == 0).ToDirectionInt();
                    velocity = velocity.RotatedBy(0.2f * yDirection);
                    Projectile lightBomb = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<ExoLightold>(), damage, knockback, player.whoAmI);

                    lightBomb.localAI[1] = yDirection;
                    lightBomb.netUpdate = true;
                }
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() > AmmoNotConsumeChance;
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/PhotovisceratoroldGlow").Value);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ElementalEruption>().
                AddIngredient<CleansingBlaze>().
                AddIngredient<HalleysInferno>().
                AddIngredient<MiracleMatter>().
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
