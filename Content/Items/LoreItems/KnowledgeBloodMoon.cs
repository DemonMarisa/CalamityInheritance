using CalamityMod.Items.LoreItems;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeBloodMoon : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.LightRed;
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.BloodMoonStarter).
                AddIngredient(ItemID.SoulofNight, 3).
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
AddIngredient<LoreBloodMoon>().
AddTile(TileID.Bookcases).
Register();
        }
    }
}
