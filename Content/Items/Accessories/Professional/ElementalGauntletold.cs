using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Professional
{

    [AutoloadEquip([EquipType.HandsOn, EquipType.HandsOff])]
    public class ElementalGauntletold : CIAccessories
    {
        public override int AccessoriesStyle => Professional;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 30;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().ElemGauntlet = true;
            player.LAP().MaxLifeMultiplier += 0.1f;
            player.LAP().ExImmuneTime += 40;
            player.lavaMax = 600;
            player.GetDamage<MeleeDamageClass>() += 0.30f;
            player.GetCritChance<MeleeDamageClass>() += 15;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.15f;
            player.kbGlove = true;
            player.autoReuseGlove = true;
            player.meleeScaleGlove = true;
            player.BoostTrueMelee(0.15f);
        }

        public override void AddRecipes()
        {
        }
    }
}
