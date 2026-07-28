using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.MagicGun.Cannon;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.MagicGun.Cannon
{
    internal class PurgeGuzzlerLegacy : CIMagic
    {
        private const float Spread = 0.025f;

        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 44;
            Item.damage = 120;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 22;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.UseSound = CISounds.LaserCannon;
            Item.shoot = ProjectileType<HolyLaserLegacy>();
            Item.shootSpeed = 6f;

            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Fire extra lasers to the left and right
            Projectile.NewProjectile(source, position, velocity.RotatedBy(-Spread), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(+Spread), type, damage, knockback, player.whoAmI);

            // Still also fire the center laser
            return true;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe()
                    .AddIngredient(CalamityMaterials.UnholyEssence, 8)
                    .AddIngredient(CalamityMaterials.DivineGeode, 4)
                    .AddTile(TileID.LunarCraftingStation)
                    .Register();
            }
            else
            {

            }
        }
    }
}
