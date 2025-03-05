using System;
using System.Collections.Generic;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientSilva
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientSilvaArmor : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 40;
        }
        
      
        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 400;
            player.statManaMax2 += 400;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SilvaArmor>().
                AddIngredient<EffulgentFeather>(35).
                AddIngredient<PlantyMush>(35).
                AddIngredient<AscendantSpiritEssence>(15).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}