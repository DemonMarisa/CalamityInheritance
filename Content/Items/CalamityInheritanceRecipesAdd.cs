using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityMod.Items.DraedonMisc;
using CalamityInheritance.Content.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using DraedonsForge = CalamityMod.Items.Placeables.Furniture.CraftingStations.DraedonsForge;
using DraedonsForgeTiles = CalamityMod.Tiles.Furniture.CraftingStations.DraedonsForge;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.Items.Placeables.FurnitureAuric;
using CalamityMod.Items.Placeables.FurnitureBotanic;
using CalamityMod.Items.Placeables.FurnitureSilva;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Items.Placeables.FurnitureAcidwood;
using CalamityMod.Items.Placeables.FurnitureExo;
using CalamityMod.Items.Placeables.FurnitureSacrilegious;
using CalamityMod.Items.Placeables.FurnitureWulfrum;
using CalamityMod.Items.Placeables.FurnitureVoid;
using CalamityMod.Items.Placeables.FurnitureStratus;
using CalamityMod.Items.Placeables.FurnitureAshen;
using CalamityMod.Items.Placeables.FurnitureAbyss;
using CalamityMod.Items.Placeables.FurnitureAncient;
using CalamityMod.Items.Placeables.FurnitureCosmilite;
using CalamityMod.Items.Placeables.FurnitureEutrophic;
using CalamityMod.Items.Placeables.FurnitureMonolith;
using CalamityMod.Items.Placeables.FurniturePlagued;
using CalamityMod.Items.Placeables.FurnitureOtherworldly;
using CalamityMod.Items.Placeables.FurnitureProfaned;
using CalamityMod.Items.Placeables.FurnitureStatigel;
using CalamityMod.Items.LoreItems;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityInheritance.Content.Items.Accessories;
using VividClarity = CalamityMod.Items.Weapons.Magic.VividClarity;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityMod.Items.Tools;
using CalamityMod.Items.Armor.Demonshade;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityMod.Items.Dyes;
using CalamityModMusic.Items.Placeables;
using CalamityMod.Items.Tools.ClimateChange;
using CalamityInheritance.Content.Items.Tools;
using CalamityMod.Items.SummonItems;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Placeables.Plates;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Fishing.BrimstoneCragCatches;
using CalamityMod.Items.Fishing.SunkenSeaCatches;
using DraedonsForgeold = CalamityInheritance.Content.Items.Placeables.Furniture.CraftingStations.DraedonsForgeold;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Content.Items.Armor.Silva;
using CalamityInheritance.Content.Items.MiscItem;
using CalamityMod.Items;
using System;
using CalamityInheritance.Content.Items.Placeables.Banner;
using CalamityInheritance.Content.Items.Accessories.Wings;

namespace CalamityInheritance.Content.Items
{

    //新添加的合成表现在被更加严格的划分好了
    public class CalamityInheritanceRecipesAdd : ModSystem
    {
        public override void AddRecipes()
        {
            Exo();                  //星流系列
            Auric();                //金源系列
            Legendary();            //传奇武器
            Shadowspecs();          //魔影系列
            CalamitousEssence();    //灾厄精华
            Cyrobar();              //冰灵锭
            ScalDecoration();       //谁他妈让召唤boss用的祭坛用来合家具的？
            Accelerator();          //粒子加速器
            // CalamityLegacyRecipe(); //灾厄旧版合成配方
            Misc();                 //其他合成表, 因为我也不知道怎么起名
            FargoWiltasRecipe();      //与法做联动的一些合成表
            FuckYouFargoSoul();     //与……fargoSoul做联动。
        }

        internal static void FuckYouFargoSoul()
        {
        }

        internal static void FargoWiltasRecipe()
        {
            if (!ModLoader.TryGetMod("Fargowiltas", out Mod fargoWiltas))
                return;

            Recipe.Create(ItemID.EnchantedSword).
                AddIngredient<CosmicElementalBanner>().
                AddTile(TileID.Solidifier).
                Register();

            if (!ModLoader.TryGetMod("FargowiltasSouls", out Mod fargoSoul))
                return;

            Recipe.Create(ModContent.ItemType<ShadowspecBar>(), 50).
                AddIngredient<CalamitousEssence>().
                AddTile(fargoWiltas.Find<ModTile>("CrucibleCosmosSheet").Type).
                DisableDecraft().
                Register();
        }

        public static void CalamityLegacyRecipe()
        {
            #region 饰品
            //粘性绷带
            Recipe.Create(ItemID.AdhesiveBandage).
                AddIngredient(ItemID.Silk, 10).
                AddIngredient(ItemID.Gel, 10).
                AddIngredient(ItemID.HealingPotion, 1).
                AddTile(TileID.Anvils).
                Register();
            //盔甲抛光
            Recipe.Create(ItemID.ArmorPolish).
                AddIngredient(ItemID.Bone, 10).
                AddIngredient<AncientBoneDust>(3).
                AddTile(TileID.Anvils).
                Register();
            //再生手环 
            Recipe.Create(ItemID.BandofRegeneration).
                AddIngredient(ItemID.LifeCrystal).
                AddTile(TileID.Anvils).
                Register();
            //牛黄
            Recipe.Create(ItemID.Bezoar).
                AddIngredient(ItemID.JungleSpores, 5).
                AddIngredient(ItemID.Stinger, 5).
                AddTile(TileID.Anvils).
                Register();
            //天界磁石
            Recipe.Create(ItemID.CelestialMagnet).
                AddIngredient(ItemID.ManaPotion, 1).
                AddIngredient(ItemID.FallenStar, 10).
                AddTile(TileID.Anvils).
                Register();
            //绿色手套
            Recipe.Create(ItemID.FeralClaws).
                AddIngredient(ItemID.Leather, 10).
                AddTile(TileID.Anvils).
                Register();
            //冰龟壳
            Recipe.Create(ItemID.FrozenTurtleShell).
                AddIngredient(ItemID.TulipShell, 2).
                AddIngredient(ItemID.SoulofLight, 5).
                AddDecraftCondition(Condition.Hardmode).
                AddTile(TileID.IceMachine).
                Register();
            //魔法箭袋
            Recipe.Create(ItemID.MagicQuiver).
                AddIngredient(ItemID.EndlessQuiver).
                AddIngredient(ItemID.PixieDust, 5).
                AddIngredient(ItemID.Lens, 5).
                AddIngredient(ItemID.SoulofLight, 5).
                AddDecraftCondition(Condition.Hardmode).
                AddTile(TileID.CrystalBall).
                Register();
            //熔岩钩
            Recipe.Create(ItemID.LavaFishingHook).
                AddIngredient(ItemID.Seashell).
                AddIngredient(ItemID.Chain, 5).
                AddIngredient(ItemID.HellstoneBar, 10).
                AddTile(ItemID.Hellforge).
                Register();
            //维生素
            Recipe.Create(ItemID.Vitamins).
                AddIngredient(ItemID.BottledWater).
                AddIngredient(ItemID.Waterleaf, 5).
                AddIngredient(ItemID.Blinkroot, 5).
                AddIngredient(ItemID.Daybloom, 5).
                AddIngredient<BloodOrb>(5).
                AddTile(TileID.AlchemyTable).
                Register();
            //金属探测器
            Recipe.Create(ItemID.MetalDetector).
                AddIngredient(ItemID.Wire, 10).
                AddIngredient(ItemID.SpelunkerGlowstick, 10).
                AddRecipeGroup("AnyCobaltBar", 10).
                AddTile(TileID.Anvils).
                Register();
            #endregion
            #region 其他
            //冰雪机
            Recipe.Create(ItemID.IceMachine).
                AddIngredient(ItemID.IceBlock, 10).
                AddIngredient(ItemID.SnowBlock, 10).
                AddRecipeGroup(RecipeGroupID.IronBar, 5).
                AddTile(TileID.Anvils).
                Register();
            //天摩
            Recipe.Create(ItemID.SkyMill).
                AddIngredient(ItemID.SunplateBlock, 10).
                AddIngredient(ItemID.Cloud, 10).
                AddTile(TileID.Anvils).
                Register();
            //冰雪回旋镖
            Recipe.Create(ItemID.IceBoomerang).
                AddIngredient(ItemID.WoodenBoomerang).
                AddIngredient(ItemID.IceTorch, 10).
                AddTile(TileID.IceMachine).
                Register();
            #endregion
        }

