using Terraria.Audio;

namespace CalamityInheritance.Sounds.Custom
{
    public partial class CISoundMenu
    {
        public static string SoundRoute => "CalamityInheritance/Sounds/Custom";
        public static string ItemSoundRoute => "CalamityInheritance/Sounds/Item";
        public static string CelestusSound => $"{SoundRoute}/Celestus";
        public static string ExoFlameSound => $"{SoundRoute}/ExoFlame";
        public static string MagnomalySound => $"{SoundRoute}/MagnomalyCannon";
        public static string VortexSound => $"{SoundRoute}/SubsumingVortex";
        public static string KarasawaSound => $"{SoundRoute}/ACTKarasawa";
        public static string VividSound => $"{SoundRoute}/Vivid";
        public static string CosmicImSound => $"{SoundRoute}/CosmicIm";
        public static string AtomSound => $"{SoundRoute}/Atom";
        public static string IronHeartSound => $"{SoundRoute}/IronHeart";

        public static readonly SoundStyle OpalStriker = new("CalamityInheritance/Sounds/Item/OpalStrike");
        public static readonly SoundStyle YharimsSelfRepair = new($"{SoundRoute}/XerocMadnessSoundActive");
        public static readonly SoundStyle YharimsThuner = new($"{SoundRoute}/thunder2");
        public static readonly SoundStyle StepBonk = new($"{SoundRoute}/bonk");
        public static readonly SoundStyle HammerSmashID1 = new($"{SoundRoute}/Smash1");
        public static readonly SoundStyle HammerSmashID2 = new($"{SoundRoute}/Smash2");
        public static readonly SoundStyle HammerReturnID1 = new($"{SoundRoute}/Return1");
        public static readonly SoundStyle HammerReturnID2 = new($"{SoundRoute}/Return2");
        public static readonly SoundStyle FireworkLauncher = new($"{SoundRoute}/launch1");
        public static readonly SoundStyle Slasher = new($"{SoundRoute}/Slasher") { Volume = 0.7f, Pitch = 0.5f };
        public static readonly SoundStyle MagnomalyShootSound = new($"{MagnomalySound}/MagnomalyShoot", 3) { Volume = 0.9f, PitchVariance = 0.2f };
        public static readonly SoundStyle MagnomalyHitsound = new($"{MagnomalySound}/MagnomalyBoom") { Volume = 0.9f, PitchVariance = 0.3f };
        public static readonly SoundStyle CelestusOnHit1 = new($"{CelestusSound}/CelestusHit1") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle CelestusOnHit2 = new($"{CelestusSound}/CelestusHit2") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle CelestusOnHit3 = new($"{CelestusSound}/CelestusHit3") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle CelestusToss1 = new($"{CelestusSound}/CelestusShoot1") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle CelestusToss2 = new($"{CelestusSound}/CelestusShoot2") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle CelestusToss3 = new($"{CelestusSound}/CelestusShoot3") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle VortexDone = new($"{VortexSound}/VortexDone") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle VortexStart = new($"{VortexSound}/VortexStart") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle VortexBoom = new($"{VortexSound}/VortexBoom") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle VortexToss1 = new($"{VortexSound}/VortexToss1") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle VortexToss2 = new($"{VortexSound}/VortexToss2") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle VortexToss3 = new($"{VortexSound}/VortexToss3") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle ExoFlameLeft = new($"{ExoFlameSound}/ExoFlameLeft") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle ExoFlameRight = new($"{ExoFlameSound}/ExoFlameRight") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle ExoFlameHard = new($"{ExoFlameSound}/ExoFlameHard") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle SupernovaRightClick = new($"{SoundRoute}/SupernovaRightClick") { Volume = 0.8f, PitchVariance = 0.3f };
        public static readonly SoundStyle VividToss1 = new($"{VividSound}/VividClarity1") { Volume = 0.7f, Pitch = 0.6f };
        public static readonly SoundStyle VividToss2 = new($"{VividSound}/VividClarity2") { Volume = 0.7f, Pitch = 0.6f };
        public static readonly SoundStyle VividToss3 = new($"{VividSound}/VividClarity3") { Volume = 0.7f, Pitch = 0.6f };
        public static readonly SoundStyle AtomToss1 = new($"{AtomSound}/AtomToss1") { Volume = 0.6f, Pitch = 0.9f };
        public static readonly SoundStyle AtomToss2 = new($"{AtomSound}/AtomToss2") { Volume = 0.6f, Pitch = 0.9f };
        public static readonly SoundStyle AtomToss3 = new($"{AtomSound}/AtomToss3") { Volume = 0.6f, Pitch = 0.9f };
        public static readonly SoundStyle AtomHit1 = new($"{AtomSound}/AtomHit1") { Volume = 0.6f, Pitch = 0.9f };
        public static readonly SoundStyle AtomHit2 = new($"{AtomSound}/AtomHit2") { Volume = 0.6f, Pitch = 0.9f };
        public static readonly SoundStyle AtomHit3 = new($"{AtomSound}/AtomHit3") { Volume = 0.6f, Pitch = 0.9f };
        public static readonly SoundStyle CosmicImToss1 = new($"{CosmicImSound}/CosmicIm1") { Volume = 0.9f, Pitch = 0.7f };
        public static readonly SoundStyle CosmicImToss2 = new($"{CosmicImSound}/CosmicIm2") { Volume = 0.9f, Pitch = 0.7f };
        public static readonly SoundStyle KarasawaCharge = new($"{KarasawaSound}/KarasawaCharge") { MaxInstances = 0 };
        public static readonly SoundStyle KarasawaEnergyPulse = new($"{KarasawaSound}/KarasawaEnergyPulse") { MaxInstances = 0, IsLooped = false };
        public static readonly SoundStyle KarasawaLaunch = new($"{KarasawaSound}/KarasawaLaunch", 2) { MaxInstances = 0, PitchVariance = 0.1f };
        public static readonly SoundStyle KarasawaChargeFailed = new($"{KarasawaSound}/KarasawaChargeFailed") { MaxInstances = 2 };
        #region 铁心
        public static readonly SoundStyle IronHeartDeath = new($"{IronHeartSound}/IronHeartDeath") { Volume = 0.6f, Pitch = 0.9f };
        public static readonly SoundStyle IronHeartBigHurt = new($"{IronHeartSound}/IronHeartBigHurt") { Volume = 0.9f, Pitch = 0.7f };
        public static readonly SoundStyle IronHeartHurt = new($"{IronHeartSound}/IronHeartHurt") { Volume = 0.9f, Pitch = 0.7f };
        #endregion
        #region 日蚀矛
        public static readonly SoundStyle EclipseSpearAttackNor = new($"{ItemSoundRoute}/EclipseSpear/EclipseSpearAttackNor") { Volume = 0.9f, Pitch = 0.3f };
        public static readonly SoundStyle EclipseSpearAttackStealth = new($"{ItemSoundRoute}/EclipseSpear/EclipseSpearAttackStealth") { Volume = 0.9f, Pitch = 0.7f };
        public static readonly SoundStyle EclipseSpearBoom = new($"{ItemSoundRoute}/EclipseSpear/EclipseSpearBoom") { Volume = 0.9f, Pitch = 0.7f };
        #endregion
        #region 月明投矛
        public static readonly SoundStyle LumiShardHit = new($"{ItemSoundRoute}/LumiSpear/LumiShardHit") { Volume = 0.9f, Pitch = 0.3f };
        public static readonly SoundStyle LumiSpearAttackNor = new($"{ItemSoundRoute}/LumiSpear/LumiSpearAttackNor") { Volume = 0.6f, Pitch = 0.3f };
        #endregion
        #region 阿尔法射线相关
        public static readonly SoundStyle WingManFire = new($"{ItemSoundRoute}/AlphaRay/WingManFire") { Volume = 0.9f, Pitch = 0.3f };
        public static readonly SoundStyle GenisisFire = new($"{ItemSoundRoute}/AlphaRay/GenisisFire") { Volume = 0.9f, Pitch = 0.3f };
        #endregion
    }
}