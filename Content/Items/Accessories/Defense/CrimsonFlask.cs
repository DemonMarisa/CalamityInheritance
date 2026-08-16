using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Defense
{
    public class CrimsonFlask : CIAccessories
    {
        public override int AccessoriesStyle => Defense;
        public override void SetDefaults()
        {
            Item.defense = 6;
            Item.accessory = true;
            Item.width = 32;
            Item.height = 48;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddDR(0.04f);
            player.buffImmune[BuffID.Ichor] = true;
            if (CIUtils.HasCalamity())
                player.buffImmune[CalDeBuff.BurningBlood] = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ViciousPowder, 15).
                AddIngredient(ItemID.Vertebrae, 10).
                Register();
        }
    }
}
