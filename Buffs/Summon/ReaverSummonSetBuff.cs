using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Typeless;
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
using CalamityInheritance.Content.Projectiles.ArmorProj;

namespace CalamityInheritance.Buffs.Summon
{
    public class ReaverSummonSetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }


        public override void Update(Player player, ref int buffIndex)
        {
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ReaverOrbOld>()] > 0)
            {
                modPlayer1.reaverSummonerOrb = true;
            }
            if (!modPlayer1.reaverSummonerOrb)
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
