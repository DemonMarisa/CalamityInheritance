using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Professional
{
    public class AncientEtherealTalisman : CIAccessories
    {
        public override int AccessoriesStyle => Professional;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.manaMagnet = true;
            if (!hideVisual)
                player.manaFlower = true;
            player.LAP().MaxManaAdditive += 250;
            player.GetDamage<MagicDamageClass>() += 0.30f;
            player.manaCost *= 0.8f;
            player.GetCritChance<MagicDamageClass>() += 30;
            player.pStone = true;
            player.LAP().LifeRegen += 6;
        }
    }
}
