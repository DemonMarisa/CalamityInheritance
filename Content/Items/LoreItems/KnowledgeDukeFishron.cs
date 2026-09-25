using CalamityInheritance.Content.BaseClass.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeDukeFishron : LoreItem
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
            player.CI().AddEffect("KnowledgeDukeFishron", () =>
            {
                if (player.IsUnderwater())
                {
                    player.GetDamage(DamageClass.Generic) += 0.05f;
                    player.GetCritChance<GenericDamageClass>() += 5;
                    player.moveSpeed += 0.1f;
                }
                else
                {
                    player.GetDamage(DamageClass.Generic) -= 0.02f;
                    player.GetCritChance<GenericDamageClass>() -= 2;
                    player.moveSpeed -= 0.04f;
                }
            });
        }
    }
}
