using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class BloodPactLegacy : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:26,
            itemHeight:26,
            itemRare:ItemRarityID.Yellow,
            itemValue:CIShopValue.RarityPriceYellow
        );
        public override void ExSSD() => Type.ShimmerEach<BloodPact>();

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.CIMod();
            modPlayer.AncientBloodPact= true;
        }
    }
}
