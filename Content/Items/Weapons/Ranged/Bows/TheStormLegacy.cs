using CalamityInheritance.Content.Projectiles.Ranged.Bows;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Bows
{
    public class TheStormLegacy : CIRanged, ILocalizedModType
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.AnimatesAsSoul[Type] = true;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 9));
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 90;
            Item.damage = 35;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 7;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
            Item.UseSound = SoundID.Item122;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<BoltLegacy>();
            Item.shootSpeed = 28f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int i = Main.myPlayer;
            float arrowSpeed = Main.rand.Next(25, 30);
            player.itemTime = Item.useTime;
            Vector2 realPlayerPos = player.RotatedRelativePoint(player.MountedCenter, true);
            float mouseXDist = (float)Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
            float mouseYDist = (float)Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
            if (player.gravDir == -1f)
            {
                mouseYDist = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - realPlayerPos.Y;
            }
            float mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
            if ((float.IsNaN(mouseXDist) && float.IsNaN(mouseYDist)) || (mouseXDist == 0f && mouseYDist == 0f))
            {
                mouseXDist = (float)player.direction;
                mouseYDist = 0f;
                mouseDistance = arrowSpeed;
            }
            else
            {
                mouseDistance = arrowSpeed / mouseDistance;
            }

            for (int j = 0; j < 3; j++)
            {
                realPlayerPos = new Vector2(player.position.X + (float)player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + ((float)Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                realPlayerPos.X = (realPlayerPos.X + player.Center.X) / 2f + (float)Main.rand.Next(-200, 201);
                realPlayerPos.Y -= (float)(100 * j);
                mouseXDist = (float)Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
                mouseYDist = (float)Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
                if (mouseYDist < 0f)
                {
                    mouseYDist *= -1f;
                }
                if (mouseYDist < 20f)
                {
                    mouseYDist = 20f;
                }
                mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
                mouseDistance = arrowSpeed / mouseDistance;
                mouseXDist *= mouseDistance;
                mouseYDist *= mouseDistance;
                float speedX4 = mouseXDist + (float)Main.rand.Next(-120, 121) * 0.01f;
                float speedY5 = mouseYDist + (float)Main.rand.Next(-120, 121) * 0.01f;
                if (CIConfig.Instance.AmmoConversion)
                {
                    Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5 * 0.9f, ProjectileType<BoltLegacy>(), damage, knockback, i);
                    Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5 * 0.8f, ProjectileType<BoltLegacy>(), damage, knockback, i);
                }
                else
                {
                    if (CalamityUtils.CheckWoodenAmmo(type, player))
                    {
                        Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5 * 0.9f, ProjectileType<BoltLegacy>(), damage, knockback, i);
                        Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5 * 0.8f, ProjectileType<BoltLegacy>(), damage, knockback, i);
                    }
                    else
                    {
                        int arrow1 = Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5 * 0.9f, type, damage, knockback, i);
                        Main.projectile[arrow1].noDropItem = true;
                        int arrow2 = Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5 * 0.8f, type, damage, knockback, i);
                        Main.projectile[arrow2].noDropItem = true;
                    }
                }
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ArmoredShell>(2).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
