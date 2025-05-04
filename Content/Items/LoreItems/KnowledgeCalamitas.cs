using CalamityInheritance.Utilities;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.LoreItems;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Placeables.Relic;

namespace CalamityInheritance.Content.Items.LoreItems
{
    
    public class KnowledgeCalamitas : LoreItem, ILocalizedModType
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
                player.CIMod(). SCalLore= true;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(CIRecipeGroup.SCalTrophy).
                DisableDecraft().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddRecipeGroup(CIRecipeGroup.LorePostSCal).
                DisableDecraft().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                Register();
        }
    }
}
