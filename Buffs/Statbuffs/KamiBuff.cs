using CalamityInheritance.Content.Items.Weapons.Typeless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Sounds.Custom;
using CalamityMod;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Buffs.Statbuffs
{
    public class KamiBuff : ModBuff
    {
        public const float RunSpeedBoost = 0.15f;
        public const float RunAccelerationBoost = 0.15f;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.CIMod().kamiBoost = true;
        }
    }
}
