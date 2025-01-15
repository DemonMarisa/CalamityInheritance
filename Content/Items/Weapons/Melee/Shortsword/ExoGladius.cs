using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class ExoGladius : ModItem, ILocalizedModType 
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee.Shortsword";
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 13;
            Item.width = 56;
            Item.height = 56;
            Item.damage = 640;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 9.9f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ItemRarityID.Red;
            Item.shoot = ModContent.ProjectileType<ExoGladiusProj>();
            Item.shootSpeed = 4.8f;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GalileoGladius>());
            recipe.AddIngredient(ModContent.ItemType<CosmicShivold>());
            recipe.AddIngredient(ModContent.ItemType<Lucrecia>());
            recipe.AddIngredient(ModContent.ItemType<MiracleMatter>());
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();

            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<GalileoGladius>());
            recipe1.AddIngredient(ModContent.ItemType<CosmicShiv>());
            recipe1.AddIngredient(ModContent.ItemType<Lucrecia>());
            recipe1.AddIngredient(ModContent.ItemType<MiracleMatter>());
            recipe1.AddTile(ModContent.TileType<DraedonsForge>());
            recipe1.Register();
        }
    }
}
