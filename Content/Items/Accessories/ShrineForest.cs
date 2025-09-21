using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class ShrineForest: CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:34,
            itemHeight:32,
            itemRare:ItemRarityID.Orange,
            itemValue:CIShopValue.RarityPriceOrange
        );
        public override void ExSSD() => Type.ShimmerEach<TrinketofChi>();
        public override void UpdateAccessory(Player player, bool hideVisual) => player.CIMod().SForest = true;
    }
}