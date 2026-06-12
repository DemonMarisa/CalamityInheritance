using CalamityInheritance.Utilities;
using CalamityMod.NPCs.AquaticScourge;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.Perforator;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

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
                pool.Remove(NPCType<LeviathanStart>());

            // 腐巢生成
            bool cIHiveSpawns = spawnInfo.Player.CIMod().cIdisableHiveCystSpawns;

            if (cIHiveSpawns)
                pool.Remove(NPCType<HiveTumor>());

            // 宿主生成
            bool cIPerfSpawns = spawnInfo.Player.CIMod().cIdisablePerfCystSpawns;

            if (cIPerfSpawns)
                pool.Remove(NPCType<PerforatorCyst>());

            // 海灾生成
            bool cIASSpawns = spawnInfo.Player.CIMod().cIdisableNaturalScourgeSpawns;

            if (cIASSpawns)
                pool.Remove(NPCType<AquaticScourgeHead>());
        }
    }
}
