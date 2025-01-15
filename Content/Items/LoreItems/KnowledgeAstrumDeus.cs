using CalamityInheritance.Utilities;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeAstrumDeus : LoreItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Mods.CalamityInheritance.Content.Items.Lores";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Cyan;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.CalamityInheritance().astrumDeusLore = true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AstrumDeusTrophy>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
    AddIngredient<LoreAstrumDeus>().
    AddTile(TileID.Bookcases).
    Register();
        }
    }
}
