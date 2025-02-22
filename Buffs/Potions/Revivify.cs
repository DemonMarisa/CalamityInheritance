using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using Terraria.ID;

namespace CalamityInheritance.Buffs.Potions
{
    public class Revivify : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.CalamityInheritance().Revivify = true;
        }
    }
}
