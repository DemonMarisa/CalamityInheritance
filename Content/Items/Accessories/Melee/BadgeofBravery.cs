﻿using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Melee
{
    public class BadgeofBravery : ModItem, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Items.Accessories.Melee";
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<BlueGreen>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer1 = player.CalamityInheritance();
            player.GetAttackSpeed<MeleeDamageClass>() += 0.15f;
            modPlayer1.BraveryBadgeLegacyStats = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.FeralClaws).
                AddIngredient(ItemID.Leather, 3).
                AddIngredient<UelibloomBar>(2).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
