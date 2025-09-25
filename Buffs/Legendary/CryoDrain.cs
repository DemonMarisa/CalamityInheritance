using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.NPCs;
using CalamityInheritance.Utilities;
using Terraria;

namespace CalamityInheritance.Buffs.Legendary
{
    /*
    *低温虹吸:
    *吸收敌怪的属性, 将其转化为对自身的增益
    */
    public class CryoDrain : GenericBuffDefualt 
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += CIGlobalNPC.CryoDrainDotDamage / 10;
            player.whipRangeMultiplier += 0.2f;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.CIMod().CryoDrainDoT = true;
        }
    }
}