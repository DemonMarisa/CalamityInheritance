using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Rarity;
using System.Runtime.CompilerServices;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class ReaperToothNecklaceold : CIAccessories, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 50;
            Item.accessory = true;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = ModContent.RarityType<AbsoluteGreen>();
        }
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
