using CalamityInheritance.Content.BaseClass.Items;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeSkeletron : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
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
            player.CI().AddEffect("KnowledgeSkeletron", () =>
            {
                if (player.ZoneDungeon)
                {
                    player.GetDamage<GenericDamageClass>() += 0.1f;
                    player.GetCritChance<GenericDamageClass>() += 5;
                    player.LAP().MaxLifeMinusMult *= 0.9f;
                }
            });
        }
    }
}
