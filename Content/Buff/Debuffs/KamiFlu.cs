using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
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
            npc.LAP().IgnoreDefense += 30;
            npc.LAP().StateDR *= 0.8f;
        }
    }
}
