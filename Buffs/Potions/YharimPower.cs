﻿using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Buffs.Potions
{
    public class YharimPower : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.CIMod().BuffStatsYharimsStin = true;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.075f;
        }
    }
}
