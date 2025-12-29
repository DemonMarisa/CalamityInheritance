using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class ArcanumoftheVoid : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:26,
            itemHeight:26,
            itemRare: RarityType<BlueGreen>(),
            itemValue:CIShopValue.RarityPriceBlueGreen
        );
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer modPlayer1 = player.CIMod();
            modPlayer1.projRef = true;
        }
    }
}
