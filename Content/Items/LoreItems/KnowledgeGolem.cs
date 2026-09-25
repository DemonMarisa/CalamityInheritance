using CalamityInheritance.Content.BaseClass.Items;
using System;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems;
public class KnowledgeGolem : LoreItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.rare = ItemRarityID.Yellow;
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
        player.CI().AddEffect("KnowledgeGolem", () =>
        {
            if (Math.Abs(player.velocity.X) < 0.05f && Math.Abs(player.velocity.Y) < 0.05f)
                player.statDefense += 30;
            else
                player.statDefense += 10;
        });
    }
}
