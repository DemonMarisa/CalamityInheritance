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
using CalamityMod.Items.Weapons.Rogue;
using Terraria;
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
            #region 微光嬗变启用时才会转化的
            if(CIServerConfig.Instance.CustomShimmer == true)
            {
                //魔君礼
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<YharimsGift>()] = ModContent.ItemType<YharimsGiftLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<YharimsGiftLegacy>()] = ModContent.ItemType<YharimsGift>();

                //元灵之心
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<HeartoftheElements>()] = ModContent.ItemType<WaifuHeart>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<WaifuHeart>()] = ModContent.ItemType<HeartoftheElements>();

                //终结虚空
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<VoidofExtinction>()] = ModContent.ItemType<VoidofExtinctionLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<VoidofExtinctionLegacy>()] = ModContent.ItemType<VoidofExtinction>();

                //天蓝石
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AeroStone>()] = ModContent.ItemType<AeroStoneLegacy>();
                if(CalamityConditions.DownedDesertScourge.IsMet() && Main.hardMode) //干掉肉山后这俩就可以互转了
                    ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AeroStoneLegacy>()] = ModContent.ItemType<AeroStone>();

                //猎魂鲨项链->远古猎魂鲨项链
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<ReaperToothNecklace>()] = ModContent.ItemType<AncientReaperToothNecklace>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AncientReaperToothNecklace>()] = ModContent.ItemType<ReaperToothNecklace>();

                //嘉登之心
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<DraedonsHeart>()] = ModContent.ItemType<DraedonsHeartLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<DraedonsHeartLegacy>()] = ModContent.ItemType<DraedonsHeart>();

                //大宁凝胶
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<GrandGelatin>()] = ModContent.ItemType<GrandGelatinLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<GrandGelatinLegacy>()] = ModContent.ItemType<GrandGelatin>();

                //利维坦龙涎香
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<LeviathanAmbergris>()] = ModContent.ItemType<LeviathanAmbergrisLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<LeviathanAmbergrisLegacy>()] = ModContent.ItemType<LeviathanAmbergris>();

                //血契
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BloodPact>()] = ModContent.ItemType<BloodPactLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BloodPactLegacy>()] = ModContent.ItemType<BloodPact>();

                //血炎晶核
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BloodflareCore>()] = ModContent.ItemType<BloodflareCoreLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BloodflareCoreLegacy>()] = ModContent.ItemType<BloodflareCore>();

                //普灾喷火器
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<HavocsBreathLegacy>()] = ModContent.ItemType<HavocsBreath>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<HavocsBreath>()] = ModContent.ItemType<HavocsBreathLegacy>();

                //腐巢喷火器
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<ShadethrowerLegacy>()] = ModContent.ItemType<Shadethrower>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<Shadethrower>()] = ModContent.ItemType<ShadethrowerLegacy>();

                //🐱
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<MeowthrowerLegacy>()] = ModContent.ItemType<Meowthrower>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<Meowthrower>()] = ModContent.ItemType<MeowthrowerLegacy>();

                //纯元
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<PristineFuryLegacy>()] = ModContent.ItemType<PristineFury>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<PristineFury>()] = ModContent.ItemType<PristineFuryLegacy>();

                //我记得好像是那个能画五角星的喷火器
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AuroraBlazerLegacy>()] = ModContent.ItemType<AuroraBlazer>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AuroraBlazer>()] = ModContent.ItemType<AuroraBlazerLegacy>();

                //瘟疫喷火器
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BlightSpewer>()] = ModContent.ItemType<BlightSpewerLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<BlightSpewerLegacy>()] = ModContent.ItemType<BlightSpewer>();

                //史莱姆神的过载凝胶
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<OverloadedBlaster>()] = ModContent.ItemType<OverloadedBlasterLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<OverloadedBlasterLegacy>()] = ModContent.ItemType<OverloadedBlaster>();

                //鹦哥鱼
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<PolarisParrotfishLegacy>()] = ModContent.ItemType<PolarisParrotfish>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<PolarisParrotfish>()] = ModContent.ItemType<PolarisParrotfishLegacy>();

                //阿塔西亚
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<Ataraxia>()] = ModContent.ItemType<AtaraxiaOld>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AtaraxiaOld>()] = ModContent.ItemType<Ataraxia>();

                //湮灭
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<Eradicator>()] = ModContent.ItemType<MeleeTypeEradicator>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<MeleeTypeEradicator>()] = ModContent.ItemType<Eradicator>();

                //锤子系列
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<MeleeTypeHammerFallenPaladinsLegacy>()] = ModContent.ItemType<FallenPaladinsHammer>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<FallenPaladinsHammer>()] = ModContent.ItemType<MeleeTypeHammerFallenPaladinsLegacy>();
            }
            #region 神龛饰品    
            //蘑菇
            ShimmerEach<ShrineMushroom>(ModContent.ItemType<FungalSymbiote>());
            //气功念珠
            ShimmerEach<ShrineForest>(ModContent.ItemType<TrinketofChi>());
            //角斗士
            ShimmerEach<ShrineMarble>(ModContent.ItemType<GladiatorsLocket>());
            //我忘了是啥了，好像是花岗岩
            ShimmerEach<ShrineMarnite>(ModContent.ItemType<UnstableGraniteCore>());
            #endregion
            //宙能
            ShimmerEach<ACTExcelsus>(ModContent.ItemType<Excelsus>());
            #endregion
        }
        public static void ShimmerEach<I>(int result) where I : ModItem
        {
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<I>()] = result;
            ItemID.Sets.ShimmerTransformToItem[result] = ModContent.ItemType<I>();
        }
    }
}