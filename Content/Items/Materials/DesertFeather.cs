using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Materials
{
    public class DesertFeather : CIMaterials, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 15;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(copper: 15);
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Feather).
                AddIngredient(ItemID.SandBlock).
                DisableDecraft().
                Register();
        }
    }
}