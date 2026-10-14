using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.BloodflareLegacy
{
    [AutoloadEquip(EquipType.Body)]
    public class BloodflareBodyLegacy : CIArmor
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.defense = 35;
            Item.rare = RarityType<AbsoluteGreen>();
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 40;
            player.GetDamage<GenericDamageClass>() += 0.12f;
            player.GetCritChance<GenericDamageClass>() += 8;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodstoneCore>(16).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
