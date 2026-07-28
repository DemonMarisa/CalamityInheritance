using CalamityInheritance.Content.BaseClass.Buff;
using Terraria;

namespace CalamityInheritance.Content.Buff.Buffs
{
    public class PolarisBuffLegacy : CIBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
