using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Armor.Bloodflare;

namespace CalamityInheritance.Content.Items.Armor.AncientBloodflare
{
[AutoloadEquip(EquipType.Legs)]
public class AncientBloodflareCuisses : ModItem, ILocalizedModType
{
    public new string LocalizationCategory => "Content.Items.Armor";
    public override void SetStaticDefaults()
    {
    }

    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;
        Item.value = CIShopValue.RarityPriceBlueGreen;
        Item.rare= ModContent.RarityType<BlueGreen>();
        Item.defense = 29;
    }
    

    public override void UpdateEquip(Player player)
    {
    	player.moveSpeed += 0.3f;
    	player.lavaImmune = true;
    	player.ignoreWater = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<BloodflareCuisses>(1).
            AddIngredient<BloodflareCore>(10).
            AddIngredient<RuinousSoul>(10).
            AddTile(TileID.LunarCraftingStation).
            Register();
    }
}}