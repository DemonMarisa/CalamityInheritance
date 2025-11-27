using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Accessories.Magic;
using CalamityInheritance.Content.Items.Accessories.Melee;
using CalamityInheritance.Content.Items.Accessories.Wings;
using CalamityInheritance.Content.Items.Tools;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Texture;
using CalamityMod.Buffs.Pets;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Melee.Shortswords;
using CalamityMod.Projectiles.Melee.Spears;
using CalamityMod.Projectiles.Pets;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public static void IR<T>(Asset<Texture2D> getTex) where T : ModItem
        {
            TextureAssets.Item[ModContent.ItemType<T>()] = getTex;
        }
        public static void PR<T>(Asset<Texture2D> getTex) where T : ModProjectile
        {
            TextureAssets.Projectile[ModContent.ProjectileType<T>()] = getTex;
        }
        public static Asset<Texture2D> GI<T>() where T : ModItem
        {
            return TextureAssets.Item[ModContent.ItemType<T>()];
        }
        public static Asset<Texture2D> GP<T>() where T : ModProjectile
        {
            return TextureAssets.Projectile[ModContent.ProjectileType<T>()];
        }
        //3.6: 将重绘的排序更加严格地分类
        public static void RespriteOptions()
        {
            var R = CIRespriteConfig.Instance;  var item = TextureAssets.Item; var proj = TextureAssets.Projectile;
            #region Texture
            #region 钨钢Family
            if (GI<WulfrumAxe>() != null || GI<WulfrumHammer>() != null || GI<WulfrumPickaxe>() != null)
            {
                if(!R.WulfumTexture)
                {
                    IR<WulfrumAxe>(CIResprite.WulfrumAxeNew);
                    IR<WulfrumHammer>(CIResprite.WulfrumHammerNew);
                    IR<WulfrumPickaxe>(CIResprite.WulfrumPickaxeNew);
                }
                if (R.WulfumTexture)
                {
                    IR<WulfrumAxe>(CIResprite.WulfrumAxeOld);
                    IR<WulfrumHammer>(CIResprite.WulfrumHammerOld);
                    IR<WulfrumPickaxe>(CIResprite.WulfrumPickaxeOld);
                }
            }
            #endregion
            #region 全部的元素武器
            if (GI<ElementalShivold>() != null ||  GI<ElementalRayold>()  != null || GI<MeleeTypeElementalDisk>() != null || GI<ElementalLance>() != null || GI<ElementalGauntletold>() != null ||
                GI<ElementalBlaster>() != null || GI<ArkoftheElementsold>() != null || GI<Swordsplosion>() != null) 
            {
                if (!R.AllElemental)
                {
                    //方舟
                    IR<ArkoftheElementsold>(CIWeaponsResprite.ElemSwordCal);   
                    //短剑
                    IR<ElementalShivold>(CIWeaponsResprite.ElemShivCal);
                    PR<ElementalShivoldProj>(CIWeaponsResprite.ElemShivCal);
                    //长矛
                    IR<ElementalLance>(CIWeaponsResprite.ElemLanceCal);
                    PR<ElementalLanceProjectile>(CIWeaponsResprite.ElemLanceProjCal);
                    //飞盘(近战)
                    IR<MeleeTypeElementalDisk>(CIWeaponsResprite.ElemDiskCal);
                    PR<MeleeTypeElementalDiskProj>(CIWeaponsResprite.ElemDiskCal);
                    //射线
                    IR<ElementalRayold>(CIWeaponsResprite.ElemRayCal);
                    //手套
                    IR<ElementalGauntletold>(CIResprite.ElemGloveCal);
                    // 冲击波已经离开了我们
                    //元素BYD
                    IR<ElementalBlaster>(CIWeaponsResprite.ElemBYDCal);
                    //爆破
                    IR<Swordsplosion>(CIWeaponsResprite.RareArkCal);
                    IR<ElementalEruptionLegacy>(CIWeaponsResprite.ElemFlamethrowerCal);

                }
                if (R.AllElemental)
                {
                    //方舟
                    IR<ArkoftheElementsold>(CIWeaponsResprite.ElemSwordAlt);   
                    //短剑
                    IR<ElementalShivold>(CIWeaponsResprite.ElemShivAlt);
                    PR<ElementalShivoldProj>(CIWeaponsResprite.ElemShivAlt);
                    //长矛
                    IR<ElementalLance>(CIWeaponsResprite.ElemLanceAlt);
                    PR<ElementalLanceProjectile>(CIWeaponsResprite.ElemLanceProjAlt);
                    //飞盘(近战)
                    IR<MeleeTypeElementalDisk>(CIWeaponsResprite.ElemDiskAlt);
                    PR<MeleeTypeElementalDiskProj>(CIWeaponsResprite.ElemDiskAlt);
                    PR<MeleeTypeElementalDiskProjSplit>(CIWeaponsResprite.ElemDiskAlt);
                    //射线
                    IR<ElementalRayold>(CIWeaponsResprite.ElemRayAlt);
                    //手套
                    IR<ElementalGauntletold>(CIResprite.ElemGloveAlt);
                    //byd
                    IR<ElementalBlaster>(CIWeaponsResprite.ElemBYDAlt);
                    //爆破
                    IR<Swordsplosion>(CIWeaponsResprite.RareArkAlt);
                    //元素喷火器
                    IR<ElementalEruptionLegacy>(CIWeaponsResprite.ElemFlamethrowerAlt);
                }
            }
            #endregion
            //星流武器
            if (GI<SubsumingVortex>() != null ||
                GI<Exoblade>() != null ||
                GI<HeavenlyGale>() != null ||
                GI<VividClarity>() != null)
            {
                IR<SubsumingVortex>(CIRespriteConfig.Instance.FuckAllExo ? CIWeaponsResprite.LoveVortexLegacy : CIWeaponsResprite.FuckVortexCal);
            }
            MeleeResprite(R);
            RangedResprite(R);
            MagicResprite(R);
            SummonResprite(item, proj, R);
            RogueResprite(R);
            MeleeRogueResprite(R);
            //壁垒
            if (GI<CIRampartofDeities>() != null)
                IR<CIRampartofDeities>(R.RampartofDeitiesTexture ? CIResprite.RampartofDeitiesOld : CIResprite.RampartofDeitiesNew);
            //壁垒
            if (GI<CIRampartofDeities>() != null)
                IR<CIRampartofDeities>(R.RampartofDeitiesTexture ? CIResprite.RampartofDeitiesOld : CIResprite.RampartofDeitiesNew);
            //空灵护符
            if (GI<AncientEtherealTalisman>() != null)
                IR<AncientEtherealTalisman>(R.EtherealTalismancTexture ? CIResprite.EtherealTalismanOld : CIResprite.EtherealTalismanNew);
            //无记名灵基
            if (GP<DaawnlightSpiritOriginMinion>() != null &&
                TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] != null)
            {
                if (!R.FateGirlSprite)
                {
                    PR<DaawnlightSpiritOriginMinion>(CIResprite.FateGirlOriginal);
                    TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] = CIResprite.FateGirlOriginalBuff;
                }
                if (R.FateGirlSprite)
                {
                    PR<DaawnlightSpiritOriginMinion>(CIResprite.FateGirlLegacy);
                    TextureAssets.Buff[ModContent.BuffType<ArcherofLunamoon>()] = CIResprite.FateGirlLegacyBuff;
                }
            }
            //天使鞋
            if (GI<AngelTreads>() != null)
                IR<AngelTreads>(R.AngelTreadsResprite       ? CIResprite.AngelTreadsAlter   : CIResprite.AngelTreadsCalamity); 
            //叶明鞋
            if (GI<FasterLunarTracers>() != null)
                IR<FasterLunarTracers>(R.LunarBootsResprite ? CIResprite.LunarBootsAlter    : CIResprite.LunarBootsCalamity);
            //MOAB
            if (GI<MOAB>() != null)
                IR<MOAB>(R.MOABResprite                     ? CIResprite.MOABAlter          : CIResprite.MOABCalamity);
            //寒霜壁垒
            if (GI<FrigidBulwark>() != null)
                IR<FrigidBulwark>(R.FrigidBulwarkResprite ? CIResprite.FrigidBulwarkOld : CIResprite.FrigidBulwarkNew);
            //寒冰屏障
            if (GI<FrostBarrier>() != null)
                IR<FrostBarrier>(R.FrostBarrierResprite ? CIResprite.FrostBarrierOld : CIResprite.FrigidBulwarkNew);
            #region 增益道具
            if (GI<BloodOrange>() != null)
                IR<BloodOrange>(R.BloodOrangeResprite   ? CIResprite.HealthOrangeAlter  : CIResprite.HealthOrange);

            if (GI<MiracleFruit>() != null)
                IR<MiracleFruit>(R.MiracleFruitResprite ? CIResprite.HealthMiraAlter    : CIResprite.HealthMira);
            
            if (GI<Elderberry>() != null)
                IR<Elderberry>(R.ElderberryResprite     ? CIResprite.HealthBerryAlter   : CIResprite.HealthBerry);
            
            if (GI<Dragonfruit>() != null)
                IR<Dragonfruit>(R.DragonfruitResprite   ? CIResprite.HealthDragonAlter  : CIResprite.HealthDragon);
            
            if (GI<CometShard>() != null)
                IR<CometShard>(R.CometShardResprite     ? CIResprite.ManaShardAlter     : CIResprite.ManaShard);

            if (GI<EtherealCore>() != null)
                IR<EtherealCore>(R.EtherealCoreResprite ? CIResprite.ManaCoreAlter      : CIResprite.ManaCore);

            if (GI<PhantomHeart>() != null)
                IR<PhantomHeart>(R.EtherealCoreResprite ? CIResprite.ManaHeartAlter     : CIResprite.ManaHeart);
            //传奇武器
            if (GI<DukeLegendary>() != null)
                IR<DukeLegendary>(R.BrinyBaronResprite ? CIWeaponsResprite.BrinyBaronLegacy : CIWeaponsResprite.BrinyBaron);
            if (GI<PlanteraLegendary>() != null)
                IR<PlanteraLegendary>(R.PlantBowResprite ? CIWeaponsResprite.PlantBowLegacy : CIWeaponsResprite.PlantBow);
            if (GI<RavagerLegendary>() != null)
                IR<RavagerLegendary>(R.Vesu ? CIWeaponsResprite.VolcanoLegacy : CIWeaponsResprite.Volcano);
            if (GI<DestroyerLegendary>() != null)
                IR<DestroyerLegendary>(R.SHPC ? CIWeaponsResprite.P90Legacy: CIWeaponsResprite.SHPC);
            if (GI<PBGLegendary>() != null)
                IR<PBGLegendary>(R.Mala ? CIWeaponsResprite.MalaLegacy : CIWeaponsResprite.Mala);
            #endregion
            #endregion
        }

        private static void MeleeRogueResprite(CIRespriteConfig R)
        {
            #region 战/盗混合武器
            #region 泰阿克提斯之锤
            //TODO:大锤子射弹贴图没能成功切换
            if(GI<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>()!=null)
            {
                if(!R.TriactisHammerResprite)
                {
                    IR<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>(CIWeaponsResprite.GiantHammerCal);
                    PR<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>(CIWeaponsResprite.GiantHammerCal);
                    PR<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>(CIWeaponsResprite.GiantHammerCal);
                    PR<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>(CIWeaponsResprite.GiantHammerCal);
                }
                if(R.TriactisHammerResprite)
                {
                    IR<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>(CIWeaponsResprite.GiantHammerAlt);
                    PR<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>(CIWeaponsResprite.GiantHammerCal);
                    PR<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>(CIWeaponsResprite.GiantHammerCal);
                    PR<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>(CIWeaponsResprite.GiantHammerCal);
                }
            }
            #endregion
            #region 圣时之锤
            //TODO:同上
            if(GI<MeleeTypeHammerPwnageLegacy>()!=null)
            {
                if(!R.PwnagehammerResprite)
                {
                    IR<MeleeTypeHammerPwnageLegacy>(CIWeaponsResprite.PwnageHammerCal);
                    IR<RogueTypeHammerPwnageLegacy>(CIWeaponsResprite.PwnageHammerCal);

                    PR<MeleeTypeHammerPwnageLegacyProj>(CIWeaponsResprite.PwnageHammerCal);
                    PR<RogueTypeHammerPwnageLegacyProj>(CIWeaponsResprite.PwnageHammerCal);
                }
                if(R.PwnagehammerResprite)
                {
                    IR<MeleeTypeHammerPwnageLegacy>(CIWeaponsResprite.PwnageHammerAlt);
                    IR<RogueTypeHammerPwnageLegacy>(CIWeaponsResprite.PwnageHammerAlt);
                    
                    PR<MeleeTypeHammerPwnageLegacyProj>(CIWeaponsResprite.PwnageHammerCal);
                    PR<RogueTypeHammerPwnageLegacyProj>(CIWeaponsResprite.PwnageHammerCal);
                }
            }
            #endregion
            #region 星体击碎者(们)
            //mod内有两把星体击碎者，两把都用同一句话的判定
            if (GI<MeleeTypeHammerStellarContemptLegacy>() != null &&
                GP<MeleeTypeHammerStellarContemptLegacyProj>() != null &&
                GI<RogueTypeHammerStellarContempt>() != null &&
                GP<RogueTypeHammerStellarContemptProj>() != null)
            {
                if (!R.StellarContemptResprite)
                {
                    IR<MeleeTypeHammerStellarContemptLegacy>(CIWeaponsResprite.StellarContemptNew);
                    PR<MeleeTypeHammerStellarContemptLegacyProj>(CIWeaponsResprite.StellarContemptNew);
                    IR<RogueTypeHammerStellarContempt>(CIWeaponsResprite.StellarContemptNew);
                    PR<RogueTypeHammerStellarContemptProj>(CIWeaponsResprite.StellarContemptNew);
                    PR<RogueTypeHammerStellarContemptProjClone>(CIWeaponsResprite.StellarContemptNew);
                }
                if (R.StellarContemptResprite)
                {
                    IR<MeleeTypeHammerStellarContemptLegacy>(CIWeaponsResprite.StellarContemptOld);
                    PR<MeleeTypeHammerStellarContemptLegacyProj>(CIWeaponsResprite.StellarContemptOld);
                    IR<RogueTypeHammerStellarContempt>(CIWeaponsResprite.StellarContemptOld);
                    PR<RogueTypeHammerStellarContemptProj>(CIWeaponsResprite.StellarContemptOld);
                    PR<RogueTypeHammerStellarContemptProjClone>(CIWeaponsResprite.StellarContemptOld);
                }
            }
            #endregion
            #region 苍穹飞刀
            //飞刀类，因为原灾也有，所以也归入战/盗混合武器
            //Todo: 可能给原灾的飞刀上贴图切换?
            if (GI<RogueTypeKnivesEmpyrean>() != null &&
                GP<RogueTypeKnivesEmpyreanProj>() != null)
            {
                if (!R.GodSlayerKnivesResprite)
                {
                    IR<RogueTypeKnivesEmpyrean>(CIWeaponsResprite.EmpyreanKnivesCal);
                    PR<RogueTypeKnivesEmpyreanProj>(CIWeaponsResprite.EmpyreanKnivesCalProj);
                }
                if (R.GodSlayerKnivesResprite)
                {
                    IR<RogueTypeKnivesEmpyrean>(CIWeaponsResprite.EmpyreanKnivesAlt);
                    PR<RogueTypeKnivesEmpyreanProj>(CIWeaponsResprite.EmpyreanKnivesAltProj);
                }
            }
            #endregion
            #region 圣光飞刀
            if (GI<RogueTypeKnivesShadowspec>() != null &&
                GP<RogueTypeKnivesShadowspecProj>() != null)
            {
                switch (R.ShadowspecKnivesResprite)
                {
                    case 1:
                        IR<RogueTypeKnivesShadowspec>(CIWeaponsResprite.ShadowKnivesCal);
                        PR<RogueTypeKnivesShadowspecProj>(CIWeaponsResprite.ShadowKnivesCalProj);
                        break;
                    case 2:
                        IR<RogueTypeKnivesShadowspec>(CIWeaponsResprite.ShadowKnivsAlt3);
                        PR<RogueTypeKnivesShadowspecProj>(CIWeaponsResprite.ShadowKnivsAlt3Proj);
                        break;
                    case 3:
                        IR<RogueTypeKnivesShadowspec>(CIWeaponsResprite.ShadowKnivsAlt2);
                        PR<RogueTypeKnivesShadowspecProj>(CIWeaponsResprite.ShadowKnivsAlt2Proj);
                        break;
                    case 4:
                        IR<RogueTypeKnivesShadowspec>(CIWeaponsResprite.ShadowKnivsAlt1);
                        PR<RogueTypeKnivesShadowspecProj>(CIWeaponsResprite.ShadowKnivsAlt3Proj);
                        break;
                }
            }
            #endregion
            #endregion

        }

        private static void SummonResprite(Asset<Texture2D>[] item, Asset<Texture2D>[] proj, CIRespriteConfig r)
        {
        }

        private static void RogueResprite(CIRespriteConfig R)
        {
            if (GI<Prismalline>() != null)
                IR<Prismalline>(R.PrismallineResprite ? CIWeaponsResprite.PrismllAlt : CIWeaponsResprite.PrismllCal);

            if (GI<RadiantStar>() != null)
                IR<RadiantStar>(R.RadiantStarResprite ? CIWeaponsResprite.RadiantAlt : CIWeaponsResprite.RadiantCal);

            if (GI<ShatteredSun>() != null)
                IR<ShatteredSun>(R.ShatteredSunResprite? CIWeaponsResprite.ShatteredAlt : CIWeaponsResprite.ShatteredCal);
            
            if (GI<ScarletDevil>() is not null)
                IR<ScarletDevil>(R.ScarletDevil ? CIWeaponsResprite.ScarletDevilAlter : CIWeaponsResprite.ScarletDevil);
        }

        private static void MagicResprite(CIRespriteConfig R)
        {
            if (GI<HeliumFlashLegacy>() != null)
                IR<HeliumFlashLegacy>(R.HeliumFlashResprite ? CIWeaponsResprite.HeliumFlashLegacy : CIWeaponsResprite.HeliumCal);

            if (GI<PlasmaRod>() is not null)
                IR<PlasmaRod>(R.PlasmaRod ? CIWeaponsResprite.PlasmaRodAlter : CIWeaponsResprite.PlasmaRod);

            if (GI<StaffofBlushie>() is not null)
                IR<StaffofBlushie>(R.StaffofBlushie ? CIWeaponsResprite.StaffofBlushieLegacy : CIWeaponsResprite.StaffofBlushie);
        }

        private static void RangedResprite(CIRespriteConfig R)
        {
            if (GI<Skullmasher>() != null)
                IR<Skullmasher>(R.SkullmasherResprite ? CIWeaponsResprite.Skullmasher : CIWeaponsResprite.SkullmasherCal);

            if (GI<P90Legacy>() != null)
                IR<P90Legacy>(R.P90Resprite ? CIWeaponsResprite.P90Legacy : CIWeaponsResprite.P90);
                
            //byd你用星体击碎者的开关切换龙弓贴图是吧 
            if (GI<DrataliornusLegacy>() != null &&
                GP<DragonBow>() != null)
            {
                if (!R.DrataliornusResprite)
                {
                    IR<DrataliornusLegacy>(CIWeaponsResprite.DrataBowLegacyAlt);
                    PR<DragonBow>(CIWeaponsResprite.DrataBowLegacyAlt);
                }
                if (R.DrataliornusResprite)
                {
                    IR<DrataliornusLegacy>(CIWeaponsResprite.DrataliornusLegacy);
                    PR<DragonBow>(CIWeaponsResprite.DrataliornusLegacy);
                }
            }
            if (GI<SomaPrimeOld>() is not null)
                IR<SomaPrimeOld>(R.SomaPrime ? CIWeaponsResprite.SomaPrimeAlter : CIWeaponsResprite.SomaPrime);
        }

        private static void MeleeResprite(CIRespriteConfig R)
        {
            if (GI<ArkoftheCosmosold>() != null)
                IR<ArkoftheCosmosold>( R.ArkofCosmosTexture ?  CIWeaponsResprite.AotCAlt : CIWeaponsResprite.AotCCal);
       
            if (GI<MirrorBlade>() != null) 
            {
                IR<MirrorBlade>(R.MirrorBlade ? CIWeaponsResprite.MirrorBladeAlter : CIWeaponsResprite.MirrorBlade);
                PR<MirrorBlast>(R.MirrorBlade ? CIWeaponsResprite.MirrorBladeProjAlter : CIWeaponsResprite.MirrorBladeProj);
            }
            #region 圣短剑
            if (GI<ExcaliburShortsword>() != null ||
                GI<TrueExcaliburShortsword>() != null ||
                GI<NightsStabber>() != null ||
                GI<TrueNightsStabber>() != null ||
                GI<EutrophicShank>() != null ||
                GI<GalileoGladius>() != null ||
                GI<FlameburstShortsword>() != null ||
                GI<AncientShiv>() != null ||
                GI<LeechingDagger>() != null
                )
            {
                if (!R.AllShivs)
                {
                    //圣短剑
                    IR<ExcaliburShortsword>         (CIWeaponsResprite.HallowedShivCal);
                    PR<ExcaliburShortswordProj>     (CIWeaponsResprite.HallowedShivCal);
                    //真圣短剑
                    IR<TrueExcaliburShortsword>     (CIWeaponsResprite.TrueHallowedShivCal);
                    PR<TrueExcaliburShortswordProj> (CIWeaponsResprite.TrueHallowedShivCal);
                    //永夜短剑
                    IR<NightsStabber>               (CIWeaponsResprite.NightShivCal);
                    PR<NightsStabberProj>           (CIWeaponsResprite.NightShivCal);
                    //真永夜短剑
                    IR<TrueNightsStabber>           (CIWeaponsResprite.TrueNightShivCal);
                    PR<TrueNightsStabberProj>       (CIWeaponsResprite.TrueNightShivCal);
                    //伽利略
                    IR<GalileoGladius>              (CIWeaponsResprite.GalileoCal);
                    PR<GalileoGladiusProj>          (CIWeaponsResprite.GalileoCal);
                    //水华
                    IR<EutrophicScimitar>           (CIWeaponsResprite.SeaShivCal);
                    PR<EutrophicScimitarProj>       (CIWeaponsResprite.SeaShivCal);
                    //狱炎
                    IR<FlameburstShortsword>        (CIWeaponsResprite.FlameShivCal);
                    PR<FlameburstShortswordProj>    (CIWeaponsResprite.FlameShivCal);
                    //远古
                    IR<AncientShiv>                 (CIWeaponsResprite.DungeonShivCal);
                    PR<AncientShivProj>             (CIWeaponsResprite.DungeonShivCal);
                    //腐巢
                    IR<LeechingDagger>              (CIWeaponsResprite.HiveMindShivCal);
                    PR<LeechingDaggerProj>          (CIWeaponsResprite.HiveMindShivCal);
                }
                if (R.AllShivs)
                {
                    //圣短剑
                    IR<ExcaliburShortsword>         (CIWeaponsResprite.HallowedShivAlt);
                    PR<ExcaliburShortswordProj>     (CIWeaponsResprite.HallowedShivAlt);
                    //真圣短剑
                    IR<TrueExcaliburShortsword>     (CIWeaponsResprite.TrueHallowedShivAlt);
                    PR<TrueExcaliburShortswordProj> (CIWeaponsResprite.TrueHallowedShivAlt);
                    //永夜短剑
                    IR<NightsStabber>               (CIWeaponsResprite.NightShivAlt);
                    PR<NightsStabberProj>           (CIWeaponsResprite.NightShivAlt);
                    //真永夜短剑
                    IR<TrueNightsStabber>           (CIWeaponsResprite.TrueNightShivAlt);
                    PR<TrueNightsStabberProj>       (CIWeaponsResprite.TrueNightShivAlt);
                    //伽利略
                    IR<GalileoGladius>              (CIWeaponsResprite.GalileoAlt);
                    PR<GalileoGladiusProj>          (CIWeaponsResprite.GalileoAlt);
                    //水华
                    IR<EutrophicScimitar>           (CIWeaponsResprite.SeaShivAlt);
                    PR<EutrophicScimitarProj>       (CIWeaponsResprite.SeaShivAlt);
                    //狱炎
                    IR<FlameburstShortsword>        (CIWeaponsResprite.FlameShivAlt);
                    PR<FlameburstShortswordProj>    (CIWeaponsResprite.FlameShivAlt);
                    //远古
                    IR<AncientShiv>                 (CIWeaponsResprite.DungeonShivAlt);
                    PR<AncientShivProj>             (CIWeaponsResprite.DungeonShivAlt);
                    //腐巢
                    IR<LeechingDagger>              (CIWeaponsResprite.HiveMindShivAlt);
                    PR<LeechingDaggerProj>          (CIWeaponsResprite.HiveMindShivAlt);
                }
            }
            #endregion
            #region 庇护
            if (GI<AegisBlade>() != null)
                IR<AegisBlade>(R.AegisResprite ? CIWeaponsResprite.AegisAlt : CIWeaponsResprite.AegisCal);
            #endregion
        }

    }
}