using CalamityInheritance.Content.BaseClass.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeTwins : LoreItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Pink;
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
            player.CI().AddEffect("KnowledgeTwins", () =>
            {
                if (!Main.dayTime)
                {
                    player.invis = true;
                    player.GetCritChance<GenericDamageClass>() += 5;
                    player.GetDamage<GenericDamageClass>() += 0.05f;
                }
                if (player.statLife >= (int)(player.statLifeMax2 * 0.5))
                    player.statDefense -= 10;
            });
        }
    }
}
