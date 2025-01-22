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
using CalamityInheritance.Content.Items.Placeables.Furniture.CraftingStations;
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
using VoidVortex = CalamityMod.Items.Weapons.Magic.VoidVortex;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityMod.Items.Tools;
using CalamityMod.Items.Armor.Demonshade;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityMod.Items.Dyes;
using CalamityModMusic.Items.Placeables;
using CalamityMod.Items.Tools.ClimateChange;
using CalamityInheritance.Content.Items.Tools;

namespace CalamityInheritance.Content.Items
{
    //Recipe recipe = CreateRecipe();

    //recipe.AddIngredient(ModContent.ItemType<TrueNightsStabber>());
    //recipe.AddIngredient(ItemID.BrokenHeroSword);

    //recipe.AddTile(TileID.MythrilAnvil);
    //recipe.AddTile(TileID.DemonAltar);
    //recipe.AddTile(ModContent.TileType<DraedonsForge>());
    //recipe.AddTile(ModContent.TileType<CosmicAnvil>());

    //recipe.Register();

    //Scarlet：几乎为所有大面积的玩意合成添加封装成了不同类型的函数。
    //同时使旧奇迹物质支持所有星流物品
    public class CalamityInheritanceRecipesAdd : ModSystem
    {
        public override void AddRecipes()
        {
            ExoWeaponTrain();//嘟嘟嘟，星流武器开火车咯！
            AllAuricStuffTrain();//金源物品火车
            LegendaryItemTrain();//传奇物品火车
            AllShadowSpecTrain();//魔影武器火车
            AllShadowSpecCustomTrain();//魔影特殊材料合成火车


            #region WeaponsConvertandrecipeadd

            Recipe.Create(ModContent.ItemType<GalaxySmasher>()).
                AddIngredient<StellarContemptOld>().
                AddIngredient<CosmiliteBar>(10).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<StellarContempt>()).
                AddIngredient<FallenPaladinsHammerOld>().
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
            //泰拉射线
            Recipe.Create(ModContent.ItemType<TerraRay>()).
                AddIngredient<CarnageRay>().
                AddIngredient<ValkyrieRay>().
                AddIngredient<LivingShard>().
                AddTile(TileID.MythrilAnvil).
                Register();

            Recipe.Create(ModContent.ItemType<TerraRay>()).
                AddIngredient<NightsRayold>().
                AddIngredient<ValkyrieRay>().
                AddIngredient<LivingShard>().
                AddTile(TileID.MythrilAnvil).
                Register();
            #endregion

