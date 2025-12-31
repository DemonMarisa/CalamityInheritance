using CalamityInheritance.Content.Projectiles.Magic.Books;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Magic;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Books
{
    public class PrimordialEarthLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Magic;
        public override void ExSD()
        {
            Item.width = 36;
            Item.height = 42;
            Item.damage = 46;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 19;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<SupremeDustProjectile>();
            Item.shootSpeed = 4f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DeathValleyDusterLegacy>().
                AddIngredient(ItemID.AncientBattleArmorMaterial, 3).
                AddIngredient(ItemID.MeteoriteBar, 5).
                AddIngredient(ItemID.Ectoplasm, 5).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
