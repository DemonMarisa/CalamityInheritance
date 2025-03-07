using CalamityInheritance.NPCs;
using Terraria;

namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
    {
        public static CIGlobalNPC CalamityInheritance(this NPC npc)
        {
            return npc.GetGlobalNPC<CIGlobalNPC>();
        }
        /// <summary>
        /// 这个用来去找玩家附近是否存在这个NPC
        /// </summary>
        /// <param name="npcType">NPC单位</param>
        /// <param name="player">玩家单位</param>
        /// <param name="range">最大检索距离</param>
        /// <returns>真:表示有这个npc</returns>
        public static bool IsThereNpcNearby(int npcType, Player player, float range)
        {
            if (npcType <= 0)
                return false;
            int npcIndex = NPC.FindFirstNPC(npcType);
            if (npcIndex != -1)
            {
                NPC npc = Main.npc[npcIndex];
                return npc.active && npc.Distance(player.Center) <= range;
            }
            return false;
        }
    }
}
