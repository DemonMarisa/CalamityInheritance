using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.BloodflareLegacy
{
    [AutoloadEquip(EquipType.Legs)]
    public class BloodflareLegLegacy : CIArmor
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.defense = 29;
            Item.rare = RarityType<AbsoluteGreen>();
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.17f;
            player.GetDamage<GenericDamageClass>() += 0.1f;
            player.GetCritChance<GenericDamageClass>() += 7;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodstoneCore>(13).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
