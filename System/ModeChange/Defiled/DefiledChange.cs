using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.World;

namespace CalamityInheritance.System.ModeChange.Defiled
{
    public class DefiledChange : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            CIWorld world = ModContent.GetInstance<CIWorld>();
            if (world.Defiled)
                npc.value *= 2;
        }
    }
}
