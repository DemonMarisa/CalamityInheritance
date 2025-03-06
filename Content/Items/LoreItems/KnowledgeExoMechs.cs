using CalamityInheritance.Rarity;
using CalamityMod.Items.LoreItems;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeExoMechs : LoreItem, ILocalizedModType
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
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.CalamityInheritance().LoreExo = true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<LoreExoMechs>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddRecipeGroup("CalamityInheritance:AnyExoTropy").
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
