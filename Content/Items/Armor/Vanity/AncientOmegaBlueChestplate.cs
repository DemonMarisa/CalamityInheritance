using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientOmegaBlueChestplate : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.Vanity";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.value = Item.buyPrice(0, 75, 0, 0);
            Item.vanity = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient<Necroplasm>(5).
            AddIngredient<ReaperTooth>(2).
            AddTile(TileID.LunarCraftingStation).
            Register();
        }
    }
}
