using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Placeables.Abyss;

namespace CalamityInheritance.Content.Items.Armor.Silva
{
    [AutoloadEquip(EquipType.Legs)]
    public class SilvaLeggingsold : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 39;
            Item.rare = RarityType<DeepBlue>();
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.45f;
            player.GetDamage<GenericDamageClass>() += 0.12f;
            player.GetCritChance<GenericDamageClass>() += 7;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlantyMush>(9).
                AddIngredient<EffulgentFeather>(7).
                AddIngredient(ItemType<DarksunFragment>(), 10).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
