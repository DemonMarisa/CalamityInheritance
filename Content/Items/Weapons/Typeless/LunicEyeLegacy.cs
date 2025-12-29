using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Sounds;
using LAP.Content.RecipeGroupAdd;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Typeless
{
    public class LunicEyeLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.Typeless";
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
            Item.UseSound = CommonCalamitySounds.LaserCannonSound;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ProjectileType<LunicBeamLegacy>();
            Item.shootSpeed = 12f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(LAPRecipeGroup.AnyGoldBar, 10).
                AddIngredient<StarblightSoot>(20).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
