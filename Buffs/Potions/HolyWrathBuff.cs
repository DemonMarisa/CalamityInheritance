using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Buffs.Potions
{
    public class HolyWrathBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //需要造成亵渎之火
            player.CIMod().BuffStatsHolyWrath = true;
            player.GetDamage<GenericDamageClass>() += Main.zenithWorld ? 0.48f : 0.12f;
        }
    }
}
