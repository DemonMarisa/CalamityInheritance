using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using LAP.Content.RecipeGroupAdd;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Combats
{
    public class LuxorsGiftLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 48;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CIMod().LuxorsGiftLegacy = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Ruby, 1).
                AddRecipeGroup(LAPRecipeGroup.AnyGoldBar, 10).
                AddIngredient(ItemID.HellstoneBar, 5).
                AddIngredient(ItemID.Bone, 5).
                AddIngredient(ItemID.JungleSpores, 5).
                AddIngredient(ItemID.BeeWax, 5).
                AddIngredient<SulphuricScale>(5).
                DisableDecraft().
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
