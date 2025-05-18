using CalamityInheritance.Rarity;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientGodSlayer
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientGodSlayerLeggings : CIArmor, ILocalizedModType
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
            Item.defense = 20;
        }
        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 200;
            player.moveSpeed += 0.50f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GodSlayerLeggings>().
                AddIngredient<CosmiliteBar>(20).
                AddIngredient<AscendantSpiritEssence>(10).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}