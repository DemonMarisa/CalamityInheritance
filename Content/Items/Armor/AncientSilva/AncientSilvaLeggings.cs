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
    [AutoloadEquip(EquipType.Legs)]
    public class AncientSilvaLeggings : CIArmor, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare =ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 200;
            player.moveSpeed += 0.50f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SilvaLeggings>().
                AddIngredient<EffulgentFeather>(15).
                AddIngredient<PlantyMush>(15).
                AddIngredient<AscendantSpiritEssence>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}