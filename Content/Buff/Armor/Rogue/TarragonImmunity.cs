using CalamityInheritance.Content.BaseClass.Buff;
using Terraria;

namespace CalamityInheritance.Content.Buff.Armor.Rogue
{
    public class TarragonImmunity : CIBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.SetImmuneTimeForAllTypes(2);
        }
    }
}
