using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Accessories.Melee;
using CalamityInheritance.Content.Items.Armor.Xeroc;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.MiscItem;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.System.DownedBoss;
using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Aerospec;
using CalamityMod.Items.Armor.Empyrean;
using CalamityMod.Items.Weapons.Ranged;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.System
{
    public partial class ILShimmer : ModSystem
    {
        public override void OnModLoad()
        {
            On_ShimmerTransforms.IsItemTransformLocked += ShimmerRequirementHandler;
        }
        public override void PostSetupContent()
        {
            CalamityInheritance.Calamity.Call("MakeItemExhumable", ModContent.ItemType<DesertFeather>(), ModContent.ItemType<DefiledFeather>());
        }
    }
    public partial class ILShimmer
    {
        public static bool ShimmerRequirementHandler(On_ShimmerTransforms.orig_IsItemTransformLocked orig, int type)
        {
            //这下面返回的都是相反的结果，即如果你需要让一个东西要击败妖龙后转化，你应该返回!妖龙
            //天蓝石互转需要击败肉山
            if (type.SameItem<AeroStone>() || type.SameItem<AeroStoneLegacy>())
            {
                return !Main.hardMode;
            }
            //大比目鱼互转需要击败妖龙
            if (type.SameItem<HalibutCannon>() || type.SameItem<HalibutCannonLegendary>())
            {
                return !DownedBossSystem.downedPrimordialWyrm;
            }
            //晋升证章与魔君证章
            if (type.SameItem<YharimsInsignia>() || type.SameItem<AscendantInsignia>())
            {
                return !DownedBossSystem.downedPolterghast;
            }
            //星辉秘术与纯净，这俩合成难度疑似不是一个等级的
            if (type.SameItem<AstralArcanum>() || type.SameItem<Radiance>())
            {
                return !DownedBossSystem.downedYharon && !CIDownedBossSystem.DownedLegacyYharonP2;
            }
            //神圣护符与神圣护符，虽然我不知道有谁会用原灾版本
            if (type.SameItem<DeificAmulet>() || type.SameItem<DeificAmuletLegacy>())
            {
                return !DownedBossSystem.downedAstrumDeus;
            }
            //壁垒与壁垒，同上
            if (type.SameItem<RampartofDeities>() || type.SameItem<CIRampartofDeities>())
            {
                return !DownedBossSystem.downedYharon && !CIDownedBossSystem.DownedLegacyYharonP2;
            }
            //百草瓶
            if (type.SameItem<AmbrosialAmpoule>() || type.SameItem<AmbrosialAmpouleOld>())
            {
                return !Condition.DownedCultist.IsMet();
            }
            //克希洛克与黄天
            if (type.SameItem<AncientXerocMask>() || type.SameItem<EmpyreanMask>())
            {
                return !Condition.DownedMoonLord.IsMet();
            }
            if (type.SameItem<AncientXerocPlateMail>() || type.SameItem<EmpyreanCloak>())
            {
                return !Condition.DownedMoonLord.IsMet();
            }
            if (type.SameItem<AncientXerocCuisses>() || type.SameItem<EmpyreanCuisses>())
            {
                return !Condition.DownedMoonLord.IsMet();
            }
            return orig(type);
        }
    }
    public static class SimpleMethod
    {
        public static bool SameItem<T>(this int type) where T : ModItem => type == ModContent.ItemType<T>();

    }
}
