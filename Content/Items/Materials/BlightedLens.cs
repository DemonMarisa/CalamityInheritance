using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Materials
{
    public class BlightedLens : CIMaterials, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Materials";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
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
