using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class DragonStaff: CIMagic, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 290;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.width = 72;
            Item.height = 70;
            Item.useTime = 15;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DragonStaffProj>();
            Item.shootSpeed = 30f;
            Item.rare = CIConfig.Instance.SpecialRarityColor ? ModContent.RarityType<YharonFire>() : ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float pSpeed = Item.shootSpeed;
            player.itemTime = Item.useTime;
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float mPosX = Main.mouseX + Main.screenPosition.X - vector2.X;
            float mPosY = Main.mouseY + Main.screenPosition.Y - vector2.Y;
            if (player.gravDir == -1f)
            {
                mPosY = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - vector2.Y;
            }
            float mDist = (float)Math.Sqrt((double)(mPosX * mPosX + mPosY * mPosY));
            if (float.IsNaN(mPosX) && float.IsNaN(mPosY) || mPosX == 0f && mPosY == 0f)
            {
                mPosX = player.direction;
                mPosY = 0f;
                mDist = pSpeed;
            }
            else
            {
                mDist = pSpeed / mDist;
            }

            int pCounts = 30;
            if (Main.rand.NextBool(3))
            {
                pCounts++;
            }
            if (Main.rand.NextBool(3))
            {
                pCounts++;
            }
            for (int num108 = 0; num108 < pCounts; num108++)
            {
                vector2 = new Vector2(player.position.X + player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                vector2.X = (vector2.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
                vector2.Y -= 100 * num108;
                mPosX = Main.mouseX + Main.screenPosition.X - vector2.X;
                mPosY = Main.mouseY + Main.screenPosition.Y - vector2.Y;
                if (mPosY < 0f)
                {
                    mPosY *= -1f;
                }
                if (mPosY < 20f)
                {
                    mPosY = 20f;
                }
                mDist = (float)Math.Sqrt((double)(mPosX * mPosX + mPosY * mPosY));
                mDist = pSpeed / mDist;
                mPosX *= mDist;
                mPosY *= mDist;
                float speedX4 = mPosX + Main.rand.Next(-30, 31) * 0.02f;
                float speedY5 = mPosY + Main.rand.Next(-30, 31) * 0.02f;
                Projectile.NewProjectile(source,vector2.X, vector2.Y, speedX4, speedY5, type, damage, knockback, player.whoAmI, 0f, (float)Main.rand.Next(15));
            }
            return false;
        }
    }
}
