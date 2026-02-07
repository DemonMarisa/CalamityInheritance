using CalamityInheritance.Content.Projectiles.Rogue.Bombs;
using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Bombs
{
    public class AlphaVirus : CIRogueClass
    {
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;
            Item.damage = 248;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 31;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 31;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.value = CIShopValue.RarityPricePureRed;
            Item.shoot = ProjectileType<AlphaVirusProjectile>();
            Item.shootSpeed = 4f;
            Item.rare = ItemRarityID.Purple;
            Item.DamageType = RogueDamageClass.Instance;
        }

        public override float StealthDamageMultiplier => 0.70f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, Item.shootSpeed, 0f);
                if (p.WithinBounds(Main.maxProjectiles))
                    Main.projectile[p].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }


        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<EpidemicShredder>().
                AddIngredient(ItemID.LunarBar, 5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
