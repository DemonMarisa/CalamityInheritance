using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeProvidence : LoreItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Lores";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.consumable = false;
            Item.rare = ModContent.RarityType<BlueGreen>();
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
            CreateRecipe().
AddIngredient<LoreProvidence>().
AddTile(TileID.Bookcases).
Register();
        }
    }
}
