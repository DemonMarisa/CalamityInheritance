using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Ranged.Cannon;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.HandGun
{
    public class OpalStrikerLegacy : CIRanged
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 30;
            Item.useTime = 5;
            Item.reuseDelay = 25;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0f;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = CISounds.OpalStriker;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<OpalStrikeLegacy>();
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;

        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ProjectileType<OpalStrikeLegacy>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextBool();

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Marble, 20).
                AddIngredient(ItemID.Diamond, 3).
                AddIngredient(ItemID.Amber, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
