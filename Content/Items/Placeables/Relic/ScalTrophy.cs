using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Tiles.Relic;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Placeables.Relic
{
    public class ScalTrophy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Placeables.Relic";
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = CIShopValue.RarityPricePureRed;
            Item.rare = RarityType<PureRed>();
            Item.createTile = TileType<ScalTrophyTiles>();
        }
    }
}
