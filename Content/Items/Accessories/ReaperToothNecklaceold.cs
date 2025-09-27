using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class ReaperToothNecklaceold : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:44,
            itemHeight:50,
            itemRare:ModContent.RarityType<AbsoluteGreen>(),
            itemValue:CIShopValue.RarityPriceAbsoluteGreen
        );

        public override bool CanEquipAccessory(Player player, int slot, bool modded) => !player.CIMod().ReaperToothNecklaceEquipper;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<GenericDamageClass>() += 0.20f;
            player.GetArmorPenetration<GenericDamageClass>() += 100;
            player.CIMod().ReaperToothNecklaceLegacyEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SandSharkToothNecklace>().
                AddIngredient<ReaperTooth>(6).
                AddIngredient<DepthCells>(15).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }
}
