using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeDevourerofGods : LoreItem, ILocalizedModType
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
            Item.rare = ModContent.RarityType<AbsoluteGreen>();
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.CalamityInheritance().DoGLore = true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DevourerofGodsTrophy>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
AddIngredient<LoreDevourerofGods>().
AddTile(TileID.Bookcases).
Register();
        }
    }
}
