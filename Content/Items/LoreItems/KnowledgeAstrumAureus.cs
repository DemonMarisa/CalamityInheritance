﻿using CalamityInheritance.Utilities;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeAstrumAureus : LoreItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Lores";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Lime;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.CIMod().LoreAureus = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AstrumAureusTrophy>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddIngredient<LoreAstrumAureus>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
