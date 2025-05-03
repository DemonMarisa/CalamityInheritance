using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;
using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Accessories.DashAccessories;
using CalamityInheritance.Content.Items.Accessories.Melee;
using CalamityInheritance.Content.Items.Accessories.Ranged;
using CalamityInheritance.Content.Items.Accessories.Summon;
using CalamityInheritance.Content.Items.Accessories.Wings;
using CalamityInheritance.Content.Items.Armor.GodSlayerOld;
using CalamityInheritance.Content.Items.Armor.Silva;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using BadgeofBravery = CalamityInheritance.Content.Items.Accessories.Melee.BadgeofBravery;

namespace CalamityInheritance
{
    public class CalamityInheritanceRecipes : ModSystem
    {
        // A place to store the recipe group so we can easily use it later
        public static RecipeGroup ElementalRayGroup;
        public static RecipeGroup PhantasmalFuryGroup;
        public static RecipeGroup HeliumFlashGroup;
        public static RecipeGroup WoodSwordRecipeGroup;
        public static RecipeGroup ExoTropyGroup;
        public static RecipeGroup CosmicShivGroup; 
        public static RecipeGroup DAmuletGroup;

        public static RecipeGroup PhantasmalRuinGroup;
        public static RecipeGroup TerratomereGroup;
        public static RecipeGroup ElementalShivGroup;
        public static RecipeGroup TerraRayGroup;
        public static RecipeGroup NightsRayGroup;
        public static RecipeGroup MiniGunGroup;
        public static RecipeGroup P90Group;

        public static RecipeGroup GodSlayerBodyGroup;
        public static RecipeGroup GodSlayerLegGroup;
        public static RecipeGroup GodSlayerHeadMeleeGroup;
        public static RecipeGroup GodSlayerHeadRangedGroup;
        public static RecipeGroup GodSlayerHeadRogueGroup;
        public static RecipeGroup SilvaBodyGroup;
        public static RecipeGroup SilvaLegGroup;
        public static RecipeGroup SilvaHeadMagicGroup;
        public static RecipeGroup SilvaHeadSummonGroup;

        public static RecipeGroup AmbrosialAmpoule;
        public static RecipeGroup ElysianAegis;
        public static RecipeGroup AsgardsValor;
        public static RecipeGroup DragonSpear;
        public static RecipeGroup DragonStaff;
        public static RecipeGroup DragonGun;
        public static RecipeGroup DragonGift;
        public static RecipeGroup DragonCannon;
        public static RecipeGroup DragonSky;
        public static RecipeGroup DragonSword;
        public static RecipeGroup DragonSummon;
        public static RecipeGroup Norfleet;
        public static RecipeGroup Excelsus;
        public static RecipeGroup Eradicator;
        public static RecipeGroup Arkhalis;
        public static RecipeGroup TerraBlade;
        public static RecipeGroup DartGun;
        public static RecipeGroup ClockworkBow;
        public static RecipeGroup Phangasm;
        public static RecipeGroup ElementalEruption;
        public static RecipeGroup CleansingBlaze;
        public static RecipeGroup HalleysInferno;
        public static RecipeGroup BloodBoiler;
        public static RecipeGroup LoreGolem;
        public static RecipeGroup LoreProvi;
        public static RecipeGroup LoreDoG;
        public static RecipeGroup LoreYharon;
        public static RecipeGroup LoreDuke;
        public static RecipeGroup LorePlant;
        public static RecipeGroup LoreCryogen;
        public static RecipeGroup LorePBG;
        public static RecipeGroup LoreRavager;
        public static RecipeGroup LoreAA;
        public static RecipeGroup LoreAS;
        public static RecipeGroup LoreLevi;

