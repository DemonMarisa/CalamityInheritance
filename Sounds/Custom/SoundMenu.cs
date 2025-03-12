using Terraria.Audio;

namespace CalamityInheritance.Sounds.Custom
{
    public partial class CISoundMenu
    {
        public static readonly  SoundStyle YharimsSelfRepair = new("CalamityInheritance/Sounds/Custom/XerocMadnessSoundActive");
        public static readonly  SoundStyle YharimsThuner = new("CalamityInheritance/Sounds/Custom/thunder2");
        public static readonly  SoundStyle StepBonk = new("CalamityInheritance/Sounds/Custom/bonk");
        public static readonly  SoundStyle HammerSmashID1= new("CalamityInheritance/Sounds/Custom/Smash1");
        public static readonly  SoundStyle HammerSmashID2= new("CalamityInheritance/Sounds/Custom/Smash2");
        public static readonly  SoundStyle HammerReturnID1= new("CalamityInheritance/Sounds/Custom/Return1");
        public static readonly  SoundStyle HammerReturnID2=new("CalamityInheritance/Sounds/Custom/Return2");
        //act�Ĵż���Ч
        public static readonly  SoundStyle MagnomalyShootSound = new SoundStyle("CalamityInheritance/Sounds/Custom/MagnomalyCannon/MagnomalyShoot", 3) { Volume = 0.9f, PitchVariance = 0.2f };
        public static readonly  SoundStyle MagnomalyHitsound = new("CalamityInheritance/Sounds/Custom/MagnomalyCannon/MagnomalyBoom") { Volume = 0.9f, PitchVariance = 0.3f };
        public static readonly SoundStyle OpalStriker = new("CalamityInheritance/Sounds/Item/OpalStrike");
    }
}