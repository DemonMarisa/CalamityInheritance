using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Buffs.StatDebuffs
{
    public class MaliceModeCold : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Freezing Weather");
            // Description.SetDefault("The weather slows your movement as you freeze to death. You need to look for equipment to protect you from the cold.");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
    }
}
