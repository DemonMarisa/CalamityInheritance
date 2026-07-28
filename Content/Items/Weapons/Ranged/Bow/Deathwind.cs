using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Ranged.Bow;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Bow
{
    public class Deathwind : CIRanged
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 82;
            Item.damage = 148;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<CatalystViolet>();
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<DWArrow>();
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Arrow;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int index = 0; index < 4; ++index)
            {
                float SpeedX = velocity.X + Main.rand.Next(-20, 21) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-20, 21) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ProjectileType<DWArrow>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe()
                    .AddIngredient(CalamityMaterials.CosmiliteBar, 12)
                    .AddTile(CalamityTile.CosmicAnvilTile)
                    .Register();
            }
            else
            {

            }
        }
    }
}
