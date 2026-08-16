using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Combat
{
    public class ReaperToothNecklaceold : CIAccessories
    {
        public override int AccessoriesStyle => Combat;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 44;
            Item.height = 50;
            Item.rare = RarityType<AbsoluteGreen>();
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<GenericDamageClass>() += 0.20f;
            player.GetArmorPenetration<GenericDamageClass>() += 100;
        }

        public override void AddRecipes()
        {
            if (HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.ReaperTooth, 6).
                    AddIngredient(CalamityMaterials.DepthCells, 15).
                    AddTile(TileID.TinkerersWorkbench).
                    Register();
            }
            else
            {

            }
        }
    }
}
