using CalamityInheritance.Content.Items.Armor.AncientAuric;
using CalamityInheritance.Content.Items.Armor.AncientBloodflare;
using CalamityInheritance.Content.Items.Armor.AncientGodSlayer;
using CalamityInheritance.Content.Items.Armor.AncientSilva;
using CalamityInheritance.Content.Items.Armor.AncientTarragon;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.SummonItems;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.MiscItem
{
    public class YharonEssence: CIMisc, ILocalizedModType 
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.rare = CIConfig.Instance.SpecialRarityColor? ModContent.RarityType<YharonFire>() : ModContent.RarityType<PureRed>();
            //有意为之
            Item.value = CIShopValue.RarityPricePureRed;
            Item.consumable = true;
            Item.maxStack = 9999;
        }
        public override bool CanRightClick() => true;
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<YharimAuricTeslaHelm>(), 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<YharimAuricTeslaBodyArmor>(), 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<YharimAuricTeslaCuisses>(), 1));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<YharonEggLegacy>()));
        }
        public override void AddRecipes()
        {
            if (CIServerConfig.Instance.LegendaryitemsRecipes)
                CreateRecipe().
                    AddIngredient<AncientTarragonHelm>().
                    AddIngredient<AncientTarragonBreastplate>().
                    AddIngredient<AncientTarragonLeggings>().
                    AddIngredient<AncientBloodflareMask>().
                    AddIngredient<AncientBloodflareBodyArmor>().
                    AddIngredient<AncientBloodflareCuisses>().
                    AddIngredient<AncientSilvaHelm>().
                    AddIngredient<AncientSilvaArmor>().
                    AddIngredient<AncientSilvaLeggings>().
                    AddIngredient<AncientGodSlayerHelm>().
                    AddIngredient<AncientGodSlayerChestplate>().
                    AddIngredient<AncientGodSlayerLeggings>().
                    AddIngredient<AuricBarold>(15).
                    AddIngredient<CalamitousEssence>(15).
                    DisableDecraft().
                    AddRecipeGroup(CIRecipeGroup.DragonGift).
                    AddIngredient<YharonSoulFragment>(15).
                    AddTile<DemonshadeTile>().
                    Register();
        }
    }
}