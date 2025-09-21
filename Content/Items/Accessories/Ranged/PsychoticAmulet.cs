using CalamityMod.CalPlayer;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Accessories.Ranged
{
    public class PsychoticAmulet : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Ranged";
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:26,
            itemHeight:26,
            itemRare:ItemRarityID.Pink,
            itemValue:CIShopValue.RarityPricePink
        );
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer modPlayer = player.CIMod();
            modPlayer.PsychoticAmulet = true;
            player.shroomiteStealth = true;
            player.GetDamage<ThrowingDamageClass>() += 0.05f;
            player.GetCritChance<ThrowingDamageClass>() += 5;
            player.GetDamage(DamageClass.Ranged) += 0.05f;
            player.GetCritChance(DamageClass.Ranged) += 5;
        }
    }
}
