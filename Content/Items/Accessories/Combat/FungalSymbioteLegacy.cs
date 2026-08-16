using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Combat
{
    public class FungalSymbioteLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Combat;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 38;
            Item.height = 36;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<TrueMelee>() += 0.25f;
            player.CI().FungalSymbiote = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GlowingMushroom, 15);
            recipe.Register();
        }
    }
}