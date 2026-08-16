using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Core.Utils;
using Terraria;

namespace CalamityInheritance.Content.Buff.Armor.Rogue
{
    public class AncientAstralBuff : CIBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += player.statDefense * 0.3f;
            player.AddDR(0.3f);
            player.CI().BlockDefenseDamage = true;
        }
    }
}