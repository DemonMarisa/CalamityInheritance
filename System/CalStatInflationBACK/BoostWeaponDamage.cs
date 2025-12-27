using CalamityInheritance.Content.Items.Ammo.RangedAmmo;
using CalamityInheritance.Content.Items.Tools;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem;
using CalamityInheritance.Content.Projectiles.Rogue.Spears;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Ammo;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using LAP.Core.Enums;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.System.CalStatInflationBACK
{
    public class BoostWeaponDamage : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (CheckItem.PostMLWeapons.Contains(item.type))
            {
                item.LAP().UseCICalStatInflation = true;
                item.LAP().WeaponTier = AllWeaponTier.PostMoonLord;
            }
            if (CheckItem.PostProfanedWeapons.Contains(item.type) || CheckItem.PostSentinelsWeapon.Contains(item.type))
            {
                item.LAP().UseCICalStatInflation = true;
                item.LAP().WeaponTier = AllWeaponTier.PostProvidence;
            }
            if (CheckItem.PostPolterghastWeapons.Contains(item.type))
            {
                item.LAP().UseCICalStatInflation = true;
                item.LAP().WeaponTier = AllWeaponTier.PostPolterghast;
            }
            if (CheckItem.PostOldDukeWeapons.Contains(item.type))
            {
                item.LAP().UseCICalStatInflation = true;
                item.LAP().WeaponTier = AllWeaponTier.PostOldDuke;
                if (item.type == ModContent.ItemType<InsidiousImpalerLegacy>())
                {
                    item.LAP().UseCustomStatInflationMult = true;
                    item.LAP().StatInflationMult = 1.4f;
                }
            }
            if (CheckItem.PostDOGWeapons.Contains(item.type))
            {
                item.LAP().UseCICalStatInflation = true;
                item.LAP().WeaponTier = AllWeaponTier.PostDOG;
            }
            if (CheckItem.PostyharonWeapons.Contains(item.type))
            {
                item.LAP().UseCICalStatInflation = true;
                item.LAP().WeaponTier = AllWeaponTier.PostYharon;
            }
            if (CheckItem.PostExoAndScalWeapons.Contains(item.type))
            {
                item.LAP().UseCICalStatInflation = true;
                item.LAP().WeaponTier = AllWeaponTier.PostExoMech;
            }
            if (CheckItem.PostShadowspecWeapons.Contains(item.type))
            {
                item.LAP().UseCICalStatInflation = true;
                item.LAP().WeaponTier = AllWeaponTier.DemonShadow;
            }
            AuricBlance(item);
            ShadowspecBlance(item);
            CosmicBlance(item);
            PolterghastBlance(item);
            ProfanedBlance(item);
            AmmoChange(item);
            ExoWeapons(item);
            YharonBlance(item);
        }

        public override float UseAnimationMultiplier(Item item, Player player)
        {
            if (!CIServerConfig.Instance.CalStatInflationBACK)
                return base.UseAnimationMultiplier(item, player);

            if (item.type == ModContent.ItemType<CosmicShiv>())
                return 0.4f;
            else
                return base.UseAnimationMultiplier(item, player);
        }

        public override float UseTimeMultiplier(Item item, Player player)
        {
            if (!CIServerConfig.Instance.CalStatInflationBACK)
                return base.UseTimeMultiplier(item, player);

            if (item.type == ModContent.ItemType<CosmicShiv>())
                return 0.4f;
            else
                return base.UseTimeMultiplier(item, player);
        }

        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
            if (!CIServerConfig.Instance.CalStatInflationBACK)
                return;
            if (item.type == ModContent.ItemType<Earth>())
            {
                scale = 2.5f;
            }
        }
        #region 特殊平衡改动
        public static void SetCustomMult(Item item, float mult)
        {
            item.LAP().UseCICalStatInflation = true;
            item.LAP().UseCustomStatInflationMult = true;
            item.LAP().StatInflationMult = mult;
        }
        public static void SetCustomMult(Item item, int TargetDamage)
        {
            item.LAP().UseCICalStatInflation = true;
            item.LAP().UseCustomStatInflationMult = true;
            float mult = TargetDamage / (float)item.damage;
            item.LAP().StatInflationMult = mult;
        }
        #region 亵渎
        public static void ProfanedBlance(Item item)
        {
            #region 射手
            if (item.type == ModContent.ItemType<TelluricGlare>())
                item.LAP().GlobalMult *= 3f;
            #endregion
            #region 法师
            if (item.type == ModContent.ItemType<PlasmaRifle>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<ThePrince>())
                item.LAP().GlobalMult *= 1.5f;
            #endregion
        }
        #endregion
        #region 幽花后
        public static void PolterghastBlance(Item item)
        {
            #region 战士
            if (item.type == ModContent.ItemType<NeptunesBounty>())
                item.LAP().GlobalMult *= 1.3f;
            #endregion
            #region 射手
            if (item.type == ModContent.ItemType<ClaretCannon>())
                item.LAP().GlobalMult *= 1.6f;

            if (item.type == ModContent.ItemType<SulphuricAcidCannon>())
                item.LAP().GlobalMult *= 1.7f;

            if (item.type == ModContent.ItemType<DodusHandcannon>())
                item.LAP().GlobalMult *= 1.6f;

            if (item.type == ModContent.ItemType<TheMaelstrom>())
                item.LAP().GlobalMult *= 1.4f;

            if (item.type == ModContent.ItemType<BloodBoiler>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<HalleysInferno>())
                item.LAP().GlobalMult *= 1.5f;
            #endregion
            #region 法师
            if (item.type == ModContent.ItemType<ClamorNoctus>())
                item.LAP().GlobalMult *= 1.4f;

            if (item.type == ModContent.ItemType<DarkSpark>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<ShadowboltStaff>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<DeathhailStaff>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<VenusianTrident>())
                item.LAP().GlobalMult *= 1.6f;

            if (item.type == ModContent.ItemType<PhantasmalFuryOld>())
                SetCustomMult(item, 3f);
            #endregion
            #region 召唤
            if (item.type == ModContent.ItemType<Sirius>())
                item.LAP().GlobalMult *= 3.5f;

            if (item.type == ModContent.ItemType<SiriusLegacy>())
                item.LAP().GlobalMult *= 3.5f;
            #endregion
            #region 盗贼
            if (item.type == ModContent.ItemType<Valediction>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<NightsGaze>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<PhantasmalRuinold>())
                SetCustomMult(item, 1.2f);
            #endregion
        }
        #endregion
        #region 宇宙
        public static void CosmicBlance(Item item)
        {
            #region 战士

            if (item.type == ModContent.ItemType<EmpyreanKnives>())
                SetCustomMult(item, 5f);

            if (item.type == ModContent.ItemType<PrismaticBreaker>())
                SetCustomMult(item, 5.6f);

            if (item.type == ModContent.ItemType<Excelsus>())
                item.LAP().GlobalMult *= 1.5f;

            if (item.type == ModContent.ItemType<GalaxySmasher>())
                item.LAP().GlobalMult *= 1.5f;

            if (item.type == ModContent.ItemType<ScourgeoftheCosmos>())
                item.LAP().GlobalMult *= 1.5f;

            if (item.type == ModContent.ItemType<TheObliterator>())
                SetCustomMult(item, 2.5f);

            if (item.type == ModContent.ItemType<EssenceFlayer>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<Murasama>())
                SetCustomMult(item, 9.0013f);
            #endregion
            #region 射手
            if (item.type == ModContent.ItemType<Deathwind>())
                item.LAP().GlobalMult *= 1.4f;

            if (item.type == ModContent.ItemType<Alluvion>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<AntiMaterielRifle>())
                SetCustomMult(item, 5.2f);

            if (item.type == ModContent.ItemType<ThePack>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<Starmageddon>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<Starmada>())
                item.LAP().GlobalMult *= 1.3f;

            if (item.type == ModContent.ItemType<CleansingBlaze>())
                item.LAP().GlobalMult *= 1.3f;

            if (item.type == ModContent.ItemType<PulseRifle>())
                item.LAP().GlobalMult *= 1.3f;

            if (item.type == ModContent.ItemType<Karasawa>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<RubicoPrime>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<Ultima>())
                item.LAP().GlobalMult *= 1.4f;

            if (item.type == ModContent.ItemType<UniversalGenesis>())
                item.LAP().GlobalMult *= 1.4f;

            if (item.type == ModContent.ItemType<ACTMinigun>())
                SetCustomMult(item, 2.2f);
            #endregion
            #region 法师

            if (item.type == ModContent.ItemType<RecitationoftheBeast>())
                item.LAP().GlobalMult *= 1.4f;

            if (item.type == ModContent.ItemType<FaceMelter>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<IceBarrage>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<ACTAlphaRay>())
                item.LAP().GlobalMult *= 0.8f;

            #endregion
            #region 召唤

            if (item.type == ModContent.ItemType<SarosPossession>())
                item.LAP().GlobalMult *= 1.6f;

            if (item.type == ModContent.ItemType<CorvidHarbringerStaff>())
                item.LAP().GlobalMult *= 1.4f;

            if (item.type == ModContent.ItemType<CosmicViperEngine>())
                item.LAP().GlobalMult *= 1.5f;

            if (item.type == ModContent.ItemType<EndoHydraStaff>())
                item.LAP().GlobalMult *= 1.5f;
            #endregion
            #region 盗贼
            if (item.type == ModContent.ItemType<Penumbra>())
                item.LAP().GlobalMult *= 2f;

            if (item.type == ModContent.ItemType<EclipsesFall>())
                item.LAP().GlobalMult *= 1.5f;

            if (item.type == ModContent.ItemType<Hypothermia>())
                item.LAP().GlobalMult *= 1.5f;

            if (item.type == ModContent.ItemType<Eradicator>())
                item.LAP().GlobalMult *= 1.2f;

            if (item.type == ModContent.ItemType<PlasmaGrenade>())
                item.LAP().GlobalMult *= 1.8f;

            if (item.type == ModContent.ItemType<RogueTypeHammerGalaxySmasher>())
                SetCustomMult(item, 1.5f);
            #endregion
        }
        #endregion
        #region 金源
        public static void AuricBlance(Item item)
        {
            if (item.type == ModContent.ItemType<VoidVortex>())
                SetCustomMult(item, 8f);
            if (item.type == ModContent.ItemType<YharimsCrystal>())
                SetCustomMult(item, 9f);
            #region 战士
            if (item.type == ModContent.ItemType<ArkoftheCosmos>())
                SetCustomMult(item, 9f);

            if (item.type == ModContent.ItemType<ArkoftheCosmosold>())
                SetCustomMult(item, 3.57857142f);

            if (item.type == ModContent.ItemType<Ataraxia>())
            {
                item.LAP().GlobalMult *= 1.4f;
                item.useTurn = false;
            }

            if (item.type == ItemID.Zenith)
            {
                SetCustomMult(item, 12f);
            }

            if (item.type == ModContent.ItemType<TheOracle>())
                SetCustomMult(item, 7.5f);

            if (item.type == ModContent.ItemType<MurasamaNeweffect>())
                SetCustomMult(item, 9.995502248f);

            if (item.type == ModContent.ItemType<Murasamaold>())
                SetCustomMult(item, 9.995502248f);
            #endregion
            #region 射手
            if (item.type == ModContent.ItemType<TyrannysEndOld>())
                SetCustomMult(item, 3f);
            #endregion
            #region 法师
            if (item.type == ModContent.ItemType<VoidVortexLegacy>())
                SetCustomMult(item, 2.4f);
            if (item.type == ModContent.ItemType<HadopelagicEcho>())
                SetCustomMult(item, 2f);
            #endregion
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
                SetCustomMult(item, 1085);
            // 耀界之光
            if (item.type == ModContent.ItemType<VividClarityOld>())
                SetCustomMult(item, 1250);
            // 星流短剑
            if (item.type == ModContent.ItemType<ExoGladius>())
                SetCustomMult(item, 3600);
            // 星流之刃
            if (item.type == ModContent.ItemType<Exoblade>())
                SetCustomMult(item, 5175);
            // 链刃
            if (item.type == ModContent.ItemType<ExoFlail>())
                SetCustomMult(item, 6125);
            // 磁极异变
            if (item.type == ModContent.ItemType<MagnomalyCannon>())
                SetCustomMult(item, 2200);
            // 天堂之风
            if (item.type == ModContent.ItemType<HeavenlyGaleold>())
                SetCustomMult(item, 708);
            // 星火解离者
            if (item.type == ModContent.ItemType<Photovisceratorold>())
                SetCustomMult(item, 1300);
            // 星神之杀
            if (item.type == ModContent.ItemType<Celestusold>())
                SetCustomMult(item, 1054);
            // 超新星
            if (item.type == ModContent.ItemType<Supernovaold>())
                SetCustomMult(item, 5200);
            // 弧光
            if (item.type == ModContent.ItemType<ExoTheApostle>())
                SetCustomMult(item, 10000);
            // 归墟
            if (item.type == ModContent.ItemType<CosmicImmaterializerOld>())
                SetCustomMult(item, 2400);
            // 热寂
            if (item.type == ModContent.ItemType<CelestialObliterator>())
                SetCustomMult(item, 600);
            #endregion
            // 星火解离者
            if (item.type == ModContent.ItemType<Photoviscerator>())
                SetCustomMult(item, 2300);
            // 天堂之风
            if (item.type == ModContent.ItemType<HeavenlyGale>())
                SetCustomMult(item, 800);
            // 星神之杀
            if (item.type == ModContent.ItemType<Celestus>())
                SetCustomMult(item, 1222);
            // 超新星
            if (item.type == ModContent.ItemType<Supernova>())
                SetCustomMult(item, 22000);
            // 星流刀
            if (item.type == ModContent.ItemType<Exoblade>())
                SetCustomMult(item, 3000);
            // 旋涡
            if (item.type == ModContent.ItemType<SubsumingVortex>())
                SetCustomMult(item, 1165);
            // 耀界
            if (item.type == ModContent.ItemType<VividClarity>())
                SetCustomMult(item, 1450);
            // 归墟
            if (item.type == ModContent.ItemType<CosmicImmaterializer>())
                SetCustomMult(item, 1800);
        }
        #endregion
        #region 魔影
        public static void ShadowspecBlance(Item item)
        {
            if (item.type == ModContent.ItemType<Eternity>())
                SetCustomMult(item, 5000);
            if (item.type == ModContent.ItemType<Apotheosis>())
                SetCustomMult(item, 9999);
            //这玩意菜的有点逆天了
            if (item.type == ModContent.ItemType<ScarletDevil>())
                SetCustomMult(item, 45876);
            if (item.type == ModContent.ItemType<NanoblackReaper>())
                SetCustomMult(item, 2000);
            if (item.type == ModContent.ItemType<TriactisTruePaladinianMageHammerofMight>())
                SetCustomMult(item, 60000);
            if (item.type == ModContent.ItemType<Sylvestaff>())
                SetCustomMult(item, 1050);
            if (item.Same<TheDanceofLight>())
                SetCustomMult(item, 6000);

            //红日需要100的倍率
            if (item.Same<RedSun>())
                item.LAP().GlobalMult *= 10f;
            //光辉飞刀需要接近四万五的面板，比大锤子稍低
            //这个面板已经是灾厄基础近50倍了
            if (item.Same<IllustriousKnives>())
                SetCustomMult(item, 22000);
            //魔影悠悠球面板需要x10
            if (item.Same<Ozzathoth>())
                SetCustomMult(item, 900);
            //考虑到实战，无限大地起码需要额外10倍的面板+超高的scale
            if (item.Same<Earth>())
            {
                SetCustomMult(item, 17000);
                item.scale *= 2.5f;
            }
            //龙破斩谁爱玩玩去（
            //瘟疫需要起码30000的面板
            if (item.Same<Contagion>())
                SetCustomMult(item, 30000);
            //太虚炮起码需要额外7倍
            if (item.Same<Voidragon>())
                item.LAP().GlobalMult *= 7f;
            //斯万不需补强
            //大比目鱼需要额外补强一点
            if (item.Same<HalibutCannon>())
                SetCustomMult(item, 2500);
            #region 遗产魔影
            //这个需要两倍
            if (item.type == ModContent.ItemType<FabstaffOld>())
                SetCustomMult(item, 2890);
            if (item.type == ModContent.ItemType<SomaPrimeOld>())
                SetCustomMult(item, 2400);
            if (item.type == ModContent.ItemType<CrystylCrusherLegacy>())
                SetCustomMult(item, 2000);
            if (item.type == ModContent.ItemType<Animus>())
                SetCustomMult(item, 10000);
            if (item.type == ModContent.ItemType<AzathothLegacy>())
                SetCustomMult(item, 1000);
            //这个不需要补强了，回血效率问题
            if (item.type == ModContent.ItemType<RogueTypeKnivesShadowspec>())
                SetCustomMult(item, 2000);
            //很难想象这个东西居然是需要加强的
            //800 -> 1000
            if (item.Same<MeleeTypeNanoblackReaper>())
                SetCustomMult(item, 1000);
            //同样不需要补强，强度足够了，还有武器特性问题
            if (item.type == ModContent.ItemType<RogueTypeHammerTriactisTruePaladinianMageHammerofMight>())
                SetCustomMult(item, 5800);
            //为啥开数值膨胀之后面板比没开低了？
            //不对，这jb的是原版神吞书我草
            //无敌了
            //if (item.type == ModContent.ItemType<Apotheosis>())
            //圣神之象需要翻4倍
            if (item.type == ModContent.ItemType<ApotheosisLegacy>())
                SetCustomMult(item, 2480);
            if (item.type == ModContent.ItemType<SvantechnicalLegacy>())
                SetCustomMult(item, 720);
            if (item.type == ModContent.ItemType<TemporalUmbrellaOld>())
                SetCustomMult(item, 1000);
            //光之舞不需要
            if (item.type == ModContent.ItemType<DanceofLightLegacy>())
                SetCustomMult(item, 480);
            if (item.type == ModContent.ItemType<StepToolShadows>())
                SetCustomMult(item, 5141);
            if (item.Same<ShizukuSword>())
                SetCustomMult(item, 5000);

            #endregion

        }
        #endregion
        #region 弹药
        public static void AmmoChange(Item item)
        {
            if (item.type == ModContent.ItemType<ElysianArrow>())
                SetCustomMult(item, 20);
            if (item.type == ModContent.ItemType<BloodfireArrow>())
                SetCustomMult(item, 40);
            if (item.type == ModContent.ItemType<VanquisherArrow>())
                SetCustomMult(item, 33);
            if (item.type == ModContent.ItemType<HolyFireBullet>())
                SetCustomMult(item, 27);
            if (item.type == ModContent.ItemType<BloodfireBullet>())
                SetCustomMult(item, 40);
            if (item.type == ModContent.ItemType<GodSlayerSlug>())
                SetCustomMult(item, 42);

            if (item.type == ModContent.ItemType<HolyFireBulletOld>())
                SetCustomMult(item, 27);
            if (item.type == ModContent.ItemType<VanquisherArrowold>())
                SetCustomMult(item, 33);
            if (item.type == ModContent.ItemType<ElysianArrowOld>())
                SetCustomMult(item, 20);
        }
        #endregion
        #endregion
        #region 龙一龙二改变
        public static void YharonBlance(Item item)
        {
            if (item.type == ModContent.ItemType<DragonSword>())
                SetCustomMult(item, 1005);

            if (item.type == ModContent.ItemType<BurningSkyLegacy>())
                SetCustomMult(item, 652);

            if (item.type == ModContent.ItemType<DragonsBreathold>())
                SetCustomMult(item, 256);

            if (item.type == ModContent.ItemType<AncientDragonsBreath>())
                SetCustomMult(item, 500);

            if (item.type == ModContent.ItemType<DragonRage>())
                SetCustomMult(item, 2200);

            if (item.type == ModContent.ItemType<DragonStaff>())
                SetCustomMult(item, 200);

            if (item.type == ModContent.ItemType<PhoenixFlameBarrage>())
                SetCustomMult(item, 220);

            if (item.type == ModContent.ItemType<YharonSonStaff>())
                SetCustomMult(item, 320);
        }
        #endregion
    }
}
