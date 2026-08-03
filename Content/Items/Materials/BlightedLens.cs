using CalamityInheritance.Content.BaseClass;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Materials
{
    public class BlightedLens : CIMaterials
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 22;
            Item.value = Item.sellPrice(silver: 56);
            Item.rare = ItemRarityID.Pink;
            Item.maxStack = 9999;
        }
    }
}
