using CalamityMod.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Healing;
using CalamityMod.Projectiles;
using CalamityInheritance.Content.Projectiles.Melee;
using Terraria.GameContent.Drawing;
using Terraria.DataStructures;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class TerraEdge : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.damage = 115;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.knockBack = 6.25f;
            Item.UseSound = SoundID.Item1;
            Item.height = 58;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<TerraEdgeEnergySword>();
            Item.shootSpeed = 3f;
            Item.noMelee = true; // This is set the sword itself doesn't deal damage (only the projectile does).
            Item.shootsEveryUse = true; // This makes sure Player.ItemAnimationJustStarted is set when swinging.
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float adjustedItemScale = player.GetAdjustedItemScale(Item);

            Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);

            if (velocity == Vector2.Zero)
            {
                velocity = new Vector2(player.direction, 0f);
            }

            Vector2 beamVelocity = Vector2.Normalize(velocity) * 16f;
            Projectile.NewProjectile(source, position, beamVelocity, ModContent.ProjectileType<TerraEdgeBeam>(), (int)(damage * 1.5f), knockback, player.whoAmI);

            NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);

            return false; // Return false because we've manually created the projectiles.
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TrueBloodyEdge>());
            recipe.AddIngredient(ItemID.TrueExcalibur);
            recipe.AddIngredient(ModContent.ItemType<LivingShard>(), 7);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ItemID.TrueNightsEdge);
            recipe1.AddIngredient(ItemID.TrueExcalibur);
            recipe1.AddIngredient(ModContent.ItemType<LivingShard>(), 7);
            recipe1.AddTile(TileID.MythrilAnvil);
            recipe1.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.TerraBlade);
            }
        }
        public void OnHitEffects(Player player)
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

        public void OnHitHealEffect(int damage)
        {
            int heal = (int)Math.Round(damage * 0.025);
            if (heal > 100)
                heal = 100;

            if (Main.player[Main.myPlayer].lifeSteal <= 0f || heal <= 0)
                return;
        }
    }
}
