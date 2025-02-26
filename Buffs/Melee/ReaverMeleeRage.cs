using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Buffs.Melee
{
    public class ReaverMeleeRage : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.CalamityInheritance().ReaverMeleeRage = true;
        }
    }
}
