using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Items.Armor.AuricTesla;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Items.Armor.AncientTarragon;
using CalamityInheritance.Content.Items.Armor.AncientBloodflare;
using CalamityInheritance.Content.Items.Armor.AncientGodSlayer;
using CalamityInheritance.Content.Items.Armor.AncientSilva;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Armor.AncientAuric
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
            Item.defense = 20;
        }
        
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.5f;
			player.statLifeMax2 += 600;
            player.carpet = true;
        }

        public override void AddRecipes()
        {
            if (CIServerConfig.Instance.LegendaryitemsRecipes == true)
            {
                CreateRecipe().
                AddIngredient<AuricTeslaCuissesold>().
                AddIngredient<AncientTarragonLeggings>().
                AddIngredient<AncientBloodflareCuisses>().
                AddIngredient<AncientGodSlayerLeggings>().
                AddIngredient<AncientSilvaLeggings>().
                AddIngredient<AuricBarold>(15).
                AddTile<DraedonsForgeold>().
                Register();
            }
        }
    }
}