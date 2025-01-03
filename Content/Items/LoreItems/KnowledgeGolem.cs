﻿using CalamityInheritance.Utilities;
using CalamityMod.Items.LoreItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems;
    public class KnowledgeGolem : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Yellow;
            Item.consumable = false;
        }
    public override void UpdateInventory(Player player)
    {
        if (Item.favorited)
        {
            player.CalamityInheritance().golemLore = true;
        }
    }
    public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.GolemTrophy).
                AddTile(TileID.Bookcases).
                Register();
        CreateRecipe().
            AddIngredient<LoreGolem>().
            AddTile(TileID.Bookcases).
            Register();
    }
    }
