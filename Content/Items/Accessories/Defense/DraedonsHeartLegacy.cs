using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.DataStructures;

namespace CalamityInheritance.Content.Items.Accessories.Defense
{
    public class DraedonsHeartLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Defense;
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 7));
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 26;
            Item.rare = RarityType<PureRed>();
            Item.value = CIShopValue.RarityPricePureRed;
            Item.defense = 48;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.LAP().LifeRegen += 16;
            player.AddDR(0.5f);
            player.CI().FinalDefenseMult += 0.35f;
            player.CI().BlockDefenseDamage = true;
        }
    }
}