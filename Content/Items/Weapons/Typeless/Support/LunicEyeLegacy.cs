using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Typeless.Weapon.Support;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Content.RecipeGroupAdd;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Typeless.Support
{
    public class LunicEyeLegacy : CITypeless
    {
        public override void SetDefaults()
        {
            Item.width = 78;
            Item.height = 48;
            Item.damage = 9;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4.5f;
            Item.UseSound = CISounds.LaserCannon;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ProjectileType<LunicBeamLegacy>();
            Item.shootSpeed = 12f;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddRecipeGroup(LAPRecipeGroup.AnyGoldBar, 10).
                    AddIngredient(CalamityMaterials.StarblightSoot, 20).
                    AddTile(TileID.Anvils).
                    Register();
            }
            else
            {

                CreateRecipe().
                    AddRecipeGroup(LAPRecipeGroup.AnyGoldBar, 10).
                    AddTile(TileID.Anvils).
                    Register();
            }
        }
    }
}
