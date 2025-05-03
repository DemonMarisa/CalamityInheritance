using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityInheritance.Content.Projectiles.Melee.Spear;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Spear
{
    public class HolidayHalberd : CIMelee, ILocalizedModType
    {

        public override void SetDefaults()
        {
            Item.width = 70;
            Item.damage = 98;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = false;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 72;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 12f;

            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<FulgurationHalberdProj>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<HolidayHalberdProj>(), damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
