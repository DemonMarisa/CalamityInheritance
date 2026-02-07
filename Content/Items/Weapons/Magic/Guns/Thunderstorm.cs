using CalamityInheritance.Content.Projectiles.Magic.Guns;
using CalamityInheritance.Rarity;
using CalamityInheritance.Sounds.Custom;
using CalamityMod.Items.Materials;
using CalamityMod.Sounds;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Guns
{
    public class Thunderstorm : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Magic;
        public override void ExSD()
        {
            Item.width = 48;
            Item.height = 22;
            Item.damage = 132;
            Item.mana = 50;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = RarityType<BlueGreen>();
            Item.UseSound = CISoundMenu.PlasmaBlast;
            Item.autoReuse = true;
            Item.shootSpeed = 6f;
            Item.shoot = ProjectileType<ThunderstormShot>();
        }
        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ArmoredShell>(2).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
