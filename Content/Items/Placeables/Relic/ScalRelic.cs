using CalamityInheritance.Rarity;
using CalamityInheritance.Tiles.Relic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Placeables.Relic
{
    public class ScalRelic : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Placeables.Relic";

        public override void SetDefaults()
        {
            // Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle aswell as setting a few values that are common across all placeable items
            Item.DefaultToPlaceableTile(ModContent.TileType<ScalRelicTiles>(), 0);

            Item.width = 40;
            Item.height = 66;
            Item.maxStack = 9999;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.master = true; // This makes sure that "Master" displays in the tooltip, as the rarity only changes the item name color
            Item.value = Item.buyPrice(0, 5,0 ,0);
        }
    }
}
