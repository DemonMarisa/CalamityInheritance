using CalamityInheritance.Content.Items.Armor.GodSlayerOld;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientGodSlayer
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientGodSlayerChestplate : CIArmor, ILocalizedModType
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
            Item.defense = 80;
        }
        

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.CIMod();
            modPlayer.GodSlayerReflect = true;
            player.thorns = 1f;
            player.statLifeMax2 += 500;
            player.statManaMax2 += 500;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GodSlayerChestplateold>().
                AddIngredient<CosmiliteBar>(40).
                AddIngredient<AscendantSpiritEssence>(15).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}