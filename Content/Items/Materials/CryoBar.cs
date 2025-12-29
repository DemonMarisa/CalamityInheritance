using CalamityInheritance.Tiles.Bars;
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
            Item.DefaultToPlaceableTile(TileType<CryoBarTile>());
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
        }
    }
}
