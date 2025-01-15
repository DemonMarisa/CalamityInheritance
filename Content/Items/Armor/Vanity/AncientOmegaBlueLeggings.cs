using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.Vanity 
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientOmegaBlueLeggings: ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Vanity";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.vanity = true;
        }

                public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient<Necroplasm>(3).
            AddIngredient<ReaperTooth>(1).
            AddTile(TileID.LunarCraftingStation).
            Register();
        }
    }
}
