using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.CDs;
using CalamityInheritance.Core.Utils;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Terraria;

namespace CalamityInheritance.Content.Buff.Buffs.PotionBuff
{
    public class DraconicSurgeBuff : CIBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.CI().DraconicSurge = true;
            if (!player.HasCD<DraconicElixirCD>())
            {
                player.LAP().WingTimeMaxMult += 0.25f;
                player.statDefense += 16;
                player.wingAccRunSpeed += 0.1f;
                player.accRunSpeed += 0.1f;
            }
        }
    }
}
