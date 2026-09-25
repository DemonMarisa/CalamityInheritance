using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgePolterghast : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.consumable = false;
            Item.rare = RarityType<AbsoluteGreen>();
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
            player.CI().AddEffect("KnowledgePolterghast", () =>
            {
                player.CI().ExGrabRange += 300;
                player.CI().ExShootSpeed += 0.15f;
            });
        }
    }
}
