using System;
using System.Collections.Generic;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientBloodflare
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientBloodflareBodyArmor : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ModContent.RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.defense = 33;
        }
        
        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 300;
            player.statManaMax2 += 300;
            if (player.lavaWet == true)
            {
                player.statDefense += 30;
                player.lifeRegen += 10;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodflareBodyArmor>(1).
                AddIngredient<BloodstoneCore>(25).
                AddIngredient<RuinousSoul>(25).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}