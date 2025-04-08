using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Buffs.StatDebuffs
{
    public class Horror : ModBuff
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
            player.CIMod().horror = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.CIMod().horrorNPC = true;
        }
    }
}
