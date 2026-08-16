using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Defense
{
    public class AstralBulwark : CIAccessories
    {
        public override int AccessoriesStyle => Defense;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 26;
            Item.rare = ItemRarityID.Cyan;
            Item.value = CIShopValue.RarityPriceCyan;
            Item.defense = 15;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().DeificAmuletFallenStar = true;
            if (HasCalamity())
                player.buffImmune[CalDeBuff.AstralInfectionDebuff] = true;
            player.buffImmune[BuffType<CIAstralInfection>()] = true;
        }
    }
}
