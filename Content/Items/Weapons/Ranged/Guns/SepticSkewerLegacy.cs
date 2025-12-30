using CalamityInheritance.Content.Projectiles.Ranged.Guns;
using CalamityInheritance.Rarity;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Guns
{
    public class SepticSkewerLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Ranged;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsRangedSpecialistWeapon[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 24;
            Item.damage = 272;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item10;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
            Item.shoot = ProjectileType<SepticSkewerHarpoonLegacy>();

            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
    }
}
