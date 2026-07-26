using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Ranged.Guns;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Common.CalamityModCross;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Guns
{
    public class RubicoPrimeLegacy : CIRanged
    {
        public override void ExSD()
        {
            Item.damage = 1050;
            Item.knockBack = 10f;
            Item.useTime = 6;
            Item.useAnimation = 45;
            Item.useLimitPerAnimation = 5;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<DeepBlue>();
            Item.crit += 40;
            Item.CI().donerItem = true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<RubicoPrimeBullet>(), damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.PestilentDefiler).
                    AddIngredient(CalamityMaterials.CosmiliteBar, 8).
                    AddIngredient(CalamityMaterials.NightmareFuel, 20).
                    AddTile(CalTileID.CosmicAnvilID).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.VenusMagnum).
                    AddIngredient(ItemID.LunarBar, 8).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
        }
    }
}
