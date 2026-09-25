using CalamityInheritance.Content.BaseClass.Buff;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Buff.Debuffs
{
    public class Horror : CIDeBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance *= 0.9f;
            player.statDefense -= 15;
            player.blind = true;
            player.moveSpeed -= 0.15f;
            player.accRunSpeed -= 0.15f;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.LAP().IncomingDamageAddMult += 0.1f;
            npc.LAP().StateDR *= 0.9f;
        }
    }
}
