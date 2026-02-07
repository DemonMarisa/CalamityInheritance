using CalamityInheritance.Content.Projectiles.Magic.Books;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Books
{
    public class FrostBoltLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Magic;
        public override void ExSD()
        {
            Item.width = 34;
            Item.height = 38;
            Item.damage = 14;
            Item.mana = 6;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<FrostBoltProj>();
            Item.shootSpeed = 6f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.IceBlock, 20).
                AddIngredient(ItemID.SnowBlock, 10).
                AddIngredient(ItemID.Shiverthorn, 2).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
