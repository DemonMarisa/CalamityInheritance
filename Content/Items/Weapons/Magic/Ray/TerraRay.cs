using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.Ray;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Ray
{
    public class TerraRay : CIMagic
    {

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 55;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 54;
            Item.height = 54;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<TerraBeamMain>();
            Item.shootSpeed = 6f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 shootVelocity = velocity;
            Vector2 shootPosition = position + shootVelocity * 12f;
            Projectile.NewProjectile(source, shootPosition, shootVelocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe()
                    .AddIngredient<NightsRayold>()
                    .AddIngredient(CalamityWeapons.ValkyrieRay)
                    .AddIngredient(ItemID.BrokenHeroSword)
                    .Register();

                CreateRecipe()
                    .AddIngredient<CarnageRay>()
                    .AddIngredient(CalamityWeapons.ValkyrieRay)
                    .AddIngredient(ItemID.BrokenHeroSword)
                    .AddTile(TileID.MythrilAnvil)
                    .Register();
            }
            else
            {
                CreateRecipe()
                    .AddIngredient<CarnageRay>()
                    .AddIngredient<NightsRayold>()
                    .AddIngredient(ItemID.BrokenHeroSword)
                    .AddTile(TileID.MythrilAnvil)
                    .Register();
            }
        }
    }
}
