﻿using CalamityInheritance.Utilities;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;


namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgePerforators : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Orange;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.CalamityInheritance().perforatorLore = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PerforatorTrophy>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}