using CalamityInheritance.Content.Projectiles.HeldProj.Melee;
using CalamityInheritance.Content.Projectiles.Melee.Swords;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class BladecrestOathswordLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        public int Filp = 1;
        public override void ExSD()
        {
            Item.width = 56;
            Item.height = 56;
            Item.damage = 25;
            Item.useAnimation = 50;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 50;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            //是的，这是新建射弹
            Item.shoot = ProjectileType<BladecrestOathswordHeld>();
            Item.shootSpeed = 6f;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Filp == 1)
            {
                Filp = -1;
            }
            else
            {
                Filp = 1;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Filp);
            return false;
        }
    }
}
