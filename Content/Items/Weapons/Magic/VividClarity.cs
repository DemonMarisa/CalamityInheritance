﻿using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class VividClarity : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";

        public static readonly SoundStyle UseSound = new("CalamityMod/Sounds/Item/VividClarityShoot") { Volume = 0.30f };
        public static readonly SoundStyle BeamSound = new("CalamityMod/Sounds/Item/VividClarityBeamAppear");
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 90;
            Item.height = 112;
            Item.damage = 605;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 40;
            Item.useTime = 6;
            Item.useAnimation = 54;
            Item.reuseDelay = 25;
            Item.useLimitPerAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.UseSound = UseSound;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CIVividBeam>();
            Item.shootSpeed = 12f;
            Item.rare = ModContent.RarityType<Violet>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo projSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 playerPos = player.RotatedRelativePoint(player.MountedCenter, true);
            float speed = Item.shootSpeed;
            float xPos = (float)Main.mouseX + Main.screenPosition.X - playerPos.X;
            float yPos = (float)Main.mouseY + Main.screenPosition.Y - playerPos.Y;
            float f = Main.rand.NextFloat() * MathHelper.TwoPi;
            float sourceVariationLow = 120f;
            float sourceVariationHigh = 180f;
            Vector2 source = playerPos + f.ToRotationVector2() * MathHelper.Lerp(sourceVariationLow, sourceVariationHigh, Main.rand.NextFloat());
            for (int i = 0; i < 50; i++)
            {
                source = playerPos + f.ToRotationVector2() * MathHelper.Lerp(sourceVariationLow, sourceVariationHigh, Main.rand.NextFloat());
                if (Collision.CanHit(playerPos, 0, 0, source + (source - playerPos).SafeNormalize(Vector2.UnitX) * 8f, 0, 0))
                {
                    break;
                }
                f = Main.rand.NextFloat() * MathHelper.TwoPi;
            }
            Vector2 velocityReal = Main.MouseWorld - source;
            Vector2 velocityVariation = new Vector2(xPos, yPos).SafeNormalize(Vector2.UnitY) * speed;
            velocityReal = velocityReal.SafeNormalize(velocityVariation) * speed;
            velocityReal = Vector2.Lerp(velocityReal, velocityVariation, 0.25f);

            Projectile.NewProjectile(projSource, source, velocityReal, type, damage, knockback, player.whoAmI, 0f, Main.rand.Next(3));

            Vector2 targetPosition = Main.MouseWorld;
            player.itemRotation = CalamityInheritanceUtils.CalculateItemRotation(player, targetPosition, 7);

            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Magic/VividClarityGlow").Value);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ElementalRay>());
            recipe.AddIngredient(ModContent.ItemType<PhantasmalFury>());
            recipe.AddIngredient(ModContent.ItemType<ShadowboltStaff>());
            recipe.AddIngredient(ModContent.ItemType<UltraLiquidator>());
            recipe.AddIngredient(ModContent.ItemType<MiracleMatter>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}

