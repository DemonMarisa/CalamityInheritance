using Terraria.Audio;

namespace CalamityInheritance.Sounds.Custom
{
    public partial class CISoundMenu
    {
        public static string ASSoundRoute => "CalamityInheritance/Sounds/AbilitySounds";
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
        public static readonly SoundStyle PlasmaBlast = new($"{ItemSoundRoute}/PlasmaBlast");

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
        public static readonly SoundStyle LaserRifleFire = new($"{SoundRoute}/{nameof(LaserRifleFire)}");
        #region ����
        public static readonly SoundStyle IronHeartDeath = new($"{IronHeartSound}/IronHeartDeath") { Volume = 0.6f, Pitch = 0.9f };
        public static readonly SoundStyle IronHeartBigHurt = new($"{IronHeartSound}/IronHeartBigHurt") { Volume = 0.9f, Pitch = 0.7f };
        public static readonly SoundStyle IronHeartHurt = new($"{IronHeartSound}/IronHeartHurt") { Volume = 0.9f, Pitch = 0.7f };
        #endregion
        #region ��ʴì
        public static readonly SoundStyle EclipseSpearAttackNor = new($"{ItemSoundRoute}/EclipseSpear/EclipseSpearAttackNor") { Volume = 0.9f, Pitch = 0.3f };
        public static readonly SoundStyle EclipseSpearAttackStealth = new($"{ItemSoundRoute}/EclipseSpear/EclipseSpearAttackStealth") { Volume = 0.9f, Pitch = 0.7f };
        public static readonly SoundStyle EclipseSpearBoom = new($"{ItemSoundRoute}/EclipseSpear/EclipseSpearBoom") { Volume = 0.9f, Pitch = 0.7f };
        public static readonly SoundStyle GaussRifleFired = new SoundStyle($"{ItemSoundRoute}/GaussRifleSound/MechGaussRifle");
        public static readonly SoundStyle HeavyGaussRifleFired = new SoundStyle($"{ItemSoundRoute}/GaussRifleSound/LargeMechGaussRifle");
        #endregion
        #region ����Ͷì
        public static readonly SoundStyle LumiShardHit = new($"{ItemSoundRoute}/LumiSpear/LumiShardHit") { Volume = 0.9f, Pitch = 0.3f };
        public static readonly SoundStyle LumiSpearAttackNor = new($"{ItemSoundRoute}/LumiSpear/LumiSpearAttackNor") { Volume = 0.6f, Pitch = 0.3f };
        #endregion
        #region �������������
        public static readonly SoundStyle WingManFire = new($"{ItemSoundRoute}/AlphaRay/WingManFire") { Volume = 0.9f, Pitch = 0.3f };
        public static readonly SoundStyle GenisisFire = new($"{ItemSoundRoute}/AlphaRay/GenisisFire") { Volume = 0.9f, Pitch = 0.3f };
        #endregion
        #region �����Ҳ�ǹ
        public static readonly SoundStyle MarniteBayonetShoot = new($"{ItemSoundRoute}/MarniteBayonet/MarniteBayonetShoot") { Volume = 0.9f, Pitch = 0.3f };
        #endregion
        #region Yamei��
        public static readonly SoundStyle YanmeiKnifeHit = new($"{ItemSoundRoute}/YanmeisKnifeSounds/YanmeiKnifeHit") { Volume = 0.9f, Pitch = 0.3f };
        public static readonly SoundStyle YanmeiKnifeExpire = new($"{ItemSoundRoute}/YanmeisKnifeSounds/YanmeiKnifeExpire") { Volume = 0.9f, Pitch = 0.3f };
        #endregion
        #region ��Ŀ����
        public static readonly SoundStyle HalibutCannonFire = new($"{ItemSoundRoute}/HalibutCannon/HalibutCannonFire") { Volume = 0.9f, Pitch = 0.3f, MaxInstances = 0 };
        public static readonly SoundStyle ShizukuSwordCharge = new($"{ItemSoundRoute}/WaterMirror");
        #endregion
        public static readonly SoundStyle AncientShivProjSpawn = new($"{ItemSoundRoute}/AncientShivSounds/AncientShivProjSpawn") { Volume = 0.9f, Pitch = 0.3f };
        #region  R99
        //这里用private是为了防止自动补全混乱。如果需要单独调用其中一种音效，直接调用数组其中一个元素就行了
        public static string R99Shield => "/" + "R99/R99Shield";
        private static readonly SoundStyle R99ShieldHit1 = new($"{SoundRoute}" + "/" + "R99/R99ShieldOnHit1");
        private static readonly SoundStyle R99ShieldHit2 = new($"{SoundRoute}" + "/" + "R99/R99ShieldOnHit2");
        private static readonly SoundStyle R99ShieldCracked1 = new($"{SoundRoute}" + "/" + "R99/R99ShieldCracked1");
        private static readonly SoundStyle R99ShieldCracked2 = new($"{SoundRoute}" + "/" + "R99/R99ShieldCracked2");
        private static readonly SoundStyle R99ShieldCracked3 = new($"{SoundRoute}" + "/" + "R99/R99ShieldCracked3");
        private static readonly SoundStyle R99ShieldCracked4 = new($"{SoundRoute}" + "/" + "R99/R99ShieldCracked4");
        private static readonly SoundStyle R99FleshHit1 = new($"{SoundRoute}" + "/" +"R99/R99FleshHit1");
        private static readonly SoundStyle R99FleshHit2 = new($"{SoundRoute}" + "/" +"R99/R99FleshHit2");
        private static readonly SoundStyle R99FleshHit3 = new($"{SoundRoute}" + "/" +"R99/R99FleshHit3");
        private static readonly SoundStyle R99FleshHit4 = new($"{SoundRoute}" + "/" +"R99/R99FleshHit4");
        private static readonly SoundStyle R99Fired1 = new($"{SoundRoute}/R99/R99Fired1");
        private static readonly SoundStyle R99Fired2 = new($"{SoundRoute}/R99/R99Fired2");
        private static readonly SoundStyle R99Fired3 = new($"{SoundRoute}/R99/R99Fired3");
        public static SoundStyle[] R99Fired =
            [
                R99Fired1,
                R99Fired2,
                R99Fired3
            ];
        public static SoundStyle[] R99FleshHit =
            [
                R99FleshHit1,
                R99FleshHit2,
                R99FleshHit3,
                R99FleshHit4
            ];
        public static SoundStyle[] R99ShieldCracked =
            [
                R99ShieldCracked1,
                R99ShieldCracked2,
                R99ShieldCracked3,
                R99ShieldCracked4
            ];
        public static SoundStyle[] R99ShieldHit =
            [
                R99ShieldHit1,
                R99ShieldHit2
            ];
        #endregion
        public static readonly SoundStyle Pipes = new($"{SoundRoute}/Pipes");
        public static readonly SoundStyle SilvaActivation = new ($"{ASSoundRoute}/SilvaActivation");
        public static readonly SoundStyle SilvaDispel = new($"{ASSoundRoute}/SilvaDispel");

    }
}