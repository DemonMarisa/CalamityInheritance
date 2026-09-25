using CalamityInheritance.Content.Buff.Buffs;
using CalamityInheritance.Core.GlobalInstance.NPCs.GlobalAI;
using CalamityInheritance.Core.MiscData;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.NPCs
{
    public partial class CIGNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public float[] BossNewAI = new float[10];
        public override bool PreAI(NPC npc)
        {
            ToPlayerBehavior(npc);
            return true;
        }
        public static void ToPlayerBehavior(NPC npc)
        {
            if (!Main.player.IndexInRange(npc.target))
                return;
            Player targetPlayer = Main.player[npc.target];
            if (targetPlayer is null || !targetPlayer.active)
                return;
            if (targetPlayer.CI().BeeFriendly)
            {
                if (CIList.BeeEnemy.Contains(npc.type))
                    BeeFriendly.LoreQueenBeeEffect(npc);
            }
        }
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.CI().EnemySpawnRateMult != 1f)
                spawnRate = (int)(spawnRate * player.CI().EnemySpawnRateMult);
            if (player.CI().EnemyMaxSpawnMult != 1f)
                maxSpawns = (int)(maxSpawns * player.CI().EnemyMaxSpawnMult);
            if (player.HasBuff<BossEffects>())
            {
                spawnRate *= 5;
                maxSpawns = (int)(maxSpawns * 0.001f);
            }
        }
    }
}
