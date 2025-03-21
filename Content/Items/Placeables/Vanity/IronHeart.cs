using CalamityInheritance.Tiles.Vanity;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Placeables.Vanity
{
    public class IronHeart : CIPlaceable, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Placeables.Vanity";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<IronHeartTiles>();
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
