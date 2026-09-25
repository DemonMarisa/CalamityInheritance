using CalamityInheritance.Content.BaseClass.Items;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgePlantera : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.LightPurple;
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
            player.CI().AddEffect("KnowledgePlantera", () =>
            {
                player.CI().ExGrabRange += 150;
                player.CI().ExShootSpeed += 0.1f;
                if (player.statLife >= (int)(player.statLifeMax2 * 0.5))
                {
                    player.GetDamage<GenericDamageClass>() -= 0.05f;
                    player.statDefense += 15;
                }
                if (player.statLife <= (int)(player.statLifeMax2 * 0.5))
                {
                    player.GetDamage<GenericDamageClass>() += 0.1f;
                    player.statDefense -= 10;
                }
            });
        }
    }
}
