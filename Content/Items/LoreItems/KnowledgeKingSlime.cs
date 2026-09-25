using CalamityInheritance.Content.BaseClass.Items;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeKingSlime : LoreItem
    {
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
                AddEffect(player);
            }
        }
        public static void AddEffect(Player player)
        {
            player.CI().AddEffect("KnowledgeKingSlime", () =>
            {
                player.moveSpeed += 0.05f;
                player.jumpSpeedBoost += 0.1f;
                player.statDefense -= 3;
            });
        }
    }
}
