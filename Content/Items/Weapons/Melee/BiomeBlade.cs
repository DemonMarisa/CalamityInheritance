using CalamityInheritance.Content.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class BiomeBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Biome Blade(old)");
            // Tooltip.SetDefault("Fires different projectiles based on what biome you're in");
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.damage = 55;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 42;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<BiomeOrb>();
            Item.shootSpeed = 12f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.WoodenSword);
            recipe.AddIngredient(ItemID.DirtBlock, 20);
            recipe.AddIngredient(ItemID.SandBlock, 20);
            recipe.AddIngredient(ItemID.IceBlock, 20); //intentionally not any ice
            recipe.AddRecipeGroup("AnyEvilBlock", 20);
            recipe.AddIngredient(ItemID.GlowingMushroom, 20);
            recipe.AddIngredient(ItemID.Marble, 20);
            recipe.AddIngredient(ItemID.Granite, 20);
            recipe.AddIngredient(ItemID.Hellstone, 20);
            recipe.AddIngredient(ItemID.Coral, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 0);
            }
        }
    }
}
