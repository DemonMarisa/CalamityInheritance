using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityInheritance.Sounds.Custom.Shizuku
{
    public class ShizukuSounds : ModSystem
    {
        public string SoundPath => (GetType().Namespace + ".").Replace('.', '/');
        public static SoundStyle DaggerHit;
        public static SoundStyle DaggerToss;
        private SoundStyle StarToss1;
        private SoundStyle StarToss2;
        private SoundStyle StarToss3;
        private SoundStyle Return1;
        private SoundStyle Return2;
        private SoundStyle DaggerHitDirectly1;
        private SoundStyle DaggerHitDirectly2;
        private SoundStyle DaggerHitDirectly3;
        public static List<SoundStyle> StarToss = [];
        public static List<SoundStyle> Return = [];
        public static List<SoundStyle> DaggerHitDirectly= [];
        public override void Load()
        {
            DaggerHit = new(SoundPath + "DaggerHit");
            DaggerToss = new(SoundPath + "DaggerToss");
            StarToss1 = new(SoundPath + "StarToss1");
            StarToss2 = new(SoundPath + "StarToss2");
            StarToss3 = new(SoundPath + "StarToss3");
            Return1 = new(SoundPath + "Return1");
            Return2 = new(SoundPath + "Return2");
            DaggerHitDirectly1 = new(SoundPath + "DaggerHitDirectly1");
            DaggerHitDirectly2 = new(SoundPath + "DaggerHitDirectly2");
            DaggerHitDirectly3 = new(SoundPath + "DaggerHitDirectly3");
            StarToss =
            [
                StarToss1,
                StarToss2,
                StarToss3
            ];
            Return =
            [
                Return1,
                Return2
            ];
            DaggerHitDirectly =
            [
                DaggerHitDirectly1,
                DaggerHitDirectly2,
                DaggerHitDirectly3
            ];
        }
        public override void Unload()
        {
            StarToss = null;
            Return = null;
            DaggerHitDirectly = null;
        }
    }
}