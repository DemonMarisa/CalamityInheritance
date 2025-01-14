using CalamityInheritance.Utilities;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.LoreItems;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeCalamitas : LoreItem
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
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.CalamityInheritance(). SCalLore= true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SupremeCalamitasTrophy>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddIngredient<LoreCalamitas>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddIngredient<LoreCynosure>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
