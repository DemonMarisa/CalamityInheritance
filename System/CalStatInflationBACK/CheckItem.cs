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
            PostProfanedWeapons.Remove(ItemType<AetherfluxCannon>());
            PostProfanedWeapons.Remove(ItemType<AzathothLegacy>());
            // 怎么tr短剑被添加了两次
            PostProfanedWeapons.Remove(ItemType<TerraShiv>());
            PostProfanedWeapons.Remove(ItemType<TerraShiv>());
            #endregion
            #region 亵渎武器表单的添加
            // 三使徒就这几个了，打表了
            // 风编
            PostProfanedWeapons.Add(ItemType<SkytideDragoon>());
            PostProfanedWeapons.Add(ItemType<TheStorm>());
            PostProfanedWeapons.Add(ItemType<Volterion>());
            // 虚空
            PostProfanedWeapons.Add(ItemType<MirrorBlade>());
            PostProfanedWeapons.Add(ItemType<VoidConcentrationStaff>());
            // 西格
            PostProfanedWeapons.Add(ItemType<Cosmilamp>());
            PostProfanedWeapons.Add(ItemType<CosmicKunai>());
            // 旧极乐火箭
            PostProfanedWeapons.Add(ItemType<ProfanedLancher>());
            #endregion
            #region 幽花表单
            PostPolterghastWeapons.Add(ItemType<VoidEdge>());
            PostPolterghastWeapons.Add(ItemType<CalamarisLament>());
            PostPolterghastWeapons.Add(ItemType<Valediction>());
            PostPolterghastWeapons.Add(ItemType<EidolicWail>());
            PostPolterghastWeapons.Add(ItemType<LionHeart>());
            PostPolterghastWeapons.Add(ItemType<SulphuricAcidCannon>());
            PostPolterghastWeapons.Add(ItemType<PhosphorescentGauntlet>());
            PostPolterghastWeapons.Add(ItemType<SoulEdge>());
            PostPolterghastWeapons.Add(ItemType<SiriusLegacy>());
            #endregion
            #region 幽花表单删除
            PostPolterghastWeapons.Remove(ItemType<SpectreRifle>());
            #endregion
            #region 老猪表单
            PostOldDukeWeapons.Add(ItemType<InsidiousImpalerLegacy>());

            #endregion

            #region 神长表单删除
            // 怎么你也没有过滤金源武器
            PostDOGWeapons.Remove(ItemType<Ataraxia>());
            PostDOGWeapons.Remove(ItemType<AtaraxiaOld>());
            PostDOGWeapons.Remove(ItemType<YharimsCrystal>());
            PostDOGWeapons.Remove(ItemType<YharimsCrystalLegendary>());
            #endregion
            #region 神长添加
            PostDOGWeapons.Add(ItemType<ACTExcelsus>());
            PostDOGWeapons.Add(ItemType<ACTKarasawa>());
            PostDOGWeapons.Add(ItemType<ACTMinigun>());
            PostDOGWeapons.Add(ItemType<ScorchedEarth>());
            #endregion
            #region 龙后
            PostyharonWeapons.Add(ItemType<Murasama>());
            //把天顶干掉了，天顶会有个单独的增幅
            PostyharonWeapons.Remove(ItemID.Zenith);
            // 终焉百合
            PostyharonWeapons.Add(ItemType<LiliesOfFinality>());
            #endregion
            #region 終灾表单添加
            PostExoAndScalWeapons.Add(ItemType<GruesomeEminence>());
            PostExoAndScalWeapons.Add(ItemType<CindersOfLament>());
            PostExoAndScalWeapons.Add(ItemType<Rancor>());
            PostExoAndScalWeapons.Add(ItemType<Metastasis>());
            #endregion
            #region 魔影
            PostShadowspecWeapons.Add(ItemType<HalibutCannon>());
            PostShadowspecWeapons.Remove(ItemType<ApotheosisLegacy>());
            PostShadowspecWeapons.Add(ItemType<R99>());
            PostShadowspecWeapons.Add(ItemType<LightAmmo>());
            PostShadowspecWeapons.Add(ItemType<ShizukuSword>());
            #endregion
        }
        #region 月后初期的武器
        public static bool PostMLWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有星系异石 必须没有宇宙锭/神圣晶石
            return item.damage > 0
                    && (recipe.HasIngredient(ItemType<GalacticaSingularity>()) || recipe.HasIngredient(ItemID.LunarBar))
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == GetInstance<CalamityInheritance>() || CIServerConfig.Instance.CalSIBEOtherMod)
                    && !recipe.HasIngredient(ItemType<CosmiliteBar>())
                    && !recipe.HasIngredient(ItemType<UelibloomBar>())
                    && !recipe.HasIngredient(ItemType<RuinousSoul>())
                    && !recipe.HasIngredient(ItemType<DivineGeode>());
        }
        #endregion
        #region 亵渎后的武器
        public static bool PostProvidenceWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有神圣晶石/龙蒿锭 必须没有宇宙锭/金源锭/毁灭之灵/猎魂鲨牙
            return item.damage > 0
                    && (recipe.HasIngredient(ItemType<DivineGeode>()) || recipe.HasIngredient(ItemType<UelibloomBar>()))
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == GetInstance<CalamityInheritance>() || CIServerConfig.Instance.CalSIBEOtherMod)
                    && !recipe.HasIngredient(ItemType<CosmiliteBar>())
                    && !recipe.HasIngredient(ItemType<AuricBar>())
                    && !recipe.HasIngredient(ItemType<ReaperTooth>())
                    && !recipe.HasIngredient(ItemType<RuinousSoul>());
        }
        #endregion
        #region 特判：
        public static bool PostSentinelsWeapons(Recipe recipe, Item item)
        {
            //三个bool。指定only 三使徒材料，其他的全部被排除
            bool isSentinelsWeaponOnly = item.damage > 0 && (recipe.HasIngredient(ItemType<TwistingNether>()) || recipe.HasIngredient(ItemType<ArmoredShell>()) || recipe.HasIngredient(ItemType<DarkPlasma>()))
                                                         && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == GetInstance<CalamityInheritance>() || CIServerConfig.Instance.CalSIBEOtherMod)
                                                         && !recipe.HasIngredient(ItemType<CosmiliteBar>())
                                                         && !recipe.HasIngredient(ItemType<UelibloomBar>())
                                                         && !recipe.HasIngredient(ItemType<DivineGeode>())
                                                         && !recipe.HasIngredient(ItemType<ReaperTooth>())
                                                         && !recipe.HasIngredient(ItemType<RuinousSoul>())
                                                         && !recipe.HasIngredient(ItemType<ReaperTooth>())
                                                         && !recipe.HasIngredient(ItemType<DarksunFragment>())
                                                         && !recipe.HasIngredient(ItemType<ShadowspecBar>());
            return isSentinelsWeaponOnly;

        }
        #endregion
        #region 幽花后的武器
        public static bool PostPolterghastWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有毁灭之灵/猎魂鲨牙 必须没有宇宙锭/金源锭/魔影锭
            return item.damage > 0
                    && (recipe.HasIngredient(ItemType<RuinousSoul>())
                    || recipe.HasIngredient(ItemType<ReaperTooth>()) ||
                    recipe.HasIngredient(ItemType<BloodstoneCore>()))
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == GetInstance<CalamityInheritance>() || CIServerConfig.Instance.CalSIBEOtherMod)
                    && !recipe.HasIngredient(ItemType<CosmiliteBar>())
                    && !recipe.HasIngredient(ItemType<ShadowspecBar>())
                    && !recipe.HasIngredient(ItemType<AuricBar>())
                    && !recipe.HasIngredient(ItemType<AuricBarold>())
                    && !recipe.HasIngredient(ItemType<AuricBar>());
        }
        #endregion
        #region 神后武器
        public static bool PostDOGWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有宇宙锭/ 必须没有金源锭/魔影锭
            return item.damage > 0
                    && (recipe.HasIngredient(ItemType<CosmiliteBar>()) || recipe.HasIngredient(ItemType<AscendantSpiritEssence>()))
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == GetInstance<CalamityInheritance>() || CIServerConfig.Instance.CalSIBEOtherMod)
                    && !recipe.HasIngredient(ItemType<ShadowspecBar>())
                    && !recipe.HasIngredient(ItemType<AuricBar>())
                    && !recipe.HasIngredient(ItemType<AuricBarold>());
        }
        #endregion
        #region 龙后武器
        public static bool PostYharonWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有宇宙锭/ 必须没有金源锭/魔影锭
            return item.damage > 0
                    && (recipe.HasIngredient(ItemType<AuricBar>())
                    || recipe.HasIngredient(ItemType<YharonSoulFragment>())
                    || recipe.HasIngredient(ItemType<AuricBarold>()))
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == GetInstance<CalamityInheritance>() || CIServerConfig.Instance.CalSIBEOtherMod)
                    && !recipe.HasIngredient(ItemType<ShadowspecBar>());
        }
        #endregion
        #region 魔影武器
        public static bool PostShadowspecWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有魔影锭
            return item.damage > 0
                    && recipe.HasIngredient(ItemType<ShadowspecBar>())
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == GetInstance<CalamityInheritance>() || CIServerConfig.Instance.CalSIBEOtherMod);
        }
        #endregion
        #region boss掉落
        public static void BossAdd()
        {
            // 金龙
            int dragonfollyType = NPCType<Dragonfolly>(); // 获取指定Boss的NPC类型ID
            var lootItems1 = CIFunction.FindLoots(dragonfollyType, false); // 获取所有掉落物，除了材料
            PostMLWeapons.AddRange(lootItems1.Where(id => !PostMLWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重

            // 亵渎
            int providenceType = NPCType<Providence>(); // 获取指定Boss的NPC类型ID
            var lootItems2 = CIFunction.FindLoots(providenceType, false); // 获取所有掉落物，除了材料
            PostProfanedWeapons.AddRange(lootItems2.Where(id => !PostProfanedWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重

            // 幽花
            int polterghastType = NPCType<Polterghast>(); // 获取指定Boss的NPC类型ID
            var lootItems3 = CIFunction.FindLoots(polterghastType, false); // 获取所有掉落物，除了材料
            PostPolterghastWeapons.AddRange(lootItems3.Where(id => !PostPolterghastWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重

            // 老核弹
            int oldDukeType = NPCType<OldDuke>(); // 获取指定Boss的NPC类型ID
            var lootItems4 = CIFunction.FindLoots(oldDukeType, false); // 获取所有掉落物，除了材料
            PostOldDukeWeapons.AddRange(lootItems4.Where(id => !PostOldDukeWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重

            // 神长
            int DOGType = NPCType<DevourerofGodsHead>(); // 获取指定Boss的NPC类型ID
            var lootItems5 = CIFunction.FindLoots(DOGType, false); // 获取所有掉落物，除了材料
            PostDOGWeapons.AddRange(lootItems5.Where(id => !PostDOGWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重


            // 龙
            int yharonType = NPCType<Yharon>(); // 获取指定Boss的NPC类型ID
            var lootItems6 = CIFunction.FindLoots(yharonType, false); // 获取所有掉落物，除了材料
            PostDOGWeapons.AddRange(lootItems6.Where(id => !PostDOGWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重

            // 巨械
            int ExoType = NPCType<AresBody>(); // 获取指定Boss的NPC类型ID
            var lootItems7 = CIFunction.FindLoots(ExoType, false); // 获取所有掉落物，除了材料
            PostExoAndScalWeapons.AddRange(lootItems7.Where(id => !PostExoAndScalWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重

            // 終灾
            int ScalType = NPCType<SupremeCalamitas>(); // 获取指定Boss的NPC类型ID
            var lootItems8 = CIFunction.FindLoots(ScalType, false); // 获取所有掉落物，除了材料
            PostExoAndScalWeapons.AddRange(lootItems8.Where(id => !PostExoAndScalWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重
        }
        #endregion
    }
}