        public static RecipeGroup ESpear;
        public static RecipeGroup IceClasper;
        public static RecipeGroup Wand;
        public static RecipeGroup GoldBottle;
        public static RecipeGroup Arcanum;
        public static RecipeGroup PlantBow;
        public static RecipeGroup T2RangedAcc;
        public static RecipeGroup EvilFlask;
        public static RecipeGroup BloodPact;
        public static RecipeGroup GrandGelatin;
        public static RecipeGroup BraveBadge;
        public static RecipeGroup StatisNinjaBelt;
        public static RecipeGroup LunicTarcer;
        public static RecipeGroup CosmicTracer;
        public static RecipeGroup EvilBar;
        public override void Unload()
        {
            RecipeGroup[] Train =
            [
                EvilBar,
                ElementalRayGroup,
                HeliumFlashGroup,
                WoodSwordRecipeGroup,
                ExoTropyGroup,
                CosmicShivGroup,
                DAmuletGroup,
                AmbrosialAmpoule,
                ElysianAegis,
                AsgardsValor,
                PhantasmalFuryGroup,
                TerratomereGroup,
                ElementalShivGroup,
                TerraRayGroup,
                NightsRayGroup,
                MiniGunGroup,
                P90Group,
                Norfleet,
                Excelsus,
                Eradicator,
                Arkhalis,
                TerraBlade,
                DartGun,
                ClockworkBow,
                Phangasm,
                ElementalEruption,
                CleansingBlaze,
                HalleysInferno,
                BloodBoiler,
                ESpear,
                IceClasper,
                Wand,
                GoldBottle,
                Arcanum,
                PlantBow,
                T2RangedAcc,
                EvilFlask,
                BloodPact,
                GrandGelatin,
                BraveBadge,
                StatisNinjaBelt,
                LunicTarcer,
                CosmicTracer,

                GodSlayerBodyGroup,
                GodSlayerLegGroup,
                GodSlayerHeadMeleeGroup,
                GodSlayerHeadRangedGroup,
                GodSlayerHeadRogueGroup,

                SilvaBodyGroup,
                SilvaLegGroup,
                SilvaHeadMagicGroup,
                SilvaHeadSummonGroup,
                
                DragonCannon,
                DragonGift,
                DragonGun,
                DragonSky,
                DragonSpear,
                DragonStaff,
                DragonSummon,
                DragonSword,

                LoreGolem,
                LoreProvi,
                LoreDoG,
                LoreYharon,
                LoreDuke,
                LorePlant,
                LoreCryogen,
                LoreAA,
                LoreRavager,
                LorePBG,
                LoreAS,
                LoreLevi
            ];
            for (int i = 0; i < Train.Length; i++)
                Train[i] = null;
            
        }
        public override void AddRecipeGroups()
        {
            #region 其它组
            // 创建并存储一个配方组
            // Language.GetTextValue("LegacyMisc.37") 是英文中的 "Any" 一词，并对应其他语言中的相应词汇
            WoodSwordRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.WoodenSword)}",
                                                   ItemID.WoodenSword, ItemID.AshWoodSword, ItemID.BorealWoodSword,
                                                   ItemID.EbonwoodSword, ItemID.ShadewoodSword, ItemID.PearlwoodSword,
                                                   ItemID.PalmWoodSword, ModContent.ItemType<Basher>());

            ExoTropyGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<AresTrophy>())}",
                                            ModContent.ItemType<ThanatosTrophy>(), ModContent.ItemType<ApolloTrophy>(),
                                            ModContent.ItemType<ArtemisTrophy>(), ModContent.ItemType<AresTrophy>());

            //邪恶锭
            EvilBar = SetUpTwoVanilia (ItemID.DemoniteBar, ItemID.CrimtaneBar);
            #endregion

            #region 新旧弑神
            GodSlayerHeadRogueGroup     = SetUpTwo<GodSlayerHeadRogueold>   (ModContent.ItemType<GodSlayerHeadRogue>());
            GodSlayerHeadMeleeGroup     = SetUpTwo<GodSlayerHeadMeleeold>   (ModContent.ItemType<GodSlayerHeadMelee>());
            GodSlayerHeadRangedGroup    = SetUpTwo<GodSlayerHeadRangedold>  (ModContent.ItemType<GodSlayerHeadRanged>());
            GodSlayerBodyGroup          = SetUpTwo<GodSlayerChestplateold>  (ModContent.ItemType<GodSlayerChestplate>());
            GodSlayerLegGroup           = SetUpTwo<GodSlayerLeggingsold>    (ModContent.ItemType<GodSlayerLeggings>());
            #endregion

            #region 新旧林海
            SilvaBodyGroup       = SetUpTwo<SilvaArmorold>      (ModContent.ItemType<SilvaArmor>());
            SilvaLegGroup        = SetUpTwo<SilvaLeggingsold>   (ModContent.ItemType<SilvaLeggings>());
            SilvaHeadMagicGroup  = SetUpTwo<SilvaHeadMagicold>  (ModContent.ItemType<SilvaHeadMagic>());
            SilvaHeadSummonGroup = SetUpTwo<SilvaHeadSummonold> (ModContent.ItemType<SilvaHeadSummon>());
            #endregion

            #region 武器组
            ElementalRayGroup   = SetUpTwo<ElementalRayold>     (ModContent.ItemType<ElementalRay>());
            HeliumFlashGroup    = SetUpTwo<HeliumFlashLegacy>   (ModContent.ItemType<HeliumFlash>());
            PhantasmalRuinGroup = SetUpTwo<PhantasmalRuin>      (ModContent.ItemType<PhantasmalRuinold>());
            PhantasmalFuryGroup = SetUpTwo<PhantasmalFuryOld>   (ModContent.ItemType<PhantasmalFury>());
            CosmicShivGroup     = SetUpTwo<CosmicShivold>       (ModContent.ItemType<CosmicShiv>());
            TerratomereGroup    = SetUpTwo<Terratomere>         (ModContent.ItemType<TerratomereOld>());
            ElementalShivGroup  = SetUpTwo<ElementalShiv>       (ModContent.ItemType<ElementalShivold>());
            TerraRayGroup       = SetUpTwo<Photosynthesis>      (ModContent.ItemType<TerraRay>());
            NightsRayGroup      = SetUpTwo<NightsRay>           (ModContent.ItemType<NightsRayold>());
            MiniGunGroup        = SetUpTwo<Kingsbane>           (ModContent.ItemType<Minigun>());
            P90Group            = SetUpTwo<P90>                 (ModContent.ItemType<P90Legacy>());
            Norfleet            = SetUpTwo<Norfleet>            (ModContent.ItemType<NorfleetLegacy>());
            Excelsus            = SetUpTwo<ACTExcelsus>         (ModContent.ItemType<Excelsus>());
            Eradicator          = SetUpTwo<MeleeTypeEradicator> (ModContent.ItemType<Eradicator>());
            ClockworkBow        = SetUpTwo<ClockBowLegacy>      (ModContent.ItemType<ClockworkBow>());
            Phangasm            = SetUpTwo<PhangasmOS>          (ModContent.ItemType<Phangasm>());
            ElementalEruption   = SetUpTwo<ElementalEruption>   (ModContent.ItemType<ElementalEruptionLegacy>());
            CleansingBlaze      = SetUpTwo<CleansingBlaze>      (ModContent.ItemType<CleansingBlazeLegacy>());
            HalleysInferno      = SetUpTwo<HalleysInferno>      (ModContent.ItemType<HalleysInfernoLegacy>());
            BloodBoiler         = SetUpTwo<BloodBoiler>         (ModContent.ItemType<BloodBoilerLegacy>());
            ESpear              = SetUpTwo<EclipsesFall>        (ModContent.ItemType<EclipseSpear>());
            IceClasper          = SetUpTwo<AncientIceChunk>     (ModContent.ItemType<AncientAncientIceChunk>());
            PlantBow            = SetUpTwo<PlanteraLegendary>   (ModContent.ItemType<BlossomFlux>());
            DartGun             = SetUpTwoVanilia               (ItemID.DartRifle, ItemID.DartPistol);
            Arkhalis            = SetUpTwoVanilia               (ItemID.Arkhalis, ItemID.Terragrim);
            Wand                = SetUpTwoVanilia               (ItemID.WandofSparking, ItemID.WandofFrosting);
            TerraBlade          = SetUpTwoVanilia               (ItemID.TerraBlade, ModContent.ItemType<TerraEdge>());
            //旧龙系列
            DragonCannon        = SetUpTwo<ChickenCannon>       (ModContent.ItemType<ChickenCannonLegacy>());
            DragonGift          = SetUpTwo<YharimsGift>         (ModContent.ItemType<YharimsGiftLegacy>());
            DragonGun           = SetUpTwo<DragonsBreathold>    (ModContent.ItemType<AncientDragonsBreath>());
            DragonSky           = SetUpTwo<TheBurningSky>       (ModContent.ItemType<BurningSkyLegacy>());
            DragonSpear         = SetUpTwo<Wrathwing>           (ModContent.ItemType<DragonSpear>());
            DragonStaff         = SetUpTwo<PhoenixFlameBarrage> (ModContent.ItemType<DragonStaff>());
            DragonSummon        = SetUpTwo<YharonsKindleStaff>  (ModContent.ItemType<DoubleSonYharon>());
            DragonSword         = SetUpTwo<DragonRage>          (ModContent.ItemType<DragonSword>());
            #endregion

            #region 饰品
            ElysianAegis    = SetUpTwo<ElysianAegisold>         (ModContent.ItemType<ElysianAegis>());
            AsgardsValor    = SetUpTwo<AsgardsValorold>         (ModContent.ItemType<AsgardsValor>());
            GoldBottle      = SetUpTwo<AmbrosialAmpouleOld>     (ModContent.ItemType<AmbrosialAmpoule>());
            Arcanum         = SetUpTwo<AstralArcanum>           (ModContent.ItemType<InfectedJewel>());
            DAmuletGroup    = SetUpTwo<DeificAmuletLegacy>      (ModContent.ItemType<DeificAmulet>());
            T2RangedAcc     = SetUpTwo<DeadshotBrooch>          (ModContent.ItemType<DaedalusEmblem>());
            EvilFlask       = SetUpTwo<CrimsonFlask>            (ModContent.ItemType<CorruptFlask>());
            BloodPact       = SetUpTwo<BloodPactLegacy>         (ModContent.ItemType<BloodPact>());
            GrandGelatin    = SetUpTwo<GrandGelatinLegacy>      (ModContent.ItemType<GrandGelatin>());
            BraveBadge      = SetUpTwo<BadgeofBravery>          (ModContent.ItemType<CalamityMod.Items.Accessories.BadgeofBravery>());
            StatisNinjaBelt = SetUpTwo<StatisNinjaBeltLegacy>   (ModContent.ItemType<StatisNinjaBelt>());
            CosmicTracer    = SetUpTwo<FasterGodSlayerTracers>  (ModContent.ItemType<TracersElysian>());
            LunicTarcer     = SetUpTwo<FasterLunarTracers>      (ModContent.ItemType<TracersCelestial>());
            #endregion

            #region 传颂之物
            LoreGolem   = SetUpTwo<KnowledgeGolem>              (ModContent.ItemType<LoreGolem>());
            LoreProvi   = SetUpTwo<KnowledgeProvidence>         (ModContent.ItemType<LoreProvidence>()); 
            LoreDoG     = SetUpTwo<KnowledgeDevourerofGods>     (ModContent.ItemType<LoreDevourerofGods>());
            LoreYharon  = SetUpTwo<KnowledgeYharon>             (ModContent.ItemType<LoreYharon>());
            LoreDuke    = SetUpTwo<KnowledgeDukeFishron>        (ModContent.ItemType<LoreDukeFishron>());
            LorePlant   = SetUpTwo<KnowledgePlantera>           (ModContent.ItemType<LorePlantera>());
            LoreCryogen = SetUpTwo<KnowledgeCryogen>            (ModContent.ItemType<LoreArchmage>());
            LoreAA      = SetUpTwo<KnowledgeAstrumAureus>       (ModContent.ItemType<LoreAstrumAureus>());
            LoreRavager = SetUpTwo<KnowledgeRavager>            (ModContent.ItemType<LoreRavager>());
            LoreAS      = SetUpTwo<KnowledgeAquaticScourge>     (ModContent.ItemType<LoreAquaticScourge>());
            LoreLevi    = SetUpTwo<KnowledgeLeviathanAnahita>   (ModContent.ItemType<LoreLeviathanAnahita>());
            LorePBG     = SetUpTwo<KnowledgePlaguebringerGoliath>(ModContent.ItemType<LorePlaguebringerGoliath>());
            #endregion

            #region 其它组
            // 为了避免名称冲突，当模组物品是配方组的标志性或第一个物品时，命名配方组为：ModName:ItemName
            //咱就是说，能不能用点简单的名字.
            //排序请用字母表顺序谢谢
            EvilBar.                    NameHelper("AnyDemoniteBar");
            WoodSwordRecipeGroup.       NameHelper("AnyWoodenSword");
            ExoTropyGroup.              NameHelper("AnyExoTropy");
            CosmicShivGroup.            NameHelper("AnyCosmicShiv");
            DAmuletGroup.               NameHelper("AnyDeificAmulet");
            Arkhalis.                   NameHelper("AnyArkhalis");
            #endregion
            //饰品
            //阿斯加德系
            ElysianAegis.               NameHelper("AnyElysianAegis");
            AsgardsValor.               NameHelper("AnyAsgardsValor");
            //百草
            GoldBottle.                 NameHelper("AnyAmbrosialAmpoule");
            //奥秘
            Arcanum.                    NameHelper("AnyAstralArcanum");
            //代达罗斯
            T2RangedAcc.                NameHelper("AnyDaedalusEmblem");
            //我也忘了是啥了，你随便看吧
            EvilFlask.                  NameHelper("AnyCrimsonFlask");
            //血契
            BloodPact.                  NameHelper("AnyBloodPact");
            GrandGelatin.               NameHelper("AnyGrandGelatin");
            BraveBadge.                 NameHelper("AnyBadgeofBravery");
            StatisNinjaBelt.            NameHelper("AnyStatisNinjaBelt");
            LunicTarcer.                NameHelper("AnyTracersCelestial");
            CosmicTracer.               NameHelper("AnyTracersElysian");

            #region 新旧弑神
            GodSlayerBodyGroup.         NameHelper("AnyGodSlayerBody");
            GodSlayerLegGroup.          NameHelper("AnyGodSlayerLeg");
            GodSlayerHeadMeleeGroup.    NameHelper("AnyGodSlayerHeadMelee");
            GodSlayerHeadRangedGroup.   NameHelper("AnyGodSlayerHeadRanged");
            GodSlayerHeadRogueGroup.    NameHelper("AnyGodSlayerHeadRogue");
            #endregion

            #region 新旧林海
            SilvaBodyGroup.             NameHelper("AnySilvaBody");
            SilvaLegGroup.              NameHelper("AnySilvaLeg");
            SilvaHeadMagicGroup.        NameHelper("AnySilvaHeadMagic");
            SilvaHeadSummonGroup.       NameHelper("AnySilvaHeadSummon");
            #endregion

            #region 武器组
            BloodBoiler.                NameHelper("AnyBloodBoiler");
            CleansingBlaze.             NameHelper("AnyCleansingBlaze");
            ClockworkBow.               NameHelper("AnyClockworkBow");
            DartGun.                    NameHelper("AnyDartGun");
            ElementalEruption.          NameHelper("AnyElementalEruption");
            ElementalRayGroup.          NameHelper("AnyElementalRay");
            ElementalShivGroup.         NameHelper("AnyElementalShiv");
            Eradicator.                 NameHelper("AnyEradicator");
            ESpear.                     NameHelper("AnyEclipsesFall");
            Excelsus.                   NameHelper("AnyExcelsus");
            HalleysInferno.             NameHelper("AnyHalleysInferno");
            HeliumFlashGroup.           NameHelper("AnyHeliumFlash");
            IceClasper.                 NameHelper("AnyAncientIceChunk");
            MiniGunGroup.               NameHelper("AnyMiniGun");
            NightsRayGroup.             NameHelper("AnyNightsRay");
            Norfleet.                   NameHelper("AnyNorfleet");
            P90Group.                   NameHelper("AnyP90");
            PlantBow.                   NameHelper("AnyBlossomFlux");
            PhantasmalFuryGroup.        NameHelper("AnyPhantasmalFury");
            PhantasmalRuinGroup.        NameHelper("AnyPhantasmalRuin");
            Phangasm.                   NameHelper("AnyPhangasm");
            TerraRayGroup.              NameHelper("AnyTerraRay");
            TerratomereGroup.           NameHelper("AnyTerratomere");
            TerraBlade.                 NameHelper("AnyTerraBlade");
            Wand.                       NameHelper("AnyWand");
            #endregion
            

            #region 旧龙武器，给龙弓用
            DragonCannon.               NameHelper("AnyChickenCannon");
            DragonGift.                 NameHelper("AnyYharim'sGift");
            DragonGun.                  NameHelper("AnyDragonsBreathLegacy");
            DragonSky.                  NameHelper("AnyBurningSky");
            DragonSpear.                NameHelper("AnyWrathwing");
            DragonStaff.                NameHelper("AnyPhoenixFlameBarrage");
            DragonSummon.               NameHelper("AnyYharon'sKindleStaff");
            DragonSword.                NameHelper("AnyDragonRage");
            #endregion
            #region 传颂
            LoreGolem.                  NameHelper("AnyLoreGolem");
            LoreProvi.                  NameHelper("AnyLoreProvidence");
            LoreDoG.                    NameHelper("AnyLoreDevourerofGods");
            LoreYharon.                 NameHelper("AnyLoreYharon");
            LoreDuke.                   NameHelper("AnyLoreDuke");
            LorePlant.                  NameHelper("AnyLorePlantera");
            LoreCryogen.                NameHelper("AnyLoreCryogen");
            LoreAA.                     NameHelper("AnyLoreAstrumAureus");
            LorePBG.                    NameHelper("AnyLorePlaguebringerGoliath");
            LoreRavager.                NameHelper("AnyLoreRavager");
            LoreAS.                     NameHelper("AnyLoreAquaticScourge");
            LoreLevi.                   NameHelper("AnyLoreLeviathanAnahita");
            #endregion
        }
        public static RecipeGroup SetUpTwoVanilia (int showOnRecipe, int another)
        {
            return new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(showOnRecipe)}", showOnRecipe, another);
        }
        public static RecipeGroup SetUpTwo<T> (int showOnRecipe) where T : ModItem
        {
            return new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(showOnRecipe)}", showOnRecipe, ModContent.ItemType<T>());
        }
        // public static RecipeGroup SetUpGroup<T> (int showOnRecipe, int[] group) where T : ModItem
        // {
        //     return new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(showOnRecipe)}", showOnRecipe, group);
        // }
        public static void RegisterHelper(RecipeGroup group, string groupName)
        {
            RecipeGroup.RegisterGroup("CIMod:" + groupName, group);
        }
    }
    public class CIRecipeGroup
    {
        public static string DragonCannon       => "AnyChickenCannon".GetGroupName();
        public static string DragonGift         => "AnyYharim'sGift".GetGroupName(); 
        public static string DragonGun          => "AnyDragonsBreathLegacy".GetGroupName();
        public static string DragonSky          => "AnyBurningSky".GetGroupName();
        public static string DragonSpear        => "AnyWrathwing".GetGroupName();
        public static string DragonStaff        => "AnyPhoenixFlameBarrage".GetGroupName();
        public static string DragonSword        => "AnyDragonRage".GetGroupName();
        public static string DragonSummon       => "AnyYharon'sKindleStaff".GetGroupName();
        public static string Excelsus           => "AnyExcelsus".GetGroupName();
        public static string Eradicator         => "AnyEradicator".GetGroupName();
        public static string Norfleet           => "AnyNorfleet".GetGroupName();
        public static string Arkhalis           => "AnyArkhalis".GetGroupName();
        public static string TerraBlade         => "AnyTerraBlade".GetGroupName();
        public static string DartGun            => "AnyDartGun".GetGroupName();
        public static string ClockworkBow       => "AnyClockworkBow".GetGroupName();
        public static string Phangasm           => "AnyPhangasm".GetGroupName();
        #region 喷火器
        public static string ElementalEruption  => "AnyElementalEruption".GetGroupName();
        public static string CleansingBlaze     => "AnyCleansingBlaze".GetGroupName();
        public static string HalleysInferno     => "AnyHalleysInferno".GetGroupName();
        public static string BloodBoiler        => "AnyBloodBoiler".GetGroupName();
        #endregion
        #region Lores
        public static string LoreGolem          => "AnyLoreGolem".GetGroupName();
        public static string LoreProvidence     => "AnyLoreProvidence".GetGroupName();
        public static string LoreDevourerofGods => "AnyLoreDevourerofGods".GetGroupName();
        public static string LoreYharon         => "AnyLoreYharon".GetGroupName();
        public static string LoreDuke           => "AnyLoreDuke".GetGroupName();
        public static string LorePlant          => "AnyLorePlantera".GetGroupName();
        public static string LoreCryo           => "AnyLoreCryogen".GetGroupName();
        public static string LoreRavager        => "AnyLoreRavager".GetGroupName();
        public static string LoreAA             => "AnyLoreAstrumAureus".GetGroupName();
        public static string LorePBG            => "AnyLorePlaguebringerGoliath".GetGroupName();
        public static string LoreAS             => "AnyLoreAquaticScourge".GetGroupName();
        public static string LoreLevi           => "AnyLoreLeviathanAnahita".GetGroupName();
        #endregion
        public static string EclipsesFall       => "AnyEclipsesFall".GetGroupName();
        public static string AncientIceChunk    => "AnyAncientIceChunk".GetGroupName();
        public static string WandofSparking     => "AnyWand".GetGroupName();
        public static string GoldBottle         => "AnyAmbrosialAmpoule".GetGroupName();
        public static string AstralArcanum      => "AnyAstralArcanum".GetGroupName();
        public static string BlossomFlux        => "AnyBlossomFlux".GetGroupName();
        public static string DaedalusEmblem     => "AnyDaedalusEmblem".GetGroupName(); 
        public static string CrimsonFlask       => "AnyCrimsonFlask".GetGroupName();
        public static string BloodPact          => "AnyBloodPact".GetGroupName();
        public static string GrandGelatin       => "AnyGrandGelatin".GetGroupName();
        public static string BadgeofBravery     => "AnyBadgeofBravery".GetGroupName();
        public static string StatisNinjaBelt    => "AnyStatisNinjaBelt".GetGroupName(); 
        public static string TracersCelestial   => "AnyTracersCelestial".GetGroupName();
        public static string TracersElysian     => "AnyTracersElysian".GetGroupName();
        public static string DemoniteBar        => "AnyDemoniteBar".GetGroupName();

    }

}
