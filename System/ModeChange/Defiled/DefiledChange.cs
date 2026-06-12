using CalamityInheritance.World;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.System.ModeChange.Defiled
{
    public class DefiledChange : GlobalNPC
    {
        public override void SetDefaults(NPC entity)
        {
            if (CIWorld.defiled)
                entity.value *= 5;
        }
    }
}
