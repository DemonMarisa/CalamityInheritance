using CalamityInheritance.Utilities;
using CalamityMod.Items.Placeables.Furniture.Trophies;
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
    public class KnowledgePlaguebringerGoliath : LoreItem
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
                player.CalamityInheritance().plaguebringerGoliathLore = true;
                player.lifeRegen -= 8;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlaguebringerGoliathTrophy>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
