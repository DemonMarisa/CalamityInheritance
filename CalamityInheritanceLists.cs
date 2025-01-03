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
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using CalamityMod.Projectiles.Typeless;

namespace CalamityInheritance
{
    public class CalamityInheritanceLists
    {
        public static List<int> rangedProjectileExceptionList;
        public static List<int> AuricdebuffList;
        public static List<int> AMRextraProjList;

        public static void LoadLists()
        {
            rangedProjectileExceptionList = new List<int>
            {
                    ProjectileID.Phantasm,
                    ProjectileID.VortexBeater,
                    ProjectileID.DD2PhoenixBow,
                    ProjectileID.IchorDart,
                    ProjectileID.PhantasmArrow,
                    ProjectileID.RainbowBack,
                    ProjectileType<PhangasmBow>(),
                    ProjectileType<ContagionBow>(),
                    ProjectileType<DaemonsFlameBow>(),
                    ProjectileType<DrataliornusBow>(),
                    ProjectileType<FlakKrakenProjectile>(),
                    ProjectileType<ButcherGun>(),
                    ProjectileType<StarfleetMK2Gun>(),
                    ProjectileType<DryadsTearSplit>(),
                    ProjectileType<SproutingArrowSplit>(),
                    ProjectileType<HyperiusSplit>(),
                    ProjectileType<NorfleetCannon>(),
                    ProjectileType<NorfleetComet>(),
                    ProjectileType<NorfleetExplosion>(),
                    ProjectileType<AetherBeam>(),
                    ProjectileType<FlurrystormCannonShooting>(),
                    ProjectileType<MagnomalyBeam>(),
                    ProjectileType<MagnomalyAura>(),
                    ProjectileType<RainbowTrail>(),
                    ProjectileType<PrismaticBeam>(),
                    ProjectileType<ExoLight>(),
                    ProjectileType<UltimaBowProjectile>(),
                    ProjectileType<UltimaSpark>(), // Because of potential dust lag.
                    ProjectileType<ChickenCannonHeld>(),
                    ProjectileType<ClockworkBowHoldout>(),
                    ProjectileType<ExoCrystalArrow>(),
                    ProjectileType<HeavenlyGaleProj>(),
                    ProjectileType<UltimaSpark>(),
                    ProjectileType<UltimaRay>(),
                    ProjectileType<ChickenExplosion>(),
                    ProjectileType<CondemnationHoldout>(),
                    ProjectileType<CondemnationArrow>(),
                    ProjectileType<CondemnationArrowHoming>(),
                    ProjectileType<ExoTornado>(),
                    ProjectileType<ExoSparkold>(),
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

            AMRextraProjList = new List<int>
            {
                ProjectileType<GodSlayerShrapnelRound>(),
                ProjectileType<BloodflareSoulold>(),
                ProjectileType<BloodflareSoul>(),
                ProjectileType<BloodBomb>(),
                ProjectileType<BloodBombExplosion>(),
            };
        }

        public static void UnloadLists()
        {
            rangedProjectileExceptionList = null;
            AuricdebuffList = null;
            AMRextraProjList = null;
        }
    }
}
