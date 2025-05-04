using CalamityMod.Items.LoreItems;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeMechs : LoreItem, ILocalizedModType
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
                AddRecipeGroup(CIRecipeGroup.TrophyMechs).
                DisableDecraft().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddIngredient<LoreMechs>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
