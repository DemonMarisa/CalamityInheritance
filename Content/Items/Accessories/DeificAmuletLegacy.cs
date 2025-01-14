using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class DeificAmuletLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Cyan;
            Item.value = CIShopValue.RarityPriceCyan;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer1 = player.CalamityInheritance();
            player.pStone = true;
            player.longInvince = true;
            modPlayer1.deificAmuletEffect = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.StarVeil).
                AddIngredient(ItemID.CharmofMyths).
                AddIngredient<AstralBar>(10).
                AddIngredient<SeaPrism>(15).
                AddTile(TileID.LunarCraftingStation).
                Register();
            
            CreateRecipe().
                AddIngredient(ItemID.CharmofMyths).
                AddIngredient<DeificAmulet>().
                Register();
        }
    }
}
