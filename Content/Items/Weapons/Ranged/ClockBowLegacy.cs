using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Ranged;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class ClockBowLegacy: CIRanged, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<ClockworkBow>(false);
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 100;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 100;
            Item.useTime = Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.25f;
            Item.value = CIShopValue.RarityPricePurple;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = CISoundID.SoundBow;
            Item.autoReuse = true; 
            Item.shootSpeed = 30f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = 40;
            Item.Calamity().canFirePointBlankShots = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Cog, 50).
                AddIngredient(ItemID.LunarBar, 15).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int i = Main.myPlayer;
            float num72 = Item.shootSpeed;
            int num73 = damage;
            float num74 = Item.knockBack;
            num74 = player.GetWeaponKnockback(Item, num74);
            player.itemTime = Item.useTime;
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 vector3 = Main.MouseWorld + vector2;
            float num78 = (float)Main.mouseX + Main.screenPosition.X + vector2.X;
            float num79 = (float)Main.mouseY + Main.screenPosition.Y + vector2.Y;
            if (player.gravDir == -1f)
            {
                num79 = Main.screenPosition.Y + (float)Main.screenHeight + (float)Main.mouseY + vector2.Y;
            }
            float num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
            if ((float.IsNaN(num78) && float.IsNaN(num79)) || (num78 == 0f && num79 == 0f))
            {
                num78 = (float)player.direction;
                num79 = 0f;
                num80 = num72;
            }
            else
            {
                num80 = num72 / num80;
            }

            int num130 = 15;
            if (Main.rand.NextBool(3))
            {
                num130++;
            }
            if (Main.rand.NextBool(4))
            {
                num130++;
            }
            if (Main.rand.NextBool(5))
            {
                num130++;
            }

            Vector2 realPlayerPos2 = vector2 - player.Center;
            float rotoffset = player.direction == 1 ? MathHelper.Pi : 0f;
            player.itemRotation = realPlayerPos2.ToRotation() + rotoffset;

            for (int num131 = 0; num131 < num130; num131++)
            {
                vector2 = new Vector2(player.position.X + (float)player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + ((float)Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y);
                vector2.X = (vector2.X + player.Center.X) / 2f + (float)Main.rand.Next(-200, 201);
                vector2.Y -= (float)(100 * num131);
                num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                if (num79 < 0f)
                {
                    num79 *= -1f;
                }
                if (num79 < 20f)
                {
                    num79 = 20f;
                }
                num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
                num80 = num72 / num80;
                num78 *= num80;
                num79 *= num80;
                float speedX4 = num78 + (float)Main.rand.Next(-600, 601) * 0.02f;
                float speedY5 = num79 + (float)Main.rand.Next(-600, 601) * 0.02f;
                int projectile = Projectile.NewProjectile(source, vector2.X, vector2.Y, speedX4, speedY5, type, num73, num74, i, 0f, 0f);
                Main.projectile[projectile].tileCollide = false;
                Main.projectile[projectile].timeLeft = 240;
                Main.projectile[projectile].noDropItem = true;
            }
            return false;
        }
    }
}