using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Materials
{
    public class AncientVictideBar : CIMaterials
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 60; // Meteorite
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Coral, 1).
                AddIngredient(ItemID.Starfish, 1).
                AddIngredient(ItemID.Seashell, 1).
                AddTile(TileID.Furnaces).
                Register();
        }
    }
}
