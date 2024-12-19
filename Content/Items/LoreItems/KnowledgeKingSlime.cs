using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.LoreItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeKingSlime : LoreItem
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
                player.CalamityInheritance().kingSlimeLore = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.KingSlimeTrophy).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
