using CalamityInheritance.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Weapons.Magic;
using Terraria.GameContent.ItemDropRules;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.NPCs.CalamityAIs.CalamityBossAIs;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.ExoMechs;
using CalamityMod.Items.Weapons.Ranged;
using System.Collections;
using CalamityMod.Items.Weapons.Summon;
using CalamityInheritance.Content.Items.Placeables.MusicBox;
using CalamityMod.Projectiles.Pets;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.Yharon;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.SupremeCalamitas;
using System.Security.Policy;
using static System.Net.Mime.MediaTypeNames;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Content.Items.Weapons.Legendary;

namespace CalamityInheritance.System
{
    public class CalStatInflationBACK : ModSystem
    {
        // 存储武器类型
        public static List<int> PostMLWeapons = new List<int>();
        public static List<int> PostProfanedWeapons = new List<int>();
        public static List<int> PostPolterghastWeapons = new List<int>();
        public static List<int> PostOldDukeWeapons = new List<int>();
        public static List<int> PostDOGWeapons = new List<int>();
        public static List<int> PostyharonWeapons = new List<int>();
        public static List<int> PostExoAndScalWeapons = new List<int>();
        public static List<int> PostShadowspecWeapons = new List<int>();
        // 加载/卸载列表
        public override void Load()
        {
            PostMLWeapons = [];
            PostProfanedWeapons = [];
            PostPolterghastWeapons = [];
            PostOldDukeWeapons = [];
            PostDOGWeapons = [];
            PostyharonWeapons = [];
            PostExoAndScalWeapons = [];
            PostShadowspecWeapons = [];
        }

        public override void Unload()
        {
            PostMLWeapons = null;
            PostProfanedWeapons = null;
            PostPolterghastWeapons = null;
            PostOldDukeWeapons = null;
            PostDOGWeapons = null;
            PostyharonWeapons = null;
            PostExoAndScalWeapons = null;
            PostShadowspecWeapons = null;
        }
        public override void PostAddRecipes()
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
            #endregion
            #region 神长表单删除
            // 怎么你也没有过滤金源武器
            PostDOGWeapons.Remove(ModContent.ItemType<Ataraxia>());
            #endregion
            #region 龙后
            PostyharonWeapons.Add(ModContent.ItemType<Murasama>());
            PostyharonWeapons.Add(ItemID.Zenith);
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
                    && recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity
                    && !recipe.HasIngredient(ModContent.ItemType<CosmiliteBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<TwistingNether>())
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
                    && recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity
                    && !recipe.HasIngredient(ModContent.ItemType<CosmiliteBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<TwistingNether>())
                    && !recipe.HasIngredient(ModContent.ItemType<AuricBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<ReaperTooth>())
                    && !recipe.HasIngredient(ModContent.ItemType<RuinousSoul>());
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
                    && recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity
                    && !recipe.HasIngredient(ModContent.ItemType<CosmiliteBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<ShadowspecBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<AuricBar>());
        }
        #endregion
        #region 神后武器
        public static bool PostDOGWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有宇宙锭/ 必须没有金源锭/魔影锭
            return item.damage > 0
                    && (recipe.HasIngredient(ModContent.ItemType<CosmiliteBar>()) || recipe.HasIngredient(ModContent.ItemType<AscendantSpiritEssence>()))
                    && recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity
                    && !recipe.HasIngredient(ModContent.ItemType<ShadowspecBar>())
                    && !recipe.HasIngredient(ModContent.ItemType<AuricBar>());
        }
        #endregion
        #region 龙后武器
        public static bool PostYharonWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有宇宙锭/ 必须没有金源锭/魔影锭
            return item.damage > 0
                    && (recipe.HasIngredient(ModContent.ItemType<AuricBar>())
                    || recipe.HasIngredient(ModContent.ItemType<YharonSoulFragment>()))
                    && recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity
                    && !recipe.HasIngredient(ModContent.ItemType<ShadowspecBar>());
        }
        #endregion
        #region 魔影武器
        public static bool PostShadowspecWeapon(Recipe recipe, Item item)
        {
            // 必须是有伤害的 必须合成表有魔影锭
            return item.damage > 0
                    && recipe.HasIngredient(ModContent.ItemType<ShadowspecBar>())
                    && recipe.createItem.ModItem.Mod == CalamityInheritance.Calamity;
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
        public const float PostProfanedWeaponsBoost = 1.6f; // 亵渎后
        public const float PostPolterghastWeaponsBoost = 2f; // 幽花后
        public const float PostOldDukeWeaponsBoost = 2.4f; // 幽花后
        public const float PostDOGWeaponsBoost = 3.3f; // 神后
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
                if (CalStatInflationBACK.PostProfanedWeapons.Contains(item.type))
                    item.damage = (int)(item.damage * PostProfanedWeaponsBoost);
                if (CalStatInflationBACK.PostPolterghastWeapons.Contains(item.type))
                    item.damage = (int)(item.damage * PostPolterghastWeaponsBoost);
                if (CalStatInflationBACK.PostOldDukeWeapons.Contains(item.type))
                    item.damage = (int)(item.damage * PostOldDukeWeaponsBoost);
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
            }
        }
        #endregion
        #region 特殊平衡改动
        public static void AuricBlance(Item item)
        {
            if (item.type == ModContent.ItemType<DragonRage>())
                item.damage = 4000;
            if (item.type == ModContent.ItemType<TheOracle>())
                item.damage = 1500;
            if (item.type == ModContent.ItemType<HeliumFlash>())
                item.damage = 6666;
            if (item.type == ModContent.ItemType<HeliumFlash>())
                item.damage = 6666;
            if (item.type == ModContent.ItemType<VoidVortex>())
                item.damage = 1800;
            if (item.type == ModContent.ItemType<YharimsCrystal>())
                item.damage = 600;
            if (item.type == ModContent.ItemType<ArkoftheCosmos>())
                item.damage = 14000;
            if (item.type == ModContent.ItemType<YharonsKindleStaff>() || item.type == ModContent.ItemType<MidnightSunBeacon>())
                item.damage = (int)(item.damage * 2.4f);
        }
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
                    npc.lifeMax = (int)(npc.lifeMax * 1.8f);
                    npc.life = (int)(npc.life * 1.8f);
                    npc.defense = (int)(npc.defense * 1.8f);
                }
                if (CalamityInheritanceLists.DOG.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 2.5f);
                    npc.life = (int)(npc.life * 2.5f);
                    npc.defense = (int)(npc.defense * 2.5f);
                }
                if (npc.type == ModContent.NPCType<Yharon>())
                {
                    npc.lifeMax = (int)(npc.lifeMax * 4f);
                    npc.life = (int)(npc.life * 4f);
                    npc.defense = (int)(npc.defense * 4f);
                }
                if (CalamityInheritanceLists.ExoMech.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 5f);
                    npc.life = (int)(npc.life * 5f);
                    npc.defense = (int)(npc.defense * 5f);
                }
                if (CalamityInheritanceLists.Scal.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 6.6f);
                    npc.life = (int)(npc.life * 6.6f);
                    npc.defense = (int)(npc.defense * 6.6f);
                }
                if (CalamityMod.Events.BossRushEvent.BossRushActive)
                {
                    npc.lifeMax = (int)(npc.lifeMax * 10f);
                    npc.life = (int)(npc.life * 10f);
                    npc.defense = (int)(npc.defense * 10f);
                }
            }
        }

    }
}
