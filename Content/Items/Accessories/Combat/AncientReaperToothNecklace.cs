using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Combat
{
    public class AncientReaperToothNecklace : CIAccessories
    {
        public override int AccessoriesStyle => Combat;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 26;
            Item.rare = RarityType<AbsoluteGreen>();
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetArmorPenetration<GenericDamageClass>() += 300;
            player.GetDamage<GenericDamageClass>() *= 1.20f;
            player.GetCritChance<GenericDamageClass>() += 50;
            player.endurance *= 0.01f;
            player.statDefense /= 100;
            if (player.lifeRegen > 0)
                player.lifeRegen /= 100;
        }
    }
}
