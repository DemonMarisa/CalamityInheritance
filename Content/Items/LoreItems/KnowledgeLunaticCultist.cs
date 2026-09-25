using CalamityInheritance.Content.BaseClass.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeLunaticCultist : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Cyan;
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
            player.CI().AddEffect("KnowledgeLunaticCultist", () =>
            {
                if (NPC.LunarApocalypseIsUp)
                {
                    player.blind = true;
                    player.AddDR(0.05f);
                    player.statDefense += 6;
                    player.GetDamage(DamageClass.Generic) += 0.04f;
                    player.GetCritChance<GenericDamageClass>() += 4;
                    player.GetKnockback(DamageClass.Summon).Base += 0.5f;
                    player.moveSpeed += 0.1f;
                }
            });
        }
    }
}
