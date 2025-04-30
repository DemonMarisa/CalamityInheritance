using CalamityInheritance.Rarity;
using CalamityInheritance.Tiles.Relic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Placeables.Relic
{
    public class CalCloneRelic : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Placeables.Relic";

        public override void SetDefaults()
        {
            // Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle aswell as setting a few values that are common across all placeable items
            Item.DefaultToPlaceableTile(ModContent.TileType<CalCloneRelicTiles>(), 0);

            Item.width = 48;
            Item.height = 62;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Lime;
            Item.master = true; // This makes sure that "Master" displays in the tooltip, as the rarity only changes the item name color
            Item.value = Item.buyPrice(0, 5, 0, 0);
        }
    }
}
