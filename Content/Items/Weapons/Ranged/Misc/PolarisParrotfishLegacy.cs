using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Ranged.Misc;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Misc
{
    public class PolarisParrotfishLegacy : CIRanged
    {

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 92;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 38;
            Item.height = 34;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.25f;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = CISounds.LaserCannon;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<PolarStarLegacy>();
            Item.shootSpeed = 17f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var modPlayer = player.CI();
            if (modPlayer.PolarisPhase == 2) //追踪
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileType<PolarStarLegacy>(), damage, knockback, player.whoAmI, 0f, 2f);
                return false;
            }
            else if (modPlayer.PolarisPhase == 1) //分裂
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileType<PolarStarLegacy>(), (int)(damage * 1.25), knockback, player.whoAmI, 0f, 1f);
                return false;
            }
            return true;
        }
    }
}
