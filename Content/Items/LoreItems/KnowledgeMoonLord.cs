using CalamityInheritance.Content.BaseClass.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeMoonLord : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Red;
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
            player.CI().AddEffect("KnowledgeMoonLord", () =>
            {
                if (player.gravDir == -1f && player.gravControl2)
                {
                    player.AddDR(0.05f);
                    player.statDefense += 10;
                    player.GetDamage(DamageClass.Generic) += 0.1f;
                    player.GetCritChance<GenericDamageClass>() += 10;
                    player.GetKnockback(DamageClass.Summon).Base += 1.5f;
                    player.moveSpeed += 0.15f;
                }
                else
                    player.slowFall = true;
            });
        }
    }
}
