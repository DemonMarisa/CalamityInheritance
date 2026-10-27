using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Armor.ArmorItems.BloodflareLegacy;
using CalamityInheritance.Content.Items.Armor.ArmorItems.GodSlayerOld;
using CalamityInheritance.Content.Items.Armor.ArmorItems.Silva;
using CalamityInheritance.Content.Items.Armor.ArmorItems.TarragonLegacy;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AuricTeslaLegacy
{
    [AutoloadEquip(EquipType.Legs)]
    public class AuricTeslaCuissesold : CIArmor
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.defense = 44;
            Item.rare = RarityType<CatalystViolet>();
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.5f;
            player.carpet = true;
            player.GetDamage<GenericDamageClass>() += 0.12f;
            player.GetCritChance<GenericDamageClass>() += 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GodSlayerLeggingsold>().
                AddIngredient<SilvaLeggingsold>().
                AddIngredient<BloodflareLegLegacy>().
                AddIngredient<TarragonLeglegacy>().
                AddIngredient(ItemID.FlyingCarpet).
                AddIngredient<AuricBarold>(2).
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
