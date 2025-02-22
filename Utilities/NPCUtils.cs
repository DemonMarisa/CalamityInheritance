using CalamityInheritance.NPCs;
using Terraria;

namespace CalamityInheritance.Utilities
{
    public static partial class CalamityInheritanceUtils
    {
        public static CalamityInheritanceGlobalNPC CalamityInheritance(this NPC npc)
        {
            return npc.GetGlobalNPC<CalamityInheritanceGlobalNPC>();
        }
    }
}
