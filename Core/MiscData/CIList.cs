using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.MiscData
{
    public class CIList : ModSystem
    {
        public static HashSet<int> BeeEnemy;
        public override void Load()
        {
            BeeEnemy = [NPCID.Bee, NPCID.BeeSmall, NPCID.Hornet, NPCID.HornetFatty, NPCID.HornetHoney, NPCID.HornetLeafy, NPCID.HornetSpikey, NPCID.HornetStingy, 
                NPCID.BigHornetStingy, NPCID.LittleHornetStingy, NPCID.BigHornetSpikey, NPCID.LittleHornetSpikey, NPCID.BigHornetLeafy, NPCID.LittleHornetLeafy, 
                NPCID.BigHornetHoney, NPCID.LittleHornetHoney, NPCID.BigHornetFatty, NPCID.LittleHornetFatty ];
        }
        public override void Unload()
        {
            BeeEnemy = null;
        }
    }
}
