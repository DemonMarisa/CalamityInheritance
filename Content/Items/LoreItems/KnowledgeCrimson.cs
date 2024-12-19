using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeCrimson : LoreItem
    {
        public static bool state = false;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                if (player.Calamity().disablePerfCystSpawns == true)
                    player.Calamity().disablePerfCystSpawns = false;
                else
                    player.Calamity().disablePerfCystSpawns = true;
                state = player.Calamity().disablePerfCystSpawns;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.BrainofCthulhuTrophy).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
