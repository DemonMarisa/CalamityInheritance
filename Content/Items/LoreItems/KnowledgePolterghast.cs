﻿using CalamityInheritance.Utilities;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgePolterghast : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.consumable = false;
            Item.rare = ModContent.RarityType<PureGreen>();
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.CalamityInheritance().polterghastLore = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PolterghastTrophy>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}