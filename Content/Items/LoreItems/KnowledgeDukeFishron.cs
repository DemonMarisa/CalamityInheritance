using CalamityInheritance.Utilities;
using CalamityMod.Items.LoreItems;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeDukeFishron : LoreItem
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
                player.CalamityInheritance().dukeFishronLore = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.DukeFishronTrophy).
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
AddIngredient<LoreDukeFishron>().
AddTile(TileID.Bookcases).
Register();
        }
    }
}
