using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.TarragonLegacy
{
    [AutoloadEquip(EquipType.Body)]
    public class TarragonBreastplateLegacy : CIArmor
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.defense = 37;
            Item.rare = ModContent.RarityType<BlueGreen>();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<GenericDamageClass>() += 0.12f;
            player.GetCritChance<GenericDamageClass>() += 8;
            player.LAP().LifeRegen += 4;
            player.LAP().MaxLifeAdditive += 40;
        }

        public override void AddRecipes()
        {
        }
    }
}
