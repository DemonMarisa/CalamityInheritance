using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Defense
{
    public class CorruptFlask : CIAccessories
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
            player.buffImmune[BuffID.CursedInferno] = true;
            if (CIUtils.HasCalamity())
                player.buffImmune[CalDeBuff.BrainRot] = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.VilePowder, 15).
                AddIngredient(ItemID.RottenChunk, 10).
                Register();
        }
    }
}
