using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items
{
    public class CIGlobalShimmer: GlobalItem
    {
        public override void SetStaticDefaults()
        {
            #region 微光嬗变启用时才会转化的
            if(CIConfig.Instance.CustomShimmer == true)
            {
                #region 魔君的馈赠
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<YharimsGift>()] = ModContent.ItemType<YharimsGiftLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<YharimsGiftLegacy>()] = ModContent.ItemType<YharimsGift>();
                #endregion

                #region 元素的心脏
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<HeartoftheElements>()] = ModContent.ItemType<WaifuHeart>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<WaifuHeart>()] = ModContent.ItemType<HeartoftheElements>();
                #endregion

                #region 虚空终结者
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<VoidofExtinction>()] = ModContent.ItemType<VoidofExtinctionLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<VoidofExtinctionLegacy>()] = ModContent.ItemType<VoidofExtinction>();
                #endregion

                #region 天蓝之礼
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AeroStone>()] = ModContent.ItemType<AeroStoneLegacy>();
                if(CalamityConditions.DownedDesertScourge.IsMet() && Main.hardMode) //干掉肉山后这俩就可以互转了
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AeroStoneLegacy>()] = ModContent.ItemType<AeroStone>();
                #endregion

                #region 顶级猎杀者的遗物
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<ReaperToothNecklace>()] = ModContent.ItemType<AncientReaperToothNecklace>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AncientReaperToothNecklace>()] = ModContent.ItemType<ReaperToothNecklace>();
                #endregion

                #region 精湛无比的科技造物
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<DraedonsHeart>()] = ModContent.ItemType<DraedonsHeartLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<DraedonsHeartLegacy>()] = ModContent.ItemType<DraedonsHeart>();
                #endregion

                #region 绚烂之彩虹胶质物
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<GrandGelatin>()] = ModContent.ItemType<GrandGelatinLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<GrandGelatinLegacy>()] = ModContent.ItemType<GrandGelatin>();
                #endregion

                #region 深海香水
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<LeviathanAmbergris>()] = ModContent.ItemType<LeviathanAmbergrisLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<LeviathanAmbergrisLegacy>()] = ModContent.ItemType<LeviathanAmbergris>();
                #endregion

                #region 血腥的契约
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BloodPact>()] = ModContent.ItemType<BloodPactLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BloodPactLegacy>()] = ModContent.ItemType<BloodPact>();
                #endregion
                #region 血炎晶核
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BloodflareCore>()] = ModContent.ItemType<BloodflareCoreLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BloodflareCoreLegacy>()] = ModContent.ItemType<BloodflareCore>();
                #endregion
           
            }
            #endregion
        }
    }
}