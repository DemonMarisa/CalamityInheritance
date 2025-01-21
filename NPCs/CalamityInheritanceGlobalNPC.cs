using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.NPCs
{
    public partial class CalamityInheritanceGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public bool silvaStun = false;
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (silvaStun)
            {
                npc.velocity.Y = 0f;
                npc.velocity.X = 0f;
            }
        }
        #region Reset Effects
        public override void ResetEffects(NPC npc)
        {
            silvaStun = false;
        }
        #endregion

        #region Pre AI
        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.Bee || npc.type == NPCID.BeeSmall || npc.type == NPCID.Hornet || npc.type == NPCID.HornetFatty || npc.type == NPCID.HornetHoney ||
                npc.type == NPCID.HornetLeafy || npc.type == NPCID.HornetSpikey || npc.type == NPCID.HornetStingy || npc.type == NPCID.BigHornetStingy || npc.type == NPCID.LittleHornetStingy ||
                npc.type == NPCID.BigHornetSpikey || npc.type == NPCID.LittleHornetSpikey || npc.type == NPCID.BigHornetLeafy || npc.type == NPCID.LittleHornetLeafy ||
                npc.type == NPCID.BigHornetHoney || npc.type == NPCID.LittleHornetHoney || npc.type == NPCID.BigHornetFatty || npc.type == NPCID.LittleHornetFatty)
            {
                if (Main.player[npc.target].CalamityInheritance().queenBeeLore)
                {
                    CalamityInheritanceGlobalAI.QueenBeeLoreEffect(npc);
                    return false;
                }
            }
            return true;
        }
        #endregion
        #region Edit Spawn Rate
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.CalamityInheritance().hiveMindLore)
            {
                spawnRate = (int)(spawnRate * 1.3);
                maxSpawns = (int)(maxSpawns * 0.6f);
            }
            if (player.CalamityInheritance().perforatorLore)
            {
                spawnRate = (int)(spawnRate * 0.7);
                maxSpawns = (int)(maxSpawns * 1.8f);
            }
        }
        #endregion
    }
}
