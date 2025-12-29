using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.Summon;
using LAP.Core.Utilities;
using Terraria;

namespace CalamityInheritance.Buffs.Summon
{
    public class DazzlingStabberBuffLegacy : MinionBuffClass
    {
        public override void UpdateMinion(Player player, CalamityInheritancePlayer usPlayer, ref int buffIndex)
        {
            if (!player.HasProj<DazzlingStabberProj>())
            {
                usPlayer.DazzlingStabberMinionLegacy = true;
            }
            if (!usPlayer.DazzlingStabberMinionLegacy)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }

        }
    }
}
