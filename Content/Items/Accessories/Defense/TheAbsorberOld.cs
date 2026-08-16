using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Accessories.Misc;
using CalamityInheritance.Content.Items.Accessories.Movement;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Defense
{
    public class TheAbsorberOld : CIAccessories, ILocalizedModType
    {
        public override int AccessoriesStyle => Defense;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 30;
            Item.height = 38;
            Item.rare = ItemRarityID.Red;
            Item.value = CIShopValue.RarityPriceRed;
            Item.defense = 10;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if ((double)Math.Abs(player.velocity.X) < 0.05 && (double)Math.Abs(player.velocity.Y) < 0.05 && player.itemAnimation == 0)
            {
                player.LAP().LifeRegen += 2;
                player.manaRegenBonus += 2;
            }
            player.ignoreWater = true;
            player.CI().AmidiasSpark = true;
            player.CI().FungalCarapaceHurt = true;
            player.CI().HurtHeal += 0.05f;
            player.noKnockback = true;
            player.LAP().MaxLifeAdditive += 20;
            player.AddDR(0.1f);
            player.LAP().LifeRegen += 2;
            player.moveSpeed += 0.1f;
            player.accRunSpeed += 0.12f;
            player.jumpSpeedBoost += 0.5f;
            //海贝壳继承
            if (player.IsUnderwater())
            {
                player.statDefense += 3;
                player.endurance += 0.05f;
                player.moveSpeed += 0.1f;
                player.ignoreWater = true;
            }
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<SeaShell>().
                    AddIngredient(CalAccessories.IlmerisSpark).
                    AddIngredient<GrandGelatinLegacy>().
                    AddIngredient(CalAccessories.CrawCarapace).
                    AddIngredient<FungalCarapace>().
                    AddIngredient(CalAccessories.GiantTortoiseShell).
                    AddIngredient(CalamityMaterials.DepthCells, 15).
                    AddIngredient(CalamityMaterials.Lumenyl, 15).
                    AddIngredient(CalamityMaterials.Voidstone, 5).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient<SeaShell>().
                    AddIngredient<GrandGelatinLegacy>().
                    AddIngredient<FungalCarapace>().
                    AddIngredient(ItemID.TurtleShell).
                    AddIngredient(ItemID.Ectoplasm, 10).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
        }
    }
}
