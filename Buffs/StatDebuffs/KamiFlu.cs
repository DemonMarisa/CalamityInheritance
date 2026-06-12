using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.StatDebuffs
{
    public class KamiFlu : ModBuff
    {
        public const float MultiplicativeDamageReduction = 0.8f;
        // Hard-cap for npc speed when afflicted with this debuff. Does not affect certain NPCs and does not affect any bosses (Basically only works on boss minions).
        public const float MaxNPCSpeed = 16f;

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.CIMod().kamiFlu = true;
            npc.DelBuff(buffIndex);
            buffIndex--;
        }
    }
}
