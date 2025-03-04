using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class BloodPactLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetStaticDefaults()
        {
            if (CIConfig.Instance.CustomShimmer == true) //微光嬗变config启用时，将会使原灾的血杯与这一速杀版本的血神核心微光相互转化
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BloodPact>()] = ModContent.ItemType<BloodPactLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BloodPactLegacy>()] = ModContent.ItemType<BloodPact>();
            }
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.CalamityInheritance();
            modPlayer.ancientBloodFact= true;
        }
    }
}
