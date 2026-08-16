using CalamityInheritance.Content.BaseClass;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Materials
{
    public class WulfrumShard : CIMaterials
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }
        public override void SetDefaults()
        {
            Item.width = 13;
            Item.height = 10;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(copper: 80);
            Item.rare = ItemRarityID.Blue;
        }
    }
}
