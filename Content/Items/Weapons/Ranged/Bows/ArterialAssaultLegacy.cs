using CalamityInheritance.Content.Projectiles.Ammo;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Bows
{
    public class ArterialAssaultLegacy : CIRanged, ILocalizedModType
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

            realPlayerPos = new Vector2(player.position.X + (float)player.width * 0.5f + (-(float)player.direction) + ((float)Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
            realPlayerPos.X = (realPlayerPos.X + player.Center.X) / 2f;
            realPlayerPos.Y -= 100f;
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
            float speedX4 = mouseXDist;
            float speedY5 = mouseYDist;
            int shotArrow = Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5, ProjectileType<BloodfireArrowProjLegacy>(), damage, knockback, player.whoAmI);
            Main.projectile[shotArrow].noDropItem = true;
            Main.projectile[shotArrow].tileCollide = false;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodstoneCore>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
