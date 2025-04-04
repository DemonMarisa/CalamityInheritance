﻿using System.Collections.Generic;
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
        public static void LoadLists()
        {
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
                    ProjectileType<UltimaBowProjectile>(),
                    ProjectileType<UltimaSpark>(), // Because of potential dust lag.
                    ProjectileType<UltimaRay>(),
                    ProjectileType<ExoTornado>(),
                    ProjectileType<ExoSparkold>(),
                    ProjectileType<ExoGunBlastsplit>(),
                    ProjectileType<ExoGunBlast>(),
                    ProjectileType<ExoLightold>(),
                    ProjectileType<ExoSpearBack>(),
            };

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

            ProjNoCIdeadshotBrooch = new List<int>
            {
                ProjectileType<ExoFlareold>(),
                ProjectileType<ExoLightold>(),
                ProjectileType<RicoshotCoin>(),
                ProjectileType<ExoFlareClusterold>()
            };

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
        }

        public static void UnloadLists()
        {
            rangedProjectileExceptionList = null;
            AuricdebuffList = null;
            ProjNoCIdeadshotBrooch = null;
            beeEnemyList = null;
            beeProjectileList = null;
            exoDraedibsArsenalWeapon = null;
        }
    }
}
