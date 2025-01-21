using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Accessories;
using Terraria.ID;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class AncientCotBG: ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";

        public override void SetStaticDefaults()
        {
            if(CalamityInheritanceConfig.Instance.CustomShimmer == true) //微光嬗变config启用时，将会使原灾的血杯与这一速杀版本的血神核心微光相互转化
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<ChaliceOfTheBloodGod>()] = ModContent.ItemType<AncientCotBG>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AncientCotBG>()] = ModContent.ItemType<ChaliceOfTheBloodGod>();
            }
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 48;
            Item.accessory = true;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = ModContent.RarityType<DeepBlue>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();
            usPlayer.ancientCoreofTheBloodGod = true;
        }

        public override void AddRecipes()
        {   
            //微光嬗变config关闭时使用这个合成表
            if(CalamityInheritanceConfig.Instance.CustomShimmer == false)
            {
                CreateRecipe().
                    AddIngredient<BloodPact>().
                    AddIngredient<BloodflareCore>().
                    AddIngredient<BloodyWormScarf>().
                    AddIngredient<BloodstoneCore>(5).
                    AddIngredient<AscendantSpiritEssence>(1).
                    AddTile<CosmicAnvil>().
                    Register();

            }
        }

    }
}
