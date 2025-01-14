using CalamityMod;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeOcean : LoreItem
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
            Item.rare = ItemRarityID.Lime;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                if (player.Calamity().disableAnahitaSpawns == true)
                    player.Calamity().disableAnahitaSpawns = false;
                else
                    player.Calamity().disableAnahitaSpawns = true;
                state = player.Calamity().disableAnahitaSpawns;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<LeviathanTrophy>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddIngredient<AnahitaTrophy>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
AddIngredient<LoreAbyss>().
AddTile(TileID.Bookcases).
Register();
        }
    }
}
