using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Core.GlobalInstance.NPCs;
using CalamityInheritance.Core.Utils;
using Terraria;

namespace CalamityInheritance.Content.Buff.Debuffs
{
    public class CryoDrain :CIDeBuff 
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 10;
            player.whipRangeMultiplier += 0.2f;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.CI().DeBuffDamage += 100;
        }
    }
}