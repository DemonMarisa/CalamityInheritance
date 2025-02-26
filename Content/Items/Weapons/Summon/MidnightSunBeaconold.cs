using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.DataStructures;
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Weapons.Summon
{
    public class MidnightSunBeaconold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Summon";
        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.damage = 240;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item90;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MidnightSunBeaconProjold>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;

            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (Main.projectile.IndexInRange(p))
                Main.projectile[p].originalDamage = Item.damage;
            return false;
        }
        public override void AddRecipes()
        {

            CreateRecipe().
                AddIngredient(ItemID.XenoStaff).
                AddIngredient(ItemID.MoonlordTurretStaff).
                AddIngredient<DarksunFragment>(25). //->补上了缺失的日食碎片
                AddIngredient<AuricBar>(5).
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient(ItemID.XenoStaff).
                AddIngredient(ItemID.MoonlordTurretStaff).
                AddIngredient<DarksunFragment>(25).
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
