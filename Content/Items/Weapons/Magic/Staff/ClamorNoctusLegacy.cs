using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.Staff;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Staff
{
    public class ClamorNoctusLegacy : CIMagic
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 74;
            Item.damage = 270;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.2f;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = RarityType<BlueGreen>();
            Item.UseSound = SoundID.Item105;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<ClamorNoctusStar>();
            Item.shootSpeed = 25f;
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float projSpeed = Item.shootSpeed;
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
                mouseDistance = projSpeed;
            }
            else
            {
                mouseDistance = projSpeed / mouseDistance;
            }

            int projAmt = 3;
            for (int i = 0; i < projAmt; i++)
            {
                float damageMult = 1f;
                float kbMult = 1f;
                if (Main.rand.NextBool(10))
                {
                    type = ProjectileType<ClamorNoctusWyvern>();
                }
                if (type == ProjectileType<ClamorNoctusWyvern>())
                {
                    damageMult = 25f;
                    kbMult = 1.5f;
                }
                realPlayerPos = new Vector2(player.Center.X + player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + (Main.mouseX + Main.screenPosition.X - player.Center.X), player.MountedCenter.Y - 600f);
                realPlayerPos.X = (realPlayerPos.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
                realPlayerPos.Y -= 100 * i;
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
                mouseDistance = projSpeed / mouseDistance;
                mouseXDist *= mouseDistance;
                mouseYDist *= mouseDistance;
                float speedX4 = mouseXDist + Main.rand.Next(-30, 31) * 0.02f;
                float speedY5 = mouseYDist + Main.rand.Next(-30, 31) * 0.02f;
                Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5, type, (int)(damage * damageMult), (int)(knockback * kbMult), player.whoAmI, 0f, Main.rand.Next(15));
            }
            return false;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.Lumenyl, 5).
                    AddIngredient(CalamityMaterials.RuinousSoul, 5).
                    AddIngredient(CalamityMaterials.ExodiumCluster, 10).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {

            }
        }
    }
}
