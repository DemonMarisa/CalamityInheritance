using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.NPCs
{
    public partial class CIGNPC : GlobalNPC
    {
        public int GaussFluxTimer;
        public override void PostAI(NPC npc)
        {
            if (GaussFluxTimer > 0)
                GaussFluxTimer--;
        }
    }
}
