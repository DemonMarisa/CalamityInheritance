using System.Collections.Generic;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityMod.Buffs.Cooldowns;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Buffs;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Ranged;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalamityMod.NPCs.PlagueEnemies;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Weapons.Magic;
using CalamityInheritance.Content.Projectiles.CalProjChange;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.CeaselessVoid;
using CalamityMod.NPCs.Signus;
using CalamityMod.NPCs.StormWeaver;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.AcidRain;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.NPCs.ExoMechs;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.PrimordialWyrm;

namespace CalamityInheritance
{
    public class CalamityInheritanceLists
    {
        public static List<int> rangedProjectileExceptionList;
        public static List<int> AuricdebuffList;
        public static List<int> pierceResistExceptionList;
        public static List<int> ProjNoCIdeadshotBrooch;
        public static List<int> beeProjectileList;
        public static List<int> beeEnemyList;
        public static List<int> exoDraedibsArsenalWeapon;
        // 月后NPC打表
        public static List<int> PostMLBoss = new List<int>();
        public static List<int> PostProfanedBoss = new List<int>();
        public static List<int> PostPolterghastBoss = new List<int>();
        public static List<int> DOG = new List<int>();
        public static List<int> ExoMech = new List<int>();
        public static List<int> Scal = new List<int>();
        public static List<int> PrimordialWyrm = new List<int>();
        public static void LoadLists()
        {
            #region 用于元素箭袋分裂
            rangedProjectileExceptionList = new List<int>
            {
                    ProjectileID.IchorDart,
                    ProjectileID.RainbowBack,
                    ProjectileID.PhantasmArrow,
                    ProjectileType<StarfleetMK2Gun>(),
                    ProjectileType<DryadsTearSplit>(),
                    ProjectileType<NorfleetCannon>(),
                    ProjectileType<NorfleetComet>(),
                    ProjectileType<NorfleetExplosion>(),
                    ProjectileType<AetherBeam>(),
                    ProjectileType<MagnomalyBeam>(),
                    ProjectileType<MagnomalyAura>(),
                    ProjectileType<RainbowTrail>(),
                    ProjectileType<ExoLight>(),
                    ProjectileType<UltimaSpark>(), // Because of potential dust lag.
                    ProjectileType<UltimaRay>(),
                    ProjectileType<ExoTornado>(),
                    ProjectileType<ExoSparkold>(),
                    ProjectileType<ExoGunBlastsplit>(),
                    ProjectileType<ExoGunBlast>(),
                    ProjectileType<ExoLightold>(),
                    //这东西怎么能是>100像素的弓呢？
                    ProjectileType<HeavenlyGaleProj>(),
                    //因某人强烈要求，给极了炮的粉尘打了个不可分裂的表
                    ProjectileType<ProfanedNukeDust>(),
                    //小鸡大炮爆炸，太卡了
                    ProjectileType<ChickenExplosion>()
            };
            #endregion
            #region 用于金源的buff免疫
            AuricdebuffList = new List<int>()
            {
                BuffID.Poisoned,
                BuffID.Darkness,
                BuffID.Cursed,
                BuffID.OnFire,
                BuffID.Bleeding,
                BuffID.Confused,
                BuffID.Slow,
                BuffID.Weak,
                BuffID.Silenced,
                BuffID.BrokenArmor,
                BuffID.CursedInferno,
                BuffID.Frostburn,
                BuffID.Chilled,
                BuffID.Frozen,
                BuffID.Burning,
                BuffID.Suffocation,
                BuffID.Ichor,
                BuffID.Venom,
                BuffID.Blackout,
                BuffID.Electrified,
                BuffID.Rabies,
                BuffID.Webbed,
                BuffID.Stoned,
                BuffID.Dazed,
                BuffID.VortexDebuff,
                BuffID.WitheredArmor,
                BuffID.WitheredWeapon,
                BuffID.OgreSpit,
                BuffID.BetsysCurse,
                BuffType<SulphuricPoisoning>(),
                BuffType<Shadowflame>(),
                BuffType<BurningBlood>(),
                BuffType<BrainRot>(),
                BuffType<ElementalMix>(),
                BuffType<GlacialState>(),
                BuffType<GodSlayerInferno>(),
                BuffType<AstralInfectionDebuff>(),
                BuffType<HolyFlames>(),
                BuffType<Irradiated>(),
                BuffType<Plague>(),
                BuffType<CrushDepth>(),
                BuffType<RiptideDebuff>(),
                BuffType<MarkedforDeath>(),
                BuffType<AbsorberAffliction>(),
                BuffType<ArmorCrunch>(),
                BuffType<Crumbling>(),
                BuffType<Vaporfied>(),
                BuffType<Eutrophication>(),
                BuffType<Dragonfire>(),
                BuffType<Nightwither>(),
                BuffType<VulnerabilityHex>(),
                BuffType<FrozenLungs>(),
                BuffType<FishAlert>(),
                BuffType<PopoNoselessBuff>(),
                BuffType<SearingLava>(),
                BuffType<Withered>()
            };
            #endregion
            #region 用于不会受到神射手纹章影响的投射物
            ProjNoCIdeadshotBrooch = new List<int>
            {
                ProjectileType<ExoFlareold>(),
                ProjectileType<ExoLightold>(),
                ProjectileType<RicoshotCoin>(),
                ProjectileType<ExoFlareClusterold>()
            };
            #endregion
            #region 蜜蜂类型敌人
            beeEnemyList = new List<int>()
            {
                NPCID.GiantMossHornet,
                NPCID.BigMossHornet,
                NPCID.LittleMossHornet,
                NPCID.TinyMossHornet,
                NPCID.MossHornet,
                NPCID.VortexHornetQueen,
                NPCID.VortexHornet,
                NPCID.Bee,
                NPCID.BeeSmall,
                NPCID.QueenBee,
                NPCType<PlaguebringerGoliath>(),
                NPCType<PlaguebringerMiniboss>(),
                NPCType<PlagueChargerLarge>(),
                NPCType<PlagueCharger>()
            };
            beeProjectileList = new List<int>()
            {
                ProjectileID.Stinger,
                ProjectileID.HornetStinger,
                ProjectileType<PlagueStingerGoliath>(),
                ProjectileType<PlagueStingerGoliathV2>(),
                ProjectileType<PlagueExplosion>()
            };
            #endregion
            #region 嘉登武器
            exoDraedibsArsenalWeapon = new List<int>()
            {
                ItemType<Exoblade>(),
                ItemType<Celestus>(),
                ItemType<CosmicImmaterializer>(),
                ItemType<HeavenlyGale>(),
                ItemType<MagnomalyCannon>(),
                ItemType<Photoviscerator>(),
                ItemType<SubsumingVortex>(),
                ItemType<Supernova>(),
                ItemType<VividClarity>(),
            };
            #endregion
            #region 月后boss
            PostMLBoss = new List<int>()
            {
                NPCType<ProfanedGuardianCommander>(),
                NPCType<ProfanedGuardianDefender>(),
                NPCType<ProfanedGuardianHealer>(),
                NPCType<ProfanedRocks>(),
                NPCType<Bumblefuck>(),
                NPCType<Bumblefuck2>(),
            };
            #endregion
            #region 亵渎后包括亵渎
            PostProfanedBoss = new List<int>()
            {
                NPCType<Providence>(),
                NPCType<Signus>(),
                NPCType<CosmicLantern>(),
                NPCType<CosmicMine>(),
                NPCType<CeaselessVoid>(),
                NPCType<DarkEnergy>(),
                NPCType<StormWeaverHead>(),
                NPCType<StormWeaverBody>(),
                NPCType<StormWeaverTail>(),
                NPCType<Polterghast>(),
                NPCType<PolterPhantom>(),
                NPCType<PolterghastHook>(),
                NPCType<PhantomFuckYou>(),
            };
            #endregion
            #region 幽花后包括幽花
            PostPolterghastBoss = new List<int>()
            {
                NPCType<OldDuke>(),
                NPCType<SulphurousSharkron>(),
                NPCType<OldDukeToothBall>(),

                NPCType<BobbitWormHead>(),
                NPCType<ColossalSquid>(),
                NPCType<EidolonWyrmHead>(),
                NPCType<GulperEelHead>(),
                NPCType<ReaperShark>(),

                NPCType<GammaSlime>(),
                NPCType<Mauler>(),
                NPCType<NuclearTerror>(),
            };
            #endregion
            #region 神长
            DOG = new List<int>()
            {
                NPCType<DevourerofGodsHead>(),
                NPCType<DevourerofGodsBody>(),
                NPCType<DevourerofGodsTail>(),
                NPCType<CosmicGuardianHead>(),
                NPCType<CosmicGuardianBody>(),
                NPCType<CosmicGuardianTail>(),
            };
            #endregion
            #region 巨械
            ExoMech = new List<int>()
            {
                NPCType<Draedon>(),
                NPCType<AresBody>(),
                NPCType<AresGaussNuke>(),
                NPCType<AresLaserCannon>(),
                NPCType<AresPlasmaFlamethrower>(),
                NPCType<AresTeslaCannon>(),
                NPCType<Artemis>(),
                NPCType<Apollo>(),
                NPCType<ThanatosHead>(),
                NPCType<ThanatosBody1>(),
                NPCType<ThanatosBody2>(),
                NPCType<ThanatosTail>(),
            };
            #endregion
            #region 終灾
            Scal = new List<int>()
            {
                NPCType<SupremeCalamitas>(),
                NPCType<SupremeCataclysm>(),
                NPCType<SupremeCatastrophe>(),
                NPCType<SoulSeekerSupreme>(),
                NPCType<BrimstoneHeart>(),
                NPCType<SepulcherHead>(),
                NPCType<SepulcherBody>(),
                NPCType<SepulcherBodyEnergyBall>(),
                NPCType<SepulcherArm>(),
                NPCType<SepulcherTail>(),
            };
            #endregion
            #region 幻海妖龙
            PrimordialWyrm = new List<int>()
            {
                NPCType<PrimordialWyrmHead>(),
                NPCType<PrimordialWyrmBody>(),
                NPCType<PrimordialWyrmBodyAlt>(),
                NPCType<PrimordialWyrmTail>(),
            };
            #endregion
        }

        public static void UnloadLists()
        {
            rangedProjectileExceptionList = null;
            AuricdebuffList = null;
            ProjNoCIdeadshotBrooch = null;
            beeEnemyList = null;
            beeProjectileList = null;
            exoDraedibsArsenalWeapon = null;

            PostMLBoss = null;
            PostProfanedBoss = null;
            PostPolterghastBoss = null;
            DOG = null;
            ExoMech = null;
            Scal = null;
            PrimordialWyrm = null;
        }
    }
}
