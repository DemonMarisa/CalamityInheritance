using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Buff.Buffs.PotionBuff
{
    public class HolyWrathBuff : CIBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            //需要造成亵渎之火
            player.CI().HolyWrath = true;
            player.GetDamage<GenericDamageClass>() += 0.12f;
        }
    }
}
