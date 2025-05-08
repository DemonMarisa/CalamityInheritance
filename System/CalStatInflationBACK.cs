using CalamityInheritance.Utilities;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.Yharon;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons;
using Terraria.Localization;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Ammo.RangedAmmo;
using CalamityMod.Items.Ammo;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityInheritance.Content.Items.Weapons.Summon;
using static CalamityInheritance.System.CalStatInflationBACK;

namespace CalamityInheritance.System
{
    public class CalStatInflationBACK : ModSystem
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
        #region 弱引用
        public static Mod FuckGoozmaMod {get; set;}
        public static int GoozmaBoss { get; private set;}
        #endregion
        // 加载/卸载列表
        public override void Load()
        {
            PostMLWeapons = [];
            PostProfanedWeapons = [];
            PostSentinelsWeapon = [];
            PostPolterghastWeapons = [];
            PostOldDukeWeapons = [];
            PostDOGWeapons = [];
            PostyharonWeapons = [];
            PostExoAndScalWeapons = [];
            PostShadowspecWeapons = [];
            if (ModLoader.TryGetMod("CalamityHunt", out Mod hunt))
            {
                FuckGoozmaMod = hunt;
                if (FuckGoozmaMod.TryFind("Goozma", out ModNPC FuckGoozma))
                    GoozmaBoss = FuckGoozma.Type;

            }
        }

        public override void Unload()
        {
            PostMLWeapons = null;
            PostProfanedWeapons = null;
            PostSentinelsWeapon = null;
            PostPolterghastWeapons = null;
            PostOldDukeWeapons = null;
            PostDOGWeapons = null;
            PostyharonWeapons = null;
            PostExoAndScalWeapons = null;
            PostShadowspecWeapons = null;
            FuckGoozmaMod =  null;
        }
        public override void PostAddRecipes()
        {
            if (CIServerConfig.Instance.CalStatInflationBACK)
                AddRec();
        }
        public static void AddRec()
        {
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
            // boss掉落物和宝藏袋
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
            PostProfanedWeapons.Add(ModContent.ItemType<StormDragoon>());
            PostProfanedWeapons.Add(ModContent.ItemType<TheStorm>());
            PostProfanedWeapons.Add(ModContent.ItemType<Thunderstorm>());
            // 虚空
            PostProfanedWeapons.Add(ModContent.ItemType<MirrorBlade>());
            PostProfanedWeapons.Add(ModContent.ItemType<VoidConcentrationStaff>());
            // 西格
            PostProfanedWeapons.Add(ModContent.ItemType<Cosmilamp>());
            PostProfanedWeapons.Add(ModContent.ItemType<CosmicKunai>());
            // 旧极乐火箭
            PostProfanedWeapons.Add(ModContent.ItemType<ProfanedLancher>());
            // 终焉百合
            PostProfanedWeapons.Add(ModContent.ItemType<LiliesOfFinality>());
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
            #endregion
            #region 龙后
            PostyharonWeapons.Add(ModContent.ItemType<Murasama>());
            //把天顶干掉了，天顶会有个单独的增幅
            PostyharonWeapons.Remove(ItemID.Zenith);
            #endregion
            #region 終灾表单添加
            PostExoAndScalWeapons.Add(ModContent.ItemType<GruesomeEminence>());
            PostExoAndScalWeapons.Add(ModContent.ItemType<CindersOfLament>());
            PostExoAndScalWeapons.Add(ModContent.ItemType<Rancor>());
            PostExoAndScalWeapons.Add(ModContent.ItemType<Metastasis>());
            #endregion
            #region 魔影
            PostShadowspecWeapons.Add(ModContent.ItemType<IridescentExcalibur>());
            PostShadowspecWeapons.Add(ModContent.ItemType<HalibutCannon>());
            #endregion
        }

