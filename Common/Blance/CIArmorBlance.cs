using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CalamityInheritance.Common.Blance
{
    public class CIArmorBlance
    {
        public static int TarragonMagicLeafBaseD(Player player) => player.CI().AuricSet ? 600 : 200;
        public static int BloodflareMagicGhostlyBolt(Player player) => player.CI().AuricSet ? 3000 : 1500;
        public static int BloodflareMagicBlast(Player player) => player.CI().AuricSet ? 2400 : 1200;
        public static int GodSlayerMagicFire(Player player) => player.CI().AuricSet ? 600 : 400;
        public static int GodSlayerSummonFire(Player player) => player.CI().AuricSet ? 800 : 600;
        public static int GodSlayerMagicHurtBlast(Player player) => player.CI().AuricSet ? 1800 : 1200;
        public static int SilvaMagicHit(Player player) => player.CI().AuricSet ? 8000 : 4000;
        public static int GodSlayerMeleeDart(Player player) => player.CI().AuricSet ? 4000 : 2000;
        public static int TarragonRangedHit(Player player) => player.CI().AuricSet ? 800 : 120;
        public static int BloodFlareRangedSetBonus(Player player) => player.CI().AuricSet ? 3200 : 1200;
        public static int BloodFlareRangedProj(Player player) => player.CI().AuricSet ? 3000 : 1500;
        public static int GodSlayerRangedProj(Player player) => player.CI().AuricSet ? 4000 : 3000;
        public static float SilvaRangedAttackSpeed(Player player) => player.CI().AuricSet ? 0.2f : 0.1f;
        public static float SilvaRogueAttackSpeed(Player player) => player.CI().AuricSet ? 0.2f : 0.1f;
    }
}
