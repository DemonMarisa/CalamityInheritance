using CalamityInheritance.Content.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class BiomeBlade : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.damage = 55;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 42;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<BiomeOrb>();
            Item.shootSpeed = 12f;
        }

        

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Dirt);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WoodenSword).
                AddIngredient(ItemID.DirtBlock, 20).
                AddIngredient(ItemID.SandBlock, 20).
                AddIngredient(ItemID.IceBlock, 20). //intentionally not any ice
                AddRecipeGroup("AnyEvilBlock", 20).
                AddIngredient(ItemID.GlowingMushroom, 20).
                AddIngredient(ItemID.Marble, 20).
                AddIngredient(ItemID.Granite, 20).
                AddIngredient(ItemID.Hellstone, 20).
                AddIngredient(ItemID.Coral, 20).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
