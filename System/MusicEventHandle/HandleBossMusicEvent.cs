using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.System.MusicEventHandle
{
    public class HandleBossMusicEvent : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void OnKill(NPC npc)
        {
        }
    }
}
