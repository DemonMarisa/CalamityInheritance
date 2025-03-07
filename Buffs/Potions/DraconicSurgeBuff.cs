using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Buffs.Potions
{
    public class DraconicSurgeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.CalamityInheritance().DraconicSurgeStats = true;
        }
    }
}
