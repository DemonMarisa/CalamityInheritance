using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class HadopelagicEcho : ModItem
    {
        private int counter = 0;
        public static readonly SoundStyle UseSound = new("CalamityInheritance/Sounds/Custom/WyrmScream") { Volume = 1f };
        public override void SetDefaults()
        {
            Item.damage = 2300;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 8;
            Item.reuseDelay = 20;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<HadopelagicEchoSoundwave>();
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float damageMult = 1f;
            if (counter == 1)
                damageMult = 1.1f;
            if (counter == 2)
                damageMult = 1.2f;
            if (counter == 3)
                damageMult = 1.35f;
            if (counter == 4)
                damageMult = 1.5f;
            Projectile.NewProjectile(source,position.X, position.Y-5, velocity.X, velocity.Y, type, (int)(damage * damageMult), knockback, player.whoAmI, counter, 0f);
            counter++;
            if (counter >= 5)
                counter = 0;
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<EidolicWail>());
            recipe.AddIngredient(ModContent.ItemType<ReaperTooth>(), 20);
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 20);
            recipe.AddIngredient(ModContent.ItemType<Lumenyl>(), 20);
            recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 5);
            recipe.AddTile(ModContent.TileType<CosmicAnvil>());
            recipe.Register();
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<EidolicWail>());
            recipe1.AddIngredient(ModContent.ItemType<ReaperTooth>(), 20);
            recipe1.AddIngredient(ModContent.ItemType<DepthCells>(), 20);
            recipe1.AddIngredient(ModContent.ItemType<Lumenyl>(), 20);
            recipe1.AddIngredient(ModContent.ItemType<AuricBarold>());
            recipe1.AddTile(ModContent.TileType<CosmicAnvil>());
            recipe1.Register();
        }
    }
}
