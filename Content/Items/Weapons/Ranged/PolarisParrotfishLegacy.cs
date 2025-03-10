﻿using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Sounds;
using CalamityMod;
using CalamityMod.Items;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.NPCs.Calamitas;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class PolarisParrotfishLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 85;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 38;
            Item.height = 34;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.25f;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = CommonCalamitySounds.LaserCannonSound;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PolarStarLegacy>();
            Item.shootSpeed = 17f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var modPlayer = player.CalamityInheritance();
            if (modPlayer.PolarisBoostPhase3) //追踪
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<PolarStarLegacy>(), damage, knockback, player.whoAmI, 0f, 2f);
                if(CIFunction.IsThereNpcNearby(ModContent.NPCType<CalamitasRebornPhase2>(), player, 3000f))
                Projectile.NewProjectile(source, position, velocity/2, ModContent.ProjectileType<PolarStarLegacy>(), damage/2, knockback, player.whoAmI, 0f, 2f);
                return false;
            }
            else if (modPlayer.PolarisBoostPhase2) //分裂
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<PolarStarLegacy>(), (int)(damage * 1.25), knockback, player.whoAmI, 0f, 1f);
                return false;
            }
            return true;
        }

        public override Vector2? HoldoutOrigin() 
        {
            return new Vector2(10, 10);
        }
    }
}
