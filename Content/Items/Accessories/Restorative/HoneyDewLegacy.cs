using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Restorative
{
    public class HoneyDewLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Restorative;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 36;
            Item.height = 30;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.ZoneJungle)
            {
                player.statDefense += 9;
                player.AddDR(0.05f);
                player.CI().LifeRegen += 2;
            }
            player.CI().BeeFriendly = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Venom] = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.BottledHoney, 10).
                AddIngredient(ItemID.BeeWax, 3).
                AddIngredient(ItemID.JungleSpores, 6).
                Register();
        }
    }
}
