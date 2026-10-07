using Terraria;
using Terraria.Audio;

namespace CalamityInheritance.Assets.Sounds
{
    public partial class CISounds
    {
        public static readonly SoundStyle AuricQuantumCoolingCellInstallNew = new("CalamityInheritance/Assets/Sounds/Items/AuricQuantumCoolingCellInstallNew") { Volume = 0.2f };
        public static readonly SoundStyle WingManFire = new SoundStyle("CalamityInheritance/Assets/Sounds/Items/WingManFire");
        public static readonly SoundStyle GenisisFire = new SoundStyle("CalamityInheritance/Assets/Sounds/Items/GenisisFire");
        public static readonly SoundStyle PwnagehammerSound = new SoundStyle("CalamityInheritance/Assets/Sounds/Items/PwnagehammerSound");

        public static readonly SoundStyle Smash1 = new SoundStyle("CalamityInheritance/Assets/Sounds/Items/Smash1");
        public static readonly SoundStyle Smash2 = new SoundStyle("CalamityInheritance/Assets/Sounds/Items/Smash2");

        public static readonly SoundStyle DevourerDeath = new("CalamityInheritance/Assets/Sounds/Customs/DevourerDeath");
        public static readonly SoundStyle DevourerDeathImpact = new("CalamityInheritance/Assets/Sounds/Customs/DevourerDeathImpact");
        public static readonly SoundStyle DevourerSegmentBreak = new("CalamityInheritance/Assets/Sounds/Customs/DevourerSegmentBreak" + Main.rand.Next(1, 5));

        public static readonly SoundStyle SilvaActivation = new("CalamityInheritance/Assets/Sounds/Customs/SilvaActivation");
        public static readonly SoundStyle SilvaDispel = new("CalamityInheritance/Assets/Sounds/Customs/SilvaDispel");

        public static readonly SoundStyle RoverDriveActivate = new("CalamityInheritance/Assets/Sounds/Customs/RoverDriveActivate") { Volume = 0.85f };
        public static readonly SoundStyle RoverDriveBreak = new("CalamityInheritance/Assets/Sounds/Customs/RoverDriveBreak") { Volume = 0.75f };
        public static readonly SoundStyle RoverDriveHit = new("CalamityInheritance/Assets/Sounds/Customs/RoverDriveHit") { PitchVariance = 0.6f, Volume = 0.6f, MaxInstances = 0 };

        public static readonly SoundStyle RaidersTalismanStealthHit = new("CalamityInheritance/Assets/Sounds/Customs/RaidersTalismanStealthHit");

        public static readonly SoundStyle IronHeartBigHurt = new("CalamityInheritance/Assets/Sounds/Customs/IronHeartBigHurt") { Volume = 0.85f };
        public static readonly SoundStyle IronHeartDeath = new("CalamityInheritance/Assets/Sounds/Customs/IronHeartDeath") { Volume = 0.75f };
        public static readonly SoundStyle IronHeartHurt = new("CalamityInheritance/Assets/Sounds/Customs/IronHeartHurt") { PitchVariance = 0.6f, Volume = 0.6f, MaxInstances = 0 };

        public static readonly SoundStyle AuricBulletHit = new SoundStyle("CalamityInheritance/Assets/Sounds/Customs/AuricBulletHit");
    }
}
