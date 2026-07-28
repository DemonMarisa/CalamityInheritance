using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Ranged.Harpoon;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Harpoon
{
    public class SepticSkewerLegacy : CIRanged
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsRangedSpecialistWeapon[Type] = true;
        }
        public override void ExSD()
        {
            Item.damage = 272;
            Item.useTime = Item.useAnimation = 12;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item10;
            Item.shootSpeed = 20f;
            Item.shoot = ProjectileType<SepticSkewerLegacyHarpoon>();
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
    }
}
