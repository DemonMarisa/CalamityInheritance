using CalamityInheritance.Content.BaseClass.Items;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeRavager : LoreItem
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
            player.CI().AddEffect("KnowledgeRavager", () =>
            {
                player.LAP().PostWingTimeMaxMult -= 0.5f;
                player.GetDamage<GenericDamageClass>() += 0.2f;
                player.ClearBuff(BuffID.Featherfall);
            });
        }
    }
}
