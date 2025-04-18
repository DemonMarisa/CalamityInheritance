﻿using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.StatDebuffs
{
    public class SilvaStun : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            base.SetStaticDefaults();
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.CIMod().SilvaStunDebuff = true;
        }
    }
}
