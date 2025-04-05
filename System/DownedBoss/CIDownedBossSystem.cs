using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.System.DownedBoss
{
    // RE我草你妈，为什么不给EOW和BOC写单独的DownedBoss
    public class CIDownedBossSystem : ModSystem
    {
        public static bool _downedEOW = false;
        public static bool _downedBOC = false;
        public static bool _downedBloodMoon = false;
        public static bool _downedLegacySCal = false;

        public static bool DownedEOW
        {
            get => _downedEOW;
            set
            {
                if (!value)
                    _downedEOW = false;
                else
                    NPC.SetEventFlagCleared(ref _downedEOW, -1);
            }
        }
        public static bool DownedBOC
        {
            get => _downedBOC;
            set
            {
                if (!value)
                    _downedBOC = false;
                else
                    NPC.SetEventFlagCleared(ref _downedBOC, -1);
            }
        }
        public static bool DownedBloodMoon
        {
            get => _downedBloodMoon;
            set
            {
                if (!value)
                    _downedBloodMoon = false;
                else
                    NPC.SetEventFlagCleared(ref _downedBloodMoon, -1);
            }
        }
        public static bool DownedLegacyScal
        {
            get => _downedLegacySCal;
            set
            {
                if (!value)
                    _downedLegacySCal = false;
                else
                    NPC.SetEventFlagCleared(ref _downedLegacySCal, -1);
            }
        }
        public static void ResetAllFlags()
        {
            DownedEOW = false;
            DownedBOC = false;
            DownedBloodMoon = false;
            DownedLegacyScal = false;
        }
        public override void OnWorldLoad() => ResetAllFlags();

        public override void OnWorldUnload() => ResetAllFlags();

        public override void SaveWorldData(TagCompound tag)
        {
            List<string> downed = new List<string>();

            // 肉前击败的boss
            if (DownedEOW)
                downed.Add("CIEOW");
            if (DownedBOC)
                downed.Add("CIBOC");
            if (DownedBloodMoon)
                downed.Add("CIBloodMoon");
            if (DownedLegacyScal)
                downed.Add("CILegacyScal");

            tag["CIdownedFlags"] = downed;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            IList<string> downed = tag.GetList<string>("CIdownedFlags");

            DownedEOW = downed.Contains("CIEOW");
            DownedBOC = downed.Contains("CIBOC");
            DownedBloodMoon = downed.Contains("CIBloodMoon");
            DownedLegacyScal = downed.Contains("CILegacyScal");
        }
    }
}
