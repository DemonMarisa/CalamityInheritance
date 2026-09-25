using CalamityInheritance.Content.BaseClass.Items;
using System;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeHiveMind : LoreItem
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
            player.CI().AddEffect("KnowledgeHiveMind", () =>
            {
                if (player.ZoneCorrupt)
                {
                    player.CI().EnemyMaxSpawnMult *= 0.6f;
                    player.CI().EnemySpawnRateMult *= 1.3f;
                    player.CI().AddHitAddBuff(BuffID.CursedInferno, 90);
                }
            });
        }
    }
}
