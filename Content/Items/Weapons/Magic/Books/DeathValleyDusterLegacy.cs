using CalamityInheritance.Content.Projectiles.Magic.Books;
using LAP.Content.RecipeGroupAdd;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Books
{
    public class DeathValleyDusterLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Magic;
        public override void ExSD()
        {
            Item.width = 36;
            Item.height = 40;
            Item.damage = 83;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 9;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<DustProjectile>();
            Item.shootSpeed = 5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SpellTome).
                AddRecipeGroup(LAPRecipeGroup.AnyAdamantiteBar, 5).
                AddIngredient(ItemID.AncientBattleArmorMaterial).
                AddIngredient(ItemID.FossilOre, 25).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
