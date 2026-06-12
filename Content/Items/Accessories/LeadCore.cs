using CalamityMod.Buffs.StatDebuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class LeadCore : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth: 26,
            itemHeight: 26,
            itemRare: ItemRarityID.Pink,
            itemValue: CIShopValue.RarityPricePink
        );
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffType<Irradiated>()] = true;
        }
    }
}
