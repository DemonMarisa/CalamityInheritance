﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Melee;
using Terraria.DataStructures;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class BloodyEdge : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.damage = 70;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 23;
            Item.knockBack = 5.25f;
            Item.UseSound = SoundID.Item1;
            Item.height = 60;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<BloodyEdgeEnergySword>();
            Item.noMelee = true; // This is set the sword itself doesn't deal damage (only the projectile does).
            Item.shootsEveryUse = true; // This makes sure Player.ItemAnimationJustStarted is set when swinging.
            Item.autoReuse = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 5);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float adjustedItemScale = player.GetAdjustedItemScale(Item); // Get the melee scale of the player and item.

            Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);

            NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);

            return false; // Return false because we've manually created the projectiles.
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BurningBlood>(), 60);

            if (!target.canGhostHeal || player.moonLeech)
                return;

            int healAmount = Main.rand.Next(2) + 2;
            player.statLife += healAmount;
            player.HealEffect(healAmount);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LightsBane).
                AddIngredient(ItemID.Muramasa).
                AddIngredient(ItemID.BladeofGrass).
                AddIngredient(ItemID.FieryGreatsword).
                AddIngredient<PurifiedGel>(5).
                AddTile(TileID.DemonAltar).
                Register();
            CreateRecipe().
                AddIngredient(ItemID.BloodButcherer).
                AddIngredient(ItemID.Muramasa).
                AddIngredient(ItemID.BladeofGrass).
                AddIngredient(ItemID.FieryGreatsword).
                AddIngredient<PurifiedGel>(5).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}