            #region Accessories
            //辐辉
            Recipe.Create(ModContent.ItemType<Radiance>()).
                AddIngredient(ModContent.ItemType<AmbrosialAmpouleOld>()).
                AddIngredient(ModContent.ItemType<InfectedJewel>()).
                AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<Radiance>()).
                AddIngredient(ModContent.ItemType<AmbrosialAmpoule>()).
                AddIngredient(ModContent.ItemType<InfectedJewel>()).
                AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(),4).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<Radiance>()).
                AddIngredient(ModContent.ItemType<AmbrosialAmpoule>()).
                AddIngredient(ModContent.ItemType<AstralArcanum>()).
                AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<Radiance>()).
                AddIngredient(ModContent.ItemType<AmbrosialAmpoule>()).
                AddIngredient(ModContent.ItemType<AstralArcanum>()).
                AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4).
                AddIngredient(ModContent.ItemType<AuricBar>(),5).
                AddTile<CosmicAnvil>().
                Register();
            //神壁
            Recipe.Create(ModContent.ItemType<RampartofDeities>()).
                AddIngredient(ItemID.FrozenShield).
                AddIngredient<DeificAmuletLegacy>().
                AddIngredient<AscendantSpiritEssence>(4).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<TracersSeraph>()).
                AddIngredient<UtensilPoker>().
                AddIngredient(ItemID.LifeCrystal).
                AddIngredient(ItemID.Bone, 92).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<TheSponge>()).
                AddIngredient(ModContent.ItemType<AmbrosialAmpouleOld>()).
                AddIngredient(ModContent.ItemType<TheAbsorberOld>()).
                AddIngredient<MysteriousCircuitry>(10).
                AddIngredient<DubiousPlating>(20).
                AddIngredient(ModContent.ItemType<CosmiliteBar>(), 15).
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


            Recipe.Create(ModContent.ItemType<PlasmaDriveCore>()).
                AddIngredient(ModContent.ItemType<DubiousPlating>(), 5).
                AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 10).
                AddRecipeGroup("AnyMythrilBar", 10).
                AddTile(TileID.MythrilAnvil).
                Register();

            Recipe.Create(ModContent.ItemType<SuspiciousScrap>()).
                AddRecipeGroup("AnyCopperBar", 10).
                AddRecipeGroup("IronBar", 10).
                AddTile(TileID.Anvils).
                Register();
            #endregion
            #region Item
            Recipe.Create(ItemID.TerraBlade).
                AddIngredient(ModContent.ItemType<TrueBloodyEdge>()).
                AddIngredient(ItemID.TrueExcalibur).
                AddIngredient(ModContent.ItemType<LivingShard>(),7).
                AddTile(TileID.MythrilAnvil).
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
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();

            Recipe.Create(ItemID.Zenith).
                AddIngredient<TerraEdge>(). //为天顶剑添加泰拉边锋的合成路线
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
            
            #region Tools
            Recipe.Create(ModContent.ItemType<Cosmolight>()).
                AddIngredient(ModContent.ItemType<Moonlight>()).
                AddIngredient(ModContent.ItemType<Daylight>()). 
                AddIngredient(ItemID.FallenStar).
                // AddTile(TileID.Anvils).
                AddTile(TileID.CrystalBall). 
                //Scarlet 1/22:修改合成站为水晶球，防止玩家在困难模式前就能通过宇宙之光微光拆解获得肉后魂
                Register();
            #endregion
        }
        public static void ExoWeaponTrain()
        {
            #region ExoWeapons
            //星流之刃
            Recipe.Create(ModContent.ItemType<Exoblade>()).
                AddIngredient<TerratomereOld>().
                AddIngredient<AnarchyBlade>().
                AddIngredient<FlarefrostBlade>().
                AddIngredient<EntropicClaymore>().
                AddIngredient<StellarStriker>().
                AddIngredient<MiracleMatter>().
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();
            
            Recipe.Create(ModContent.ItemType<Exoblade>()).
                AddIngredient<Terratomere>().
                AddIngredient<AnarchyBlade>().
                AddIngredient<FlarefrostBlade>().
                AddIngredient<EntropicClaymore>().
                AddIngredient<StellarStriker>().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();

            Recipe.Create(ModContent.ItemType<Exoblade>()).
                AddIngredient<TerratomereOld>().
                AddIngredient<AnarchyBlade>().
                AddIngredient<FlarefrostBlade>().
                AddIngredient<EntropicClaymore>().
                AddIngredient<StellarStriker>().
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
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();

            //天堂之风
            Recipe.Create(ModContent.ItemType<HeavenlyGale>()).
                AddIngredient<PlanetaryAnnihilation>().
                AddIngredient<TelluricGlare>().
                AddIngredient<ClockworkBow>().
                AddIngredient<TheBallista>().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();
            
            //星火解离者
            Recipe.Create(ModContent.ItemType<Photoviscerator>()).
                AddIngredient<ElementalEruption>().
                AddIngredient<DeadSunsWind>().
                AddIngredient<HalleysInferno>().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();
            
            //磁极异变
            Recipe.Create(ModContent.ItemType<MagnomalyCannon>()).
                AddIngredient<ThePack>().
                AddIngredient<ScorchedEarth>().
                AddIngredient(ItemID.ElectrosphereLauncher).
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();
            
            //去他妈的法师板砖
            Recipe.Create(ModContent.ItemType<SubsumingVortex>()).
                AddIngredient<AuguroftheElements>().
                AddIngredient<EventHorizon>().
                AddIngredient<TearsofHeaven>().
                AddIngredient<AncientMiracleMatter>().
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
                AddTile(ModContent.TileType<DraedonsForgeTiles>()).
                Register();

            Recipe.Create(ModContent.ItemType<VividClarity>()).
                AddRecipeGroup("CalamityInheritance:AnyElementalRay").
                AddRecipeGroup("CalamityInheritance:AnyPhantasmalFury").
                AddIngredient(ModContent.ItemType<ShadowboltStaff>()).
                AddIngredient(ModContent.ItemType<UltraLiquidator>()).
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
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                Register();
            
            Recipe.Create(ModContent.ItemType<IridescentExcalibur>()).
                AddIngredient(ItemID.TrueExcalibur).
                AddIngredient<Orderbringer>().
                AddIngredient<AncientMiracleMatter>().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddIngredient<AshesofAnnihilation>(5).
                AddTile<DraedonsForgeTiles>().
                Register();
            #endregion
        }
        public static void LegendaryItemTrain()
        {
            #region LegendaryItems
            if (CalamityInheritanceConfig.Instance.LegendaryitemsRecipes == true)
            {

                Recipe.Create(ModContent.ItemType<ConclaveCrossfire>()).
                    AddIngredient(ItemID.FragmentVortex).
                    AddIngredient<LoreGolem>().
                    AddTile(TileID.LunarCraftingStation).
                    Register();
                //庇护之刃
                Recipe.Create(ModContent.ItemType<AegisBlade>()).
                    AddIngredient<LoreGolem>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<AegisBlade>()).
                    AddIngredient<KnowledgeGolem>().
                    AddTile(TileID.MythrilAnvil).
                    Register();
                //海爵剑
                Recipe.Create(ModContent.ItemType<BrinyBaron> ()).
                    AddIngredient<LoreDukeFishron>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<BrinyBaron>()).
                    AddIngredient<KnowledgeDukeFishron>().
                    AddTile(TileID.MythrilAnvil).
                    Register();
                //宙寒弹刃
                Recipe.Create(ModContent.ItemType<CosmicDischarge> ()).
                    AddIngredient<LoreDevourerofGods>().
                    AddTile(ModContent.TileType<CosmicAnvil>()).
                    Register();

                Recipe.Create(ModContent.ItemType<CosmicDischarge>()).
                    AddIngredient<KnowledgeDevourerofGods>().
                    AddTile(ModContent.TileType<CosmicAnvil>()).
                    Register();
                //叶流
                Recipe.Create(ModContent.ItemType<BlossomFlux> ()).
                    AddIngredient<LorePlantera>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<BlossomFlux>()).
                    AddIngredient<KnowledgePlantera>().
                    AddTile(TileID.MythrilAnvil).
                    Register();
                //纯原
                Recipe.Create(ModContent.ItemType<PristineFury>()).
                    AddIngredient<LoreProvidence>().
                    AddTile(TileID.LunarCraftingStation).
                    Register();

                Recipe.Create(ModContent.ItemType<PristineFury>()).
                    AddIngredient<KnowledgeProvidence>().
                    AddTile(TileID.LunarCraftingStation).
                    Register();
                //海灼
                Recipe.Create(ModContent.ItemType<SeasSearing>()).
                    AddIngredient<LoreAquaticScourge>().
                    AddTile(TileID.Anvils).
                    Register();

                Recipe.Create(ModContent.ItemType<SeasSearing>()).
                    AddIngredient<KnowledgeAquaticScourge>().
                    AddTile(TileID.Anvils).
                    Register();
                //维苏威阿斯
                Recipe.Create(ModContent.ItemType<Vesuvius>()).
                    AddIngredient(ItemID.DefenderMedal).
                    AddIngredient<ScoriaBar>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<Vesuvius>()).
                    AddIngredient(ItemID.DefenderMedal).
                    AddIngredient<ScoriaBar>().
                    AddTile(TileID.MythrilAnvil).
                    Register();
                //魔君水晶
                Recipe.Create(ModContent.ItemType<YharimsCrystal>()).
                    AddIngredient<LoreYharon>().
                    AddTile(ModContent.TileType<CosmicAnvil>()).
                    Register();

                Recipe.Create(ModContent.ItemType<YharimsCrystal>()).
                    AddIngredient<KnowledgeYharon>().
                    AddTile(ModContent.TileType<CosmicAnvil>()).
                    Register();
                //冰寒神性
                Recipe.Create(ModContent.ItemType<GlacialEmbrace> ()).
                    AddIngredient<LoreArchmage>().
                    AddTile(TileID.Anvils).
                    Register();

                Recipe.Create(ModContent.ItemType<GlacialEmbrace>()).
                    AddIngredient<KnowledgeCryogen>().
                    AddTile(TileID.Anvils).
                    Register();
                //狮源流星
                Recipe.Create(ModContent.ItemType<LeonidProgenitor> ()).
                    AddIngredient<LoreAstrumAureus>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<LeonidProgenitor>()).
                    AddIngredient<KnowledgeAstrumAureus>().
                    AddTile(TileID.MythrilAnvil).
                    Register();
                //孔雀翎
                Recipe.Create(ModContent.ItemType<Malachite>()).
                    AddIngredient<LorePlaguebringerGoliath>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<Malachite>()).
                    AddIngredient<KnowledgePlaguebringerGoliath>().
                    AddTile(TileID.MythrilAnvil).
                    Register();
                //归一
                Recipe.Create(ModContent.ItemType<TheCommunity>()).
                    AddIngredient<LoreLeviathanAnahita>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<TheCommunity>()).
                    AddIngredient<KnowledgeLeviathanAnahita>().
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            #endregion
        }

        public static void AllAuricStuffTrain()
        {
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

            Recipe.Create(ModContent.ItemType<AuricTeslaCuisses>()).
                AddIngredient<SilvaLeggings>().
                AddIngredient<BloodflareCuisses>().
                AddIngredient<TarragonLeggings>().
                AddIngredient(ItemID.FlyingCarpet).
                AddIngredient<AuricBarold>(2).
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
                AddIngredient(ModContent.ItemType<BlossomFlux>()).
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

            Recipe.Create(ModContent.ItemType<DynamicPursuer>()).
                AddIngredient(ModContent.ItemType<Eradicator>()).
                AddIngredient(ModContent.ItemType<TrackingDisk>()).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile(ModContent.TileType<CosmicAnvil>()).
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

            Recipe.Create(ModContent.ItemType<Kingsbane>()).
                AddIngredient(ItemID.ChainGun).
                AddIngredient<P90>().
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

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

            Recipe.Create(ModContent.ItemType<TheOracle> ()).
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

            Recipe.Create(ModContent.ItemType<VoidVortex> ()).
                AddIngredient<VoltaicClimax> ().
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
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

        public static void AllShadowSpecTrain()
        {
            #region Demonshade  Weapon
            Recipe.Create(ModContent.ItemType<TriactisTruePaladinianMageHammerofMightMelee>()).
                AddIngredient<GalaxySmasherMelee>().
                AddIngredient(ItemID.SoulofMight, 30).
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForgeTiles>().
            Register();
            
            Recipe.Create(ModContent.ItemType<ShadowspecBar>(),5).
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddIngredient<AuricBarold>(1).
                AddTile<DraedonsForgeTiles>().
                Register(); 
            #endregion
        }

        public static void AllShadowSpecCustomTrain()
        {
         #region CalamitousEssence
            Recipe.Create(ModContent.ItemType<TriactisTruePaladinianMageHammerofMightMelee>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<SomaPrime>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<SomaPrimeOld>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<AngelicAlliance>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<LoreCynosure>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<KnowledgeCalamitas>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<Apotheosis>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<Azathoth>()).
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

            Recipe.Create(ModContent.ItemType<Fabstaff>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<FabstaffOld>()).
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

            Recipe.Create(ModContent.ItemType<TemporalUmbrellaOld> ()).
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
            
            Recipe.Create(ModContent.ItemType<ElementalExcalibur>()).
                AddIngredient<CalamitousEssence>().
                Register();

            Recipe.Create(ModContent.ItemType<IridescentExcalibur>()).
                AddIngredient<CalamitousEssence>().
                Register();
            #endregion
        }
    }
}
