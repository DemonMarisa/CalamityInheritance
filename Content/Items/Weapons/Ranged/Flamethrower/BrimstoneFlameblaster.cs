using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Ranged.Flamethrower;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Flamethrower
{
    public class BrimstoneFlameblaster : CIRanged
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 128;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 50;
            Item.height = 18;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item34;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<CatastropheBall>();
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Gel;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

    }
}