﻿using CalamityMod.Items.Weapons.Rogue;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Rogue;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class BouncingBetty : RogueWeapon
    {
        public const int BaseDamage = 52;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = BaseDamage;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 24;
            Item.useTime = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.width = 16;
            Item.height = 22;
            Item.maxStack = 999;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<BouncingBettyProjectile>();
            Item.shootSpeed = 16f;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            int projectileIndex = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (player.Calamity().StealthStrikeAvailable() && projectileIndex.WithinBounds(Main.maxProjectiles))
            {
                Main.projectile[projectileIndex].Calamity().stealthStrike = true;
            }
            return false;
        }
    }
}
