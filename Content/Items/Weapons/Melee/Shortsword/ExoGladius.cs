using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityInheritance.Rarity;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Items.Materials;

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
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Melee/Shortsword/ExoGladiusGlow").Value);
        }
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

            CreateRecipe().
                AddIngredient(ModContent.ItemType<GalileoGladius>()).
                AddIngredient(ModContent.ItemType<CosmicShiv>()).
                AddIngredient(ModContent.ItemType<Lucrecia>()).
                AddIngredient(ModContent.ItemType<AncientMiracleMatter>()).
                AddTile(ModContent.TileType<DraedonsForge>()).
                Register();

            CreateRecipe().
                AddIngredient(ModContent.ItemType<GalileoGladius>()).
                AddIngredient(ModContent.ItemType<CosmicShivold>()).
                AddIngredient(ModContent.ItemType<Lucrecia>()).
                AddIngredient(ModContent.ItemType<AncientMiracleMatter>()).
                AddTile(ModContent.TileType<DraedonsForge>()).
                Register();
        }
    }
}
