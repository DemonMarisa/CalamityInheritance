using Terraria;
using Terraria.Audio;

namespace CalamityInheritance.Sounds.Cals
{
    public class CICalSounds
    {
        public static readonly SoundStyle GaussWeaponFire = new("CalamityInheritance/Sounds/Cals/CalAllSounds/GaussWeaponFire");
        public static readonly SoundStyle GatlingLaserFireEnd = new("CalamityInheritance/Sounds/Cals/CalAllSounds/GatlingLaserFireEnd");
        public static readonly SoundStyle GatlingLaserFireLoop = new("CalamityInheritance/Sounds/Cals/CalAllSounds/GatlingLaserFireLoop");
        public static readonly SoundStyle GatlingLaserFireStart = new("CalamityInheritance/Sounds/Cals/CalAllSounds/GatlingLaserFireStart");
        public static readonly SoundStyle DevourerSegmentBreak = new("CalamityInheritance/Sounds/Cals/CalAllSounds/NPCKilled/DevourerSegmentBreak" + Main.rand.Next(1, 5));
    }
}
