using CalamityInheritance.Content.Projectiles.Ranged.Guns;
using CalamityInheritance.System.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Guns
{
    public class LeviatitanLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Ranged;
        public override void SetDefaults()
        {
            Item.width = 82;
            Item.height = 28;
            Item.damage = 77;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 9;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item92;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<AquaBlastLegacy>();
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
            if (CIConfig.Instance.AmmoConversion)
            {
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ProjectileType<AquaBlastToxic>(), (int)(damage * 1.5), knockback, player.whoAmI);
            }
            else
            {
                if (type == ProjectileID.Bullet)
                {
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ProjectileType<AquaBlastToxic>(), (int)(damage * 1.5), knockback, player.whoAmI);
                }
                else
                {
                    if (Main.rand.NextBool(3))
                        Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ProjectileType<AquaBlastToxic>(), (int)(damage * 1.5), knockback, player.whoAmI);
                    else
                        Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
                }
            }
            return false;
        }
    }
}
