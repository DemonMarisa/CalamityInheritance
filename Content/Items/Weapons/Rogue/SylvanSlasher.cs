﻿using CalamityMod.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityMod.Rarities;
using CalamityInheritance.Content.Projectiles.Rogue;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class SylvanSlasher : RogueWeapon
    {
        public override void SetDefaults()
        {
            Item.width = 72;
            Item.damage = 320;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 5;
            Item.knockBack = 3f;
            Item.autoReuse = false;
            Item.height = 78;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.shoot = ModContent.ProjectileType<SylvanSlashAttack>();
            Item.shootSpeed = 24f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}