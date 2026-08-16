using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Misc
{
    public class LeadCore : CIAccessories
    {
        public override int AccessoriesStyle => Misc;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 26;
            Item.height = 26;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.defense = 12;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffType<CIIrradiated>()] = true;
            if (CIUtils.HasCalamity())
                player.buffImmune[CalDeBuff.Irradiated] = true;
        }
    }
}
