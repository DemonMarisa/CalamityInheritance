using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Legendary
{
    public class CryoDrain : GenericBuffDefualt 
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.CIMod().CryoDrainDoT = true;
        }
    }
}