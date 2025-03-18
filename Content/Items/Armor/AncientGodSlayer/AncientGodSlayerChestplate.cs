using System;
using System.Collections.Generic;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientGodSlayer
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientGodSlayerChestplate : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 80;
        }
        

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.CIMod();
            modPlayer.GodSlayerReflect = true;
            player.thorns = 1f;
            player.statLifeMax2 += 400;
            player.statManaMax2 += 400;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GodSlayerChestplate>().
                AddIngredient<CosmiliteBar>(40).
                AddIngredient<AscendantSpiritEssence>(15).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}