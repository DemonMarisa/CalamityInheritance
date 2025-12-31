using CalamityInheritance.Content.Projectiles.Magic.Books;
using CalamityMod.Items.Materials;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Books
{
    public class FrigidflashBoltLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Magic;
        public static readonly SoundStyle UseSound = new("CalamityMod/Sounds/Item/FrigidflashUse");
        public static readonly SoundStyle ProjDeathSound = new("CalamityMod/Sounds/Item/FrigidflashDeath");
        public override void ExSD()
        {
            Item.width = 38;
            Item.height = 42;
            Item.damage = 80;
            Item.mana = 13;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = UseSound;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<FrigidflashBoltProj>();
            Item.shootSpeed = 9f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FrostBoltLegacy>().
                AddIngredient<FlareBoltLegacy>().
                AddIngredient<EssenceofEleum>(2).
                AddIngredient<EssenceofHavoc>(2).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