        public override void PostAddRecipes()
        {
            PostModifyModYharon();
            YharonEclispe();        //龙一/龙二, 请确保这一修改最后被执行。
        }

        public static void PostModifyModYharon()
        {
            if (!CIServerConfig.Instance.SolarEclipseChange)
                return;
            int[] silvaLegacy =
            [
                ModContent.ItemType<SilvaHeadMelee>(),
                ModContent.ItemType<SilvaHeadRanged>(),
                ModContent.ItemType<SilvaHeadMagicold>(),
                ModContent.ItemType<SilvaHeadSummonold>(),
                ModContent.ItemType<SilvaHeadRogue>(),
                ModContent.ItemType<SilvaArmorold>(),
                ModContent.ItemType<SilvaLeggingsold>()
            ];
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe rec = Main.recipe[i];
    
                foreach (var s in silvaLegacy)
                {
                    if (rec.HasResult(s) && rec.createItem.stack == 1)
                    {
                        rec.RemoveIngredient(ModContent.ItemType<AscendantSpiritEssence>());
                        rec.AddIngredient(ModContent.ItemType<DarksunFragment>(), 10);
                    }
                }
            }
        }

        public static void YharonEclispe()
        {
            if (!CIServerConfig.Instance.SolarEclipseChange)
                return;

            //Silva系列物品做准备。
            int[] silvaTrain =
            [
                ModContent.ItemType<SilvaArmor>(),
                ModContent.ItemType<SilvaLeggings>(),
                ModContent.ItemType<SilvaHeadSummon>(),
                ModContent.ItemType<SilvaHeadMagic>(),
                ModContent.ItemType<SilvaWings>(),
                //备注：我们模组里面的林海物件也会算进去，这样会方便不少。
            ];
            //军工厂系列
            int[] draedonTrain =
            [
                ModContent.ItemType<PlasmaGrenade>(),
                ModContent.ItemType<Phaseslayer>(),
                ModContent.ItemType<PoleWarper>(),
                ModContent.ItemType<PulseRifle>(),
                ModContent.ItemType<TeslaCannon>(),
                ModContent.ItemType<TheAnomalysNanogun>(),
            ];
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe rec = Main.recipe[i];
                
                int stack = rec.createItem.stack;
                int spirit = ModContent.ItemType<AscendantSpiritEssence>();
                int darkSun = ModContent.ItemType<DarksunFragment>();
                int blueOne = ModContent.ItemType<EndothermicEnergy>();
                int bar = ModContent.ItemType<CosmiliteBar>();
                //1.化魂神晶：移除日食碎片
                if (rec.HasResult<AscendantSpiritEssence>() && stack == 1)
                {
                    rec.RemoveIngredient(darkSun);
                }
                //2.林海系列：移除化魂神晶，添加日食碎片
                foreach(var silvaItem in silvaTrain)
                {
                    if (rec.HasResult(silvaItem) && stack == 1)
                    {
                        rec.RemoveIngredient(spirit);
                        //统一+10份，我们不做差分了
                        rec.AddIngredient(darkSun, 10);
                    }
                }
                //3.军工套件：补充日食碎片
                foreach(var draedonWeapon in draedonTrain)
                {
                    if (rec.HasResult(draedonWeapon) && stack == 1)
                    {
                        //移除化魂神晶，不然也太怪了
                        rec.RemoveIngredient(spirit);
                        //补5份
                        rec.AddIngredient(darkSun, 5);
                    }
                }
                //4.查漏补缺
                //4.1 秩序大剑->移除恒温魔能, 添加化魂神晶与日食碎片
                if (rec.HasResult<Orderbringer>() && stack == 1)
                {
                    rec.RemoveIngredient(blueOne);
                    rec.AddIngredient(spirit, 2);
                    rec.AddIngredient(darkSun, 20);
                }
                //4.2 星辰重炮->移除日食碎片，修改为宇宙锭
                if (rec.HasResult<Starmada>() && stack == 1)
                {
                    rec.RemoveIngredient(darkSun);
                    rec.AddIngredient(bar, 10);
                }
                //4.3 天底：移除金源锭
                if (rec.HasResult<Nadir>() && stack == 1)
                {
                    rec.RemoveIngredient(ModContent.ItemType<AuricBar>());
                }
                //4.4 氦闪：金源锭 -> 宇宙锭
                if (rec.HasResult<HeliumFlash>() && stack == 1)
                {
                    rec.RemoveIngredient(ModContent.ItemType<AuricBar>());
                    rec.AddIngredient(bar, 10);
                }
                //4.5 极昼信标：金源锭 -> 宇宙锭与日食碎片
                if (rec.HasResult<MidnightSunBeacon>() && stack == 1)
                {
                    rec.RemoveIngredient(ModContent.ItemType<AuricBar>());
                    rec.AddIngredient(bar, 10); 
                    rec.AddIngredient(darkSun, 25);
                }
                //4.6本mod的卡拉萨瓦大人：恒温魔能 -> 日式碎片
                if (rec.HasResult<ACTKarasawa>() && stack == 1)
                {
                    rec.RemoveIngredient(blueOne);
                    rec.AddIngredient(darkSun, 10);
                }
            }
        }

        public static void Misc()
        {
            Recipe.Create(ModContent.ItemType<Rock>()).
                AddIngredient<DefiledFeather>().
                DisableDecraft().
                Register();

            Recipe.Create(ModContent.ItemType<Cosmolight>()).
                AddIngredient(ModContent.ItemType<Moonlight>()).
                AddIngredient(ModContent.ItemType<Daylight>()). 
                AddIngredient(ItemID.FallenStar).
                AddTile(TileID.CrystalBall). 
                //Scarlet 1/22:修改合成站为水晶球，防止玩家在困难模式前就能通过宇宙之光微光拆解获得肉后魂
                Register();
            Recipe.Create(ModContent.ItemType<FrostcrushValari>()).
                AddIngredient<Kylie>().
                AddIngredient<CryoBar>(6).
                AddIngredient<Voidstone>(40).
                AddIngredient<CoreofEleum>(5).
                AddTile(TileID.MythrilAnvil).
                Register();

            Recipe.Create(ModContent.ItemType<IceBarrage>()).
                AddIngredient(ItemID.BlizzardStaff, 1).
                AddIngredient(ItemID.IceRod, 1).
                AddIngredient<IcicleStaff>().
                AddIngredient<EndothermicEnergy>(23).
                AddIngredient<CryoBar>(18).
                AddTile<CosmicAnvil>().
                Register();
            
            Recipe.Create(ModContent.ItemType<FlarefrostBlade>()).
                AddIngredient(ItemID.HellstoneBar, 8).
                AddIngredient<CryoBar>(8).
                AddIngredient(ItemID.SoulofLight, 3).
                AddTile(TileID.MythrilAnvil).
                Register();
            Recipe.Create(ModContent.ItemType<GalaxySmasher>()).
                AddIngredient<MeleeTypeHammerStellarContemptLegacy>().
                AddIngredient<CosmiliteBar>(10).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<StellarContempt>()).
                AddIngredient<MeleeTypeHammerFallenPaladinsLegacy>().
                AddIngredient<GalacticaSingularity>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();

            Recipe.Create(ModContent.ItemType<Murasama>()).
                AddIngredient(ModContent.ItemType<MurasamaNeweffect>()).
                Register();

            Recipe.Create(ModContent.ItemType<ElementalShiv>()).
                AddIngredient(ModContent.ItemType<TerraShiv>()).
                AddIngredient(ModContent.ItemType<GalacticaSingularity>()).
                AddIngredient(ItemID.LunarBar).
                AddTile(TileID.LunarCraftingStation).
                Register();

            Recipe.Create(ModContent.ItemType<Terratomere>()).
                AddIngredient(ModContent.ItemType<TerraEdge>()).
                AddIngredient(ModContent.ItemType<Hellkite>()).
                AddIngredient(ModContent.ItemType<UelibloomBar>()).
                AddIngredient(ModContent.ItemType<Floodtide>()).
                AddIngredient(ItemID.LunarBar).
                AddTile(TileID.LunarCraftingStation).
                Register();

            Recipe.Create(ModContent.ItemType<DraedonPowerCell>(), 333).
                AddIngredient(ModContent.ItemType<DubiousPlating>(), 2).
                AddIngredient(ModContent.ItemType<MysteriousCircuitry>()).
                AddIngredient(ItemID.CopperBar).
                Register();

            Recipe.Create(ModContent.ItemType<ElementalRay>()).
                AddIngredient<TerraRay>().
                AddIngredient(ItemID.LunarBar, 5).
                AddIngredient<LifeAlloy>(5).
                AddIngredient<GalacticaSingularity>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();

            //光合射线[泰拉射线]
            Recipe.Create(ModContent.ItemType<Photosynthesis>()).
                AddIngredient<CarnageRay>().
                AddIngredient<ValkyrieRay>().
                AddIngredient<LivingShard>().
                AddTile(TileID.MythrilAnvil).
                Register();

            Recipe.Create(ModContent.ItemType<Photosynthesis>()).
                AddIngredient<NightsRayold>().
                AddIngredient<ValkyrieRay>().
                AddIngredient<LivingShard>().
                AddTile(TileID.MythrilAnvil).
                Register();
            
            Recipe.Create(ModContent.ItemType<PlasmaDriveCore>()).
                AddIngredient(ModContent.ItemType<DubiousPlating>(), 5).
                AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 10).
                AddRecipeGroup("AnyCopperBar", 10).
                AddTile(TileID.MythrilAnvil).
                Register();

            Recipe.Create(ModContent.ItemType<SuspiciousScrap>()).
                AddRecipeGroup("AnyCopperBar", 10).
                AddRecipeGroup("IronBar", 10).
                AddTile(TileID.Anvils).
                Register();
            
            Recipe.Create(ItemID.TerraBlade).
                AddIngredient(ModContent.ItemType<TrueBloodyEdge>()).
                AddIngredient(ItemID.TrueExcalibur).
                AddIngredient(ModContent.ItemType<LivingShard>(),7). //我不是很清楚为啥这个生命碎片还留着
                AddTile(TileID.MythrilAnvil).
                Register();

        }
        public static void Accelerator()
        {
            #region T1粒子加速器
            #region 神龛物品

            Recipe.Create(ModContent.ItemType<GladiatorsLocket>()).
                AddRecipeGroup("AnyGoldBar", 5).
                AddIngredient(ItemID.MarbleBlock, 10).
                AddIngredient(ItemID.Gladius, 1).
                AddTile<AcceleratorT1Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<LuxorsGift>()).
                AddIngredient(ItemID.Ruby, 1).
                AddRecipeGroup("AnyGoldBar", 10).
                AddIngredient(ItemID.HellstoneBar, 5).
                AddIngredient(ItemID.Bone, 5).
                AddIngredient(ItemID.JungleSpores, 5).
                AddIngredient(ItemID.BeeWax, 5).
                AddIngredient<SulphuricScale>(5).
                DisableDecraft().
                AddTile<AcceleratorT1Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<TrinketofChi>()).
                AddIngredient(ItemID.Silk, 15).
                AddIngredient(ItemID.Ruby).
                AddTile<AcceleratorT1Tile>().
                Register();
            
            Recipe.Create(ModContent.ItemType<FungalSymbiote>()).
                AddIngredient(ItemID.GlowingMushroom, 30).
                AddIngredient(ItemID.Mushroom, 10).
                AddTile<AcceleratorT1Tile>().
                Register();
            
            Recipe.Create(ModContent.ItemType<UnstableGraniteCore>()).
                AddIngredient<EnergyCore>(1).
                AddIngredient(ItemID.GraniteBlock, 50).
                AddTile<AcceleratorT1Tile>().
                Register();
            
            Recipe.Create(ModContent.ItemType<TundraLeash>()).
                AddIngredient(ItemID.FlinxFur, 10).
                AddIngredient(ItemID.Leather, 10).
                AddIngredient(ItemID.HealingPotion, 5).
                AddTile<AcceleratorT1Tile>().
                Register();
            
            Recipe.Create(ModContent.ItemType<CorruptionEffigy>()).
                AddIngredient(ItemID.ShadowScale, 10).
                AddIngredient(ItemID.RottenChunk, 10).
                AddIngredient(ItemID.VileMushroom, 10).
                AddIngredient(ItemID.DemoniteBar, 10).
                DisableDecraft().
                AddTile<AcceleratorT1Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<CrimsonEffigy>()).
                AddIngredient(ItemID.TissueSample, 10).
                AddIngredient(ItemID.Vertebrae, 10).
                AddIngredient(ItemID.ViciousMushroom, 10).
                AddIngredient(ItemID.CrimtaneBar, 10).
                DisableDecraft().
                AddTile<AcceleratorT1Tile>().
                Register();

            #endregion
            #region 一些困难模式前获取起来非常逆天的物品
            Recipe.Create(ModContent.ItemType<OnyxExcavatorKey>()).
                AddIngredient<MarniteObliterator>(1).
                AddIngredient(ItemID.Amethyst, 16).
                AddIngredient<DubiousPlating>(15).
                AddIngredient<MysteriousCircuitry>(15).
                AddIngredient<Onyxplate>(150).
                AddIngredient(ItemID.BlackPaint, 50).
                AddTile<AcceleratorT1Tile>().
                Register();

            //纯净凝胶, 是的你没听错
            Recipe.Create(ModContent.ItemType<BlightedGel>(), 25).
                AddIngredient(ItemID.Gel, 50).
                AddTile<AcceleratorT1Tile>().
                Register();

            //为什么法狗的合成台要用两份地狱熔炉？
            Recipe.Create(ItemID.Hellforge). 
                AddIngredient(ItemID.Furnace, 1).
                AddIngredient(ItemID.HellstoneBar, 10).
                DisableDecraft().
                AddTile<AcceleratorT1Tile>().
                Register();

            //天蓝两boss的材料互转
            Recipe.Create(ModContent.ItemType<BloodSample>()).
                AddIngredient<RottenMatter>().
                AddCondition(Condition.CorruptWorld).
                AddTile<AcceleratorT1Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<RottenMatter>()).
                AddIngredient<BloodSample>().
                AddCondition(Condition.CrimsonWorld).
                AddTile<AcceleratorT1Tile>().
                Register();
            #endregion
            #region 一些极其早期的, 困难模式时才能进行的合成
            //神灯烈焰的材料
            Recipe.Create(ItemID.DjinnLamp, 1).
                AddRecipeGroup("AnyGoldBar", 15).
                AddIngredient(ItemID.SoulofNight, 10).
                DisableDecraft().
                AddTile<AcceleratorT1Tile>().
                Register();

            //允许黑白碎片的互转
            Recipe.Create(ItemID.DarkShard).
                AddIngredient(ItemID.LightShard).
                DisableDecraft().
                AddTile<AcceleratorT1Tile>().
                Register();

            Recipe.Create(ItemID.LightShard).
                AddIngredient(ItemID.DarkShard).
                DisableDecraft().
                AddTile<AcceleratorT1Tile>().
                Register();

            //玛瑙爆破枪, 我知道这样会把这武器提前一个机械boss
            //但我请问了这个东西提前了就能很超模的打机械boss了吗灾厄?
            Recipe.Create(ItemID.OnyxBlaster).
                AddIngredient(ItemID.Shotgun).
                AddIngredient(ItemID.DarkShard, 2).
                AddIngredient(ItemID.SoulofNight, 10).
                AddTile<AcceleratorT1Tile>().
                Register();

            //复仇模式三件套
            Recipe.Create(ModContent.ItemType<StressPills>()).
                AddIngredient(ItemID.HealingPotion, 5).
                AddIngredient(ItemID.IronskinPotion, 5).
                AddIngredient<CoastalDemonfish>(5).
                AddIngredient<BloodOrb>(5).
                AddIngredient<EssenceofEleum>(5).
                AddTile<AcceleratorT1Tile>().
                Register();
            
            Recipe.Create(ModContent.ItemType<HeartofDarkness>()).
                AddIngredient(ItemID.HealingPotion, 5).
                AddIngredient(ItemID.WrathPotion, 5).
                AddIngredient<Shadowfish>(5).
                AddIngredient<BloodOrb>(5).
                AddIngredient<EssenceofHavoc>(5).
                AddTile<AcceleratorT1Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<Laudanum>()).
                AddIngredient(ItemID.HealingPotion, 5).
                AddIngredient(ItemID.RegenerationPotion, 5).
                AddIngredient<SunkenSailfish>(5).
                AddIngredient<BloodOrb>(5).
                AddIngredient<EssenceofSunlight>(5).
                DisableDecraft().
                AddTile<AcceleratorT1Tile>().
                Register();
            #endregion
            #endregion
            #region T2粒子加速器
            //绞肉机合成
            Recipe.Create(ItemID.MeatGrinder).
                AddIngredient(ItemID.SharpeningStation, 1).
                AddIngredient(ItemID.CookingPot, 1).
                AddTile<AcceleratorT2Tile>().
                Register();
            //变态刀合成 
            Recipe.Create(ItemID.PsychoKnife).
                AddIngredient(ItemID.FalconBlade).
                AddIngredient<BloodOrb>(50).
                AddTile<AcceleratorT2Tile>().
                Register();
            //光束剑
            Recipe.Create(ItemID.BeamSword).
                AddRecipeGroup("AnyCobaltBar", 15).
                AddRecipeGroup(RecipeGroupID.IronBar, 10).
                AddIngredient(ItemID.Diamond).
                AddTile<AcceleratorT2Tile>().
                Register();

            //T2粒子加速器就可以造电路板之类的玩意了
            Recipe.Create(ModContent.ItemType<MysteriousCircuitry>(), 10).
                AddRecipeGroup(RecipeGroupID.IronBar, 5).
                AddRecipeGroup("AnyCopperBar", 5).
                AddIngredient(ItemID.Wire, 5).
                AddTile<AcceleratorT2Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<DubiousPlating>(), 20).
                AddRecipeGroup(RecipeGroupID.IronBar, 5).
                AddRecipeGroup("AnyCopperBar", 5).
                AddIngredient(ItemID.Glass, 5).
                AddTile<AcceleratorT2Tile>().
                Register();
            
            //海参贝壳，合成表仅仅致敬用
            Recipe.Create(ItemID.NeptunesShell, 1).
                AddIngredient(ItemID.Coral, 15).
                AddIngredient(ItemID.Goldfish, 15).
                AddIngredient(ItemID.SharkFin, 5).
                AddIngredient(ItemID.SoulofFright, 20).
                AddIngredient(ItemID.SoulofLight, 5).
                AddIngredient(ItemID.SoulofNight, 5).
                AddTile<AcceleratorT2Tile>().
                Register();
            Recipe.Create(ItemID.TitanGlove, 1).
                AddIngredient(ItemID.CobaltBar, 10).
                AddIngredient(ItemID.FeralClaws, 1).
                AddTile<AcceleratorT2Tile>().
                Register();

            Recipe.Create(ItemID.CrossNecklace, 1).
                AddRecipeGroup("AnyGoldBar", 20).
                AddIngredient(ItemID.SoulofLight, 10).
                AddTile<AcceleratorT2Tile>().
                Register();
                
            #region T2粒子加速器 月后早期合成
            Recipe.Create(ModContent.ItemType<ExodiumCluster>(), 5). //旧版Exodium Cluster的合成方法
                AddIngredient(ItemID.LunarOre, 5).
                AddIngredient<GalacticaSingularity>(5).
                AddTile<AcceleratorT2Tile>().
                Register();
            #endregion
            #endregion
            #region T3粒子加速器
            //终末石
            Recipe.Create(ModContent.ItemType<Terminus>()).
                AddIngredient<ShadowspecBar>(5).
                DisableDecraft().
                AddTile<AcceleratorT3Tile>().
                Register();
            #endregion
        }
        public static void ScalDecoration()
        {
            Recipe.Create(ModContent.ItemType<CeremonialUrn>()).
                AddIngredient<AshesofAnnihilation>(5).
                AddIngredient<AshesofCalamity>(15).
                AddTile<AcceleratorT3Tile>().
                Register();
            
            Recipe.Create(ModContent.ItemType<EyeOfTheAccursedBanner>()).
                AddIngredient<OccultBrickItem>(3).
                AddIngredient(ItemID.Silk, 5).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<OccultLegionnaireBanner>()).
                AddIngredient<OccultBrickItem>(3).
                AddIngredient(ItemID.Silk, 5).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<LargeRitualCandle>()).
                AddIngredient<OccultBrickItem>(6).
                AddIngredient(ItemID.Torch).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<RitualCandle>()).
                AddIngredient<OccultBrickItem>(5).
                AddIngredient(ItemID.Torch).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<SacrilegiousCandle>()).
                AddIngredient<OccultBrickItem>(4).
                AddIngredient(ItemID.Torch).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<SacrilegiousCandelabra>()).
                AddIngredient<OccultBrickItem>(5).
                AddIngredient(ItemID.Torch, 3).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<SacrilegiousChandelier>()).
                AddIngredient<OccultBrickItem>(4).
                AddIngredient(ItemID.Torch, 4).
                AddIngredient(ItemID.Chain).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<SacrilegiousLamp>()).
                AddIngredient<OccultBrickItem>(3).
                AddIngredient(ItemID.Torch).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<SacrilegiousLantern>()).
                AddIngredient<OccultBrickItem>(6).
                AddIngredient(ItemID.Torch).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<SacrilegiousChest>()).
                AddIngredient<OccultBrickItem>(8).
                AddRecipeGroup(RecipeGroupID.IronBar, 2).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<SacrilegiousClock>()).
                AddIngredient<OccultBrickItem>(10).
                AddRecipeGroup(RecipeGroupID.IronBar, 3).
                AddIngredient(ItemID.Glass, 6).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<SacrilegiousOrgan>()).
                AddIngredient<OccultBrickItem>(15).
                AddIngredient(ItemID.Bone, 4).
                AddIngredient(ItemID.Book).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<SacrilegiousSink>()).
                AddIngredient<OccultBrickItem>(6).
                AddIngredient(ItemID.WaterBucket).
                AddIngredient(ItemID.HoneyBucket).
                AddIngredient(ItemID.LavaBucket).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<SacrilegiousTable>()).
                AddIngredient<OccultBrickItem>(8).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<MonolithOfTheAccursed>()).
                AddIngredient<OccultBrickItem>(15).
                AddTile<AcceleratorT3Tile>().
                Register();
            
            Recipe.Create(ModContent.ItemType<OccultBrickItem>(), 400).
                AddRecipeGroup("AnyStoneBlock", 400).
                AddIngredient<AshesofAnnihilation>().
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<SacrilegiousBed>()).
                AddIngredient<OccultBrickItem>(15).
                AddIngredient(ItemID.Silk, 5).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<SacrilegiousBathtub>()).
                AddIngredient<OccultBrickItem>(15).
                AddIngredient(ItemID.Silk, 5).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<SacrilegiousBench>()).
                AddIngredient<OccultBrickItem>(8).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ModContent.ItemType<SacrilegiousBookcase>()).
                AddIngredient<OccultBrickItem>(20).
                AddIngredient(ItemID.Book, 10).
                AddTile<AcceleratorT3Tile>().
                Register();
        }
        public static void Exo()
        {
            #region Exo
            //星流之刃
            Recipe.Create(ModContent.ItemType<Exoblade>()).
                AddRecipeGroup("CalamityInheritance:AnyTerratomere").
                AddIngredient<AnarchyBlade>().
                AddIngredient<FlarefrostBlade>().
                AddIngredient<EntropicClaymore>().
                AddIngredient<StellarStriker>().
                AddIngredient<MiracleMatter>().
                DisableDecraft().
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();
            
            Recipe.Create(ModContent.ItemType<Exoblade>()).
                AddRecipeGroup("CalamityInheritance:AnyTerratomere").
                AddIngredient<AnarchyBlade>().
                AddIngredient<FlarefrostBlade>().
                AddIngredient<EntropicClaymore>().
                AddIngredient<StellarStriker>().
                AddIngredient<AncientMiracleMatter>().
                DisableDecraft().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();

            //超级诺瓦天体球
            Recipe.Create(ModContent.ItemType<Supernova>()).
                AddIngredient<SealedSingularity>().
                AddIngredient<StarofDestruction>().
                AddIngredient<TotalityBreakers>().
                AddIngredient<BallisticPoisonBomb>().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                DisableDecraft().
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();
            
            //星神之杀
            Recipe.Create(ModContent.ItemType<Celestus>()).
                AddIngredient<ElementalDisk>().
                AddIngredient<MoltenAmputator>().
                AddIngredient<SubductionSlicer>().
                AddIngredient<EnchantedAxe>().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                DisableDecraft().
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();

            //天堂之风
            Recipe.Create(ModContent.ItemType<HeavenlyGale>()).
                AddIngredient<PlanetaryAnnihilation>().
                AddIngredient<TelluricGlare>().
                AddIngredient<ClockworkBow>().
                AddIngredient<TheBallista>().
                AddIngredient<AncientMiracleMatter>().
                DisableDecraft().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();
            
            //星火解离者
            Recipe.Create(ModContent.ItemType<Photoviscerator>()).
                
                AddIngredient<ElementalEruption>().
                AddIngredient<DeadSunsWind>().
                AddIngredient<HalleysInferno>().
                AddIngredient<AncientMiracleMatter>().
                DisableDecraft().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();
            
            //磁极异变
            Recipe.Create(ModContent.ItemType<MagnomalyCannon>()).
                AddIngredient<ThePack>().
                AddIngredient<ScorchedEarth>().
                AddIngredient(ItemID.ElectrosphereLauncher).
                AddIngredient<AncientMiracleMatter>().
                DisableDecraft().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();
            
            //去他妈的法师板砖
            Recipe.Create(ModContent.ItemType<SubsumingVortex>()).
                AddIngredient<AuguroftheElements>().
                AddIngredient<EventHorizon>().
                AddIngredient<TearsofHeaven>().
                AddIngredient<AncientMiracleMatter>().
                DisableDecraft().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();
                
            //星流大棒
            Recipe.Create(ModContent.ItemType<VividClarity>()).
                AddRecipeGroup("CalamityInheritance:AnyElementalRay").
                AddRecipeGroup("CalamityInheritance:AnyPhantasmalFury").
                AddIngredient(ModContent.ItemType<ShadowboltStaff>()).
                AddIngredient(ModContent.ItemType<UltraLiquidator>()).
                AddIngredient(ModContent.ItemType<MiracleMatter>()).
                DisableDecraft().
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();

            Recipe.Create(ModContent.ItemType<VividClarity>()).
                AddRecipeGroup("CalamityInheritance:AnyElementalRay").
                AddRecipeGroup("CalamityInheritance:AnyPhantasmalFury").
                AddIngredient(ModContent.ItemType<ShadowboltStaff>()).
                AddIngredient(ModContent.ItemType<UltraLiquidator>()).
                DisableDecraft().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();

            //宇宙之灵
            Recipe.Create(ModContent.ItemType<CosmicImmaterializer>()).
                AddIngredient<ElementalAxe>().
                AddIngredient<EtherealSubjugator>().
                AddIngredient<Cosmilamp>().
                AddIngredient<CalamarisLament>().
                DisableDecraft().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                Register();
            
            Recipe.Create(ModContent.ItemType<IridescentExcalibur>()).
                AddIngredient(ItemID.TrueExcalibur).
                AddIngredient<Orderbringer>().
                DisableDecraft().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddIngredient<AshesofAnnihilation>(5).
                AddTile<DraedonsForgeTiles>().
                Register();
            #endregion
        }
        public static void Legendary()
        {
            #region LegendaryItems
            if (CIServerConfig.Instance.LegendaryitemsRecipes == true)
            {

                //庇护之刃
                Recipe.Create(ModContent.ItemType<AegisBlade>()).
                    AddRecipeGroup(CIRecipeGroup.LoreGolem).
                DisableDecraft().
                    AddTile(TileID.MythrilAnvil).
                    Register();
                //海爵剑
                Recipe.Create(ModContent.ItemType<BrinyBaron> ()).
                    AddRecipeGroup(CIRecipeGroup.LoreDuke).
                DisableDecraft().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                //宙寒弹刃
                Recipe.Create(ModContent.ItemType<CosmicDischarge> ()).
                    AddRecipeGroup(CIRecipeGroup.LoreDevourerofGods).
                    DisableDecraft().
                    AddTile(ModContent.TileType<CosmicAnvil>()).
                    Register();
                
                //叶流
                Recipe.Create(ModContent.ItemType<BlossomFlux> ()).
                    AddRecipeGroup(CIRecipeGroup.LorePlant).
                    DisableDecraft().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                //纯原
                Recipe.Create(ModContent.ItemType<PristineFury>()).
                    AddRecipeGroup(CIRecipeGroup.LoreProvidence).
                    DisableDecraft().
                    AddTile(TileID.LunarCraftingStation).
                    Register();

                //海灼
                Recipe.Create(ModContent.ItemType<SeasSearing>()).
                    AddRecipeGroup(CIRecipeGroup.LoreAS).
                    DisableDecraft().
                    AddTile(TileID.Anvils).
                    Register();

                //维苏威阿斯
                Recipe.Create(ModContent.ItemType<Vesuvius>()).
                    AddRecipeGroup(CIRecipeGroup.LoreRavager).
                    DisableDecraft().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                //魔君水晶
                Recipe.Create(ModContent.ItemType<YharimsCrystal>()).
                    AddRecipeGroup(CIRecipeGroup.LoreYharon).
                    DisableDecraft().
                    AddTile(ModContent.TileType<CosmicAnvil>()).
                    Register();
                
                //冰寒神性
                Recipe.Create(ModContent.ItemType<GlacialEmbrace> ()).
                    AddRecipeGroup(CIRecipeGroup.LoreCryo).
                    DisableDecraft().
                    AddTile(TileID.Anvils).
                    Register();

                //狮源流星
                Recipe.Create(ModContent.ItemType<LeonidProgenitor> ()).
                    AddRecipeGroup(CIRecipeGroup.LoreAA).
                    DisableDecraft().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                //孔雀翎
                Recipe.Create(ModContent.ItemType<Malachite>()).
                    AddRecipeGroup(CIRecipeGroup.LorePBG).
                    DisableDecraft().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                //归一
                Recipe.Create(ModContent.ItemType<TheCommunity>()).
                    AddRecipeGroup(CIRecipeGroup.LoreLevi).
                    DisableDecraft().
                    AddTile(TileID.MythrilAnvil).
                    Register();

            }
            #endregion
        }
        public static void Auric()
        {
            bool isEnableDragonRecpieModiflication = CIServerConfig.Instance.SolarEclipseChange; 
            #region Accessories
            //辐辉
            Recipe.Create(ModContent.ItemType<Radiance>()).
                AddRecipeGroup(CIRecipeGroup.GoldBottle).
                AddRecipeGroup(CIRecipeGroup.AstralArcanum).
                AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<Radiance>()).
                AddIngredient(ModContent.ItemType<AmbrosialAmpouleOld>()).
                AddIngredient(ModContent.ItemType<AstralArcanum>()).
                AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4).
                AddIngredient(ModContent.ItemType<AuricBar>(), 5).
                AddTile<CosmicAnvil>().
                Register();
            //神壁
            Recipe.Create(ModContent.ItemType<RampartofDeities>()).
                AddIngredient(ItemID.FrozenShield).
                AddRecipeGroup("CalamityInheritance:AnyDeificAmulet").
                AddIngredient<AscendantSpiritEssence>(4).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<TracersSeraph>()).
                AddRecipeGroup(CIRecipeGroup.TracersElysian).
                AddIngredient<DrewsWings>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            #endregion
            #region placeable
            Recipe.Create(ModContent.ItemType<ThaumaticChair>()).
                AddIngredient<AbyssChair>().
                AddIngredient<AcidwoodChair>().
                AddIngredient<AncientChair>().
                AddIngredient<AshenChair>().
                AddIngredient<BotanicChair>().
                AddIngredient<CosmiliteChair>().
                AddIngredient<EutrophicChair>().
                AddIngredient<ExoChair>().
                AddIngredient<MonolithChair>().
                AddIngredient<SacrilegiousChair>().
                AddIngredient<OtherworldlyChair>().
                AddIngredient<PlaguedPlateChair>().
                AddIngredient<ProfanedChair>().
                AddIngredient<SilvaChair>().
                AddIngredient<StatigelChair>().
                AddIngredient<StratusChair>().
                AddIngredient<VoidChair>().
                AddIngredient<WulfrumChair>().
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile(ModContent.TileType<CosmicAnvil>()).
            Register();

            #endregion        
            #region materials
            Recipe.Create(ModContent.ItemType<MiracleMatter>()).
                AddIngredient<ExoPrism>(5).
                AddIngredient<AuricBarold>().
                AddIngredient<LifeAlloy>().
                AddIngredient<CoreofCalamity>().
                AddIngredient<AscendantSpiritEssence>().
                AddIngredient<GalacticaSingularity>(3).
                AddTile<DraedonsForgeTiles>().
                Register();
            #endregion
            #region AuricSet
            Recipe.Create(ModContent.ItemType<AuricTeslaBodyArmor>()).
                AddIngredient<GodSlayerChestplate>().
                AddIngredient<BloodflareBodyArmor>().
                AddIngredient<TarragonBreastplate>().
                AddIngredient<AuricBarold>(2).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<AuricTeslaBodyArmor>()).
                AddIngredient<SilvaArmor>().
                AddIngredient<BloodflareBodyArmor>().
                AddIngredient<TarragonBreastplate>().
                AddIngredient<AuricBarold>(2).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<AuricTeslaCuisses>()).
                AddIngredient<GodSlayerLeggings>().
                AddIngredient<BloodflareCuisses>().    
                AddIngredient<TarragonLeggings>().    
                AddIngredient(ItemID.FlyingCarpet).
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<AuricTeslaHoodedFacemask>()).
                AddIngredient<GodSlayerHeadRanged>().
                AddIngredient<BloodflareHeadRanged>().
                AddIngredient<TarragonHeadRanged>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<AuricTeslaPlumedHelm>()).
                AddIngredient<GodSlayerHeadRogue>().
                AddIngredient<BloodflareHeadRogue>().
                AddIngredient<TarragonHeadRogue>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<AuricTeslaRoyalHelm>()).
                AddIngredient<GodSlayerHeadMelee>().
                AddIngredient<BloodflareHeadMelee>().
                AddIngredient<TarragonHeadMelee>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<AuricTeslaSpaceHelmet>()).
                AddIngredient<SilvaHeadSummon>().
                AddIngredient<BloodflareHeadSummon>().
                AddIngredient<TarragonHeadSummon>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<AuricTeslaWireHemmedVisage>()).
                AddIngredient<SilvaHeadMagic>().
                AddIngredient<BloodflareHeadMagic>().
                AddIngredient<TarragonHeadMagic>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<AuricToilet>()).
                AddIngredient<BotanicChair>().
                AddIngredient<SilvaChair>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            #endregion
            #region Auric Weapon
            Recipe.Create(ModContent.ItemType<ArkoftheCosmos>()).
                AddIngredient(ModContent.ItemType<ArkoftheElements>()).
                AddIngredient(ModContent.ItemType<FourSeasonsGalaxia>()).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ModContent.ItemType<Seraphim>()).
                AddIngredient(ModContent.ItemType<ShatteredSun>()).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ModContent.ItemType<AcesHigh>()).
                AddIngredient(ItemID.Revolver).
                AddIngredient(ModContent.ItemType<ClaretCannon>()).
                AddIngredient(ModContent.ItemType<FantasyTalisman>(),52).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ModContent.ItemType<AetherfluxCannon>()).
                AddIngredient(ModContent.ItemType<NanoPurge>()).
                AddIngredient(ModContent.ItemType<PurgeGuzzler>()).
                AddIngredient(ModContent.ItemType<UelibloomBar>(),12).
                AddIngredient(ModContent.ItemType<DivineGeode>(),8).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ModContent.ItemType<AltarOfTheAccursedItem>()).
                AddIngredient(ModContent.ItemType<BrimstoneSlag>(),30).
                AddIngredient(ModContent.ItemType<CoreofCalamity>()).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ModContent.ItemType<Ataraxia>()).
                AddIngredient(ItemID.BrokenHeroSword).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddIngredient(ModContent.ItemType<CosmiliteBar>(),8).
                AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(),2).
                AddIngredient(ModContent.ItemType<NightmareFuel>(),20).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ModContent.ItemType<DragonPow>()).
                AddIngredient<Mourningstar>().
                AddIngredient(ItemID.DaoofPow).
                AddIngredient(ItemID.FlowerPow).
                AddIngredient(ItemID.Flairon).
                AddIngredient<BallOFugu>().
                AddIngredient<Tumbleweed>().
                AddIngredient<UrchinFlail>().
                AddIngredient<YharonSoulFragment>(4).
                AddIngredient<AuricBarold>().
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ModContent.ItemType<Drataliornus>()).
                AddRecipeGroup(CIRecipeGroup.BlossomFlux).
                AddIngredient(ModContent.ItemType<EffulgentFeather>(),12).
                AddIngredient(ModContent.ItemType<YharonSoulFragment>(),4).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ModContent.ItemType<DynamicPursuer>()).
                AddIngredient(ModContent.ItemType<Eradicator>()).
                AddIngredient(ModContent.ItemType<TrackingDisk>()).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();

            if (!isEnableDragonRecpieModiflication)
            {
                Recipe.Create(ModContent.ItemType<MidnightSunBeacon>()).
                    AddIngredient(ItemID.XenoStaff).
                    AddIngredient(ItemID.MoonlordTurretStaff).
                    AddIngredient(ModContent.ItemType<AuricBarold>()).
                    AddTile<CosmicAnvil>().
                    Register();
                
                Recipe.Create(ModContent.ItemType<Nadir>()).
                    AddIngredient(ModContent.ItemType<ElementalLance>()).
                    AddIngredient(ModContent.ItemType<TwistingNether>(),5).
                    AddIngredient(ModContent.ItemType<DarksunFragment>(),8).
                    AddIngredient(ModContent.ItemType<AuricBarold>()).
                    AddTile<CosmicAnvil>().
                    Register();
                
                Recipe.Create(ModContent.ItemType<HeliumFlash>()).
                    AddIngredient(ModContent.ItemType<VenusianTrident>()).
                    AddIngredient(ModContent.ItemType<LashesofChaos>()).
                    AddIngredient(ModContent.ItemType<ForbiddenSun>()).
                    AddIngredient(ItemID.FragmentSolar,20).
                    AddIngredient(ItemID.FragmentNebula,5).
                    AddIngredient(ModContent.ItemType<AuricBarold>()).
                    AddTile<CosmicAnvil>().
                    Register();
            }

                

            Recipe.Create(ModContent.ItemType<TheOracle>()).
                AddIngredient<BurningRevelation>().
                AddIngredient<TheObliterator>().
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<TyrannysEnd>()).
                AddIngredient<GoldenEagle>().
                AddIngredient<AntiMaterielRifle>().
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<VoidVortex>()).
                AddIngredient<VoltaicClimax>().
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ItemID.Zenith).
                AddRecipeGroup(CIRecipeGroup.TerraBlade).
                AddIngredient(ItemID.Meowmere).
                AddIngredient(ItemID.StarWrath).
                AddIngredient(ItemID.InfluxWaver).
                AddIngredient(ItemID.TheHorsemansBlade).
                AddIngredient(ItemID.Seedler).
                AddIngredient(ItemID.Starfury).
                AddIngredient(ItemID.BeeKeeper).
                AddIngredient(ItemID.EnchantedSword).
                AddIngredient(ItemID.CopperShortsword).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();

            #endregion
            #region AuricPlaceable
            Recipe.Create(ModContent.ItemType<DraedonsForge>()).
                AddIngredient(ModContent.ItemType<DraedonsForgeold>()).
                AddIngredient(ModContent.ItemType<ExoPrism>(), 5).
                Register();

            Recipe.Create(ModContent.ItemType<AuricQuantumCoolingCell>()).
                AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 8).
                AddIngredient(ModContent.ItemType<DubiousPlating>(), 8).
                AddIngredient(ModContent.ItemType<EndothermicEnergy>(), 40).
                AddIngredient(ModContent.ItemType<CoreofEleum>(), 6).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();
            #endregion
        }
        public static void Shadowspecs()
        {
            #region Demonshade Weapon
            Recipe.Create(ModContent.ItemType<TriactisTruePaladinianMageHammerofMightMelee>()).
                AddIngredient<MeleeTypeHammerGalaxySmasherLegacy>().
                AddIngredient(ItemID.SoulofMight, 30).
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForgeTiles>().
            Register();

            Recipe.Create(ModContent.ItemType<ShadowspecBar>(), 5).
                AddIngredient<AuricBarold>().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();
            #endregion
        }
        public static void CalamitousEssence()
        {
         #region CalamitousEssence
            Recipe.Create(ModContent.ItemType<TriactisTruePaladinianMageHammerofMightMelee>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<SomaPrime>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<AngelicAlliance>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<LoreCynosure>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<Apotheosis>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<Ozzathoth>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<Contagion>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<CrystylCrusher>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<DemonshadeBreastplate>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<DemonshadeGreaves>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<DemonshadeHelm>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<DraconicDestruction>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<Earth>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<Endogenesis>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<Eternity>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<Sylvestaff>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<FlamsteedRing>()).
                AddIngredient<CalamitousEssence>().
            Register();

            Recipe.Create(ModContent.ItemType<IllustriousKnives>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<NanoblackReaper>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<RainbowPartyCannon>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<RedSun>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<ScarletDevil>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<StaffofBlushie>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<Svantechnical>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<TemporalUmbrella>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<TheDanceofLight>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<UniverseSplitter>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<Voidragon>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<ShadowspecDye>(),10).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<BossRushTier1MusicBox>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<BossRushTier2MusicBox>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<BossRushTier3MusicBox>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<BossRushTier4MusicBox>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<BossRushTier5MusicBox>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<IridescentExcalibur>()).
                AddIngredient<CalamitousEssence>().
                Register();
            #endregion
        }
        public static void Cyrobar()
        {
            Recipe.Create(ModContent.ItemType<HoarfrostBow>()).
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
            Recipe.Create(ModContent.ItemType<Avalanche>()).
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
            Recipe.Create(ModContent.ItemType<Icebreaker>()).
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
            Recipe.Create(ModContent.ItemType<SnowstormStaff>()).
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
            Recipe.Create(ModContent.ItemType<SoulofCryogen>()).
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
            Recipe.Create(ModContent.ItemType<FrostFlare>()).
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
            Recipe.Create(ModContent.ItemType<CryoStone>()).
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
            Recipe.Create(ItemID.FrostStaff).
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();

            int[] frostArmor =
            [
                ItemID.FrostBreastplate,
                ItemID.FrostHelmet,
                ItemID.FrostLeggings,
            ];

            foreach(var forstArmorCustomCraft in frostArmor)
            {
                Recipe.Create(forstArmorCustomCraft).
                    AddIngredient<CryoBar>(10).
                    AddIngredient(ItemID.FrostCore, 2).
                    AddTile(TileID.IceMachine).
                    Register();
            }
        }
    }
}