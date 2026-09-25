using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeProvidence : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.consumable = false;
            Item.rare = RarityType<BlueGreen>();
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
            player.CI().AddEffect("KnowledgeProvidence", () =>
            {
                player.LAP().MaxLifeMinusMult *= 0.85f;
                player.GetDamage<GenericDamageClass>() *= 1.05f;
                if (HasCalamity())
                    player.buffImmune[CalDeBuff.IcarusFolly] = true;
            });
        }
    }

}