using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Ranged.Guns;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Guns
{
    public class NullificationPistolLegacy : CIRanged
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsRangedSpecialistWeapon[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 170;
            Item.useTime = Item.useAnimation = 20;
            Item.useAnimation = 20;
            Item.knockBack = 7f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item33;
            Item.shootSpeed = 25f;
            Item.shoot = ProjectileType<NullificationPistolBullet>();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
    }
}
