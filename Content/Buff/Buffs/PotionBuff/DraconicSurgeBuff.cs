using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Core.Utils;
using Terraria;

namespace CalamityInheritance.Content.Buff.Buffs.PotionBuff
{
    public class DraconicSurgeBuff : CIBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.CI().DraconicSurge = true;
        }
    }
}
