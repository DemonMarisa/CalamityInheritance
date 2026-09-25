using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Armor.ArmorItems.AncientTarragon;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Professional
{
    public class BadgeofBravery : CIAccessories
    {
        public override int AccessoriesStyle => Professional;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPriceBlueGreen;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetAttackSpeed<MeleeDamageClass>() += 0.15f;
            bool helm =
                player.armor[0].type == ItemType<AncientTarragonHelm>();
            bool chest =
                player.armor[1].type == ItemType<AncientTarragonBreastplate>();
            bool legs =
                player.armor[2].type == ItemType<AncientTarragonLeggings>();
            if (helm && chest && legs)
            {
                player.GetCritChance<MeleeDamageClass>() += 10;
                player.GetDamage<MeleeDamageClass>() += 0.10f;
                player.GetArmorPenetration<MeleeDamageClass>() += 15;
            }
        }
    }
}
