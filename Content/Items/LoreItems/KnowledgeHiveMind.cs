﻿using CalamityInheritance.Utilities;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeHiveMind : LoreItem, ILocalizedModType
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
            Item.rare = ItemRarityID.Orange;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (player.ZoneCorrupt && Item.favorited)
            {
                player.CIMod().LoreHive = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<HiveMindTrophy>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddIngredient<LoreHiveMind>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
