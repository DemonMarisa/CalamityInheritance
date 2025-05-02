﻿using CalamityMod.Buffs.DamageOverTime;
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
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Melee.Spear;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Spear
{
    public class FulgurationHalberd : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 64;
            Item.scale = 1.5f;
            Item.damage = 70;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 22;
            Item.useTurn = true;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.shootSpeed = 8f;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;
                Item.noUseGraphic = true;
                Item.useStyle = ItemUseStyleID.Shoot;
                Item.shoot = ModContent.ProjectileType<FulgurationHalberdProj>();
                return player.ownedProjectileCounts[Item.shoot] <= 0;
            }
            else
            {
                Item.noMelee = false;
                Item.noUseGraphic = false;
                Item.useStyle = ItemUseStyleID.Swing;
                Item.shoot = ProjectileID.None;
                return base.CanUseItem(player);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<FulgurationHalberdProj>(), damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BurningBlood>(), 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hit)
        {
            target.AddBuff(ModContent.BuffType<BurningBlood>(), 300);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("AnyAdamantiteBar", 10).
                AddIngredient(ItemID.CrystalShard, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
