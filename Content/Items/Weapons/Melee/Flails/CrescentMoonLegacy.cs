using CalamityInheritance.Content.Projectiles.Melee.Flails;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Flails
{
    public class CrescentMoonLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        public override void SetStaticDefaults()
        {
            Type.ShimmerEach<CrescentMoon>();
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.damage = 380;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item82;
            Item.rare = RarityType<AbsoluteGreen>();
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.shootSpeed = 24f;
            Item.shoot = ProjectileType<CrescentMoonFlailLegacy>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float ai3 = (Main.rand.NextFloat() - 0.5f) * 0.7853982f; //0.5
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, ai3);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Lumenyl>(8).
                AddIngredient<RuinousSoul>(3).
                AddIngredient<ExodiumCluster>(16).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
