using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeSulphurSea : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Pink;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AquaticScourgeTrophy>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
    AddIngredient<LoreSulphurSea>().
    AddTile(TileID.Bookcases).
    Register();
        }
    }
}
