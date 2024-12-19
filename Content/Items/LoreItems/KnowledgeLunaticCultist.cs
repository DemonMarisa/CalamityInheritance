using CalamityInheritance.Utilities;
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
    public class KnowledgeLunaticCultist : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Cyan;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (NPC.LunarApocalypseIsUp && Item.favorited)
            {
                player.CalamityInheritance().lunaticCultistLore = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.AncientCultistTrophy).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
