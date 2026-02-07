using CalamityInheritance.Content.Projectiles.Magic.Books;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Books
{
    public class LashesofChaosLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Magic;
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.damage = 65;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.useTime = 38;
            Item.useAnimation = 38;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<HellfireballReborn_F>();
            Item.shootSpeed = 12.5f;
        }
    }
}
