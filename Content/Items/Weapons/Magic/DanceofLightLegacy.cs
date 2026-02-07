using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.CalStatInflationBACK;
using CalamityInheritance.System.Configs;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Tiles.Furniture.CraftingStations;
using LAP.Core.Enums;
using LAP.Core.SystemsLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class DanceofLightLegacy: CIMagic, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 220;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 9;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = RarityType<DonatorPink>();
            Item.UseSound = SoundID.Item105;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<WhiteFlameLegacy>();
            Item.shootSpeed = 30f;
            Item.SetCalStatInflation(AllWeaponTier.DemonShadow);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LunarFlareBook).
                AddIngredient<WrathoftheAncients>().
                AddIngredient<ShadowspecBar>(5).
                AddTile(TileType<DraedonsForge>()).
                Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            BoostWeaponDamage.SetCustomMult(Item, 10f);
            float projSpeed = Item.shootSpeed;
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float getMouseX = Main.mouseX + Main.screenPosition.X - vector2.X;
            float getMouseY = Main.mouseY + Main.screenPosition.Y - vector2.Y;
            if (player.gravDir == -1f)
            {
                getMouseY = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - vector2.Y;
            }
            float getMouseDist = (float)Math.Sqrt((double)(getMouseX * getMouseX + getMouseY * getMouseY));
            if (float.IsNaN(getMouseX) && float.IsNaN(getMouseY) || getMouseX == 0f && getMouseY == 0f)
            {
                getMouseX = player.direction;
                getMouseY = 0f;
                getMouseDist = projSpeed;
            }
            else
            {
                getMouseDist = projSpeed / getMouseDist;
            }

            int projCounts = Main.rand.Next(4,7);
            for (int i = 0; i < projCounts; i++)
            {
                vector2 = new Vector2(player.position.X + player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                vector2.X = (vector2.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
                vector2.Y -= 100 * i;
                getMouseX = Main.mouseX + Main.screenPosition.X - vector2.X;
                getMouseY = Main.mouseY + Main.screenPosition.Y - vector2.Y;
                if (getMouseY < 0f)
                {
                    getMouseY *= -1f;
                }
                if (getMouseY < 20f)
                {
                    getMouseY = 20f;
                }
                getMouseDist = (float)Math.Sqrt((double)(getMouseX * getMouseX + getMouseY * getMouseY));
                getMouseDist = projSpeed / getMouseDist;
                getMouseX *= getMouseDist;
                getMouseY *= getMouseDist;
                float speedX4 = getMouseX + Main.rand.Next(-30, 31) * 0.02f;
                float speedY5 = getMouseY + Main.rand.Next(-30, 31) * 0.02f;
                Projectile.NewProjectile(player.GetSource_FromThis(),vector2.X, vector2.Y, speedX4, speedY5, type, damage, knockback, player.whoAmI, 0f, Main.rand.Next(15));
            }
            return false;
        }
    }
}
