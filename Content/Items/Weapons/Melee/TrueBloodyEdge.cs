using CalamityMod.Buffs.DamageOverTime;
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
    public class TrueBloodyEdge : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 48;
            Item.damage = 75;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.height = 64;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<TrueBloodyEdgeEnergySword>();
            Item.shootSpeed = 11f;
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

            // 发射 TerraEdgeEnergySword
            Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);

            // 如果 velocity 是 (0, 0)，根据玩家面向方向设置默认速度
            if (velocity == Vector2.Zero)
            {
                velocity = new Vector2(player.direction, 0f); // 设置为水平方向
            }

            // 确保速度归一化并设置合适的弹幕速度
            Vector2 beamVelocity = Vector2.Normalize(velocity) * 20f;
            Projectile.NewProjectile(source, position, beamVelocity, ModContent.ProjectileType<TrueBloodyBladeProj>(), damage / 2, knockback, player.whoAmI);

            // Multiplayer 同步
            NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);

            return false; // Return false because we've manually created the projectiles.
        }

        private void OnHitEffects(Player player)
        {
            if (player.moonLeech)
                return;

            int healAmount = Main.rand.Next(3) + 3;
            if (Main.rand.NextBool(2))
            {
                player.statLife += healAmount;
                player.HealEffect(healAmount);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodyEdge>().
                AddIngredient(ItemID.SoulofFright, 3).
                AddIngredient(ItemID.SoulofMight, 3).
                AddIngredient(ItemID.SoulofSight, 3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
