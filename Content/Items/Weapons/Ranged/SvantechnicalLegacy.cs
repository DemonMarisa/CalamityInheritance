using System;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class SvantechnicalLegacy : ModItem, ILocalizedModType
    {
        public int NewDamage = CIServerConfig.Instance.ShadowspecBuff? 700 : 350;
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 26;
            Item.damage = NewDamage;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 2;
            Item.useAnimation = 24;
            Item.reuseDelay = 4;
            Item.useLimitPerAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;

            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = ModContent.RarityType<DonatorPink>();
            Item.Calamity().devItem = true;

            Item.ArmorPenetration = 500;
            Item.UseSound = SoundID.Item31;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override void HoldItem(Player player) => player.scope = true;

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 70)
                return false;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int i = Main.myPlayer;
            float sSpeed = Item.shootSpeed;
            player.itemTime = Item.useTime;
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
            }
            else
            {
                mouseDistance = sSpeed / mouseDistance;
            }
            float randXOffset = mouseXDist;
            float randYOffset = mouseYDist;
            randXOffset += Main.rand.Next(-1, 2) * 0.5f;
            randYOffset += Main.rand.Next(-1, 2) * 0.5f;
            if (Collision.CanHitLine(player.Center, 0, 0, realPlayerPos + new Vector2(randXOffset, randYOffset) * 2f, 0, 0))
            {
                realPlayerPos += new Vector2(randXOffset, randYOffset);
            }
            Projectile.NewProjectile(source, position.X, position.Y - player.gravDir * 4f, randXOffset, randYOffset, type, damage, knockback, i, 0f, Main.rand.Next(12) / 6f);
            int bulletAmt = Main.rand.Next(4, 6);
            for (int index = 0; index < bulletAmt; ++index)
            {
                float SpeedX = velocity.X + Main.rand.Next(-60, 61) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-60, 61) * 0.05f;
                Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SDFMG>().
                AddIngredient(ItemID.SoulofMight, 10).
                AddIngredient(ItemID.SoulofSight, 10).
                AddIngredient(ItemID.SoulofFright, 10).
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
