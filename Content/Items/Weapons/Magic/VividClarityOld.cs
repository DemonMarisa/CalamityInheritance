using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Projectiles.Magic;
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
    public class VividClarityOld : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Magic";

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
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.UseSound = UseSound;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CIVividBeam>();
            Item.shootSpeed = 12f;
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }
        public override bool CanUseItem(Player player)
        {
            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();
            if (usPlayer.exoMechLore)
            {
                Item.useTime = 6;
                Item.useAnimation = 54;
                Item.reuseDelay = 25;
                Item.useLimitPerAnimation = 9;
            }
            else
            {
                Item.useTime = 4;
                Item.useAnimation = 20;
                Item.reuseDelay = Item.useAnimation;
                Item.useLimitPerAnimation = 5;
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo projSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();

            Vector2 playerPos = player.RotatedRelativePoint(player.MountedCenter, true);
            float speed = Item.shootSpeed;
            float xPos = Main.mouseX + Main.screenPosition.X - playerPos.X;
            float yPos = Main.mouseY + Main.screenPosition.Y - playerPos.Y;
            float f = Main.rand.NextFloat() * MathHelper.TwoPi;
            float spreadX = 20f;
            float spreadY = 60f;
            float sourceVariationLow = 120f;
            float sourceVariationHigh = 180f;
            Vector2 source = playerPos + f.ToRotationVector2() * MathHelper.Lerp(sourceVariationLow, sourceVariationHigh, Main.rand.NextFloat());
            Vector2 sourceExoLore = playerPos + f.ToRotationVector2() * MathHelper.Lerp(spreadX, spreadY, Main.rand.NextFloat());
            if (usPlayer.exoMechLore)
            {
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

                Projectile.NewProjectile(projSource, source, velocityReal, ModContent.ProjectileType<CIVividBeamExoLore>(), damage, knockback, player.whoAmI, 0f, Main.rand.Next(3));

                Vector2 targetPosition = Main.MouseWorld;
                player.itemRotation = CalamityInheritanceUtils.CalculateItemRotation(player, targetPosition, 7);
            }
            else
            {
                for (int i = 0; i < 50; i++)
                {
                    sourceExoLore = playerPos + f.ToRotationVector2() * MathHelper.Lerp(spreadX, spreadY, Main.rand.NextFloat());
                    if (Collision.CanHit(playerPos, 0, 0, sourceExoLore + (sourceExoLore - playerPos).SafeNormalize(Vector2.UnitX) * 8f, 0, 0))
                    {
                        break;
                    }
                    f = Main.rand.NextFloat() * MathHelper.TwoPi;
                }
                Vector2 velocityReal = Main.MouseWorld - sourceExoLore;
                Vector2 velocityVariation = new Vector2(xPos, yPos).SafeNormalize(Vector2.UnitY) * speed;
                velocityReal = velocityReal.SafeNormalize(velocityVariation) * speed;
                velocityReal = Vector2.Lerp(velocityReal, velocityVariation, 0.25f);

                Projectile.NewProjectile(projSource, sourceExoLore, velocityReal, type, damage, knockback, player.whoAmI, 0f, Main.rand.Next(3));

                Vector2 targetPosition = Main.MouseWorld;
                player.itemRotation = CalamityInheritanceUtils.CalculateItemRotation(player, targetPosition, 7);
            }

            return false;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Magic/VividClarityOldGlow").Value);

        }

        public override void AddRecipes()
        {
            CreateRecipe().
            AddRecipeGroup("CalamityInheritance:AnyElementalRay").
            AddRecipeGroup("CalamityInheritance:AnyPhantasmalFury").
            AddIngredient(ModContent.ItemType<ArchAmaryllis>()).
            AddIngredient(ModContent.ItemType<AsteroidStaff>()).
            AddIngredient(ModContent.ItemType<ShadowboltStaff>()).
            AddIngredient(ModContent.ItemType<UltraLiquidator>()).
            AddRecipeGroup("CalamityInheritance:AnyHeliumFlash").
            AddIngredient(ModContent.ItemType<MiracleMatter>()).
            AddTile(ModContent.TileType<DraedonsForge>()).
            Register();

            CreateRecipe().
            AddRecipeGroup("CalamityInheritance:AnyElementalRay").
            AddRecipeGroup("CalamityInheritance:AnyPhantasmalFury").
            AddIngredient(ModContent.ItemType<ArchAmaryllis>()).
            AddIngredient(ModContent.ItemType<AsteroidStaff>()).
            AddIngredient(ModContent.ItemType<ShadowboltStaff>()).
            AddIngredient(ModContent.ItemType<UltraLiquidator>()).
            AddRecipeGroup("CalamityInheritance:AnyHeliumFlash").
            AddIngredient<AncientMiracleMatter>().
            AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
            AddTile(ModContent.TileType<DraedonsForge>()).
            Register();
        }
    }
}

