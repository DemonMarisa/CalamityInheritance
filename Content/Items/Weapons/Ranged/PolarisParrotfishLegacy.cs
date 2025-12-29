using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Sounds;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityMod.Items.Weapons.Ranged;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class PolarisParrotfishLegacy : CIRanged, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<PolarisParrotfish>();
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
            Item.UseSound = CommonCalamitySounds.LaserCannonSound;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<PolarStarLegacy>();
            Item.shootSpeed = 17f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var modPlayer = player.CIMod();
            if (Main.zenithWorld)
            {
                for (int i = 0; i < 8 ; i++)
                {
                    Vector2 spread = velocity.RotatedByRandom(MathHelper.ToRadians(30f))  * Main.rand.NextFloat(0.8f, 1.1f);
                    Projectile.NewProjectile(source, position, spread, ProjectileType<PolarStarLegacy>(), damage/3, knockback, player.whoAmI, 0f, Main.rand.NextBool(3)? 2f :1f);
                }
            }
            if (modPlayer.PolarisPhase3) //追踪
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileType<PolarStarLegacy>(), damage, knockback, player.whoAmI, 0f, 2f);
                return false;
            }
            else if (modPlayer.PolarisPhase2) //分裂
            {
                Projectile.NewProjectile(source, position, velocity, ProjectileType<PolarStarLegacy>(), (int)(damage * 1.25), knockback, player.whoAmI, 0f, 1f);
                return false;
            }
            return true;
        }
    }
}
