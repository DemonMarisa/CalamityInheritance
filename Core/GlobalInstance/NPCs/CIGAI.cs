using CalamityInheritance.Core.GlobalInstance.NPCs.GlobalAI;
using CalamityInheritance.Core.MiscData;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.NPCs
{
    public partial class CIGNPC : GlobalNPC
    {
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
    }
}
