using Terraria.ID;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod.Items.LoreItems;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeTwins : LoreItem, ILocalizedModType
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
            Item.rare = ItemRarityID.Pink;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.CalamityInheritance().twinsLore = true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.RetinazerTrophy).
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddIngredient(ItemID.SpazmatismTrophy).
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
AddIngredient<LoreTwins>().
AddTile(TileID.Bookcases).
Register();
        }
    }
}
