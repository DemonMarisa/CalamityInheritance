using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeSentinels : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.consumable = false;
            Item.rare = RarityType<BlueGreen>();
        }
    }
}
