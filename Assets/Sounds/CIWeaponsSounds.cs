using Terraria.Audio;

namespace CalamityInheritance.Assets.Sounds
{
    public partial class CISounds
    {
        public static readonly SoundStyle MurasamaOrganicHit = new("CalamityInheritance/Assets/Sounds/Weapons/Murasamas/MurasamaHitOrganic") { Volume = 0.45f };
        public static readonly SoundStyle MurasamaInorganicHit = new("CalamityInheritance/Assets/Sounds/Weapons/Murasamas/MurasamaHitInorganic") { Volume = 0.55f };
        public static readonly SoundStyle MurasamaSwing = new("CalamityInheritance/Assets/Sounds/Weapons/Murasamas/MurasamaSwing") { Volume = 0.2f };
        public static readonly SoundStyle MurasamaBigSwing = new("CalamityInheritance/Assets/Sounds/Weapons/Murasamas/MurasamaBigSwing") { Volume = 0.25f };

        public static readonly SoundStyle EclipseSpearAttackNor = new("CalamityInheritance/Assets/Sounds/Weapons/EclipseSpear/EclipseSpearAttackNor") { Volume = 0.9f, Pitch = 0.3f };
        public static readonly SoundStyle EclipseSpearAttackStealth = new("CalamityInheritance/Assets/Sounds/Weapons/EclipseSpear/EclipseSpearAttackStealth") { Volume = 0.9f, Pitch = 0.7f };
        public static readonly SoundStyle EclipseSpearBoom = new("CalamityInheritance/Assets/Sounds/Weapons/EclipseSpear/EclipseSpearBoom") { Volume = 0.9f, Pitch = 0.7f };

        public static readonly SoundStyle AncientShivSounds = new("CalamityInheritance/Assets/Sounds/Weapons/AncientShivSounds/AncientShivProjSpawn") { Volume = 1f };

        public static readonly SoundStyle LumiSpearAttackNor = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/LumiSpear/LumiSpearAttackNor") { Volume = 0.6f, Pitch = 0.3f };
        public static readonly SoundStyle LumiShardHit = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/LumiSpear/LumiShardHit") { Volume = 0.9f, Pitch = 0.3f };

        public static readonly SoundStyle SearedPanSmash = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/SearedPan/SearedPanSmash");

        public static readonly SoundStyle YanmeiKnifeHit = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/YanmeiKnife/YanmeiKnifeHit");
        public static readonly SoundStyle YanmeiKnifeExpire = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/YanmeiKnife/YanmeiKnifeExpire");

        public static readonly SoundStyle GatlingLaserFireEnd = new("CalamityInheritance/Assets/Sounds/Weapons/GatlingLaser/GatlingLaserFireEnd") { Volume = 0.9f, Pitch = 0.3f };
        public static readonly SoundStyle GatlingLaserFireLoop = new("CalamityInheritance/Assets/Sounds/Weapons/GatlingLaser/GatlingLaserFireLoop") { Volume = 0.9f, Pitch = 0.7f };
        public static readonly SoundStyle GatlingLaserFireStart = new("CalamityInheritance/Assets/Sounds/Weapons/GatlingLaser/GatlingLaserFireStart") { Volume = 0.9f, Pitch = 0.7f };

        public static readonly SoundStyle VividClarityBeamAppear = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/VividClarity/VividClarityBeamAppear");
        public static readonly SoundStyle VividClarityShoot = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/VividClarity/VividClarityShoot");

        public static readonly SoundStyle VortexBoom = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/SubsumingVortex/VortexBoom");
        public static readonly SoundStyle VortexDone = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/SubsumingVortex/VortexDone");
        public static readonly SoundStyle VortexStart = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/SubsumingVortex/VortexStart");
        public static readonly SoundStyle VortexToss1 = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/SubsumingVortex/VortexToss1");
        public static readonly SoundStyle VortexToss2 = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/SubsumingVortex/VortexToss2");
        public static readonly SoundStyle VortexToss3 = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/SubsumingVortex/VortexToss3");

        public static readonly SoundStyle CrystylCharge = new("CalamityInheritance/Assets/Sounds/Weapons/Misc/CrystylCharge") { Volume = 1f };
        public static readonly SoundStyle SwiftSlice = new("CalamityInheritance/Assets/Sounds/Weapons/Misc/SwiftSlice") { Volume = 1f };
        public static readonly SoundStyle LouderPhantomPhoenix = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Misc/LouderPhantomPhoenix", 3);
        public static readonly SoundStyle ScissorGuillotineSnapSound = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Misc/ScissorGuillotineSnap");
        public static readonly SoundStyle LaserCannon = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Misc/LaserCannon");
        public static readonly SoundStyle WyrmScream = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Misc/WyrmScream");
        public static readonly SoundStyle YharonInfernado = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Misc/YharonInfernado");

        public static readonly SoundStyle MagnomalyBoom = new("CalamityInheritance/Assets/Sounds/Weapons/MagnomalyCannon/MagnomalyBoom") { Volume = 0.7f, PitchVariance = 0.3f };
        public static SoundStyle MagnomalyShoot => new($"CalamityInheritance/Assets/Sounds/Weapons/MagnomalyCannon/MagnomalyShoot", 3) { Volume = 0.7f, PitchVariance = 0.2f };
        // =====
        public static readonly SoundStyle LargeWeaponFire = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/LargeWeaponFire");
        public static readonly SoundStyle PlasmaBolt = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/PlasmaBolt");
        public static readonly SoundStyle OpalStriker = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/OpalStrike");
        public static readonly SoundStyle FrigidflashUse = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/FrigidflashUse");
        public static readonly SoundStyle FrigidflashDeath = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/FrigidflashDeath");
        public static readonly SoundStyle FlareSound = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/FlareSound");
        public static readonly SoundStyle HellblastFire = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/HellblastFire");
        public static readonly SoundStyle PlasmaBlast = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/PlasmaBlast");
        public static readonly SoundStyle LanceofDestinyStrong = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/LanceofDestinyStrong") { Volume = 0.4f, PitchVariance = 0.3f };
        public static readonly SoundStyle Magnum = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/Magnum");
        public static readonly SoundStyle BazookaRocket = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/BazookaRocket");
        public static readonly SoundStyle BazookaFull = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/BazookaFull");
        public static readonly SoundStyle Hydra = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/Hydra");
        public static readonly SoundStyle GaussWeaponFire = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/GaussWeaponFire");
        public static readonly SoundStyle MechGaussRifle = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/MechGaussRifle");
        public static readonly SoundStyle PulseRifleFire = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/PulseRifleFire");
        public static readonly SoundStyle LargeMechGaussRifle = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/LargeMechGaussRifle");
        public static readonly SoundStyle LaserRifleFire = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/Common/LaserRifleFire");

        // =====
        public static readonly SoundStyle KarasawaCharge = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/ACTKarasawa/KarasawaCharge");
        public static readonly SoundStyle KarasawaChargeFailed = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/ACTKarasawa/KarasawaChargeFailed");
        public static readonly SoundStyle KarasawaEnergyPulse = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/ACTKarasawa/KarasawaEnergyPulse");
        public static readonly SoundStyle KarasawaLaunch1 = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/ACTKarasawa/KarasawaLaunch1");
        public static readonly SoundStyle KarasawaLaunch2 = new SoundStyle("CalamityInheritance/Assets/Sounds/Weapons/ACTKarasawa/KarasawaLaunch2");
    }
}
