using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.StatDebuffs
{
    public class StepToolDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.CIMod().rageOfChair = true;
        }
        internal static void DrawEffects(NPC npc)
        {
            if(Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(npc.Center, 20, 20, DustID.Dirt);
                dust.noGravity = false;
                dust.noLight = true;
                dust.velocity *= 0.2f;
                dust.velocity.Y -= Main.rand.NextFloat(0.4f, 0.6f);
                dust.scale = Main.rand.NextFloat(0.4f, 0.7f);
            }
        }
    }
}