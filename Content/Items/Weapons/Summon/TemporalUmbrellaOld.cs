using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Summon;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Summon.Umbrella;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Weapons.Summon
{
    [LegacyName("BensUmbrella")]
    public class TemporalUmbrellaOld : ModItem
    {
        public override void SetDefaults()
        {
            Item.mana = 99;
            Item.damage = 963;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 74;
            Item.height = 72;
            Item.useTime = Item.useAnimation = 10;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = ModContent.RarityType<DonatorPink>();
            Item.UseSound = SoundID.Item68;
            Item.shoot = ModContent.ProjectileType<MagicHatOld>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0 && player.maxMinions >= 5;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = Main.MouseWorld;
            for (int x = 0; x < Main.projectile.Length; x++)
            {
                Projectile projectile = Main.projectile[x];
                if (projectile.active && projectile.owner == player.whoAmI && projectile.type == type)
                {
                    projectile.Kill();
                }
            }
            Projectile.NewProjectile(source, position , Vector2.Zero , type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SpikecragStaff>());
            recipe.AddIngredient(ModContent.ItemType<SarosPossession>());
            recipe.AddIngredient(ItemID.Umbrella);
            recipe.AddIngredient(ItemID.TopHat);
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 4);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
