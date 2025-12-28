using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Ranged.Scarlet;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.Yharon;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.System.CalStatInflationBACK
{
    public class CheckItem : ModSystem
    {       
        // 存储武器类型
        public static List<int> PostMLWeapons = new List<int>();
        public static List<int> PostProfanedWeapons = new List<int>();
        public static List<int> PostSentinelsWeapon = new List<int>();
        public static List<int> PostPolterghastWeapons = new List<int>();
        public static List<int> PostOldDukeWeapons = new List<int>();
        public static List<int> PostDOGWeapons = new List<int>();
        public static List<int> PostyharonWeapons = new List<int>();
        public static List<int> PostExoAndScalWeapons = new List<int>();
        public static List<int> PostShadowspecWeapons = new List<int>();

        public override void PostAddRecipes()
        {
            if (CIServerConfig.Instance.CalStatInflationBACK)
                CategoryWeapons();
        }

        public void CategoryWeapons()
        {
            #region 检测武器
            foreach (Recipe recipe in Main.recipe)
            {
                // 直接检查配方成品
                Item item = recipe.createItem;
                if (item.ModItem != null)
                {
                    // 月后武器
                    if (PostMLWeapon(recipe, item))
                        PostMLWeapons.Add(item.type);
                    // 亵渎后武器
                    if (PostProvidenceWeapon(recipe, item))
                        PostProfanedWeapons.Add(item.type);
                    //部分武器居然只有使用三使徒材料做的，有点弱智了
                    if (PostSentinelsWeapons(recipe, item))
                        PostSentinelsWeapon.Add(item.type);
                    // 幽花后武器
                    if (PostPolterghastWeapon(recipe, item))
                        PostPolterghastWeapons.Add(item.type);
                    // 神后武器
                    if (PostDOGWeapon(recipe, item))
                        PostDOGWeapons.Add(item.type);
                    // 龙后武器
                    if (PostYharonWeapon(recipe, item))
                        PostyharonWeapons.Add(item.type);
                    if (PostShadowspecWeapon(recipe, item))
                        PostShadowspecWeapons.Add(item.type);
                }
            }
            #endregion
            BossAdd();
            #region 亵渎武器表单的删除
            // 不知道为什么过滤不掉T1000
            PostProfanedWeapons.Remove(ModContent.ItemType<AetherfluxCannon>());
            PostProfanedWeapons.Remove(ModContent.ItemType<AzathothLegacy>());
            // 怎么tr短剑被添加了两次
            PostProfanedWeapons.Remove(ModContent.ItemType<TerraShiv>());
            PostProfanedWeapons.Remove(ModContent.ItemType<TerraShiv>());
            #endregion
            #region 亵渎武器表单的添加
            // 三使徒就这几个了，打表了
            // 风编
            PostProfanedWeapons.Add(ModContent.ItemType<SkytideDragoon>());
            PostProfanedWeapons.Add(ModContent.ItemType<TheStorm>());
            PostProfanedWeapons.Add(ModContent.ItemType<Volterion>());
            // 虚空
            PostProfanedWeapons.Add(ModContent.ItemType<MirrorBlade>());
            PostProfanedWeapons.Add(ModContent.ItemType<VoidConcentrationStaff>());
            // 西格
            PostProfanedWeapons.Add(ModContent.ItemType<Cosmilamp>());
            PostProfanedWeapons.Add(ModContent.ItemType<CosmicKunai>());
            // 旧极乐火箭
            PostProfanedWeapons.Add(ModContent.ItemType<ProfanedLancher>());
            #endregion
            #region 幽花表单
            PostPolterghastWeapons.Add(ModContent.ItemType<VoidEdge>());
            PostPolterghastWeapons.Add(ModContent.ItemType<CalamarisLament>());
            PostPolterghastWeapons.Add(ModContent.ItemType<Valediction>());
            PostPolterghastWeapons.Add(ModContent.ItemType<EidolicWail>());
            PostPolterghastWeapons.Add(ModContent.ItemType<LionHeart>());
            PostPolterghastWeapons.Add(ModContent.ItemType<SulphuricAcidCannon>());
            PostPolterghastWeapons.Add(ModContent.ItemType<PhosphorescentGauntlet>());
            PostPolterghastWeapons.Add(ModContent.ItemType<SoulEdge>());
            PostPolterghastWeapons.Add(ModContent.ItemType<SiriusLegacy>());
            #endregion
            #region 幽花表单删除
            PostPolterghastWeapons.Remove(ModContent.ItemType<SpectreRifle>());
            #endregion
            #region 老猪表单
            PostOldDukeWeapons.Add(ModContent.ItemType<InsidiousImpalerLegacy>());

            #endregion

            #region 神长表单删除
            // 怎么你也没有过滤金源武器
            PostDOGWeapons.Remove(ModContent.ItemType<Ataraxia>());
            PostDOGWeapons.Remove(ModContent.ItemType<AtaraxiaOld>());
            PostDOGWeapons.Remove(ModContent.ItemType<YharimsCrystal>());
            PostDOGWeapons.Remove(ModContent.ItemType<YharimsCrystalLegendary>());
            #endregion
            #region 神长添加
            PostDOGWeapons.Add(ModContent.ItemType<ACTExcelsus>());
            PostDOGWeapons.Add(ModContent.ItemType<ACTKarasawa>());
            PostDOGWeapons.Add(ModContent.ItemType<ACTMinigun>());
            PostDOGWeapons.Add(ModContent.ItemType<ScorchedEarth>());
            #endregion
            #region 龙后
            PostyharonWeapons.Add(ModContent.ItemType<Murasama>());
            //把天顶干掉了，天顶会有个单独的增幅
            PostyharonWeapons.Remove(ItemID.Zenith);
            // 终焉百合
            PostyharonWeapons.Add(ModContent.ItemType<LiliesOfFinality>());
            #endregion
            #region 終灾表单添加
            PostExoAndScalWeapons.Add(ModContent.ItemType<GruesomeEminence>());
            PostExoAndScalWeapons.Add(ModContent.ItemType<CindersOfLament>());
            PostExoAndScalWeapons.Add(ModContent.ItemType<Rancor>());
            PostExoAndScalWeapons.Add(ModContent.ItemType<Metastasis>());
            #endregion
            #region 魔影
            PostShadowspecWeapons.Add(ModContent.ItemType<HalibutCannon>());
            PostShadowspecWeapons.Remove(ModContent.ItemType<ApotheosisLegacy>());
            PostShadowspecWeapons.Add(ModContent.ItemType<R99>());
            PostShadowspecWeapons.Add(ModContent.ItemType<LightAmmo>());
            PostShadowspecWeapons.Add(ModContent.ItemType<ShizukuSword>());
            #endregion
        }
        #region 月后初期的武器
        public static bool PostMLWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有星系异石 必须没有宇宙锭/神圣晶石
            return item.damage > 0
                    && (recipe.HasIngredient(ModContent.ItemType<GalacticaSingularity>()) || recipe.HasIngredient(ItemID.LunarBar))
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>() || CIServerConfig.Instance.CalSIBEOtherMod)
                    && !recipe.HasIngredient(ModContent.ItemType<CosmiliteBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<UelibloomBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<RuinousSoul>())
                    && !recipe.HasIngredient(ModContent.ItemType<DivineGeode>());
        }
        #endregion
        #region 亵渎后的武器
        public static bool PostProvidenceWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有神圣晶石/龙蒿锭 必须没有宇宙锭/金源锭/毁灭之灵/猎魂鲨牙
            return item.damage > 0
                    && (recipe.HasIngredient(ModContent.ItemType<DivineGeode>()) || recipe.HasIngredient(ModContent.ItemType<UelibloomBar>()))
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>() || CIServerConfig.Instance.CalSIBEOtherMod)
                    && !recipe.HasIngredient(ModContent.ItemType<CosmiliteBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<AuricBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<ReaperTooth>())
                    && !recipe.HasIngredient(ModContent.ItemType<RuinousSoul>());
        }
        #endregion
        #region 特判：
        public static bool PostSentinelsWeapons(Recipe recipe, Item item)
        {
            //三个bool。指定only 三使徒材料，其他的全部被排除
            bool isSentinelsWeaponOnly = item.damage > 0 && (recipe.HasIngredient(ModContent.ItemType<TwistingNether>()) || recipe.HasIngredient(ModContent.ItemType<ArmoredShell>()) || recipe.HasIngredient(ModContent.ItemType<DarkPlasma>()))
                                                         && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>() || CIServerConfig.Instance.CalSIBEOtherMod)
                                                         && !recipe.HasIngredient(ModContent.ItemType<CosmiliteBar>())
                                                         && !recipe.HasIngredient(ModContent.ItemType<UelibloomBar>())
                                                         && !recipe.HasIngredient(ModContent.ItemType<DivineGeode>())
                                                         && !recipe.HasIngredient(ModContent.ItemType<ReaperTooth>())
                                                         && !recipe.HasIngredient(ModContent.ItemType<RuinousSoul>())
                                                         && !recipe.HasIngredient(ModContent.ItemType<ReaperTooth>())
                                                         && !recipe.HasIngredient(ModContent.ItemType<DarksunFragment>())
                                                         && !recipe.HasIngredient(ModContent.ItemType<ShadowspecBar>());
            return isSentinelsWeaponOnly;

        }
        #endregion
        #region 幽花后的武器
        public static bool PostPolterghastWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有毁灭之灵/猎魂鲨牙 必须没有宇宙锭/金源锭/魔影锭
            return item.damage > 0
                    && (recipe.HasIngredient(ModContent.ItemType<RuinousSoul>())
                    || recipe.HasIngredient(ModContent.ItemType<ReaperTooth>()) ||
                    recipe.HasIngredient(ModContent.ItemType<BloodstoneCore>()))
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>() || CIServerConfig.Instance.CalSIBEOtherMod)
                    && !recipe.HasIngredient(ModContent.ItemType<CosmiliteBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<ShadowspecBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<AuricBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<AuricBarold>())
                    && !recipe.HasIngredient(ModContent.ItemType<AuricBar>());
        }
        #endregion
        #region 神后武器
        public static bool PostDOGWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有宇宙锭/ 必须没有金源锭/魔影锭
            return item.damage > 0
                    && (recipe.HasIngredient(ModContent.ItemType<CosmiliteBar>()) || recipe.HasIngredient(ModContent.ItemType<AscendantSpiritEssence>()))
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>() || CIServerConfig.Instance.CalSIBEOtherMod)
                    && !recipe.HasIngredient(ModContent.ItemType<ShadowspecBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<AuricBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<AuricBarold>());
        }
        #endregion
        #region 龙后武器
        public static bool PostYharonWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有宇宙锭/ 必须没有金源锭/魔影锭
            return item.damage > 0
                    && (recipe.HasIngredient(ModContent.ItemType<AuricBar>())
                    || recipe.HasIngredient(ModContent.ItemType<YharonSoulFragment>())
                    || recipe.HasIngredient(ModContent.ItemType<AuricBarold>()))
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>() || CIServerConfig.Instance.CalSIBEOtherMod)
                    && !recipe.HasIngredient(ModContent.ItemType<ShadowspecBar>());
        }
        #endregion
        #region 魔影武器
        public static bool PostShadowspecWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有魔影锭
            return item.damage > 0
                    && recipe.HasIngredient(ModContent.ItemType<ShadowspecBar>())
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>() || CIServerConfig.Instance.CalSIBEOtherMod);
        }
        #endregion
        #region boss掉落
        public static void BossAdd()
        {
            // 金龙
            int dragonfollyType = ModContent.NPCType<Dragonfolly>(); // 获取指定Boss的NPC类型ID
            var lootItems1 = CIFunction.FindLoots(dragonfollyType, false); // 获取所有掉落物，除了材料
            PostMLWeapons.AddRange(lootItems1.Where(id => !PostMLWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重

            // 亵渎
            int providenceType = ModContent.NPCType<Providence>(); // 获取指定Boss的NPC类型ID
            var lootItems2 = CIFunction.FindLoots(providenceType, false); // 获取所有掉落物，除了材料
            PostProfanedWeapons.AddRange(lootItems2.Where(id => !PostProfanedWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重

            // 幽花
            int polterghastType = ModContent.NPCType<Polterghast>(); // 获取指定Boss的NPC类型ID
            var lootItems3 = CIFunction.FindLoots(polterghastType, false); // 获取所有掉落物，除了材料
            PostPolterghastWeapons.AddRange(lootItems3.Where(id => !PostPolterghastWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重

            // 老核弹
            int oldDukeType = ModContent.NPCType<OldDuke>(); // 获取指定Boss的NPC类型ID
            var lootItems4 = CIFunction.FindLoots(oldDukeType, false); // 获取所有掉落物，除了材料
            PostOldDukeWeapons.AddRange(lootItems4.Where(id => !PostOldDukeWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重

            // 神长
            int DOGType = ModContent.NPCType<DevourerofGodsHead>(); // 获取指定Boss的NPC类型ID
            var lootItems5 = CIFunction.FindLoots(DOGType, false); // 获取所有掉落物，除了材料
            PostDOGWeapons.AddRange(lootItems5.Where(id => !PostDOGWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重


            // 龙
            int yharonType = ModContent.NPCType<Yharon>(); // 获取指定Boss的NPC类型ID
            var lootItems6 = CIFunction.FindLoots(yharonType, false); // 获取所有掉落物，除了材料
            PostDOGWeapons.AddRange(lootItems6.Where(id => !PostDOGWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重

            // 巨械
            int ExoType = ModContent.NPCType<AresBody>(); // 获取指定Boss的NPC类型ID
            var lootItems7 = CIFunction.FindLoots(ExoType, false); // 获取所有掉落物，除了材料
            PostExoAndScalWeapons.AddRange(lootItems7.Where(id => !PostExoAndScalWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重

            // 終灾
            int ScalType = ModContent.NPCType<SupremeCalamitas>(); // 获取指定Boss的NPC类型ID
            var lootItems8 = CIFunction.FindLoots(ScalType, false); // 获取所有掉落物，除了材料
            PostExoAndScalWeapons.AddRange(lootItems8.Where(id => !PostExoAndScalWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重
        }
        #endregion
    }
}
