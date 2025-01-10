using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Summon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Projectiles.Summon;

namespace CalamityInheritance.Buffs.Summon
{
    public class MidnightSunBuffOld : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            //Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MidnightSunUFOold>()] > 0)
            {
                modPlayer.MidnnightSunBuff = true;
            }
            if (!modPlayer.MidnnightSunBuff)
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
