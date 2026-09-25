using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Professional
{
    public class PsychoticAmulet : CIAccessories
    {
        public override int AccessoriesStyle => Professional;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = ItemRarityID.Pink;
            Item.value = CIShopValue.RarityPricePink;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<ThrowingDamageClass>() += 0.05f;
            player.GetCritChance<ThrowingDamageClass>() += 5;
            player.GetDamage(DamageClass.Ranged) += 0.05f;
            player.GetCritChance(DamageClass.Ranged) += 5;
            if (player.CI().Stealth != 0)
            {
                player.GetDamage<ThrowingDamageClass>() += player.CI().Stealth * 0.5f;
                player.GetCritChance<ThrowingDamageClass>() += player.CI().Stealth * 25;
                player.GetDamage<RangedDamageClass>() += player.CI().Stealth * 0.5f;
                player.GetCritChance<RangedDamageClass>() += player.CI().Stealth * 25;
                player.aggro -= (int)(player.CI().Stealth * 750);
            }
        }
    }
}
