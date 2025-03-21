﻿using CalamityMod.Items.Materials;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Fishing.SunkenSeaCatches;
using CalamityInheritance.Buffs.Potions;
using CalamityInheritance.Content.Items.Potions.CIPotions;

namespace CalamityInheritance.Content.Items.Potions
{
    public class TitanScalePotion : CIPotion, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Potions";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 34;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Yellow;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = CISoundID.SoundPotions;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<TitanScale>();
            Item.buffTime = CalamityUtils.SecondsToFrames(480f);
            Item.value = CIShopValue.RarityPriceYellow;
        }

        public override void AddRecipes()
        {
            CreateRecipe(4).
                AddIngredient(ItemID.TitanPotion, 4).
                AddIngredient(ItemID.BeetleHusk).
                AddIngredient<PrismaticGuppy> ().
                AddTile(TileID.AlchemyTable).
                AddConsumeItemCallback(Recipe.ConsumptionRules.Alchemy).
                Register();

            CreateRecipe(4).
                AddIngredient(ItemID.BottledWater, 4).
                AddIngredient<BloodOrb>(40).
                AddIngredient(ItemID.BeetleHusk).
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}
