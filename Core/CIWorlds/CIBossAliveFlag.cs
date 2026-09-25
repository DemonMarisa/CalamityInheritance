using CalamityInheritance.Content.NPCs.Boss.CalamitasClone;
using CalamityInheritance.Content.NPCs.Boss.CalamitasClone.Sky;
using CalamityInheritance.Core.CIConfigs;
using CalamityInheritance.Music;
using LAP.Core.MusicEvent;
using LAP.Core.SystemsLoader;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.CIWorlds
{
    public class CIBossAliveFlag : ModSystem
    {
        public static int ActiveCalCloneIndex = -1;
        public static int ActiveCalCloneP1Flag = -1;
        public static int ActiveCalCloneFlag = -1;
        public static int LegacySCalLament;
        public override void PreUpdateNPCs()
        {
            Main.multiplayerNPCSmoothingRange = 0;
            NPC.ResetNetOffsets();
            CheckFlag(ref ActiveCalCloneIndex, NPCType<CalamitasCloneLegacy>());
            CheckFlag(ref ActiveCalCloneP1Flag, NPCType<CalamitasCloneLegacy>());
            CheckFlag(ref ActiveCalCloneFlag, NPCType<CalamitasCloneLegacy>());
            CheckFlag(ref LegacySCalLament, NPCType<CalamitasCloneLegacy>());
        }
        public static void CheckFlag(ref int index, int NPCType)
        {
            if (index != -1)
            {
                if (!Main.npc[index].active || Main.npc[index].type != NPCType)
                    index = -1;
            }
        }
    }
}
