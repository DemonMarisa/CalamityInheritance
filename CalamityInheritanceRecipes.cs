using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
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
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Placeables.MusicBox;
using CalamityInheritance.Content.Items.Placeables.Relic;
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
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.DraedonStructures;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.Placeables.FurnitureAbyss;
using CalamityMod.Items.Placeables.FurnitureAcidwood;
using CalamityMod.Items.Placeables.FurnitureAshen;
using CalamityMod.Items.Placeables.FurnitureBotanic;
using CalamityMod.Items.Placeables.FurnitureCosmilite;
using CalamityMod.Items.Placeables.FurnitureEutrophic;
using CalamityMod.Items.Placeables.FurnitureExo;
using CalamityMod.Items.Placeables.FurnitureMonolith;
using CalamityMod.Items.Placeables.FurnitureOtherworldly;
using CalamityMod.Items.Placeables.FurniturePlagued;
using CalamityMod.Items.Placeables.FurnitureProfaned;
using CalamityMod.Items.Placeables.FurnitureSacrilegious;
using CalamityMod.Items.Placeables.FurnitureSilva;
using CalamityMod.Items.Placeables.FurnitureStatigel;
using CalamityMod.Items.Placeables.FurnitureStratus;
using CalamityMod.Items.Placeables.FurnitureVoid;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.Signus;
using CalamityMod.Projectiles.Magic;
using Steamworks;
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
        public static RecipeGroup ElementalRay;
        public static RecipeGroup PhantasmalFury;
        public static RecipeGroup HeliumFlash;
        public static RecipeGroup WoodSwordRecipeGroup;
        public static RecipeGroup ExoTropyGroup;
        public static RecipeGroup CosmicShiv; 
        public static RecipeGroup DeificAmulet;

        public static RecipeGroup PhantasmalRuin;
        public static RecipeGroup Terratomere;
        public static RecipeGroup ElementalShiv;
        public static RecipeGroup TerraRay;
        public static RecipeGroup NightsRay;
        public static RecipeGroup MiniGun;
        public static RecipeGroup P90;

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
        public static RecipeGroup LorePostSCal;
        public static RecipeGroup LoreMoonLord;
        public static RecipeGroup LoreAD;

        public static RecipeGroup EclipseFall;
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
        public static RecipeGroup LumiStriker;
        public static RecipeGroup LoreSentinal;
        public static RecipeGroup TrophySentinal;
        public static RecipeGroup TwinTrophy;
        public static RecipeGroup LeviTrophy;
        public static RecipeGroup MechTrophy;
        public static RecipeGroup SCalTrophy;
        public static RecipeGroup RottenMatter;
        public static RecipeGroup CryoBar;
        public static RecipeGroup OpalStriker;
        public static RecipeGroup MagnaCannon;
        public static RecipeGroup SoulEdge;
        public static RecipeGroup OhMyGodIsChest;
        public static RecipeGroup MoonMusicBox;
        public static RecipeGroup RareReaper;
        //下版本处理
        public static RecipeGroup SeaShield;

        public override void Unload()
        {
            RecipeGroup[] Train =
            [
                EvilBar,
                ElementalRay,
                HeliumFlash,
                WoodSwordRecipeGroup,
                ExoTropyGroup,
                CosmicShiv,
                DeificAmulet,
                AmbrosialAmpoule,
                ElysianAegis,
                AsgardsValor,
                PhantasmalFury,
                Terratomere,
                ElementalShiv,
                TerraRay,
                NightsRay,
                MiniGun,
                P90,
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
                EclipseFall,
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
                LumiStriker,
                TrophySentinal,
                TwinTrophy,
                LeviTrophy,
                MechTrophy,
                SCalTrophy,
                RottenMatter,
                CryoBar,
                OpalStriker,
                MagnaCannon,
                SoulEdge,
                OhMyGodIsChest,
                MoonMusicBox,
                RareReaper,
                SeaShield,

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
                LoreLevi,
                LoreSentinal,
                LorePostSCal,
                LoreAD
            ];
            for (int i = 0; i < Train.Length; i++)
                Train[i] = null;
            
        }
        public override void AddRecipeGroups()
        {
            //旧的死板方法(SetUpTwo)除非需要扩展否则不太建议改了，但也不建议删除。
            //当然如果你想全部改成新的install方法的话也可以。反正我懒了
            #region 其它
            WoodSwordRecipeGroup = InstallIncludeVanilla(ItemID.WoodenSword, ItemID.WoodenSword ,ItemID.AshWoodSword, ItemID.BorealWoodSword, ItemID.EbonwoodSword, ItemID.ShadewoodSword, ItemID.PearlwoodSword, ItemID.PalmWoodSword, Item<Basher>());
            ExoTropyGroup = InstallGroupMod<AresTrophy>(Item<AresTrophy>(), Item<ArtemisTrophy>(), Item<ThanatosTrophy>(), Item<ApolloTrophy>());
            TrophySentinal = InstallGroupMod<WeaverTrophy>(Item<WeaverTrophy>(), Item<SignusTrophy>(), Item<CeaselessVoidTrophy>());
            MechTrophy = InstallIncludeVanilla(ItemID.SkeletronPrimeTrophy, ItemID.SkeletronPrimeTrophy, ItemID.RetinazerTrophy, ItemID.DestroyerTrophy, ItemID.SpazmatismTrophy);
            //邪恶锭
            EvilBar = SetUpTwoVanilia (ItemID.DemoniteBar, ItemID.CrimtaneBar);
            TwinTrophy = SetUpTwoVanilia (ItemID.RetinazerTrophy, ItemID.SpazmatismTrophy);
            LeviTrophy = SetUpTwo<LeviathanTrophy>(Item<AnahitaTrophy>());
            SCalTrophy = SetUpTwo<ScalTrophy>(Item<SupremeCalamitasTrophy>());
            RottenMatter = SetUpTwo<BloodSample>(Item<RottenMatter>());
            CryoBar = SetUpTwo<CryonicBar>(Item<CryoBar>());
            MoonMusicBox = InstallGroupMod<BlessingOftheMoon>(Item<BlessingOftheMoon>(), Item<Arcueid>());
            #region 箱子
            OhMyGodIsChest = InstallIncludeVanilla(
                //原版箱子
                ItemID.Chest,

                ItemID.Chest,
                ItemID.AshWoodChest,
                ItemID.BalloonChest,
                ItemID.BambooChest,
                ItemID.Barrel,
                ItemID.BlueDungeonChest,
                ItemID.BorealWoodChest,
                ItemID.BoneChest,
                ItemID.CactusChest,
                ItemID.CorruptionChest,
                ItemID.CoralChest,
                ItemID.CrimsonChest,
                ItemID.CrystalChest,
                ItemID.DeadMansChest,
                ItemID.DesertChest,
                ItemID.DynastyChest,
                ItemID.EbonwoodChest,
                ItemID.FrozenChest,
                ItemID.FleshChest,
                ItemID.GlassChest,
                ItemID.GoldChest,
                ItemID.GoldenChest,
                ItemID.GolfChest,
                ItemID.GraniteChest,
                ItemID.GreenDungeonChest,
                ItemID.HallowedChest,
                ItemID.HoneyChest,
                ItemID.IvyChest,
                ItemID.JungleChest,
                ItemID.LesionChest,
                ItemID.LihzahrdChest,
                ItemID.LivingWoodChest,
                ItemID.MarbleChest,
                ItemID.MartianChest,
                ItemID.MeteoriteChest,
                ItemID.MushroomChest,
                ItemID.NebulaChest,
                ItemID.ObsidianChest,
                ItemID.PalmWoodChest,
                ItemID.PearlwoodChest,
                ItemID.PinkDungeonChest,
                ItemID.PumpkinChest,
                ItemID.RichMahoganyChest,
                ItemID.ShadowChest,
                ItemID.ShadewoodChest,
                ItemID.SkywareChest,
                ItemID.SlimeChest,
                ItemID.SolarChest,
                ItemID.SpiderChest,
                ItemID.SpookyChest,
                ItemID.SteampunkChest,
                ItemID.TrashCan,
                ItemID.WaterChest,
                ItemID.WebCoveredChest,
                ItemID.VortexChest,
                //陷阱箱子
                ItemID.Fake_BalloonChest,
                ItemID.Fake_BambooChest,
                ItemID.Fake_BlueDungeonChest,
                ItemID.Fake_BoneChest,
                ItemID.Fake_BorealWoodChest,
                ItemID.Fake_CactusChest,
                ItemID.Fake_Chest,
                ItemID.Fake_CorruptionChest,
                ItemID.Fake_CoralChest,
                ItemID.Fake_CrimsonChest,
                ItemID.Fake_CrystalChest,
                ItemID.Fake_DesertChest,
                ItemID.Fake_DynastyChest,
                ItemID.Fake_EbonwoodChest,
                ItemID.Fake_FleshChest,
                ItemID.Fake_FrozenChest,
                ItemID.Fake_GlassChest,
                ItemID.Fake_GoldChest,
                ItemID.Fake_GoldenChest,
                ItemID.Fake_GolfChest,
                ItemID.Fake_GraniteChest,
                ItemID.Fake_GreenDungeonChest,
                ItemID.Fake_HallowedChest,
                ItemID.Fake_HoneyChest,
                ItemID.Fake_IceChest,
                ItemID.Fake_IvyChest,
                ItemID.Fake_JungleChest,
                ItemID.Fake_LesionChest,
                ItemID.Fake_LihzahrdChest,
                ItemID.Fake_LivingWoodChest,
                ItemID.Fake_MarbleChest,
                ItemID.Fake_MartianChest,
                ItemID.Fake_MeteoriteChest,
                ItemID.Fake_MushroomChest,
                ItemID.Fake_NebulaChest,
                ItemID.Fake_ObsidianChest,
                ItemID.Fake_PalmWoodChest,
                ItemID.Fake_PearlwoodChest,
                ItemID.Fake_PinkDungeonChest,
                ItemID.Fake_PumpkinChest,
                ItemID.Fake_RichMahoganyChest,
                ItemID.Fake_ShadewoodChest,
                ItemID.Fake_ShadowChest,
                ItemID.Fake_SkywareChest,
                ItemID.Fake_SlimeChest,
                ItemID.Fake_SolarChest,
                ItemID.Fake_SpiderChest,
                ItemID.Fake_SpookyChest,
                ItemID.Fake_StardustChest,
                ItemID.Fake_SteampunkChest,
                ItemID.Fake_VortexChest,
                ItemID.Fake_WaterChest,
                ItemID.Fake_WebCoveredChest,
                //能存东西的话为什么这几个不算箱子？
                ItemID.PiggyBank,
                ItemID.Safe,
                ItemID.VoidVault,
                ItemID.DefendersForge,
                ItemID.ChesterPetItem,
                ItemID.MoneyTrough,
                //灾厄箱子
                Item<AbyssChest>(),
                Item<AcidwoodChest>(),
                Item<AgedSecurityChest>(),
                Item<AshenChest>(),
                Item<AstralChest>(),
                Item<BotanicChest>(),
                Item<CosmiliteChest>(),
                Item<ExoChest>(),
                Item<EutrophicChest>(),
                Item<MonolithChest>(),
                Item<OtherworldlyChest>(),
                Item<PlaguedPlateChest>(),
                Item<ProfanedChest>(),
                Item<RustyChest>(),
                Item<SacrilegiousChest>(),
                Item<SecurityChest>(),
                Item<SilvaChest>(),
                Item<StatigelChest>(),
                Item<StratusChest>(),
                Item<VoidChest>()
                );
            #endregion
            #endregion

            #region 新旧弑神
            GodSlayerHeadRogueGroup     = SetUpTwo<GodSlayerHeadRogueold>   (Item<GodSlayerHeadRogue>());
            GodSlayerHeadMeleeGroup     = SetUpTwo<GodSlayerHeadMeleeold>   (Item<GodSlayerHeadMelee>());
            GodSlayerHeadRangedGroup    = SetUpTwo<GodSlayerHeadRangedold>  (Item<GodSlayerHeadRanged>());
            GodSlayerBodyGroup          = SetUpTwo<GodSlayerChestplateold>  (Item<GodSlayerChestplate>());
            GodSlayerLegGroup           = SetUpTwo<GodSlayerLeggingsold>    (Item<GodSlayerLeggings>());
            #endregion

            #region 新旧林海
            SilvaBodyGroup       = SetUpTwo<SilvaArmorold>      (Item<SilvaArmor>());
            SilvaLegGroup        = SetUpTwo<SilvaLeggingsold>   (Item<SilvaLeggings>());
            SilvaHeadMagicGroup  = SetUpTwo<SilvaHeadMagicold>  (Item<SilvaHeadMagic>());
            SilvaHeadSummonGroup = SetUpTwo<SilvaHeadSummonold> (Item<SilvaHeadSummon>());
            #endregion

            #region 武器组
            ElementalRay        = SetUpTwo<ElementalRay>                (Item<ElementalRayold>());
            HeliumFlash         = SetUpTwo<HeliumFlash>                 (Item<HeliumFlashLegacy>());
            PhantasmalRuin      = SetUpTwo<PhantasmalRuin>              (Item<PhantasmalRuinold>());
            PhantasmalFury      = SetUpTwo<PhantasmalFury>              (Item<PhantasmalFuryOld>());
            CosmicShiv          = SetUpTwo<CosmicShiv>                  (Item<CosmicShivold>());
            Terratomere         = SetUpTwo<Terratomere>                 (Item<TerratomereOld>());
            ElementalShiv       = SetUpTwo<ElementalShiv>               (Item<ElementalShivold>());
            TerraRay            = SetUpTwo<Photosynthesis>              (Item<TerraRay>());
            NightsRay           = SetUpTwo<NightsRay>                   (Item<NightsRayold>());
            MiniGun             = SetUpTwo<Kingsbane>                   (Item<Minigun>());
            P90                 = SetUpTwo<P90>                         (Item<P90Legacy>());
            Norfleet            = SetUpTwo<Norfleet>                    (Item<NorfleetLegacy>());
            Excelsus            = SetUpTwo<ACTExcelsus>                 (Item<Excelsus>());
            Eradicator          = SetUpTwo<Eradicator>                  (Item<MeleeTypeEradicator>());
            ClockworkBow        = SetUpTwo<ClockworkBow>                (Item<ClockBowLegacy>());
            Phangasm            = SetUpTwo<Phangasm>                    (Item<PhangasmOS>());
            ElementalEruption   = SetUpTwo<ElementalEruption>           (Item<ElementalEruptionLegacy>());
            CleansingBlaze      = SetUpTwo<CleansingBlaze>              (Item<CleansingBlazeLegacy>());
            HalleysInferno      = SetUpTwo<HalleysInferno>              (Item<HalleysInfernoLegacy>());
            BloodBoiler         = SetUpTwo<BloodBoiler>                 (Item<BloodBoilerLegacy>());
            EclipseFall         = SetUpTwo<EclipsesFall>                (Item<EclipseSpear>());
            IceClasper          = SetUpTwo<AncientIceChunk>             (Item<AncientAncientIceChunk>());
            PlantBow            = SetUpTwo<PlanteraLegendary>           (Item<BlossomFlux>());
            LumiStriker         = SetUpTwo<RealityRupture>              (Item<LumiStriker>());
            OpalStriker         = SetUpTwo<OpalStriker>                 (Item<OpalStrikerLegacy>());
            MagnaCannon         = SetUpTwo<MagnaCannon>                 (Item<MagnaCannonLegacy>());
            SoulEdge            = InstallGroupMod<SoulEdge>             (Item<SoulEdge>(),Item<VoidEdge>());
            RareReaper          = InstallGroupMod<Valediction>          (Item<Valediction>(), Item<TheOldReaper>());
            DartGun             = SetUpTwoVanilia                       (ItemID.DartRifle, ItemID.DartPistol);
            Arkhalis            = SetUpTwoVanilia                       (ItemID.Arkhalis, ItemID.Terragrim);
            Wand                = SetUpTwoVanilia                       (ItemID.WandofSparking, ItemID.WandofFrosting);
            TerraBlade          = SetUpTwoVanilia                       (ItemID.TerraBlade, Item<TerraEdge>());
            //旧龙系列
            DragonCannon        = SetUpTwo<ChickenCannon>               (Item<ChickenCannonLegacy>());
            DragonGift          = SetUpTwo<YharimsGift>                 (Item<YharimsGiftLegacy>());
            DragonGun           = SetUpTwo<AncientDragonsBreath>        (Item<DragonsBreathold>());
            DragonSky           = SetUpTwo<TheBurningSky>               (Item<BurningSkyLegacy>());
            DragonSpear         = SetUpTwo<Wrathwing>                   (Item<DragonSpear>());
            DragonStaff         = SetUpTwo<PhoenixFlameBarrage>         (Item<DragonStaff>());
            DragonSummon        = SetUpTwo<YharonsKindleStaff>          (Item<DoubleSonYharon>());
            DragonSword         = SetUpTwo<DragonRage>                  (Item<DragonSword>());
            #endregion

            //把所有合成组用旧物品显示避免混淆以及展现一点倾向性
            #region 饰品
            ElysianAegis    = SetUpTwo<ElysianAegis>            (Item<ElysianAegisold>());
            AsgardsValor    = SetUpTwo<AsgardsValor>            (Item<AsgardsValorold>());
            GoldBottle      = SetUpTwo<AmbrosialAmpoule>        (Item<AmbrosialAmpouleOld>());
            Arcanum         = SetUpTwo<InfectedJewel>           (Item<AstralArcanum>());
            DeificAmulet    = SetUpTwo<DeificAmulet>            (Item<DeificAmuletLegacy>());
            T2RangedAcc     = SetUpTwo<DeadshotBrooch>          (Item<DaedalusEmblem>());
            EvilFlask       = SetUpTwo<CrimsonFlask>            (Item<CorruptFlask>());
            BloodPact       = SetUpTwo<BloodPact>               (Item<BloodPactLegacy>());
            GrandGelatin    = SetUpTwo<GrandGelatin>            (Item<GrandGelatinLegacy>());
            StatisNinjaBelt = SetUpTwo<StatisNinjaBelt>         (Item<StatisNinjaBeltLegacy>());
            CosmicTracer    = SetUpTwo<TracersElysian>          (Item<FasterGodSlayerTracers>());
            LunicTarcer     = SetUpTwo<TracersCelestial>        (Item<FasterLunarTracers>());
            BraveBadge      = SetUpTwo<CalamityMod.Items.Accessories.BadgeofBravery>          (Item<BadgeofBravery>());
            SeaShield = InstallGroupMod<ShieldoftheOceanLegacy>(Item<ShieldoftheOceanLegacy>(), Item<ShieldoftheOcean>());
            #endregion

            #region 传颂之物
            LoreGolem   = SetUpTwo<KnowledgeGolem>              (Item<LoreGolem>());
            LoreProvi   = SetUpTwo<KnowledgeProvidence>         (Item<LoreProvidence>()); 
            LoreDoG     = SetUpTwo<KnowledgeDevourerofGods>     (Item<LoreDevourerofGods>());
            LoreYharon  = SetUpTwo<KnowledgeYharon>             (Item<LoreYharon>());
            LoreDuke    = SetUpTwo<KnowledgeDukeFishron>        (Item<LoreDukeFishron>());
            LorePlant   = SetUpTwo<KnowledgePlantera>           (Item<LorePlantera>());
            LoreCryogen = SetUpTwo<KnowledgeCryogen>            (Item<LoreArchmage>());
            LoreAA      = SetUpTwo<KnowledgeAstrumAureus>       (Item<LoreAstrumAureus>());
            LoreRavager = SetUpTwo<KnowledgeRavager>            (Item<LoreRavager>());
            LoreAS      = SetUpTwo<KnowledgeAquaticScourge>     (Item<LoreAquaticScourge>());
            LoreLevi    = SetUpTwo<KnowledgeLeviathanAnahita>   (Item<LoreLeviathanAnahita>());
            LorePBG     = SetUpTwo<KnowledgePlaguebringerGoliath>(Item<LorePlaguebringerGoliath>());
            LoreSentinal = InstallGroupMod<LoreStormWeaver>(Item<LoreStormWeaver>(), Item<LoreSignus>(), Item<LoreCeaselessVoid>());
            LorePostSCal = SetUpTwo<LoreCynosure>                (Item<LoreCalamitas>());
            LoreMoonLord = SetUpTwo<LoreRequiem>                (Item<KnowledgeMoonLord>());
            LoreAD       = SetUpTwo<KnowledgeAstrumDeus>        (Item<LoreAstrumDeus>());

            #endregion

            #region 其它组
            // 为了避免名称冲突，当模组物品是配方组的标志性或第一个物品时，命名配方组为：ModName:ItemName
            //咱就是说，能不能用点简单的名字.
            //排序请用字母表顺序谢谢
            EvilBar.                    NameHelper("AnyDemoniteBar");
            WoodSwordRecipeGroup.       NameHelper("AnyWoodenSword");
            ExoTropyGroup.              NameHelper("AnyExoTropy");
            CosmicShiv.                 NameHelper("AnyCosmicShiv");
            DeificAmulet.               NameHelper("AnyDeificAmulet");
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
            TrophySentinal.             NameHelper("AnyTrophySentinal");
            TwinTrophy.                 NameHelper("AnyTwinTrophy");
            LeviTrophy.                 NameHelper("AnyLeviTrophy");
            MechTrophy.                 NameHelper("AnyMechTrophy");
            SCalTrophy.                 NameHelper("AnySCalTrophy");
            RottenMatter.               NameHelper("AnyRottenMatter");
            CryoBar.                    NameHelper("AnyCryoBar");
            OhMyGodIsChest.             NameHelper("AnyChest");
            MoonMusicBox.               NameHelper("AnyMoonMusicBox");
            SeaShield.NameHelper("AnyShieldOfTheOcean");


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
            ElementalRay.               NameHelper("AnyElementalRay");
            ElementalShiv.              NameHelper("AnyElementalShiv");
            Eradicator.                 NameHelper("AnyEradicator");
            EclipseFall.                NameHelper("AnyEclipsesFall");
            Excelsus.                   NameHelper("AnyExcelsus");
            HalleysInferno.             NameHelper("AnyHalleysInferno");
            HeliumFlash.                NameHelper("AnyHeliumFlash");
            IceClasper.                 NameHelper("AnyAncientIceChunk");
            LumiStriker.                NameHelper("AnyLumiStriker");
            MagnaCannon.                NameHelper("AnyMagnaCannon");
            MiniGun.                    NameHelper("AnyMiniGun");
            NightsRay.                  NameHelper("AnyNightsRay");
            Norfleet.                   NameHelper("AnyNorfleet");
            OpalStriker.                NameHelper("AnyOpalStriker");
            P90.                        NameHelper("AnyP90");
            PlantBow.                   NameHelper("AnyBlossomFlux");
            PhantasmalFury.             NameHelper("AnyPhantasmalFury");
            PhantasmalRuin.             NameHelper("AnyPhantasmalRuin");
            Phangasm.                   NameHelper("AnyPhangasm");
            TerraRay.                   NameHelper("AnyTerraRay");
            Terratomere.                NameHelper("AnyTerratomere");
            TerraBlade.                 NameHelper("AnyTerraBlade");
            Wand.                       NameHelper("AnyWand");
            SoulEdge.                   NameHelper("AnySoulEdge");
            RareReaper.                 NameHelper("AnyRareReaper");
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
            LoreSentinal.               NameHelper("AnyLoreSentinal");
            LorePostSCal.               NameHelper("AnyLorePostSCal");
            LoreMoonLord.               NameHelper("AnyLoreMoonLord");
            LoreAD.                     NameHelper("AnyLoreAD");
            #endregion
        }
        public static RecipeGroup SetUpTwoVanilia (int showOnRecipe, int another)
        {
            return new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(showOnRecipe)}", showOnRecipe, another);
        }
        public static RecipeGroup SetUpTwo<T> (int showOnRecipe) where T : ModItem
        {
            return new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(showOnRecipe)}", showOnRecipe, Item<T>());
        }
        public static RecipeGroup InstallGroupMod<T>(params int[] setItems) where T : ModItem => new(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(Item<T>())}", setItems);
        public static RecipeGroup InstallIncludeVanilla(int showedItemID, params int[] setItems) => new(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(showedItemID)}", setItems);
        public static int Item<T>() where T : ModItem => ModContent.ItemType<T>();
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
        public static string LorePostSCal       => "AnyLorePostSCal".GetGroupName();
        public static string LoreAD             => "AnyLoreAD".GetGroupName();
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
        public static string LumiStriker        => "AnyLumiStriker".GetGroupName();
        public static string LoreSentinal       => "AnyLoreSentinal".GetGroupName();
        public static string TrophySentinal     => "AnyTrophySentinal".GetGroupName();
        public static string TrophyTwin         => "AnyTwinTrophy".GetGroupName();
        public static string TrophyLevi         => "AnyLeviTrophy".GetGroupName();
        public static string TrophyMechs        => "AnyMechTrophy".GetGroupName();
        public static string SCalTrophy         => "AnySCalTrophy".GetGroupName();
        public static string RottenMatter       => "AnyRottenMatter".GetGroupName();
        public static string LoreMoonLord       => "AnyLoreMoonLord".GetGroupName();
        public static string CryoBar            => "AnyCryoBar".GetGroupName();
        public static string OpalStriker        => "AnyOpalStriker".GetGroupName();
        public static string MagnaCannon        => "AnyMagnaCannon".GetGroupName();
        public static string AnySoulEdge        => "AnySoulEdge".GetGroupName();
        public static string AnyChest           => "AnyChest".GetGroupName();
        public static string AnyMoonMusicBox    => "AnyMoonMusicBox".GetGroupName();
        public static string AnyRareReaper      => "AnyRareReaper".GetGroupName();
        public static string ShieldoftheOcean => "AnyShieldOfTheOcean".GetGroupName();
    }

}
