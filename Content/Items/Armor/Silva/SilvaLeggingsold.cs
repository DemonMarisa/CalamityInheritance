﻿using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Placeables;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Armor.Silva
{
    [AutoloadEquip(EquipType.Legs)]
    public class SilvaLeggingsold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Mods.CalamityInheritance.Content.Items.Armor";
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 39;
            Item.rare = ModContent.RarityType<DeepBlue>();
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.45f;
            player.GetDamage<GenericDamageClass>() += 0.12f;
            player.GetCritChance<GenericDamageClass>() += 7;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlantyMush>(9).
                AddIngredient<EffulgentFeather>(7).
                AddIngredient<AscendantSpiritEssence>(2).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
