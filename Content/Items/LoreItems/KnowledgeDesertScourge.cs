using CalamityInheritance.Utilities;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeDesertScourge : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.CalamityInheritance().desertScourgeLore = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DesertScourgeTrophy>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
AddIngredient<LoreDesertScourge>().
AddTile(TileID.Bookcases).
Register();
        }
    }
}
