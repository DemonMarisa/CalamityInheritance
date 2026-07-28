using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Magic.MagicBook;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.MagicBook
{
    public class TomeofFateLegacy : CIMagic
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.damage = 110;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.useTime = 5;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item103;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<CosmicTentacleLegacy>();
            Item.shootSpeed = 17f;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 3;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int i = Main.myPlayer;
            float playerKnockback = knockback;
            playerKnockback = player.GetWeaponKnockback(Item, playerKnockback);
            player.itemTime = Item.useTime;
            Vector2 realPlayerPos = player.RotatedRelativePoint(player.MountedCenter, true);
            float mouseXDist = Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
            float mouseYDist = Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
            Vector2 tentacleVelocity = new Vector2(mouseXDist, mouseYDist);
            tentacleVelocity.Normalize();
            Vector2 tentacleRandVelocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
            tentacleRandVelocity.Normalize();
            tentacleVelocity = tentacleVelocity * 4f + tentacleRandVelocity;
            tentacleVelocity.Normalize();
            tentacleVelocity *= Item.shootSpeed;
            int projChoice = Main.rand.Next(7);
            float tentacleYDirection = Main.rand.Next(10, 160) * 0.001f;
            if (Main.rand.NextBool())
            {
                tentacleYDirection *= -1f;
            }
            float tentacleXDirection = Main.rand.Next(10, 160) * 0.001f;
            if (Main.rand.NextBool())
            {
                tentacleXDirection *= -1f;
            }
            if (projChoice == 0)
            {
                Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, tentacleVelocity.X, tentacleVelocity.Y, ProjectileType<BrimstoneTentacle>(), (int)((double)damage * 1.5f), playerKnockback, i, tentacleXDirection, tentacleYDirection);
            }
            else
            {
                Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, tentacleVelocity.X, tentacleVelocity.Y, type, damage, playerKnockback, i, tentacleXDirection, tentacleYDirection);
            }
            return false;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.SpellTome).
                    AddIngredient(CalamityMaterials.MeldBlob, 9).
                    AddTile(TileID.Bookcases).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.SpellTome).
                    AddIngredient<GalacticaSingularity>(5).
                    AddTile(TileID.Bookcases).
                    Register();
            }
        }
    }
}
