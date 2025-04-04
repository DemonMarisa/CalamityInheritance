using CalamityInheritance.Utilities;
using Terraria;

namespace CalamityInheritance.NPCs.Boss.Calamitas
{
    public class DespawnThis
    {
        /// <summary>
        /// 使得普灾消失
        /// </summary>
        /// <param name="npc">普灾</param>
        /// <param name="cign">全局</param>
        public static void Despawn(NPC npc, CIGlobalNPC cign)
        {
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];
            player.CIMod().PopTextFlight = false;
            if (!player.active || player.dead)
            {
                if (npc.velocity.Y > 3f)
                    npc.velocity.Y = 3f;
                npc.velocity.Y -= 0.1f;
                if (npc.velocity.Y < -12f)
                    npc.velocity.Y = -12f;

                if (npc.timeLeft > 60)
                    npc.timeLeft = 60;

                if (npc.ai[0] != 0f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    cign.BossNewAI[2] = 0f;
                    cign.BossNewAI[3] = 0f;
                    npc.alpha = 0;
                    npc.netUpdate = true;
                }
                return;
            }
        }
    }
}