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
        internal static bool _downedEOW = false;
        internal static bool _downedBOC = false;
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
        internal static void ResetAllFlags()
        {
            DownedEOW = false;
            DownedBOC = false;
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

            tag["CIdownedFlags"] = downed;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            IList<string> downed = tag.GetList<string>("CIdownedFlags");

            DownedEOW = downed.Contains("CIEOW");
            DownedBOC = downed.Contains("CIBOC");
        }
    }
}
