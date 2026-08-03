using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.NPCs
{
    public partial class CIGNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int DeBuffDamage;
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (DeBuffDamage > 0)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= DeBuffDamage;
                damage = DeBuffDamage / 5;
                DeBuffDamage = 0;
            }
        }
    }
}
