using CalamityInheritance.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
