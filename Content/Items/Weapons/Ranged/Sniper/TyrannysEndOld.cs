using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Ranged.Sniper;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Sniper
{
    public class TyrannysEndOld : CIRanged, ILocalizedModType
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 150;
            Item.height = 48;
            Item.damage = 8000;
            Item.knockBack = 9.5f;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 55;
            Item.useAnimation = 55;
            Item.shoot = ProjectileID.BulletHighVelocity;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.UseSound = CISounds.LargeWeaponFire;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = RarityType<CatalystViolet>();
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 46;

        public override Vector2? HoldoutOffset() => new Vector2(-15, 0);

        public override void HoldItem(Player player) => player.scope = true;

        public override void AddRecipes()
        {
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (type == ProjectileID.Bullet)
            {
                Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, ProjectileType<BetterAMR>(), damage, knockback, Main.myPlayer, 0f, 0f);
            }
            else
            {
                Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer, 0f, 0f);
                proj.CI().AMRextra = true;
            }
            return false;
        }
    }
}
