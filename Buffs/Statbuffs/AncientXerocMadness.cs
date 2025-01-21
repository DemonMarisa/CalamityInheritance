using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Buffs.Statbuffs
{
    public class AncientXerocMadness: ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

    }
}