        #region 月后初期的武器
        public static bool PostMLWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有星系异石 必须没有宇宙锭/神圣晶石
            return item.damage > 0
                    && (recipe.HasIngredient(ModContent.ItemType<GalacticaSingularity>()) || recipe.HasIngredient(ItemID.LunarBar))
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>())
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
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>())
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
                                                         && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>())
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
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>())
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
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>())
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
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>())
                    && !recipe.HasIngredient(ModContent.ItemType<ShadowspecBar>());
        }
        #endregion
        #region 魔影武器
        public static bool PostShadowspecWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有魔影锭
            return item.damage > 0
                    && recipe.HasIngredient(ModContent.ItemType<ShadowspecBar>())
                    && (recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity || item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>());
        }
        #endregion
        #region boss掉落
        public static void BossAdd()
        {
            // 金龙
            int dragonfollyType = ModContent.NPCType<Bumblefuck>(); // 获取指定Boss的NPC类型ID
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
            if(CIServerConfig.Instance.SolarEclipseChange)
                PostDOGWeapons.AddRange(lootItems6.Where(id => !PostDOGWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重
            else
                PostyharonWeapons.AddRange(lootItems6.Where(id => !PostyharonWeapons.Contains(id)).Distinct());// 添加到掉落物列表并去重

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
    public class CalamityStatInflationBACK : GlobalItem
    {
        #region 武器伤害增幅
        public const float PostMLWeaponsBoost = 1.3f; // 月后
        public const float PostProfanedWeaponsBoost = 2.2f; // 亵渎后||使徒 不是哥们，你们dps怎么这么低
        public const float PostPolterghastWeaponsBoost = 2.4f; // 幽花后
        public const float PostOldDukeWeaponsBoost = 2.5f; // 老猪后
        public const float PostDOGWeaponsBoost = 3f; // 神后
        public const float PostYharonWeaponsBoost = 5f; // 龙后
        public const float PostExoAndScalWeaponsBoost = 7f; // 巨械終灾后
        public const float PostShadowspecWeaponsBoost = 10f; // 巨械終灾后
        #endregion
        #region SD
        public override void SetDefaults(Item item)
        {
            if (CIServerConfig.Instance.CalStatInflationBACK)
            {
                if (CalStatInflationBACK.PostMLWeapons.Contains(item.type))
                    item.damage = (int)(item.damage * PostMLWeaponsBoost);
                if (CalStatInflationBACK.PostProfanedWeapons.Contains(item.type) || CalStatInflationBACK.PostSentinelsWeapon.Contains(item.type))
                    item.damage = (int)(item.damage * PostProfanedWeaponsBoost);
                if (CalStatInflationBACK.PostPolterghastWeapons.Contains(item.type))
                {
                    if (item.DamageType == DamageClass.Ranged)
                        item.damage = (int)(item.damage * 1.3f);
                    item.damage = (int)(item.damage * PostPolterghastWeaponsBoost);
                }
                if (CalStatInflationBACK.PostOldDukeWeapons.Contains(item.type))
                {
                    item.damage = (int)(item.damage * PostOldDukeWeaponsBoost);
                    if (item.type == ModContent.ItemType<InsidiousImpalerLegacy>())
                        item.damage = 420;
                }
                if (CalStatInflationBACK.PostDOGWeapons.Contains(item.type))
                    item.damage = (int)(item.damage * PostDOGWeaponsBoost);
                if (CalStatInflationBACK.PostyharonWeapons.Contains(item.type))
                {
                    item.damage = (int)(item.damage * PostYharonWeaponsBoost);
                    if (item.DamageType == DamageClass.Ranged || item.DamageType == ModContent.GetInstance<RogueDamageClass>())
                        item.damage = (int)(item.damage * 1.5f);
                }
                if (CalStatInflationBACK.PostExoAndScalWeapons.Contains(item.type))
                    item.damage = (int)(item.damage * PostExoAndScalWeaponsBoost);
                if (CalStatInflationBACK.PostShadowspecWeapons.Contains(item.type))
                    item.damage = (int)(item.damage * PostShadowspecWeaponsBoost);
                AuricBlance(item);
                ShadowspecBlance(item);
                CosmicBlance(item);
                PolterghastBlance(item);
                ProfanedBlance(item);
                AmmoChange(item);
                ExoWeapons(item);
            }
        }
        #endregion
        #region 特殊平衡改动
        #region 亵渎
        public static void ProfanedBlance(Item item)
        {
            #region 射手
            if (item.type == ModContent.ItemType<TelluricGlare>())
                item.damage = (int)(item.damage * 3f);
            #endregion
            #region 法师
            if (item.type == ModContent.ItemType<PlasmaRifle>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<ThePrince>())
                item.damage = (int)(item.damage * 1.5f);
            #endregion
        }
        #endregion
        #region 幽花后
        public static void PolterghastBlance(Item item)
        {
            #region 战士
            if (item.type == ModContent.ItemType<NeptunesBounty>())
                item.damage = (int)(item.damage * 1.3f);

            #endregion
            #region 射手
            if (item.type == ModContent.ItemType<ClaretCannon>())
                item.damage = (int)(item.damage * 1.6f);

            if (item.type == ModContent.ItemType<SulphuricAcidCannon>())
                item.damage = (int)(item.damage * 1.7f);

            if (item.type == ModContent.ItemType<DodusHandcannon>())
                item.damage = (int)(item.damage * 1.6f);

            if (item.type == ModContent.ItemType<TheMaelstrom>())
                item.damage = (int)(item.damage * 1.4f);

            if (item.type == ModContent.ItemType<BloodBoiler>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<HalleysInferno>())
                item.damage = (int)(item.damage * 1.5f);
            #endregion
            #region 法师
            if (item.type == ModContent.ItemType<ClamorNoctus>())
                item.damage = (int)(item.damage * 1.4f);

            if (item.type == ModContent.ItemType<DarkSpark>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<ShadowboltStaff>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<VenusianTrident>())
                item.damage = (int)(item.damage * 1.6f);
            #endregion
            #region 召唤

            if (item.type == ModContent.ItemType<Sirius>())
                item.damage = (int)(item.damage * 3.5f);

            #endregion
            #region 盗贼
            if (item.type == ModContent.ItemType<Valediction>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<NightsGaze>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<PhantasmalRuinold>())
                item.damage = (int)(item.damage * 0.5f);
            #endregion
        }
        #endregion
        #region 宇宙
        public static void CosmicBlance(Item item)
        {
            #region 战士

            if (item.type == ModContent.ItemType<EmpyreanKnives>())
                item.damage = 1400;

            if (item.type == ModContent.ItemType<PrismaticBreaker>())
                item.damage = 4000;

            if (item.type == ModContent.ItemType<Excelsus>())
                item.damage = (int)(item.damage * 1.5f);

            if (item.type == ModContent.ItemType<GalaxySmasher>())
                item.damage = (int)(item.damage * 1.5f);

            if (item.type == ModContent.ItemType<ScourgeoftheCosmos>())
                item.damage = (int)(item.damage * 1.5f);

            if (item.type == ModContent.ItemType<TheObliterator>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<EssenceFlayer>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<CosmicShiv>())
            {
                item.useAnimation = 20;
                item.useTime = 20;
            }
            if (item.type == ModContent.ItemType<TheEnforcer>())
            {
                item.useAnimation = 10;
                item.useTime = 10;
            }
            #endregion
            #region 射手
            if (item.type == ModContent.ItemType<Deathwind>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<Alluvion>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<AntiMaterielRifle>())
                item.damage = 10000;

            if (item.type == ModContent.ItemType<ThePack>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<Starmageddon>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<Starmada>())
                item.damage = (int)(item.damage * 1.3f);

            if (item.type == ModContent.ItemType<CleansingBlaze>())
                item.damage = (int)(item.damage * 1.3f);

            if (item.type == ModContent.ItemType<PulseRifle>())
                item.damage = (int)(item.damage * 1.2f);

            if (item.type == ModContent.ItemType<Karasawa>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<RubicoPrime>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<Ultima>())
                item.damage = (int)(item.damage * 1.4f);

            if (item.type == ModContent.ItemType<UniversalGenesis>())
                item.damage = (int)(item.damage * 1.4f);
            #endregion
            #region 法师

            if (item.type == ModContent.ItemType<DeathhailStaff>())
                item.useTime = 5;

            if (item.type == ModContent.ItemType<RecitationoftheBeast>())
                item.damage = (int)(item.damage * 1.4f);

            if (item.type == ModContent.ItemType<FaceMelter>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<IceBarrage>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<ACTAlphaRay>())
                item.damage = (int)(item.damage * 0.8f);

            #endregion
            #region 召唤

            if (item.type == ModContent.ItemType<SarosPossession>())
                item.damage = (int)(item.damage * 1.6f);

            if (item.type == ModContent.ItemType<CorvidHarbringerStaff>())
                item.damage = (int)(item.damage * 1.4f);

            if (item.type == ModContent.ItemType<CosmicViperEngine>())
                item.damage = (int)(item.damage * 1.5f);

            if (item.type == ModContent.ItemType<EndoHydraStaff>())
                item.damage = (int)(item.damage * 1.5f);

            #endregion
            #region 盗贼
            if (item.type == ModContent.ItemType<Penumbra>())
                item.damage = (int)(item.damage * 2f);

            if (item.type == ModContent.ItemType<EclipsesFall>())
                item.damage = (int)(item.damage * 1.5f);

            if (item.type == ModContent.ItemType<Hypothermia>())
                item.damage = (int)(item.damage * 1.5f);

            if (item.type == ModContent.ItemType<Eradicator>())
                item.damage = (int)(item.damage * 1.2f);

            if (item.type == ModContent.ItemType<PlasmaGrenade>())
                item.damage = (int)(item.damage * 1.8f);

            #endregion
        }
        #endregion
        #region 金源
        public static void AuricBlance(Item item)
        {
            if (item.type == ModContent.ItemType<VoidVortex>())
                item.damage = 1800;
            if (item.type == ModContent.ItemType<YharimsCrystal>())
                item.damage = 600;
            #region 战士
            if (item.type == ModContent.ItemType<ArkoftheCosmos>())
                item.damage = 14000;
            if (item.type == ModContent.ItemType<ArkoftheCosmosold>())
                item.damage = 501;
            if (item.type == ModContent.ItemType<Ataraxia>())
            {
                item.damage = (int)(item.damage * 1.4f);
                item.useTurn = false;
            }

            if (item.type == ItemID.Zenith)
            {
                //面板210 -> 2145
                item.damage = 2145;
                item.rare = ModContent.RarityType<PureRed>();
                item.value = CIShopValue.RarityPricePureRed;
                item.ArmorPenetration = 150;
            }

            if (item.type == ModContent.ItemType<TheOracle>())
                item.damage = 1500;
            #endregion
            #region 射手
            if (item.type == ModContent.ItemType<DrataliornusLegacy>())
                item.damage = 700;
            #endregion
            #region 法师
            if (item.type == ModContent.ItemType<VoidVortexLegacy>())
                item.damage = 240;
            if (item.type == ModContent.ItemType<HadopelagicEcho>())
                item.damage = 4444;
            #endregion
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!CIServerConfig.Instance.CalStatInflationBACK)
                return;
            if (item.type == ItemID.Zenith)
            {
                string text = Language.GetTextValue($"{Generic.GetWeaponLocal}.Melee.ZenithBuff");
                tooltips.Add(new TooltipLine(Mod, "buff", text));
            }

        }
        public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            if (!CIServerConfig.Instance.CalStatInflationBACK)
                return;
            //初始满暴
            if (item.type == ItemID.Zenith)
                crit += 96;
            if (item.type == ModContent.ItemType<ArkoftheCosmos>())
                crit += 31;
        }
        #endregion
        #region 星流武器
        public static void ExoWeapons(Item item)
        {
            #region 遗产
            // 归元旋涡
            if (item.type == ModContent.ItemType<SubsumingVortexold>())
                item.damage = 935;
            // 耀界之光
            if (item.type == ModContent.ItemType<VividClarityOld>())
                item.damage = 650;
            // 星流短剑
            if (item.type == ModContent.ItemType<ExoGladius>())
                item.damage = 2000;
            // 星流之刃
            if (item.type == ModContent.ItemType<Exobladeold>())
                item.damage = 5175;
            // 链刃
            if (item.type == ModContent.ItemType<ExoFlail>())
                item.damage = 3125;
            // 磁极异变
            if (item.type == ModContent.ItemType<MagnomalyCannon>())
                item.damage = 2100;
            // 天堂之风
            if (item.type == ModContent.ItemType<HeavenlyGaleold>())
                item.damage = 808;
            // 星火解离者
            if (item.type == ModContent.ItemType<Photovisceratorold>())
                item.damage = 1300;
            // 星神之杀
            if (item.type == ModContent.ItemType<Celestusold>())
                item.damage = 1054;
            // 超新星
            if (item.type == ModContent.ItemType<Supernovaold>())
                item.damage = 3500;
            // 弧光
            if (item.type == ModContent.ItemType<ExoTheApostle>())
                item.damage = 9000;
            // 归墟
            if (item.type == ModContent.ItemType<CosmicImmaterializerOld>())
                item.damage = 2000;
            // 热寂
            if (item.type == ModContent.ItemType<CelestialObliterator>())
                item.damage = 600;
            #endregion
            // 星火解离者
            if (item.type == ModContent.ItemType<Photoviscerator>())
                item.damage = 2300;
            // 天堂之风
            if (item.type == ModContent.ItemType<HeavenlyGale>())
                item.damage = 800;
            // 星神之杀
            if (item.type == ModContent.ItemType<Celestus>())
                item.damage = 1222;
            // 超新星
            if (item.type == ModContent.ItemType<Supernova>())
                item.damage = 22000;
            // 星流刀
            if (item.type == ModContent.ItemType<Exoblade>())
                item.damage = 3000;
            // 旋涡
            if (item.type == ModContent.ItemType<SubsumingVortex>())
                item.damage = 1165;
            // 耀界
            if (item.type == ModContent.ItemType<VividClarity>())
                item.damage = 450;
            // 归墟
            if (item.type == ModContent.ItemType<CosmicImmaterializer>())
                item.damage = 1500;
        }
        #endregion
        #region 魔影
        public static void ShadowspecBlance(Item item)
        {
            if (item.type == ModContent.ItemType<Earth>())
                item.damage = 1750; //无限大地: 200 -> 1750
            if (item.type == ModContent.ItemType<IllustriousKnives>())
                item.damage = 3500; //圣光飞刀转为3500
            if (item.type == ModContent.ItemType<Contagion>())
                item.damage = 10000; //瘟疫弓恢复为10000面板
            if (item.type == ModContent.ItemType<Eternity>())
                item.damage = 5000; //恒：5000面板
            if (item.type == ModContent.ItemType<Apotheosis>())
                item.damage = 7777; //原版神吞书：7777
            if (item.type == ModContent.ItemType<ScarletDevil>())
                item.damage = 14571; //绯红恶魔回调至14571面板
            if (item.type == ModContent.ItemType<HalibutCannon>())
                item.damage = 1500; //比目鱼
            if (item.type == ModContent.ItemType<NanoblackReaper>())
                item.damage = 4000;
            if (item.type == ModContent.ItemType<TriactisTruePaladinianMageHammerofMightMelee>())
                item.damage = 10000; //一万面板
        }
        #endregion
        #region 弹药
        public static void AmmoChange(Item item)
        {
            if (item.type == ModContent.ItemType<ElysianArrow>())
                item.damage = 20;
            if (item.type == ModContent.ItemType<BloodfireArrow>())
                item.damage = 40;
            if (item.type == ModContent.ItemType<VanquisherArrow>())
                item.damage = 33;
            if (item.type == ModContent.ItemType<HolyFireBullet>())
                item.damage = 27;
            if (item.type == ModContent.ItemType<BloodfireBullet>())
                item.damage = 40;
            if (item.type == ModContent.ItemType<GodSlayerSlug>())
                item.damage = 42;

            if (item.type == ModContent.ItemType<HolyFireBulletOld>())
                item.damage = 27;
            if (item.type == ModContent.ItemType<VanquisherArrowold>())
                item.damage = 33;
            if (item.type == ModContent.ItemType<ElysianArrowOld>())
                item.damage = 20;
        }
        #endregion
        #endregion
        #region 龙一龙二改变
        public static void EclipseChange(Item item)
        {
            if (CIServerConfig.Instance.SolarEclipseChange)
            {
                // 巨龙之怒
                if (item.type == ModContent.ItemType<DragonRage>())
                    item.damage = 2000;
                // 氦闪
                if (item.type == ModContent.ItemType<HeliumFlash>())
                    item.damage = 6666;

                // 巨龙七星灯，极昼信标
                if (item.type == ModContent.ItemType<YharonsKindleStaff>() || item.type == ModContent.ItemType<MidnightSunBeacon>())
                    item.damage = (int)(item.damage * 1.5f);
            }
            else
            {
                if (item.type == ModContent.ItemType<DragonRage>())
                    item.damage = 4000;

                if (item.type == ModContent.ItemType<HeliumFlash>())
                    item.damage = 6666;

                if (item.type == ModContent.ItemType<YharonsKindleStaff>() || item.type == ModContent.ItemType<MidnightSunBeacon>())
                    item.damage = (int)(item.damage * 2.4f);
            }
        }
        #endregion
    }
    public class CalamityStatInflationBACKNPC : GlobalNPC
    {
        public override void SetDefaults(NPC npc)
        {
            if (CIServerConfig.Instance.CalStatInflationBACK)
            {
                if (CalamityInheritanceLists.PostMLBoss.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 1.2f);
                    npc.life = (int)(npc.life * 1.2f);
                    npc.defense = (int)(npc.defense * 1.2f);
                }
                if (CalamityInheritanceLists.PostProfanedBoss.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 1.5f);
                    npc.life = (int)(npc.life * 1.5f);
                    npc.defense = (int)(npc.defense * 1.5f);
                }
                if (CalamityInheritanceLists.PostPolterghastBoss.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 1.7f);
                    npc.life = (int)(npc.life * 1.7f);
                }
                if (CalamityInheritanceLists.DOG.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 2.2f);
                    npc.life = (int)(npc.life * 2.2f);
                }
                if (npc.type == ModContent.NPCType<Yharon>())
                {
                    npc.lifeMax = (int)(npc.lifeMax * 2.8f);
                    npc.life = (int)(npc.life * 2.8f);
                }
                if (CalamityInheritanceLists.ExoMech.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 5f);
                    npc.life = (int)(npc.life * 5f);
                    npc.defense = (int)(npc.defense * 1.2f);
                }
                if (CalamityInheritanceLists.Scal.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 6.6f);
                    npc.life = (int)(npc.life * 6.6f);
                    npc.defense = (int)(npc.defense * 1.2f);
                }
                if (CalamityMod.Events.BossRushEvent.BossRushActive)
                {
                    npc.lifeMax = (int)(npc.lifeMax * 10f);
                    npc.life = (int)(npc.life * 10f);
                    npc.defense = (int)(npc.defense * 10f);
                }
                //如果为空不准执行。
                if (npc.type == GoozmaBoss)
                    npc.lifeMax = (int)(npc.lifeMax * 6.6f);
                    npc.life = (int)(npc.life * 6.6f);
                    npc.defense = (int)(npc.life * 1.2f);
                    
            }
        }

    }
}
