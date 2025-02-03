using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Projectiles.Magic.Ray;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Ray
{
    public class FabstaffOld : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 800;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 50;
            Item.width = 84;
            Item.height = 84;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = ModContent.RarityType<DonatorPink>();
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FabRayOld>();
            Item.shootSpeed = 6f;
        }
        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(0, -3);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Necroplasm>(), 100);
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 50);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();

            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                Register();
        }
    }
}
