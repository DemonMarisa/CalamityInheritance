using CalamityInheritance.Content.Projectiles.Summon;
using CalamityMod.CalPlayer;
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

namespace CalamityInheritance.Buffs.Summon
{
    public class BloodClot : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
            //Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            CalamityInheritancePlayer modPlayer = player.CIMod();
            if (player.ownedProjectileCounts[ModContent.ProjectileType<BloodClotMinion>()] > 0)
            {
                modPlayer.bloodClot = true;
            }
            if (!modPlayer.bloodClot)
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
