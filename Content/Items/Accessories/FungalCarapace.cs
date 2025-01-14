using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Items;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
namespace CalamityInheritance.Content.Items.Accessories
{
    public class FungalCarapace : ModItem
    {
        public override void SetDefaults()
        {
            Item.defense = 6;
            Item.width = 20;
            Item.height = 24;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();
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
