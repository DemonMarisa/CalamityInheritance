using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Materials
{
    public class CryoBar : CIMaterials, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Materials";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Lime;
        }
    }
}
