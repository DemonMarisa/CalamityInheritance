using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Core.Utils;
using Terraria;

namespace CalamityInheritance.Content.Buff.Debuffs
{
    public class KamiFlu : CIDeBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.AddDebuffDamage(250);
        }
    }
}
