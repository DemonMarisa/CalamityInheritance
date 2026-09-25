using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using Terraria;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeExoMechs : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.consumable = false;
            Item.rare = RarityType<CatalystViolet>();
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.CI().LoreExo = true;
            }
        }
    }
}
