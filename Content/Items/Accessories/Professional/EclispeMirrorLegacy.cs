using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Professional
{
    public class EclispeMirrorLegacy : CIAccessories
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
            player.CI().EclispeMirror = true;
            player.GetCritChance<ThrowingDamageClass>() += 50;
            player.LAP().MaxFocusAdd += 30;
            player.SetRogueArmor(0.3f, true);
            player.LAP().FocusCost *= 0.75f;
            int change = (int)player.GetCritChance<RogueDamage>();
            change = change - 100;
            if (change > 0)
                player.CI().CritDamageAdd += change / 7;
        }
    }
}