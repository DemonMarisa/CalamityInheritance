using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Armor.AuricTesla;
using CalamityInheritance.Content.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Tiles.Furniture.CraftingStations;

namespace CalamityInheritance.Content.Items.Armor.YharimAuric
{
    [AutoloadEquip(EquipType.Legs)]
    public class YharimAuricTeslaCuisses : ModItem, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Items.Armor";

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityMaliceDrop;
            Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
            Item.defense = 44;
        }
        
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.5f;
            player.carpet = true;
        }

        public override void AddRecipes()
        {
            if (CalamityInheritanceConfig.Instance.LegendaryitemsRecipes == true)
            {
                CreateRecipe().
                AddIngredient<AuricTeslaCuissesold>().
                AddIngredient<AuricBarold>(15).
                AddTile<DraedonsForgeold>().
                Register();
            }
        }
    }
}