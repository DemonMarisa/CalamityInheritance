using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class OmegaBiomeBlade : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Omega Biome Blade(old)");
            /* Tooltip.SetDefault("Fires different homing projectiles based on what biome you're in\n" +
                "Projectiles also change based on moon events"); */
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.damage = 150;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 8;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 62;
            Item.value = CIShopValue.RarityPricePurple;
            Item.rare = ItemRarityID.Purple;
            Item.shoot = ModContent.ProjectileType<OmegaBiomeOrb>();
            Item.shootSpeed = 15f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int projectiles = 0; projectiles < 3; projectiles++)
            {
                float speedX = velocity.X + Main.rand.Next(-40, 41) * 0.05f;
                float speedY = velocity.Y + Main.rand.Next(-40, 41) * 0.05f;

                Projectile.NewProjectile(source, position, new Vector2(speedX, speedY), type, damage, knockback, player.whoAmI);
            }
            return false;
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
                AddIngredient<TrueBiomeBlade>().
                AddIngredient<CoreofCalamity>().
                AddIngredient<LifeAlloy>(3).
                AddIngredient<GalacticaSingularity>(3).
                AddIngredient(ItemID.LunarBar, 5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
