using Terraria.Audio;

namespace CalamityInheritance.Assets.Sounds
{
    public partial class CISounds
    {
        public static readonly SoundStyle AuricQuantumCoolingCellInstallNew = new("CalamityInheritance/Assets/Sounds/Items/AuricQuantumCoolingCellInstallNew") { Volume = 0.2f };

        public static readonly SoundStyle DevourerDeath = new("CalamityInheritance/Assets/Sounds/Customs/DevourerDeath");
        public static readonly SoundStyle DevourerDeathImpact = new("CalamityInheritance/Assets/Sounds/Customs/DevourerDeathImpact");

        public static readonly SoundStyle SilvaActivation = new("CalamityInheritance/Assets/Sounds/Customs/SilvaActivation");
        public static readonly SoundStyle SilvaDispel = new("CalamityInheritance/Assets/Sounds/Customs/SilvaDispel");

        public static readonly SoundStyle RoverDriveActivate = new("CalamityInheritance/Assets/Sounds/Customs/RoverDriveActivate") { Volume = 0.85f };
        public static readonly SoundStyle RoverDriveBreak = new("CalamityInheritance/Assets/Sounds/Customs/RoverDriveBreak") { Volume = 0.75f };
        public static readonly SoundStyle RoverDriveHit = new("CalamityInheritance/Assets/Sounds/Customs/RoverDriveHit") { PitchVariance = 0.6f, Volume = 0.6f, MaxInstances = 0 };
    }
}
