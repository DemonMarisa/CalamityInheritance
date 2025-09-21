using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Accessories;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class ShrineMushroom: CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:38,
            itemHeight:36,
            itemRare:ItemRarityID.Orange,
            itemValue:CIShopValue.RarityPriceOrange
        );
        public override void ExSSD() => Type.ShimmerEach<FungalSymbiote>();
        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.GetDamage<TrueMeleeDamageClass>() += 0.25f;
            player.CIMod().SMushroom = true;
        }
    }
}