﻿using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityMod.Projectiles.Pets;
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
using CalamityMod.Projectiles.Melee;
using CalamityMod.NPCs.Yharon;
using ReLogic.Peripherals.RGB;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Items.Weapons.Summon;
using static CalamityMod.NPCs.BrimstoneElemental.BrimstoneElemental;
using log4net.Core;
using System.Diagnostics;
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
using CalamityModMusic;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Items.LoreItems;
using CalamityInheritance.Content.Items.LoreItems;
using Microsoft.Xna.Framework.Graphics;

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
    public class CalamityInheritanceRecipesAdd : ModSystem
    {
        public override void AddRecipes()
        {
            #region WeaponsConvertandrecipeadd
            Recipe.Create(ModContent.ItemType<ArkoftheCosmos>()).
                AddIngredient(ModContent.ItemType<ArkoftheCosmosold>()).
                Register();

            Recipe.Create(ModContent.ItemType<FourSeasonsGalaxia>()).
                AddIngredient(ModContent.ItemType<FourSeasonsGalaxiaold>()).
                Register();

            Recipe.Create(ModContent.ItemType<Exoblade>()).
                AddIngredient(ModContent.ItemType<Exobladeold>()).
                Register();

            Recipe.Create(ModContent.ItemType<Murasama>()).
                AddIngredient(ModContent.ItemType<MurasamaNeweffect>()).
                Register();

            Recipe.Create(ModContent.ItemType<ElementalShiv>()).
                AddIngredient(ModContent.ItemType<TerraShiv>()).
                AddIngredient(ModContent.ItemType<GalacticaSingularity>()).
                AddIngredient(ItemID.LunarBar).
                AddTile(TileID.AncientMythrilBrick).
                Register();

            Recipe.Create(ModContent.ItemType<Terratomere>()).
                AddIngredient(ModContent.ItemType<TerraEdge>()).
                AddIngredient(ModContent.ItemType<Hellkite>()).
                AddIngredient(ModContent.ItemType<UelibloomBar>()).
                AddIngredient(ModContent.ItemType<Floodtide>()).
                AddIngredient(ItemID.LunarBar).
                AddTile(TileID.AncientMythrilBrick).
                Register();

            Recipe.Create(ModContent.ItemType<Exoblade>()).
                AddIngredient(ModContent.ItemType<Exobladeold>()).         
                Register();

            Recipe.Create(ModContent.ItemType<DraedonPowerCell>(), 333).
                AddIngredient(ModContent.ItemType<DubiousPlating>(), 2).
                AddIngredient(ModContent.ItemType<MysteriousCircuitry>()).
                AddIngredient(ItemID.CopperBar).
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
            #region Accessories
            Recipe.Create(ModContent.ItemType<Radiance>()).
                AddIngredient(ModContent.ItemType<AmbrosialAmpoule>()).
                AddIngredient(ModContent.ItemType<InfectedJewel>()).
                AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(),4).
                AddIngredient(ModContent.ItemType<AuricBarold>()).
                AddTile<CosmicAnvil>().
                Register();

            Recipe.Create(ModContent.ItemType<RampartofDeities>()).
                AddIngredient(ItemID.FrozenShield).
                AddIngredient<DeificAmulet>().
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
            #endregion
            #region placeable

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

            Recipe.Create(ModContent.ItemType<ShadowspecBar>(),5).
                AddIngredient<ExoPrism>(5).
                AddIngredient<AuricBarold>(1).
                AddTile<DraedonsForgeTiles>().
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
            #endregion
            #region LegendaryItems
            if (CalamityInheritanceConfig.Instance.LegendaryitemsRecipes == true)
            {

                Recipe.Create(ModContent.ItemType<AegisBlade>()).
                    AddIngredient<LoreGolem>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<AegisBlade>()).
                    AddIngredient<KnowledgeGolem>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<BrinyBaron> ()).
                    AddIngredient<LoreDukeFishron>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<BrinyBaron>()).
                    AddIngredient<KnowledgeDukeFishron>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<CosmicDischarge> ()).
                    AddIngredient<LoreDevourerofGods>().
                    AddTile(ModContent.TileType<CosmicAnvil>()).
                    Register();

                Recipe.Create(ModContent.ItemType<CosmicDischarge>()).
                    AddIngredient<KnowledgeDevourerofGods>().
                    AddTile(ModContent.TileType<CosmicAnvil>()).
                    Register();

                Recipe.Create(ModContent.ItemType<BlossomFlux> ()).
                    AddIngredient<LorePlantera>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<BlossomFlux>()).
                    AddIngredient<KnowledgePlantera>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<PristineFury>()).
                    AddIngredient<LoreProvidence>().
                    AddTile(TileID.AncientMythrilBrick).
                    Register();

                Recipe.Create(ModContent.ItemType<PristineFury>()).
                    AddIngredient<KnowledgeProvidence>().
                    AddTile(TileID.AncientMythrilBrick).
                    Register();

                Recipe.Create(ModContent.ItemType<SeasSearing>()).
                    AddIngredient<LoreAquaticScourge>().
                    AddTile(TileID.Anvils).
                    Register();

                Recipe.Create(ModContent.ItemType<SeasSearing>()).
                    AddIngredient<KnowledgeAquaticScourge>().
                    AddTile(TileID.Anvils).
                    Register();

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

                Recipe.Create(ModContent.ItemType<YharimsCrystal>()).
                    AddIngredient<LoreYharon>().
                    AddTile(ModContent.TileType<CosmicAnvil>()).
                    Register();

                Recipe.Create(ModContent.ItemType<YharimsCrystal>()).
                    AddIngredient<KnowledgeYharon>().
                    AddTile(ModContent.TileType<CosmicAnvil>()).
                    Register();

                Recipe.Create(ModContent.ItemType<GlacialEmbrace> ()).
                    AddIngredient<LoreArchmage>().
                    AddTile(TileID.Anvils).
                    Register();

                Recipe.Create(ModContent.ItemType<GlacialEmbrace>()).
                    AddIngredient<KnowledgeCryogen>().
                    AddTile(TileID.Anvils).
                    Register();

                Recipe.Create(ModContent.ItemType<LeonidProgenitor> ()).
                    AddIngredient<LoreAstrumAureus>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<LeonidProgenitor>()).
                    AddIngredient<KnowledgeAstrumAureus>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<Malachite>()).
                    AddIngredient<LorePlaguebringerGoliath>().
                    AddTile(TileID.MythrilAnvil).
                    Register();

                Recipe.Create(ModContent.ItemType<Malachite>()).
                    AddIngredient<KnowledgePlaguebringerGoliath>().
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            #endregion
        }
    }
}
