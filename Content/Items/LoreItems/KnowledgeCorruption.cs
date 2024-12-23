using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.LoreItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeCorruption : LoreItem
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
                if (player.Calamity().disableHiveCystSpawns == true)
                    player.Calamity().disableHiveCystSpawns = false;
                else
                    player.Calamity().disableHiveCystSpawns = true;
                state = player.Calamity().disableHiveCystSpawns;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.EaterofWorldsTrophy).
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
AddIngredient<LoreCorruption>().
AddTile(TileID.Bookcases).
Register();
        }
    }
}
