﻿using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Summon.Umbrella;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Weapons.Summon
{
    [LegacyName("BensUmbrella")]
    public class TemporalUmbrellaOld : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Temporal Umbrella");
            Tooltip.SetDefault("Surprisingly sturdy, I reckon this could defeat the Mafia in a single blow\n" +
                               "Summons a magic hat to hover above your head\n" +
                               "The hat will release a variety of objects to assault your foes\n" +
                               "Requires 5 minion slots to use\n" +
                               "There can only be one");
        }

        public override void SetDefaults()
        {
            item.mana = 99;
            item.damage = 963;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = 74;
            item.height = 72;
            item.useTime = item.useAnimation = 10;
            item.noMelee = true;
            item.knockBack = 1f;
            item.value = Item.buyPrice(5, 0, 0, 0);
            item.Calamity().customRarity = CalamityRarity.ItemSpecific;
            item.rare = 10;
            item.UseSound = SoundID.Item68;
            item.shoot = ModContent.ProjectileType<MagicHat>();
            item.shootSpeed = 10f;
            item.summon = true;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] <= 0 && player.maxMinions >= 5;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            speedX = 0;
            speedY = 0;
            for (int x = 0; x < Main.projectile.Length; x++)
            {
                Projectile projectile = Main.projectile[x];
                if (projectile.active && projectile.owner == player.whoAmI && projectile.type == type)
                {
                    projectile.Kill();
                }
            }
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<SpikecragStaff>());
            recipe.AddIngredient(ModContent.ItemType<SarosPossession>());
            recipe.AddIngredient(ItemID.Umbrella);
            recipe.AddIngredient(ItemID.TopHat);
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 4);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
