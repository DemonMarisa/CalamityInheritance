using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.Bars;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Materials
{
    public class CryoBar : CIMaterials
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(TileType<CryoBarTile>());
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
        }
    }
}
