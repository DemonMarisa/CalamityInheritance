using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Rarity;

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

        // public override void AddRecipes()
        // {
        //     Recipe recipe = CreateRecipe();
        //     recipe.AddIngredient(null, "SilvaLeggings");
        //     recipe.AddIngredient(null, "GodSlayerLeggings");
        //     recipe.AddIngredient(null, "BloodflareCuisses");
        //     recipe.AddIngredient(null, "TarragonLeggings");
        //     recipe.AddIngredient(null, "EndothermicEnergy", 300);
        //     recipe.AddIngredient(null, "NightmareFuel", 300);
        //     recipe.AddIngredient(null, "Phantoplasm", 105);
        //     recipe.AddIngredient(null, "DarksunFragment", 45);
        //     recipe.AddIngredient(null, "BarofLife", 30);
        //     recipe.AddIngredient(null, "CoreofCalamity", 20);
        //     recipe.AddIngredient(null, "GalacticaSingularity", 15);
        //     recipe.AddIngredient(ItemID.FlyingCarpet);
        //     recipe.AddTile(null, "DraedonsForge");
        //     recipe.Register();
        // }
    }
}