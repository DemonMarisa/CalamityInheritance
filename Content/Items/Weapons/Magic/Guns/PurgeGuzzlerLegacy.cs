using CalamityInheritance.Content.Projectiles.Magic.Guns;
using CalamityInheritance.Rarity;
using CalamityMod.Items;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Rarities;
using CalamityMod.Sounds;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Guns
{
    internal class PurgeGuzzlerLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.Magic";
        private const float Spread = 0.025f;

        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 44;
            Item.damage = 120;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 22;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.UseSound = CommonCalamitySounds.LaserCannonSound;
            Item.shoot = ProjectileType<HolyLaserLegacy>();
            Item.shootSpeed = 6f;

            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Fire extra lasers to the left and right
            Projectile.NewProjectile(source, position, velocity.RotatedBy(-Spread), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(+Spread), type, damage, knockback, player.whoAmI);

            // Still also fire the center laser
            return true;
        }
    }
}
