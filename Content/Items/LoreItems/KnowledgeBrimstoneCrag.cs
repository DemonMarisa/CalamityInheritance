using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeBrimstoneCrag : LoreItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Lores";
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
                AddIngredient<BrimstoneElementalTrophy>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
AddIngredient<LoreAzafure>().
AddTile(TileID.Bookcases).
Register();
        }
    }
}
