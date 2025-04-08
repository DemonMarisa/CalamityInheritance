using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.MiscItem;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items
{
    public class CIGlobalShimmer: GlobalItem
    {
        public override void SetStaticDefaults()
        {
            #region 旧日的馈赠
            if (DownedBossSystem.downedYharon)
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<Minigun>()] = ModContent.ItemType<ACTMinigun>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<ACTMinigun>()] = ModContent.ItemType<Minigun>();
            }
            #endregion
            #region 传奇武器的强化道具强化道具
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<PBGLegendary>()] = ModContent.ItemType<PBGLegendaryUpgrade>();
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<DukeLegendary>()] = ModContent.ItemType<DukeLegendaryUpgrade>();
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<DestroyerLegendary>()] = ModContent.ItemType<DestroyerLegendaryUpgrade>();
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<RavagerLegendary>()] = ModContent.ItemType<RavagerLegendaryUpgrade>();
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<PlanteraLegendary>()] = ModContent.ItemType<PlanteraLegendaryUpgrade>();

            #endregion
            
            #region 微光嬗变启用时才会转化的
            if(CIServerConfig.Instance.CustomShimmer == true)
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

                #region 来自灾厄构造体的遗物
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<HavocsBreathLegacy>()] = ModContent.ItemType<HavocsBreath>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<HavocsBreath>()] = ModContent.ItemType<HavocsBreathLegacy>();
                #endregion

                #region 腐烂之脑的增生种
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<ShadethrowerLegacy>()] = ModContent.ItemType<Shadethrower>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<Shadethrower>()] = ModContent.ItemType<ShadethrowerLegacy>();
                #endregion

                #region 喵
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<MeowthrowerLegacy>()] = ModContent.ItemType<Meowthrower>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<Meowthrower>()] = ModContent.ItemType<MeowthrowerLegacy>();
                #endregion

                #region 纯纯的怒火
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<PristineFuryLegacy>()] = ModContent.ItemType<PristineFury>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<PristineFury>()] = ModContent.ItemType<PristineFuryLegacy>();
                #endregion

                #region 来自星星的喷火器
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AuroraBlazerLegacy>()] = ModContent.ItemType<AuroraBlazer>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AuroraBlazer>()] = ModContent.ItemType<AuroraBlazerLegacy>();
                #endregion

                #region 瘟疫,与疫病
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BlightSpewer>()] = ModContent.ItemType<BlightSpewerLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BlightSpewerLegacy>()] = ModContent.ItemType<BlightSpewer>();
                #endregion

                #region 过载充凝 
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<OverloadedBlaster>()] = ModContent.ItemType<OverloadedBlasterLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<OverloadedBlasterLegacy>()] = ModContent.ItemType<OverloadedBlaster>();
                #endregion

                #region 星辉馈赠的鱼
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<PolarisParrotfishLegacy>()] = ModContent.ItemType<PolarisParrotfish>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<PolarisParrotfish>()] = ModContent.ItemType<PolarisParrotfishLegacy>();
                #endregion
                #region 安宁.
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<Ataraxia>()] = ModContent.ItemType<AtaraxiaOld>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AtaraxiaOld>()] = ModContent.ItemType<Ataraxia>();
                #endregion
                
            
            }
            #endregion
        }
    }
}