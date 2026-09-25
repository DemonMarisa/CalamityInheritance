using CalamityInheritance.Content.BaseClass.Buff;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Buff.Buffs
{
    public class BossEffects : CIBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
        }
    }
}
