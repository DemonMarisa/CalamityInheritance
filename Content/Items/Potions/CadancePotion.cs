using CalamityMod.Items.Materials;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Buffs;

namespace CalamityInheritance.Content.Items.Potions
{
    public class CadancePotion : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 38;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item3;
            Item.rare = ItemRarityID.LightRed;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<CadancesGrace>();
            Item.buffTime = CalamityUtils.SecondsToFrames(480f);
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LovePotion).
                AddIngredient(ItemID.HeartreachPotion).
                AddIngredient(ItemID.LifeforcePotion).
                AddIngredient(ItemID.RegenerationPotion).
                AddTile(TileID.AlchemyTable).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.BottledWater).
                AddIngredient<BloodOrb>(40).
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}
