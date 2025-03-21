using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Armor.Vanity.AncientAuricArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientAuricTeslaCuisses : CIArmor, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Vanity";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 14;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.vanity = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
