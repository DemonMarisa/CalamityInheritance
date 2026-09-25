using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.Buffs;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Defense
{
    public class TrinketofChiLegacy : CIAccessories, ILocalizedModType
    {
        public override int AccessoriesStyle => Defense;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 34;
            Item.height = 32;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.statLife >= (player.statLifeMax2 * 0.99f))
            {
                player.AddBuff(BuffType<TrinketofChiLegacyBuff>(), 2);
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ClayBlock, 15);
            recipe.Register();
        }
    }
}