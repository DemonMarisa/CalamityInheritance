using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Ammo.Ranged;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Bow
{
    public class ArterialAssaultLegacy : CIRanged
    {
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 100;
            Item.damage = 110;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 3;
            Item.useAnimation = 15;
            Item.reuseDelay = 10;
            Item.useLimitPerAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.25f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
            Item.UseSound = SoundID.Item102;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<BloodfireArrowProjLegacy>();
            Item.shootSpeed = 30f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float arrowSpeed = Item.shootSpeed;
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
                mouseDistance = arrowSpeed;
            }
            else
            {
                mouseDistance = arrowSpeed / mouseDistance;
            }

            realPlayerPos = new Vector2(player.position.X + player.width * 0.5f + -(float)player.direction + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
            realPlayerPos.X = (realPlayerPos.X + player.Center.X) / 2f;
            realPlayerPos.Y -= 100f;
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
            mouseDistance = arrowSpeed / mouseDistance;
            mouseXDist *= mouseDistance;
            mouseYDist *= mouseDistance;
            float speedX4 = mouseXDist;
            float speedY5 = mouseYDist;
            int shotArrow = Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5, ProjectileType<BloodfireArrowProjLegacy>(), damage, knockback, player.whoAmI);
            Main.projectile[shotArrow].noDropItem = true;
            Main.projectile[shotArrow].tileCollide = false;
            return false;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.BloodstoneCore, 5).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {

            }
        }
    }
}
