using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Placeables;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class TrueBiomeBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("True Biome Blade(old)");
            // Tooltip.SetDefault("Fires different projectiles based on what biome you're in");
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.damage = 160;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 21;
            Item.useTime = 21;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 54;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<TrueBiomeOrb>();
            Item.shootSpeed = 12f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BiomeBlade>());
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 5);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Lumenyl>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Voidstone>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
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
