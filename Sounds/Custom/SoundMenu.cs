using Terraria.Audio;

namespace CalamityInheritance.Sounds.Custom
{
    public partial class CISoundMenu
    {
        public static string SoundRoute => "CalamityInheritance/Sounds/Custom";
        public static readonly SoundStyle YharimsSelfRepair = new($"{SoundRoute}/XerocMadnessSoundActive");
        public static readonly SoundStyle YharimsThuner = new($"{SoundRoute}/thunder2");
        public static readonly SoundStyle StepBonk = new($"{SoundRoute}/bonk");
        public static readonly SoundStyle HammerSmashID1= new($"{SoundRoute}/Smash1");
        public static readonly SoundStyle HammerSmashID2= new($"{SoundRoute}/Smash2");
        public static readonly SoundStyle HammerReturnID1= new($"{SoundRoute}/Return1");
        public static readonly SoundStyle HammerReturnID2=new($"{SoundRoute}/Return2");
        public static readonly SoundStyle MagnomalyShootSound = new($"{SoundRoute}/MagnomalyCannon/MagnomalyShoot", 3) { Volume = 0.9f, PitchVariance = 0.2f };
        public static readonly SoundStyle MagnomalyHitsound = new($"{SoundRoute}/MagnomalyCannon/MagnomalyBoom") { Volume = 0.9f, PitchVariance = 0.3f };
        public static readonly SoundStyle OpalStriker = new("CalamityInheritance/Sounds/Item/OpalStrike");
        public static readonly SoundStyle FireworkLauncher = new($"{SoundRoute}/launch1");
    }
}