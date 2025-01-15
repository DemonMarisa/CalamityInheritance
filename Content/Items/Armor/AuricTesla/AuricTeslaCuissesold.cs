using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Armor.AuricTesla
{
    [AutoloadEquip(EquipType.Legs)]
    public class AuricTeslaCuissesold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Mods.CalamityInheritance.Content.Items.Armor";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.defense = 44;
            Item.rare = ModContent.RarityType<CatalystViolet>();
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
                AddIngredient<GodSlayerLeggings>().
                AddIngredient<SilvaLeggings>().
                AddIngredient<BloodflareCuisses>().
                AddIngredient<TarragonLeggings>().
                AddIngredient(ItemID.FlyingCarpet).
                AddIngredient<AuricBarold>(2).
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient<GodSlayerLeggings>().
                AddIngredient<SilvaLeggings>().
                AddIngredient<BloodflareCuisses>().
                AddIngredient<TarragonLeggings>().
                AddIngredient(ItemID.FlyingCarpet).
                AddIngredient<AuricBar>(15).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
