using CalamityInheritance.Utilities;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeBrimstoneElemental : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Pink;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.CalamityInheritance().brimstoneElementalLore = true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BrimstoneElementalTrophy>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
