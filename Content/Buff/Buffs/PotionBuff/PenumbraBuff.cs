using CalamityInheritance.Content.BaseClass.Buff;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Buff.Buffs.PotionBuff
{
    public class PenumbraBuff : CIBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<ThrowingDamageClass>() += 0.1f;
            player.LAP().FocusRegenMult = 1f;
            player.LAP().FocusRegenMult += 0.2f;
        }
    }
}
