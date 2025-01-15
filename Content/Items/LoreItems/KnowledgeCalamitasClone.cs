using CalamityInheritance.Utilities;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Microsoft.CodeAnalysis.Operations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeCalamitasClone : LoreItem, ILocalizedModType 
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
            Item.rare = ItemRarityID.Pink;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.CalamityInheritance().calamitasCloneLore = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CalamitasCloneTrophy>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
AddIngredient<LoreCalamitasClone>().
AddTile(TileID.Bookcases).
Register();
        }
    }
}
