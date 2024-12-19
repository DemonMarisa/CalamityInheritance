using CalamityInheritance.Utilities;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Rarities;
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
    public class KnowledgeProvidence : LoreItem
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
            Item.rare = ModContent.RarityType<Turquoise>();
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.CalamityInheritance().providenceLore = true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ProvidenceTrophy>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
