using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Ranged.Guns;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Guns
{
    public class LeviatitanLegacy : CIRanged
    {
        public override void ExSD()
        {
            Item.damage = 77;
            Item.useTime = Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5f;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item92;
            Item.shoot = ProjectileType<LeviatitanBlast>();
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + Main.rand.Next(-10, 11) * 0.05f;
            float SpeedY = velocity.Y + Main.rand.Next(-10, 11) * 0.05f;
            //if (CIConfig.Instance.AmmoConversion)
            //{
            //    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ProjectileType<AquaBlastToxic>(), (int)(damage * 1.5), knockback, player.whoAmI);
            //}
            if (Main.rand.NextBool(3) || type == ProjectileID.Bullet)
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ProjectileType<LeviatitanBlastVenom>(), (int)(damage * 1.5), knockback, player.whoAmI);
            else
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
