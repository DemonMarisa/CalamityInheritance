using CalamityInheritance.Utilities;
using CalamityMod.Items.LoreItems;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeMoonLord : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Red;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.CalamityInheritance().moonLordLore = true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MoonLordTrophy).
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
AddIngredient<LoreRequiem>().
AddTile(TileID.Bookcases).
Register();
        }
    }
}
