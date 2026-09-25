using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using Terraria;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeDevourerofGods : LoreItem
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
            player.CI().AddEffect("KnowledgeDevourerofGods", () =>
            {
                player.BoostTrueMelee(0.5f);
            });
        }
    }
}
