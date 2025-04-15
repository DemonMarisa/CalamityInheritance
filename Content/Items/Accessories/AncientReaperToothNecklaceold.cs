using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class AncientReaperToothNecklace : CIAccessories, ILocalizedModType
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

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var usPlayer = player.CIMod();
            usPlayer.SpeedrunNecklace = true;
        }
        public override void AddRecipes()
        {
            //微光嬗变config关闭时使用这个合成表
            if(CIServerConfig.Instance.CustomShimmer == false)
            {
                CreateRecipe().
                    AddIngredient<ReaperTooth>(10).
                    AddIngredient<DepthCells>(10).
                    AddTile<CosmicAnvil>().
                    Register();
            }
        }
    }
}
