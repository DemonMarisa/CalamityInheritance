using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.ExoWeapons;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoWeapons
{
    public class MagnomalyCannon : CIExoWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 84;
            Item.height = 30;
            Item.damage = 279;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 9.5f;
            Item.UseSound = SoundID.Item11;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MagnomalyRocket>();
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.Rocket;
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }
        public override Vector2? HoldoutOffset() => new Vector2(-30, -10);
        public override bool CanUseItem(Player player)
        {
            if (UseAlt)
            {
                Item.shoot = ProjectileType<MagnomalyRocketExoLore>();
                Item.useAnimation = Item.useTime = 67;
                Item.UseSound = CISounds.MagnomalyShoot;
            }
            else
            {
                Item.shoot = ProjectileType<MagnomalyRocket>();
                Item.useAnimation = Item.useTime = 15;
                Item.UseSound = SoundID.Item11;
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.Next(100) >= 50;

        public override void AddRecipes()
        {
        }
    }
}
