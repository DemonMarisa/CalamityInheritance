using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Accessories.Wings;
using CalamityInheritance.Content.Items.Armor.Silva;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.MiscItem;
using CalamityInheritance.Content.Items.Placeables.Banner;
using CalamityInheritance.Content.Items.Tools;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Melee.Boomerang;
using CalamityInheritance.Content.Items.Weapons.Melee.Flails;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Core;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.Demonshade;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Items.Dyes;
using CalamityMod.Items.Fishing.BrimstoneCragCatches;
using CalamityMod.Items.Fishing.SunkenSeaCatches;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Mounts;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Abyss;
using CalamityMod.Items.Placeables.Crags;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Placeables.FurnitureAbyss;
using CalamityMod.Items.Placeables.FurnitureAcidwood;
using CalamityMod.Items.Placeables.FurnitureAncient;
using CalamityMod.Items.Placeables.FurnitureAshen;
using CalamityMod.Items.Placeables.FurnitureAuric;
using CalamityMod.Items.Placeables.FurnitureBotanic;
using CalamityMod.Items.Placeables.FurnitureCosmilite;
using CalamityMod.Items.Placeables.FurnitureExo;
using CalamityMod.Items.Placeables.FurnitureMonolith;
using CalamityMod.Items.Placeables.FurnitureNavystone.FurnitureAncientNavystone;
using CalamityMod.Items.Placeables.FurnitureOtherworldly;
using CalamityMod.Items.Placeables.FurniturePlagued;
using CalamityMod.Items.Placeables.FurnitureProfaned;
using CalamityMod.Items.Placeables.FurnitureSacrilegious;
using CalamityMod.Items.Placeables.FurnitureSilva;
using CalamityMod.Items.Placeables.FurnitureStatigel;
using CalamityMod.Items.Placeables.FurnitureStratus;
using CalamityMod.Items.Placeables.FurnitureVoid;
using CalamityMod.Items.Placeables.FurnitureWulfrum;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Placeables.Plates;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Tools;
using CalamityMod.Items.Tools.ClimateChange;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Weapons.Typeless;
using CalamityMod.Tiles.Furniture.CraftingStations;
using LAP.Content.RecipeGroupAdd;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DraedonsForge = CalamityMod.Items.Placeables.Furniture.CraftingStations.DraedonsForge;
using DraedonsForgeold = CalamityInheritance.Content.Items.Placeables.Furniture.CraftingStations.DraedonsForgeold;
using DraedonsForgeTiles = CalamityMod.Tiles.Furniture.CraftingStations.DraedonsForge;
using VividClarity = CalamityMod.Items.Weapons.Magic.VividClarity;

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

            Recipe.Create(ItemType<ShadowspecBar>(), 50).
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
                AddTile(TileID.Hellforge).
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
                AddRecipeGroup(LAPRecipeGroup.AnyCobaltBar, 10).
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
            //魔法箭袋
            Recipe.Create(ItemID.MagicQuiver).
                AddIngredient(ItemType<BlightedLens>(), 5).
                AddIngredient(ItemID.EndlessQuiver).
                AddIngredient(ItemID.PixieDust, 10).
                AddIngredient(ItemID.SoulofLight, 8).
                AddTile(TileID.IceMachine).
                Register();
            #endregion
        }
        public override void PostAddRecipes()
        {
            YharonEclispe();        //龙一/龙二, 请确保这一修改最后被执行。
        }
        public static void YharonEclispe()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe rec = Main.recipe[i];
                int stack = rec.createItem.stack;
                //1.化魂神晶：有日食碎片的版本必须关闭龙一龙二差分才能合成
                if (rec.HasResult<AscendantSpiritEssence>() && stack == 1)
                {
                    rec.AddCondition(CIConditions.TurnOffSolarEclipseChange);
                }
            }
            // 化魂神晶添加一份必须打开差分才能用的合成
            Recipe.Create(ItemType<AscendantSpiritEssence>()).
                AddIngredient<Necroplasm>(2).
                AddIngredient<NightmareFuel>(5).
                AddIngredient<EndothermicEnergy>(5).
                AddCondition(CIConditions.OpenSolarEclipseChange).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }

        public static void Misc()
        {
            Recipe.Create(ItemID.Feather).
                AddIngredient<DesertFeather>().
                AddCondition(Condition.NearWater).
                DisableDecraft().
                Register();

            Recipe.Create(ItemType<Rock>()).
                AddIngredient<DefiledFeather>().
                DisableDecraft().
                Register();

            Recipe.Create(ItemType<Cosmolight>()).
                AddIngredient(ItemType<Moonlight>()).
                AddIngredient(ItemType<Daylight>()). 
                AddIngredient(ItemID.FallenStar).
                AddTile(TileID.CrystalBall). 
                //Scarlet 1/22:修改合成站为水晶球，防止玩家在困难模式前就能通过宇宙之光微光拆解获得肉后魂
                Register();
            Recipe.Create(ItemType<FrostcrushValari>()).
                AddIngredient<Kylie>().
                AddIngredient<CryoBar>(6).
                AddIngredient<Voidstone>(40).
                AddIngredient<CoreofEleum>(5).
                AddTile(TileID.MythrilAnvil).
                Register();

            Recipe.Create(ItemType<IceBarrage>()).
                AddIngredient(ItemID.BlizzardStaff, 1).
                AddIngredient(ItemID.IceRod, 1).
                AddIngredient<IcicleStaff>().
                AddIngredient<EndothermicEnergy>(23).
                AddIngredient<CryoBar>(18).
                AddTile<CosmicAnvil>().
                Register();
            
            Recipe.Create(ItemType<FlarefrostBlade>()).
                AddIngredient(ItemID.HellstoneBar, 8).
                AddIngredient<CryoBar>(8).
                AddIngredient(ItemID.SoulofLight, 3).
                AddTile(TileID.MythrilAnvil).
                Register();
            Recipe.Create(ItemType<GalaxySmasher>()).
                AddIngredient<MeleeStellarContempt>().
                AddIngredient<CosmiliteBar>(10).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ItemType<StellarContempt>()).
                AddIngredient<MeleeFallenHammer>().
                AddIngredient(ItemID.LunarBar, 10).
                AddIngredient<GalacticaSingularity>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();

            Recipe.Create(ItemType<Murasama>()).
                AddIngredient(ItemType<MurasamaNeweffect>()).
                Register();

            Recipe.Create(ItemType<Terratomere>()).
                AddIngredient(ItemType<TerraEdge>()).
                AddIngredient(ItemType<Hellkite>()).
                AddIngredient(ItemType<UelibloomBar>()).
                AddIngredient(ItemType<Floodtide>()).
                AddIngredient(ItemID.LunarBar).
                AddTile(TileID.LunarCraftingStation).
                Register();

            Recipe.Create(ItemType<DraedonPowerCell>(), 333).
                AddIngredient(ItemType<DubiousPlating>(), 2).
                AddIngredient(ItemType<MysteriousCircuitry>()).
                AddIngredient(ItemID.CopperBar).
                Register();

            Recipe.Create(ItemType<PlasmaDriveCore>()).
                AddIngredient(ItemType<DubiousPlating>(), 5).
                AddIngredient(ItemType<MysteriousCircuitry>(), 10).
                AddRecipeGroup(LAPRecipeGroup.AnyCopperBar, 10).
                AddTile(TileID.MythrilAnvil).
                Register();

            Recipe.Create(ItemType<SuspiciousScrap>()).
                AddRecipeGroup(LAPRecipeGroup.AnyCopperBar, 10).
                AddRecipeGroup(RecipeGroupID.IronBar, 10).
                AddTile(TileID.Anvils).
                Register();
            
            Recipe.Create(ItemID.TerraBlade).
                AddIngredient(ItemType<TrueBloodyEdge>()).
                AddIngredient(ItemID.TrueExcalibur).
                AddIngredient(ItemType<LivingShard>(),7). //我不是很清楚为啥这个生命碎片还留着
                AddIngredient(ItemID.BrokenHeroSword).
                AddTile(TileID.MythrilAnvil).
                Register();

            Recipe.Create(ItemType<RoverDrive>()).
                AddIngredient(ItemType<WulfrumMetalScrap>(), 10).
                AddIngredient(ItemType<WulfrumBattery>()).
                AddIngredient(ItemType<EnergyCore>(), 2).
                AddTile(TileID.MythrilAnvil).
                Register();
            // 三核心可以合成灾厄核心
            Recipe.Create(ItemType<CoreofCalamity>(), 3).
                AddIngredient(ItemType<CoreofChaos>()).
                AddIngredient(ItemType<CoreofEleum>()).
                AddIngredient(ItemType<CoreofSunlight>()).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
        public static void Accelerator()
        {
            #region T1粒子加速器
            #region 神龛物品

            Recipe.Create(ItemType<GladiatorsLocket>()).
                AddRecipeGroup(LAPRecipeGroup.AnyGoldBar, 5).
                AddIngredient(ItemID.MarbleBlock, 10).
                AddIngredient(ItemID.Gladius, 1).
                AddTile<AcceleratorT1Tile>().
                Register();

            Recipe.Create(ItemType<LuxorsGift>()).
                AddIngredient(ItemID.Ruby, 1).
                AddRecipeGroup(LAPRecipeGroup.AnyGoldBar, 10).
                AddIngredient(ItemID.HellstoneBar, 5).
                AddIngredient(ItemID.Bone, 5).
                AddIngredient(ItemID.JungleSpores, 5).
                AddIngredient(ItemID.BeeWax, 5).
                AddIngredient<SulphuricScale>(5).
                DisableDecraft().
                AddTile<AcceleratorT1Tile>().
                Register();

            Recipe.Create(ItemType<TrinketofChi>()).
                AddIngredient(ItemID.Silk, 15).
                AddIngredient(ItemID.Ruby).
                AddTile<AcceleratorT1Tile>().
                Register();
            
            Recipe.Create(ItemType<FungalSymbiote>()).
                AddIngredient(ItemID.GlowingMushroom, 30).
                AddIngredient(ItemID.Mushroom, 10).
                AddTile<AcceleratorT1Tile>().
                Register();
            
            Recipe.Create(ItemType<UnstableGraniteCore>()).
                AddIngredient<EnergyCore>(1).
                AddIngredient(ItemID.GraniteBlock, 50).
                AddTile<AcceleratorT1Tile>().
                Register();
            
            Recipe.Create(ItemType<TundraLeash>()).
                AddIngredient(ItemID.FlinxFur, 10).
                AddIngredient(ItemID.Leather, 10).
                AddIngredient(ItemID.HealingPotion, 5).
                AddTile<AcceleratorT1Tile>().
                Register();
            
            Recipe.Create(ItemType<CorruptionEffigy>()).
                AddIngredient(ItemID.ShadowScale, 10).
                AddIngredient(ItemID.RottenChunk, 10).
                AddIngredient(ItemID.VileMushroom, 10).
                AddIngredient(ItemID.DemoniteBar, 10).
                DisableDecraft().
                AddTile<AcceleratorT1Tile>().
                Register();

            Recipe.Create(ItemType<CrimsonEffigy>()).
                AddIngredient(ItemID.TissueSample, 10).
                AddIngredient(ItemID.Vertebrae, 10).
                AddIngredient(ItemID.ViciousMushroom, 10).
                AddIngredient(ItemID.CrimtaneBar, 10).
                DisableDecraft().
                AddTile<AcceleratorT1Tile>().
                Register();

            #endregion
            #region 一些困难模式前获取起来非常逆天的物品
            Recipe.Create(ItemType<OnyxExcavatorKey>()).
                AddIngredient<MarniteObliterator>(1).
                AddIngredient(ItemID.Amethyst, 16).
                AddIngredient<DubiousPlating>(15).
                AddIngredient<MysteriousCircuitry>(15).
                AddIngredient<Onyxplate>(150).
                AddIngredient(ItemID.BlackPaint, 50).
                AddTile<AcceleratorT1Tile>().
                Register();

            //纯净凝胶, 是的你没听错
            Recipe.Create(ItemType<BlightedGel>(), 25).
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
            Recipe.Create(ItemType<BloodSample>()).
                AddIngredient<RottenMatter>().
                AddCondition(Condition.CorruptWorld).
                AddTile<AcceleratorT1Tile>().
                Register();

            Recipe.Create(ItemType<RottenMatter>()).
                AddIngredient<BloodSample>().
                AddCondition(Condition.CrimsonWorld).
                AddTile<AcceleratorT1Tile>().
                Register();
            #endregion
            #region 一些极其早期的, 困难模式时才能进行的合成
            //神灯烈焰的材料
            Recipe.Create(ItemID.DjinnLamp, 1).
                AddRecipeGroup(LAPRecipeGroup.AnyGoldBar, 15).
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
            Recipe.Create(ItemType<StressPills>()).
                AddIngredient(ItemID.HealingPotion, 5).
                AddIngredient(ItemID.IronskinPotion, 5).
                AddIngredient<CoastalDemonfish>(5).
                AddIngredient<BloodOrb>(5).
                AddIngredient<EssenceofEleum>(5).
                AddTile<AcceleratorT1Tile>().
                Register();
            
            Recipe.Create(ItemType<HeartofDarkness>()).
                AddIngredient(ItemID.HealingPotion, 5).
                AddIngredient(ItemID.WrathPotion, 5).
                AddIngredient<Shadowfish>(5).
                AddIngredient<BloodOrb>(5).
                AddIngredient<EssenceofHavoc>(5).
                AddTile<AcceleratorT1Tile>().
                Register();

            Recipe.Create(ItemType<Laudanum>()).
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
                AddRecipeGroup(LAPRecipeGroup.AnyCobaltBar, 15).
                AddRecipeGroup(RecipeGroupID.IronBar, 10).
                AddIngredient(ItemID.Diamond).
                AddTile<AcceleratorT2Tile>().
                Register();

            //T2粒子加速器就可以造电路板之类的玩意了
            Recipe.Create(ItemType<MysteriousCircuitry>(), 10).
                AddRecipeGroup(RecipeGroupID.IronBar, 5).
                AddRecipeGroup(LAPRecipeGroup.AnyCopperBar, 5).
                AddIngredient(ItemID.Wire, 5).
                AddTile<AcceleratorT2Tile>().
                Register();

            Recipe.Create(ItemType<DubiousPlating>(), 20).
                AddRecipeGroup(RecipeGroupID.IronBar, 5).
                AddRecipeGroup(LAPRecipeGroup.AnyCopperBar, 5).
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
                AddRecipeGroup(LAPRecipeGroup.AnyGoldBar, 20).
                AddIngredient(ItemID.SoulofLight, 10).
                AddTile<AcceleratorT2Tile>().
                Register();
                
            #region T2粒子加速器 月后早期合成
            Recipe.Create(ItemType<ExodiumCluster>(), 5). //旧版Exodium Cluster的合成方法
                AddIngredient(ItemID.LunarOre, 5).
                AddIngredient<GalacticaSingularity>(5).
                AddTile<AcceleratorT2Tile>().
                Register();
            #endregion
            #endregion
            #region T3粒子加速器
            //终末石
            Recipe.Create(ItemType<Terminus>()).
                AddIngredient<ShadowspecBar>(5).
                DisableDecraft().
                AddTile<AcceleratorT3Tile>().
                Register();
            #endregion
        }
        public static void ScalDecoration()
        {
            Recipe.Create(ItemType<CeremonialUrn>()).
                AddIngredient<AshesofAnnihilation>(5).
                AddIngredient<AshesofCalamity>(15).
                AddTile<AcceleratorT3Tile>().
                Register();
            
            Recipe.Create(ItemType<EyeOfTheAccursedBanner>()).
                AddIngredient<OccultBrickItem>(3).
                AddIngredient(ItemID.Silk, 5).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<OccultLegionnaireBanner>()).
                AddIngredient<OccultBrickItem>(3).
                AddIngredient(ItemID.Silk, 5).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<LargeRitualCandle>()).
                AddIngredient<OccultBrickItem>(6).
                AddIngredient(ItemID.Torch).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<RitualCandle>()).
                AddIngredient<OccultBrickItem>(5).
                AddIngredient(ItemID.Torch).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<SacrilegiousCandle>()).
                AddIngredient<OccultBrickItem>(4).
                AddIngredient(ItemID.Torch).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<SacrilegiousCandelabra>()).
                AddIngredient<OccultBrickItem>(5).
                AddIngredient(ItemID.Torch, 3).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<SacrilegiousChandelier>()).
                AddIngredient<OccultBrickItem>(4).
                AddIngredient(ItemID.Torch, 4).
                AddIngredient(ItemID.Chain).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<SacrilegiousLamp>()).
                AddIngredient<OccultBrickItem>(3).
                AddIngredient(ItemID.Torch).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<SacrilegiousLantern>()).
                AddIngredient<OccultBrickItem>(6).
                AddIngredient(ItemID.Torch).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<SacrilegiousChest>()).
                AddIngredient<OccultBrickItem>(8).
                AddRecipeGroup(RecipeGroupID.IronBar, 2).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<SacrilegiousClock>()).
                AddIngredient<OccultBrickItem>(10).
                AddRecipeGroup(RecipeGroupID.IronBar, 3).
                AddIngredient(ItemID.Glass, 6).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<SacrilegiousOrgan>()).
                AddIngredient<OccultBrickItem>(15).
                AddIngredient(ItemID.Bone, 4).
                AddIngredient(ItemID.Book).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<SacrilegiousSink>()).
                AddIngredient<OccultBrickItem>(6).
                AddIngredient(ItemID.WaterBucket).
                AddIngredient(ItemID.HoneyBucket).
                AddIngredient(ItemID.LavaBucket).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<SacrilegiousTable>()).
                AddIngredient<OccultBrickItem>(8).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<MonolithOfTheAccursed>()).
                AddIngredient<OccultBrickItem>(15).
                AddTile<AcceleratorT3Tile>().
                Register();
            
            Recipe.Create(ItemType<OccultBrickItem>(), 400).
                AddRecipeGroup(LAPRecipeGroup.AnyStoneBlock, 400).
                AddIngredient<AshesofAnnihilation>().
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<SacrilegiousBed>()).
                AddIngredient<OccultBrickItem>(15).
                AddIngredient(ItemID.Silk, 5).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<SacrilegiousBathtub>()).
                AddIngredient<OccultBrickItem>(15).
                AddIngredient(ItemID.Silk, 5).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<SacrilegiousBench>()).
                AddIngredient<OccultBrickItem>(8).
                AddTile<AcceleratorT3Tile>().
                Register();

            Recipe.Create(ItemType<SacrilegiousBookcase>()).
                AddIngredient<OccultBrickItem>(20).
                AddIngredient(ItemID.Book, 10).
                AddTile<AcceleratorT3Tile>().
                Register();
        }
        public static void Exo()
        {
            #region Exo
            //星流之刃
            Recipe.Create(ItemType<Exoblade>()).
                AddRecipeGroup(CIRecipeGroup.AnyTerratomere).
                AddIngredient<AnarchyBlade>().
                AddIngredient<FlarefrostBlade>().
                AddIngredient<EntropicClaymore>().
                AddIngredient<StellarStriker>().
                AddIngredient<MiracleMatter>().
                DisableDecraft().
                AddTile(TileType<DraedonsForgeTiles>()).
                Register();
            
            Recipe.Create(ItemType<Exoblade>()).
                AddRecipeGroup(CIRecipeGroup.AnyTerratomere).
                AddIngredient<AnarchyBlade>().
                AddIngredient<FlarefrostBlade>().
                AddIngredient<EntropicClaymore>().
                AddIngredient<StellarStriker>().
                AddIngredient<AncientMiracleMatter>().
                DisableDecraft().
                AddConsumeIngredientCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(TileType<DraedonsForgeTiles>()).
                Register();

            //超级诺瓦天体球
            Recipe.Create(ItemType<Supernova>()).
                AddIngredient<SealedSingularity>().
                AddIngredient<StarofDestruction>().
                AddIngredient<TotalityBreakers>().
                AddIngredient<BallisticPoisonBomb>().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeIngredientCallback(CIRecipesCallback.DConsumeMatter).
                DisableDecraft().
                AddTile(TileType<DraedonsForgeTiles>()).
                Register();
            
            //星神之杀
            Recipe.Create(ItemType<Celestus>()).
                AddIngredient<ReboundingRainbow>().
                AddIngredient<MoltenAmputator>().
                AddIngredient<SubductionSlicer>().
                AddIngredient<EnchantedAxe>().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeIngredientCallback(CIRecipesCallback.DConsumeMatter).
                DisableDecraft().
                AddTile(TileType<DraedonsForgeTiles>()).
                Register();

            //天堂之风
            Recipe.Create(ItemType<HeavenlyGale>()).
                AddIngredient<PlanetaryAnnihilation>().
                AddIngredient<TelluricGlare>().
                AddIngredient<ClockworkBow>().
                AddIngredient<TheBallista>().
                AddIngredient<AncientMiracleMatter>().
                DisableDecraft().
                AddConsumeIngredientCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(TileType<DraedonsForgeTiles>()).
                Register();
            
            //星火解离者
            Recipe.Create(ItemType<Photoviscerator>()).
                
                AddIngredient<ChromaticEruption>().
                AddIngredient<DeadSunsWind>().
                AddIngredient<HalleysInferno>().
                AddIngredient<AncientMiracleMatter>().
                DisableDecraft().
                AddConsumeIngredientCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(TileType<DraedonsForgeTiles>()).
                Register();
            
            //磁极异变
            Recipe.Create(ItemType<MagnomalyCannon>()).
                AddIngredient<ThePack>().
                AddIngredient<ScorchedEarth>().
                AddIngredient(ItemID.ElectrosphereLauncher).
                AddIngredient<AncientMiracleMatter>().
                DisableDecraft().
                AddConsumeIngredientCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(TileType<DraedonsForgeTiles>()).
                Register();
            
            //去他妈的法师板砖
            Recipe.Create(ItemType<SubsumingVortex>()).
                AddIngredient<AuguroftheVoid>().
                AddIngredient<EventHorizon>().
                AddIngredient<TearsofHeaven>().
                AddIngredient<AncientMiracleMatter>().
                DisableDecraft().
                AddConsumeIngredientCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(TileType<DraedonsForgeTiles>()).
                Register();
                
            //星流大棒
            Recipe.Create(ItemType<VividClarity>()).
                AddIngredient(ItemType<ElementalRayold>()).
                AddRecipeGroup(CIRecipeGroup.AnyPhantasmalFury).
                AddIngredient(ItemType<ShadowboltStaff>()).
                AddIngredient(ItemType<UltraLiquidator>()).
                AddIngredient(ItemType<MiracleMatter>()).
                DisableDecraft().
                AddTile(TileType<DraedonsForgeTiles>()).
                Register();

            Recipe.Create(ItemType<VividClarity>()).
                AddIngredient(ItemType<ElementalRayold>()).
                AddRecipeGroup(CIRecipeGroup.AnyPhantasmalFury).
                AddIngredient(ItemType<ShadowboltStaff>()).
                AddIngredient(ItemType<UltraLiquidator>()).
                DisableDecraft().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeIngredientCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(TileType<DraedonsForgeTiles>()).
                Register();

            //宇宙之灵
            Recipe.Create(ItemType<CosmicImmaterializer>()).
                AddIngredient<LegionofCelestia>().
                AddIngredient<EtherealSubjugator>().
                AddIngredient<Cosmilamp>().
                AddIngredient<CalamarisLament>().
                DisableDecraft().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeIngredientCallback(CIRecipesCallback.DConsumeMatter).
                Register();
            #endregion
        }
        public static void Legendary()
        {
            #region LegendaryItems
            if (CIServerConfig.Instance.LegendaryitemsRecipes == true)
            {

                //庇护之刃
                Recipe.Create(ItemType<AegisBlade>()).
                    AddIngredient<KnowledgeGolem>().
                DisableDecraft().
                    AddTile(TileID.MythrilAnvil).
                    Register();
                //海爵剑
                Recipe.Create(ItemType<BrinyBaron>()).
                    AddIngredient<KnowledgeDukeFishron>().
                DisableDecraft().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                //宙寒弹刃
                Recipe.Create(ItemType<CosmicDischarge>()).
                    AddIngredient<KnowledgeDevourerofGods>().
                    DisableDecraft().
                    AddTile(TileType<CosmicAnvil>()).
                    Register();
                
                //叶流
                Recipe.Create(ItemType<BlossomFlux>()).
                    AddIngredient<KnowledgePlantera>().
                    DisableDecraft().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                //纯原
                Recipe.Create(ItemType<PristineFury>()).
                    AddIngredient<KnowledgeProvidence>().
                    DisableDecraft().
                    AddTile(TileID.LunarCraftingStation).
                    Register();

                //海灼
                Recipe.Create(ItemType<SeasSearing>()).
                    AddIngredient<KnowledgeAquaticScourge>().
                    DisableDecraft().
                    AddTile(TileID.Anvils).
                    Register();

                //维苏威阿斯
                Recipe.Create(ItemType<Vesuvius>()).
                    AddIngredient<KnowledgeRavager>().
                    DisableDecraft().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                //魔君水晶
                Recipe.Create(ItemType<YharimsCrystal>()).
                    AddIngredient<KnowledgeYharon>().
                    DisableDecraft().
                    AddTile(TileType<CosmicAnvil>()).
                    Register();
                
                //冰寒神性
                Recipe.Create(ItemType<GlacialEmbrace>()).
                    AddIngredient<KnowledgeCryogen>().
                    DisableDecraft().
                    AddTile(TileID.Anvils).
                    Register();

                //狮源流星
                Recipe.Create(ItemType<LeonidProgenitor>()).
                    AddIngredient<KnowledgeAstrumAureus>().
                    DisableDecraft().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                //孔雀翎
                Recipe.Create(ItemType<Malachite>()).
                    AddIngredient<KnowledgePlaguebringerGoliath>().
                    DisableDecraft().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                //归一
                Recipe.Create(ItemType<TheCommunity>()).
                    AddIngredient<KnowledgeLeviathanAnahita>().
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
            Recipe.Create(ItemType<Radiance>()).
                AddIngredient<AmbrosialAmpouleOld>().
                AddIngredient<InfectedJewel>().
                AddIngredient<AscendantSpiritEssence>(4).
                AddIngredient<AuricBar>(5).
                AddTile<CosmicAnvil>().
                Register();
            Recipe.Create(ItemType<Radiance>()).
                AddIngredient<AmbrosialAmpoule>().
                AddIngredient<InfectedJewel>().
                AddIngredient(ItemType<AscendantSpiritEssence>(), 4).
                AddIngredient(ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();
            //神壁
            Recipe.Create(ItemType<RampartofDeities>()).
                AddIngredient(ItemID.FrozenShield).
                AddIngredient<DeificAmuletLegacy>().
                AddIngredient<AscendantSpiritEssence>(4).
                AddIngredient(ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ItemType<SeraphTracers>()).
                AddIngredient(ItemType<FasterGodSlayerTracers>()).
                AddIngredient<DrewsWings>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            #endregion
            #region placeable
            Recipe.Create(ItemType<ThaumaticChair>()).
                AddIngredient<AbyssChair>().
                AddIngredient<AcidwoodChair>().
                AddIngredient<AncientChair>().
                AddIngredient<AshenChair>().
                AddIngredient<BotanicChair>().
                AddIngredient<CosmiliteChair>().
                AddIngredient<AncientNavystoneChair>().
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
                AddIngredient(ItemType<AuricBarold>()).
                AddTile(TileType<CosmicAnvil>()).
            Register();

            #endregion        
            #region materials
            Recipe.Create(ItemType<MiracleMatter>()).
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
            Recipe.Create(ItemType<AuricTeslaBodyArmor>()).
                AddIngredient<GodSlayerChestplate>().
                AddIngredient<BloodflareBodyArmor>().
                AddIngredient<TarragonBreastplate>().
                AddIngredient<AuricBarold>(2).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ItemType<AuricTeslaBodyArmor>()).
                AddIngredient<SilvaArmor>().
                AddIngredient<BloodflareBodyArmor>().
                AddIngredient<TarragonBreastplate>().
                AddIngredient<AuricBarold>(2).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ItemType<AuricTeslaCuisses>()).
                AddIngredient<GodSlayerLeggings>().
                AddIngredient<BloodflareCuisses>().    
                AddIngredient<TarragonLeggings>().    
                AddIngredient(ItemID.FlyingCarpet).
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ItemType<AuricTeslaHeadRanged>()).
                AddIngredient<GodSlayerHeadRanged>().
                AddIngredient<BloodflareHeadRanged>().
                AddIngredient<TarragonHeadRanged>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ItemType<AuricTeslaHeadRogue>()).
                AddIngredient<GodSlayerHeadRogue>().
                AddIngredient<BloodflareHeadRogue>().
                AddIngredient<TarragonHeadRogue>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ItemType<AuricTeslaHeadMelee>()).
                AddIngredient<GodSlayerHeadMelee>().
                AddIngredient<BloodflareHeadMelee>().
                AddIngredient<TarragonHeadMelee>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ItemType<AuricTeslaHeadSummon>()).
                AddIngredient<SilvaHeadSummon>().
                AddIngredient<BloodflareHeadSummon>().
                AddIngredient<TarragonHeadSummon>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ItemType<AuricTeslaHeadMagic>()).
                AddIngredient<SilvaHeadMagic>().
                AddIngredient<BloodflareHeadMagic>().
                AddIngredient<TarragonHeadMagic>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ItemType<AuricToilet>()).
                AddIngredient<BotanicChair>().
                AddIngredient<SilvaChair>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            #endregion
            #region Auric Weapon
            Recipe.Create(ItemType<ArkoftheCosmos>()).
                AddIngredient(ItemType<ArkoftheElements>()).
                AddIngredient(ItemType<FourSeasonsGalaxia>()).
                AddIngredient(ItemType<AuricBarold>()).
                AddTile(TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ItemType<Seraphim>()).
                AddIngredient(ItemType<ShatteredDawn>()).
                AddIngredient(ItemType<AuricBarold>()).
                AddTile(TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ItemType<AcesHigh>()).
                AddIngredient(ItemID.Revolver).
                AddIngredient(ItemType<ClaretCannon>()).
                AddIngredient(ItemType<FantasyTalisman>(),52).
                AddIngredient(ItemType<AuricBarold>()).
                AddTile(TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ItemType<AetherfluxCannon>()).
                AddIngredient(ItemType<NanoPurge>()).
                AddIngredient(ItemType<PurgeGuzzler>()).
                AddIngredient(ItemType<UelibloomBar>(),12).
                AddIngredient(ItemType<DivineGeode>(),8).
                AddIngredient(ItemType<AuricBarold>()).
                AddTile(TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ItemType<AltarOfTheAccursedItem>()).
                AddIngredient(ItemType<BrimstoneSlag>(),30).
                AddIngredient(ItemType<CoreofCalamity>()).
                AddIngredient(ItemType<AuricBarold>()).
                AddTile(TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ItemType<Ataraxia>()).
                AddIngredient(ItemID.BrokenHeroSword).
                AddIngredient(ItemType<AuricBarold>()).
                AddIngredient(ItemType<CosmiliteBar>(),8).
                AddIngredient(ItemType<AscendantSpiritEssence>(),2).
                AddIngredient(ItemType<NightmareFuel>(),20).
                AddTile(TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ItemType<DragonPow>()).
                AddIngredient<Mourningstar>().
                AddIngredient(ItemID.DaoofPow).
                AddIngredient(ItemID.FlowerPow).
                AddIngredient(ItemID.Flairon).
                AddIngredient<BallOFugu>().
                AddIngredient<Tumbleweed>().
                AddIngredient<UrchinFlail>().
                AddIngredient<YharonSoulFragment>(4).
                AddIngredient<AuricBarold>().
                AddTile(TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ItemType<DynamicPursuer>()).
                AddIngredient(ItemType<DimensionTearingDisk>()).
                AddIngredient(ItemType<AerialTracker>()).
                AddIngredient(ItemType<AuricBarold>()).
                AddTile(TileType<CosmicAnvil>()).
                Register();

            if (!isEnableDragonRecpieModiflication)
            {
                Recipe.Create(ItemType<MidnightSunBeacon>()).
                    AddIngredient(ItemID.XenoStaff).
                    AddIngredient(ItemID.MoonlordTurretStaff).
                    AddIngredient(ItemType<AuricBarold>()).
                    AddTile<CosmicAnvil>().
                    Register();
                
                Recipe.Create(ItemType<Nadir>()).
                    AddIngredient(ItemType<VanishingPoint>()).
                    AddIngredient(ItemType<TwistingNether>(),5).
                    AddIngredient(ItemType<DarksunFragment>(),8).
                    AddIngredient(ItemType<AuricBarold>()).
                    AddTile<CosmicAnvil>().
                    Register();
                
                Recipe.Create(ItemType<HeliumFlash>()).
                    AddIngredient(ItemType<VenusianTrident>()).
                    AddIngredient(ItemType<LashesofChaos>()).
                    AddIngredient(ItemType<ForbiddenSun>()).
                    AddIngredient(ItemID.FragmentSolar,20).
                    AddIngredient(ItemID.FragmentNebula,5).
                    AddIngredient(ItemType<AuricBarold>()).
                    AddTile<CosmicAnvil>().
                    Register();
            }

                

            Recipe.Create(ItemType<TheOracle>()).
                AddIngredient<BurningRevelation>().
                AddIngredient<TheObliterator>().
                AddIngredient(ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ItemType<TyrannysEnd>()).
                AddIngredient<GoldenEagle>().
                AddIngredient<AntiMaterielRifle>().
                AddIngredient(ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ItemType<VoidVortex>()).
                AddIngredient<VoltaicClimax>().
                AddIngredient(ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ItemID.Zenith).
                AddIngredient(ItemID.TerraBlade).
                AddIngredient(ItemID.Meowmere).
                AddIngredient(ItemID.StarWrath).
                AddIngredient(ItemID.InfluxWaver).
                AddIngredient(ItemID.TheHorsemansBlade).
                AddIngredient(ItemID.Seedler).
                AddIngredient(ItemID.Starfury).
                AddIngredient(ItemID.BeeKeeper).
                AddIngredient(ItemID.EnchantedSword).
                AddIngredient(ItemID.CopperShortsword).
                AddIngredient(ItemType<AuricBarold>()).
                AddTile(TileType<CosmicAnvil>()).
                Register();

            #endregion
            #region AuricPlaceable
            Recipe.Create(ItemType<DraedonsForge>()).
                AddIngredient(ItemType<DraedonsForgeold>()).
                AddIngredient(ItemType<ExoPrism>(), 5).
                Register();

            Recipe.Create(ItemType<AuricQuantumCoolingCell>()).
                AddIngredient(ItemType<MysteriousCircuitry>(), 8).
                AddIngredient(ItemType<DubiousPlating>(), 8).
                AddIngredient(ItemType<EndothermicEnergy>(), 40).
                AddIngredient(ItemType<CoreofEleum>(), 6).
                AddIngredient(ItemType<AuricBarold>()).
                AddTile(TileType<CosmicAnvil>()).
                Register();
            #endregion
        }
        public static void Shadowspecs()
        {
            #region Demonshade Weapon
            Recipe.Create(ItemType<TriactisTruePaladinianMageHammerofMight>()).
                AddIngredient<MeleeGalaxySmasher>().
                AddIngredient(ItemID.SoulofMight, 30).
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForgeTiles>().
            Register();

            Recipe.Create(ItemType<ShadowspecBar>(), 5).
                AddIngredient<AuricBarold>().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(TileType<DraedonsForgeTiles>()).
                Register();
            #endregion
        }
        public static void CalamitousEssence()
        {
         #region CalamitousEssence
            Recipe.Create(ItemType<TriactisTruePaladinianMageHammerofMight>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<SomaPrime>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<AngelicAlliance>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<LoreCynosure>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<Apotheosis>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<Ozzathoth>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<Contagion>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<CrystylCrusher>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<DemonshadeBreastplate>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<DemonshadeGreaves>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<DemonshadeHelm>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<DraconicDestruction>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<Earth>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<Endogenesis>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<Eternity>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<Sylvestaff>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<FlamsteedRing>()).
                AddIngredient<CalamitousEssence>().
            Register();

            Recipe.Create(ItemType<IllustriousKnives>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<NanoblackReaper>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<RainbowPartyCannon>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<RedSun>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<ScarletDevil>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<StaffofBlushie>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<Svantechnical>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<TemporalUmbrella>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<TheDanceofLight>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<UniverseSplitter>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<Voidragon>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ItemType<ShadowspecDye>(),10).
                AddIngredient<CalamitousEssence>().
                Register();
            #endregion
        }
        public static void Cyrobar()
        {
            Recipe.Create(ItemType<HoarfrostBow>()).
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
            Recipe.Create(ItemType<Avalanche>()).
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
            Recipe.Create(ItemType<Icebreaker>()).
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
            Recipe.Create(ItemType<SnowstormStaff>()).
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
            Recipe.Create(ItemType<SoulofCryogen>()).
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
            Recipe.Create(ItemType<FrostFlare>()).
                AddIngredient<CryoBar>(10).
                DisableDecraft().
                AddTile(TileID.IceMachine).
                Register();
            Recipe.Create(ItemType<CryoStone>()).
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