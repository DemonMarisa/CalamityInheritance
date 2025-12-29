using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class AstralBulwark : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:26,
            itemHeight:26,
            itemRare:ItemRarityID.Cyan,
            itemValue:CIShopValue.RarityPriceCyan
        );
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            player.buffImmune[BuffType<AstralInfectionDebuff>()] = true;
            modPlayer.hideOfDeus = true;
        }
    }
}
