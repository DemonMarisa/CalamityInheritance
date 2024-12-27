using System.Collections.Generic;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Ranged;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityInheritance
{
    public class CalamityInheritanceLists
    {
        public static List<int> rangedProjectileExceptionList;
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
        }

        // 卸载列表
        public static void UnloadLists()
        {
            rangedProjectileExceptionList = null;
        }
    }
}
