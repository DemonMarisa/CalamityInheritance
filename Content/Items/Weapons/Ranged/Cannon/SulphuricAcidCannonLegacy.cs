using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Ranged.Cannon;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Cannon
{
    public class SulphuricAcidCannonLegacy : CIRanged
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsRangedSpecialistWeapon[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 90;
            Item.height = 30;
            Item.damage = 360;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.UseSound = SoundID.Item95;
            Item.shoot = ProjectileType<SulphuricBlast>();
            Item.shootSpeed = 16f;
            Item.rare = RarityType<AbsoluteGreen>();
        }

        public override Vector2? HoldoutOffset() => Vector2.UnitX * -15f;

    }
}
