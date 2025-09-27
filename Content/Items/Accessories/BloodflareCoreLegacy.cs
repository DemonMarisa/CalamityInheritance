using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class BloodflareCoreLegacy : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:26,
            itemHeight:26,
            itemRare:ModContent.RarityType<BlueGreen>(),
            itemValue:CIShopValue.RarityPriceBlueGreen
        );
        public override void ExSSD() => Type.ShimmerEach<BloodflareCore>();
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.statLife <= (int)(player.statLifeMax2 * 0.5f))
            {
                player.endurance += 0.05f;
                player.GetDamage<GenericDamageClass>() += 0.1f;
                if (player.statLife <= (int)(player.statLifeMax2 * 0.15f))
                {
                    player.endurance += 0.10f;
                    player.GetDamage<GenericDamageClass>() += 0.20f; ;
                }
            }
            
            if (player.statDefense <= 100)
                player.GetDamage<GenericDamageClass>() += 0.15f;
        }
    }
}
