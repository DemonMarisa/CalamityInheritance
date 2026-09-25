using CalamityInheritance.Content.BaseClass.Items;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeSlimeGod : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.LightRed;
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
            player.CI().AddEffect("KnowledgeSlimeGod", () =>
            {
                if (player.dashDelay < 0)
                    player.velocity.X *= 0.9f;
                player.slippy2 = true;
                if (Main.myPlayer == player.whoAmI)
                    player.AddBuff(BuffID.Slimed, 2);
                player.statDefense -= 10;
            });
        }
    }
}
