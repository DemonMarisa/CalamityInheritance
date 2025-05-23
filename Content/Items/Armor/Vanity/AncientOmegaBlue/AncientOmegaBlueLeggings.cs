﻿using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.Vanity.AncientOmegaBlue
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientOmegaBlueLeggings: CIArmor, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Vanity";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.vanity = true;
        }

                public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient<Necroplasm>(3).
            AddIngredient<ReaperTooth>(1).
            AddTile(TileID.LunarCraftingStation).
            Register();
        }
    }
}
