using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityMod.Items.Placeables;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using System;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class TheAbsorberOld : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:20,
            itemHeight:24,
            itemRare:ItemRarityID.Red,
            itemValue:CIShopValue.RarityPriceRed,
            itemDefense:10
        );
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer calPlayer = player.Calamity();
            CalamityInheritancePlayer usPlayer = player.CIMod();
            player.noKnockback = true; //继承至🐢壳
            //等一下, 阴阳石的免伤数据呢?
            player.endurance += 0.10f;
            
            //继承至大凝胶:
            player.statLifeMax2 += 20;
            player.statManaMax2 += 20;
            player.moveSpeed += 0.1f;
            player.jumpSpeedBoost += 1.20f;
            if ((double)Math.Abs(player.velocity.X) < 0.05 && (double)Math.Abs(player.velocity.Y) < 0.05 && player.itemAnimation == 0)
            {
                player.lifeRegen += 2;
                player.manaRegenBonus += 2;
            }

            //阴阳石新加
            calPlayer.gShell = true;
            calPlayer.aSpark = true;
            usPlayer.FungalCarapace = true;
            usPlayer.TheAbsorberOld = true;

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
            CreateRecipe().
                AddIngredient<SeaShell>().
                AddIngredient<AmidiasSpark>().
                AddRecipeGroup(CIRecipeGroup.GrandGelatin).
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
