﻿using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.HeldProj.Magic;
using CalamityInheritance.Content.Projectiles.HeldProj.Typeless;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class ACTWingman : CIMagic, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<WingmanLegacy>();
        }
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 22;
            Item.damage = 70;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item33;
            Item.autoReuse = true;
            Item.shootSpeed = 25f;
            Item.shoot = ProjectileID.LaserMachinegunLaser;

            Item.noUseGraphic = true;
            Item.channel = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<WingmanHeldProj>(), damage, knockback, player.whoAmI);

            if (player.ownedProjectileCounts[ModContent.ProjectileType<ChangeDirProj>()] < 1)
                Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<ChangeDirProj>(), damage, knockback, player.whoAmI);

            return false;
        }
    }
}
