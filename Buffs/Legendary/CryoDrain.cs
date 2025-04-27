using CalamityInheritance.Buffs.StatDebuffs;
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
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.CIMod().CryoDrainDoT = true;
        }
    }
}