using CalamityMod.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Rarity.ShopValue;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Bow
{
    public class CosmicRainbowLegacy : CIRanged
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 64;
            Item.damage = 105;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 30;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item67;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.RainbowFront;
            Item.shootSpeed = 18f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            float tenthPi = 0.314159274f;
            int directRainbowAmt = 3;
            Vector2 directRainbowVel = velocity;
            directRainbowVel.Normalize();
            directRainbowVel *= 60f;
            bool rainbowHitsTiles = Collision.CanHit(vector, 0, 0, vector + directRainbowVel, 0, 0);
            for (int i = 0; i < directRainbowAmt; i++)
            {
                float rainbowOffset = i - (directRainbowAmt - 1f) / 2f;
                Vector2 offsetSpawnPos = directRainbowVel.RotatedBy((double)(tenthPi * rainbowOffset), default);
                if (!rainbowHitsTiles)
                {
                    offsetSpawnPos -= directRainbowVel;
                }
                int rainbow = Projectile.NewProjectile(source, vector.X + offsetSpawnPos.X, vector.Y + offsetSpawnPos.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
                Main.projectile[rainbow].usesLocalNPCImmunity = true;
                Main.projectile[rainbow].localNPCHitCooldown = 10;
            }
            float skyRainbowSpeed = Item.shootSpeed;
            Vector2 realPlayerPos = player.RotatedRelativePoint(player.MountedCenter, true);
            float mouseXDist = Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
            float mouseYDist = Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
            if (player.gravDir == -1f)
            {
                mouseYDist = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - realPlayerPos.Y;
            }
            float mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
            if (float.IsNaN(mouseXDist) && float.IsNaN(mouseYDist) || mouseXDist == 0f && mouseYDist == 0f)
            {
                mouseXDist = player.direction;
                mouseYDist = 0f;
                mouseDistance = skyRainbowSpeed;
            }
            else
            {
                mouseDistance = skyRainbowSpeed / mouseDistance;
            }

            for (int j = 0; j < 2; j++)
            {
                realPlayerPos = new Vector2(player.position.X + player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                realPlayerPos.X = (realPlayerPos.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
                realPlayerPos.Y -= 100 * j;
                mouseXDist = Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
                mouseYDist = Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
                if (mouseYDist < 0f)
                {
                    mouseYDist *= -1f;
                }
                if (mouseYDist < 20f)
                {
                    mouseYDist = 20f;
                }
                mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
                mouseDistance = skyRainbowSpeed / mouseDistance;
                mouseXDist *= mouseDistance;
                mouseYDist *= mouseDistance;
                float speedX4 = mouseXDist + Main.rand.Next(-15, 16) * 0.01f;
                float speedY5 = mouseYDist + Main.rand.Next(-15, 16) * 0.01f;
                int rainbow2 = Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5, type, damage, knockback, player.whoAmI);
                Main.projectile[rainbow2].usesLocalNPCImmunity = true;
                Main.projectile[rainbow2].localNPCHitCooldown = 10;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.RainbowGun).
                AddIngredient(ItemID.PearlwoodBow).
                AddIngredient<GalacticaSingularity>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
