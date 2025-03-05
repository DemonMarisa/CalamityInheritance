using System;
using System.Collections.Generic;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientTarragon
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientTarragonLeggings : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ModContent.RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.defense = 26;
        }
        

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.20f;
            if(player.statLife <= player.statLifeMax2 * 0.5f)
            {
                player.moveSpeed += 0.15f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TarragonLeggings>().
                AddIngredient<UelibloomBar>(10).
                AddIngredient<DivineGeode>(10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}