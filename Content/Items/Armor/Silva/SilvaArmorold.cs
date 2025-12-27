using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Placeables.Abyss;

namespace CalamityInheritance.Content.Items.Armor.Silva
{
    [AutoloadEquip(EquipType.Body)]
    public class SilvaArmorold : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 44;
            Item.rare = ModContent.RarityType<DeepBlue>();
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 80;
            player.GetDamage<GenericDamageClass>() += 0.12f;
            player.GetCritChance<GenericDamageClass>() += 8;
            player.moveSpeed += 0.20f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlantyMush>(12).
                AddIngredient<EffulgentFeather>(10).
                AddIngredient(ModContent.ItemType<DarksunFragment>(), 10).
                AddIngredient<LeadCore>().
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
