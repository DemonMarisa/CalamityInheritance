using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.Buffs.PotionBuff;
using CalamityInheritance.Content.Rarity;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeYharon : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.consumable = false;
            Item.rare = RarityType<DeepBlue>();
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
            player.CI().AddEffect("KnowledgeYharon", () =>
            {
                player.LAP().InfiniteFlight = true;
                player.wingAccRunSpeed += 0.2f;
                player.accRunSpeed += 0.2f;

                if (!player.CI().DraconicSurge)
                    player.GetDamage<GenericDamageClass>() -= 0.25f;
            });
        }
    }
}
