using CalamityInheritance.Content.BaseClass.Items;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgePerforators : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Orange;
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
            player.CI().AddEffect("KnowledgePerforators", () =>
            {
                if (player.ZoneCrimson)
                {
                    player.CI().EnemyMaxSpawnMult *= 1.8f;
                    player.CI().EnemySpawnRateMult *= 0.7f;
                    player.CI().AddHitAddBuff(BuffID.Ichor, 90);
                }
            });
        }
    }
}
