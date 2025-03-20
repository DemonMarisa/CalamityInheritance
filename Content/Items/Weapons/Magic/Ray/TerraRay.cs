using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Magic.Ray;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Ray
{
    public class TerraRay : CIMagic, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Magic";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 54;
            Item.height = 54;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TerraBeamMain>();
            Item.shootSpeed = 6f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 shootVelocity = velocity;
            Vector2 shootPosition = position + shootVelocity * 12f;
            Projectile.NewProjectile(source, shootPosition, shootVelocity, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(player.GetSource_ItemUse_WithPotentialAmmo(this.Item, 0, null), position, velocity, ModContent.ProjectileType<TerraBeam>(), damage, knockback, player.whoAmI, 0f, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("CalamityInheritance:AnyTerraRay");
            recipe.AddIngredient(ModContent.ItemType<ValkyrieRay>());
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();

            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<CarnageRay>());
            recipe1.AddIngredient(ModContent.ItemType<ValkyrieRay>());
            recipe1.AddIngredient(ModContent.ItemType<LivingShard>(), 7);
            recipe1.AddTile(TileID.MythrilAnvil);
            recipe1.Register();
        }
    }
}
