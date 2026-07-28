using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.MagicBook;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.MagicBook
{
    public class SeethingDischargeLegacy : CIMagic
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.damage = 52;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 6.75f;
            Item.UseSound = CISounds.FlareSound;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<BrimstoneBarrageLegacy_F>();
            Item.shootSpeed = 16f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedBy(-0.1f), type, damage, knockback, player.whoAmI, 0f, 0f);
            Projectile.NewProjectile(source, position, velocity, ProjectileType<BrimstoneHellblastLegacy_F>(), damage, knockback, player.whoAmI, 0f, 0f);
            Projectile.NewProjectile(source, position, velocity.RotatedBy(0.1f), type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
