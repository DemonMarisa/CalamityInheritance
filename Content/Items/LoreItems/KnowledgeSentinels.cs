using CalamityInheritance.Rarity;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeSentinels : LoreItem, ILocalizedModType
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
            Item.consumable = false;
            Item.rare = ModContent.RarityType<BlueGreen>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(CIRecipeGroup.LoreSentinal).
                DisableDecraft().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddRecipeGroup(CIRecipeGroup.TrophySentinal).
                DisableDecraft().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
