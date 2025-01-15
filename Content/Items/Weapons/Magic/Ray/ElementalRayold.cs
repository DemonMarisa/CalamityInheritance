using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Magic.Ray.ElementalBeamProj;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Ray
{
    public class ElementalRayold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 62;
            Item.damage = 120;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 18;
            Item.useTime = 15;
            Item.useAnimation = 20;
            Item.reuseDelay = 14;
            Item.useLimitPerAnimation = 1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.value = CIShopValue.RarityPricePurple;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ElementalBeamMain>();
            Item.shootSpeed = 6f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 shootVelocity = velocity;
            Vector2 shootPosition = position + shootVelocity * 12f;
            Projectile.NewProjectile(source, shootPosition, shootVelocity, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(((ModItem)this).Item, 0, (string)null), position, velocity, ModContent.ProjectileType<ElementalBeam>(), damage, knockback, ((Entity)player).whoAmI, 0f, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Photosynthesis>().
                AddIngredient(ItemID.LunarBar, 5).
                AddIngredient<LifeAlloy>(5).
                AddIngredient<GalacticaSingularity>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();

            CreateRecipe().
                AddIngredient<TerraRay>().
                AddIngredient(ItemID.LunarBar, 5).
                AddIngredient<LifeAlloy>(5).
                AddIngredient<GalacticaSingularity>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
