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
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Buffs.Summon
{
    public class CosmicEnergyOld : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<CosmicEnergySpiralOld>()] > 0)
            {
                modPlayer.cosmicEnergy = true;
            }
            if (!modPlayer.cosmicEnergy)
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
