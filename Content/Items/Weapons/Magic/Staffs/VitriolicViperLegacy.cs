using CalamityInheritance.Content.Projectiles.Magic.Staffs;
using CalamityInheritance.Rarity;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Staffs
{
    public class VitriolicViperLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Magic;
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void ExSD()
        {
            Item.width = 60;
            Item.height = 62;
            Item.damage = 93;
            Item.mana = 15;
            Item.useTime = Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item46;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<VitriolicViperSpitLegacy>();
            Item.shootSpeed = 20f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -4; i <= 4; i += 1)
            {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(i));
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X * 1.65f, perturbedSpeed.Y * 1.65f, type, (int)(damage * 0.7f), knockback * 0.7f, player.whoAmI, 0f, 0f);
            }
            for (int j = -2; j <= 2; j += 1)
            {
                Vector2 perturbedSpeed2 = velocity.RotatedBy(MathHelper.ToRadians(j));
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed2.X, perturbedSpeed2.Y, ProjectileType<VitriolicViperFangLegacy>(), damage, knockback, player.whoAmI, 0f, 0f);
            }
            return false;
        }
    }
}
