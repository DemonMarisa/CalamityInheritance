﻿using CalamityMod.Items.Materials;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Fishing.AstralCatches;
using CalamityInheritance.Buffs;

namespace CalamityInheritance.Content.Items.Potions
{
    public class RevivifyPotion : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 36;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<Revivify>();
            Item.buffTime = CalamityUtils.SecondsToFrames(180f);
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HolyWater, 1);
            recipe.AddIngredient(ModContent.ItemType<StarblightSoot>(), 4);
            recipe.AddIngredient(ItemID.CrystalShard, 1);
            recipe.AddIngredient(ModContent.ItemType<EssenceofSunlight>());
            recipe.AddIngredient(ModContent.ItemType<TwinklingPollox> ());
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();
            // Blood orb recipes no alch table effect
            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.HolyWater, 1);
            recipe2.AddIngredient(ModContent.ItemType<BloodOrb>(), 20);
            recipe2.AddTile(TileID.AlchemyTable);
            recipe2.Register();
        }
    }
}