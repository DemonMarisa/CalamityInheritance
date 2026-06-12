using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Items.Accessories
{
    public class FungalCarapace : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth: 20,
            itemHeight: 24,
            itemRare: ItemRarityID.Green,
            itemValue: CIShopValue.RarityPriceGreen,
            itemDefense: 6
        );
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer modPlayer = player.CIMod();
            modPlayer.FungalCarapace = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GlowingMushroom, 15);
            recipe.Register();
        }
    }
}
