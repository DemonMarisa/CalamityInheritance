using Terraria;
using Terraria.ID;
using CalamityInheritance.Utilities;
using CalamityMod.Items.LoreItems;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeSkeletron : LoreItem, ILocalizedModType
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
            Item.rare = ItemRarityID.Green;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (player.ZoneDungeon && Item.favorited)
            {
                player.CalamityInheritance().LoreSkeletron = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SkeletronTrophy).
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
AddIngredient<LoreSkeletron>().
AddTile(TileID.Bookcases).
Register();
        }
    }
}
