using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.HeldProj.Ranged.StarCannon;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.StarCannon
{
    public class NorfleetLegacy : CIRanged
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 140;
            Item.height = 42;
            Item.damage = 1000;
            Item.knockBack = 15f;
            Item.shootSpeed = 30f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 70;
            Item.useTime = 70;
            Item.UseSound = SoundID.Item92;
            Item.shoot = ProjectileType<NorfleetCannonLegacy>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<DeepBlue>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Ranged;
            Item.channel = true;
            Item.useTurn = false;
            Item.useAmmo = AmmoID.FallenStar;
            Item.autoReuse = true;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<NorfleetCannonLegacy>(), 0, 0f, player.whoAmI);
            return false;
        }
    }
}
