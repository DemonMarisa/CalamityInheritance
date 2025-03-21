﻿using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Projectiles.Summon.Umbrella;

namespace CalamityInheritance.Buffs.Summon
{
    public class MagicHatBuffOld : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityInheritancePlayer modPlayer1 = player.CIMod();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MagicHatOld>()] > 0)
            {
                modPlayer1.MagicHatOld = true;
            }
            if (!modPlayer1.MagicHatOld)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}
