using CalamityInheritance.Content.BaseClass.Items;
using LAP.Core.SystemsLoader;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeDesertScourge : LoreItem
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
            player.CI().AddEffect("KnowledgeDesertScourge", () =>
            {
                if (player.ZoneDesert || player.CalPlayerInfo().ZoneSunkenSea)
                {
                    player.statDefense += 15;
                    player.GetDamage<GenericDamageClass>() -= 0.025f;
                }
            });
        }
    }
}
