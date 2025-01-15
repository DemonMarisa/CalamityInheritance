using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class ProporsePistol : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.damage = 90;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 36;
            Item.height = 20;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item33;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<ProBolt>();
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<ProBolt>(), damage, knockback, player.whoAmI);
            return false;
        }
    }
}
