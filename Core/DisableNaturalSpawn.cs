using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.Leviathan;
using CalamityInheritance.Utilities;
using CalamityMod;
using Terraria.ID;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.AquaticScourge;

namespace CalamityInheritance.Core
{
    public class DisableNaturalSpawn : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            // Voodoo Demon changes (including partial Voodoo Demon Voodoo Doll implementation)
            bool cIvoodooDemonDollActive = spawnInfo.Player.CIMod().cIdisableVoodooSpawns;

            // If the doll is active, Voodoo Demons cannot spawn (via modded means).
            if (cIvoodooDemonDollActive)
                pool.Remove(NPCID.VoodooDemon);

            // 阿娜西塔生成
            bool cIAnahitaSpawns = spawnInfo.Player.CIMod().cIdisableAnahitaSpawns;

            if (cIAnahitaSpawns)
                pool.Remove(ModContent.NPCType<LeviathanStart>());

            // 腐巢生成
            bool cIHiveSpawns = spawnInfo.Player.CIMod().cIdisableHiveCystSpawns;

            if (cIHiveSpawns)
                pool.Remove(ModContent.NPCType<HiveTumor>());

            // 宿主生成
            bool cIPerfSpawns = spawnInfo.Player.CIMod().cIdisablePerfCystSpawns;

            if (cIPerfSpawns)
                pool.Remove(ModContent.NPCType<PerforatorCyst>());

            // 海灾生成
            bool cIASSpawns = spawnInfo.Player.CIMod().cIdisableNaturalScourgeSpawns;

            if (cIASSpawns)
                pool.Remove(ModContent.NPCType<AquaticScourgeHead>());
        }
    }
}
