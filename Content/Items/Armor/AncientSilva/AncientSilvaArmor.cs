using CalamityInheritance.Content.Items.Armor.Silva;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientSilva
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientSilvaArmor : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 60;
        }
        
      
        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 300;
            player.statManaMax2 += 300;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SilvaArmorold>().
                AddIngredient<EffulgentFeather>(50).
                AddIngredient<PlantyMush>(100).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}