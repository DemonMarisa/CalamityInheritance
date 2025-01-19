using CalamityInheritance.Rarity;
using CalamityInheritance.Tiles.Vanity;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.FurnitureExo;
using CalamityMod.Tiles.FurnitureExo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Placeables.Vanity
{
    public class BloodIdol : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Placeables.Vanity";
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
            Item.createTile = ModContent.TileType<BloodIdolTiles>();
            Item.rare = ModContent.RarityType<PureRed>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodOrb>(10).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
