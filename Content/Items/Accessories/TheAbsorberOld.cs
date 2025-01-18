using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Items.Placeables;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class TheAbsorberOld : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 24;
            Item.defense = 10;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            player.noKnockback = true; // Inherited from Giant Tortoise Shell
            modPlayer.gShell = true;
            modPlayer.aSpark = true;
            modPlayer1.FungalCarapace = true;
            modPlayer1.TheAbsorberOld = true;
            player.ignoreWater = true;
            if (player.IsUnderwater())
            {
                player.statDefense += 3;
                player.endurance += 0.05f;
                player.moveSpeed += 0.1f;
                player.ignoreWater = true;
            }
            player.statLifeMax2 += 20;
            player.statManaMax2 += 20;
            player.accRunSpeed += 0.12f;
            player.jumpSpeedBoost += 0.24f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SeaShell>().
                AddIngredient<AmidiasSpark>().
                AddIngredient<GrandGelatinLegacy>().
                AddIngredient<CrawCarapace>().
                AddIngredient<FungalCarapace>().
                AddIngredient<GiantTortoiseShell>().
                AddIngredient<DepthCells>(15).
                AddIngredient<Lumenyl>(15).
                AddIngredient<Voidstone>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();

            CreateRecipe().
                AddIngredient<SeaShell>().
                AddIngredient<AmidiasSpark>().
                AddIngredient<GrandGelatin>().
                AddIngredient<CrawCarapace>().
                AddIngredient<FungalCarapace>().
                AddIngredient<GiantTortoiseShell>().
                AddIngredient<DepthCells>(15).
                AddIngredient<Lumenyl>(15).
                AddIngredient<Voidstone>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
