using CalamityInheritance.Content.BaseClass.Buff;
using LAP.Core.Utilities;
using Terraria;

namespace CalamityInheritance.Content.Buff.Debuffs
{
    public class CIMarkedforDeath : CIDeBuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.LAP().StateDR *= 0.6f;
        }
    }
}
