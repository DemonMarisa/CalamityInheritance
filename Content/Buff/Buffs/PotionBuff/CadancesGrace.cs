using CalamityInheritance.Content.BaseClass.Buff;
using LAP.Core.Utilities;
using Terraria;

namespace CalamityInheritance.Content.Buff.Buffs.PotionBuff
{
    public class CadancesGrace : CIBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.LAP().MaxLifeMultiplier += 0.25f;
            player.lifeMagnet = true;
            player.lifeRegen += 10;
        }
    }
}
