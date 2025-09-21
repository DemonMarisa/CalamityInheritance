using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class ShrineMarnite: CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:28,
            itemHeight:36,
            itemRare:ItemRarityID.Orange,
            itemValue:CIShopValue.RarityPriceOrange
        );
        public override void ExSSD() => Type.ShimmerEach<UnstableGraniteCore>();
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 36;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) => player.CIMod().SMarnite = true;
    }
}